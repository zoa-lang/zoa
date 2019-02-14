using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using zoa.compiler.Syntax;
using static zoa.compiler.Syntax.SyntaxFactory;
using static zoa.compiler.Syntax.SyntaxKind;

namespace zoa.compiler.Parser {
    public class Lexer {
        readonly string code;
        readonly int length;
        int idx;

        public Lexer(string code) {
            this.code = code.Trim();
            length = code.Length;
        }

        char Peek => code[idx];
        char Pop() => code[idx++];

        char Pop(char expected) {
            var actual = Pop();
            if (actual != expected) {
                throw Error($"Unexpected char {actual}, expected {expected}");
            }

            return actual;
        }

        public bool IsEOF => idx == length;

        public Exception Error(string message = "Exception from Lexer")
            => new Exception(message);

        public SyntaxToken Lex() {
            if (IsEOF)
                throw Error("Code reached EOF");

            switch (Peek) {
                case ' ':
                case '\r':
                case '\n':
                case '\t':
                    Pop();
                    return Lex();
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '>':
                case '<':
                case '=':
                case '!':
                case ':':
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '$':
                case '.':
                case ';':
                    return LexOperator();
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return LexNumeric();
                case '\'':
                    return LexChar();
                case '"':
                    return LexString();
                default:
                    return LexIdentifier();
            }
        }

        private SyntaxToken LexOperator() {
            var top = Pop();
            switch (top) {
                case '+':
                    return KeywordToken(PlusToken);
                case '-':
                    if (!IsEOF && Peek == '>') {
                        Pop();
                        return KeywordToken(RArrowToken);
                    }

                    return KeywordToken(MinusToken);
                case '*':
                    return KeywordToken(AsteriskToken);
                case '/':
                    return KeywordToken(SlashToken);
                case '%':
                    return KeywordToken(PercentToken);
                case '>':
                    if (!IsEOF && Peek == '=') {
                        Pop();
                        return KeywordToken(GreaterEqualToken);
                    }

                    return KeywordToken(GreaterToken);
                case '<':
                    if (!IsEOF && Peek == '=') {
                        Pop();
                        return KeywordToken(LessEqualToken);
                    }

                    return KeywordToken(LessToken);
                case '=':
                    if (!IsEOF && Peek == '=') {
                        Pop();
                        return KeywordToken(EqualToken);
                    }

                    return KeywordToken(AssignToken);
                case '!':
                    if (!IsEOF && Peek == '=') {
                        Pop();
                        return KeywordToken(NotEqualToken);
                    }
                    
                    // TODO: return ExclamationToken
                    throw Error("operator `!` is not implemented yet");
                case ':':
                    return KeywordToken(ColonToken);
                case '(':
                    return KeywordToken(LParenToken);
                case ')':
                    return KeywordToken(RParenToken);
                case '[':
                    return KeywordToken(LBracketToken);
                case ']':
                    return KeywordToken(RBracketToken);
                case '{':
                    return KeywordToken(LBraceToken);
                case '}':
                    return KeywordToken(RBraceToken);
                case '$':
                    return KeywordToken(DollarToken);
                case '.':
                    return KeywordToken(DotToken);
                case ';':
                    return KeywordToken(SemicolonToken);
                default:
                    throw Error($"Unexpected char {top}");
            }
        }

        private SyntaxToken LexNumeric() {
            int start = idx;
            while (!IsEOF) {
                if (Peek == '.')
                    return LexRealFromDot(start);
                if (!char.IsNumber(Peek))
                    break;

                Pop();
            }

            int len = idx - start;
            var text = code.Substring(start, len);
            return Literal(text, int.Parse(text));
        }

        private SyntaxToken LexRealFromDot(int startidx) {
            Pop(); // .
            if (IsEOF || !char.IsNumber(Pop()))
                throw Error("fractional part of real number should not be empty");

            while (!IsEOF) {
                if (Peek == '.')
                    throw Error("operator . is not allowed after real number");
                if (!char.IsNumber(Peek))
                    break;
                Pop();
            }

            int len = idx - startidx;
            var text = code.Substring(startidx, len);
            return Literal(text, Convert.ToSingle(text));
        }

        private SyntaxToken LexChar() {
            Pop('\'');
            if (IsEOF || Peek == '\'')
                throw Error("char literal should not be empty");
            if (Peek == '\n' || Peek == '\r' || Peek == '\t')
                throw Error("invalid char literal");
            var text = Pop();
            if (text == '\\')
                text = ParseEscape(true, false);
            Pop('\'');
            return Literal(text);
        }

        private SyntaxToken LexString() {
            Pop('"');
            var result = new StringBuilder();
            while (!IsEOF && Peek != '"') {
                var cur = Pop();
                if (cur == '\n' || cur == '\r' || cur == '\t')
                    throw Error("invalid string literal");
                if (cur == '\\')
                    cur = ParseEscape(false, true);
                result.Append(cur);
            }

            if (IsEOF)
                throw Error("string literal should be ended");
            Pop('"');
            return Literal(result.ToString());
        }

        private char ParseEscape(bool escapeChar, bool escapeString) {
            var unescaped = Pop();
            switch (unescaped) {
                case 'n':
                    return '\n';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
                case '\\':
                    return '\\';
                case '\'' when escapeChar:
                    return '\'';
                case '"' when escapeString:
                    return '"';
                case '0':
                    return '\0';
                default:
                    throw Error($"Unexpected escape char: {unescaped}");
            }
        }

        private SyntaxToken LexIdentifier() {
            int start = idx;
            while (!IsEOF) {
                if (Peek != '_' && !char.IsLetterOrDigit(Peek))
                    break;

                Pop();
            }

            int len = idx - start;
            if (len == 0)
                throw Error("identifier should not be empty");
            var text = code.Substring(start, len);

            if (TryAsKeyword(text, out var keyword))
                return keyword;
            return Identifier(text);
        }

        private bool TryAsKeyword(string text, out SyntaxToken result) {
            switch (text) {
                case "true":
                    result = KeywordToken(TrueKeyword);
                    break;
                case "false":
                    result = KeywordToken(FalseKeyword);
                    break;
                case "let":
                    result = KeywordToken(LetKeyword);
                    break;
                case "import":
                    result = KeywordToken(ImportKeyword);
                    break;
                case "mod":
                    result = KeywordToken(ModuleKeyword);
                    break;
                case "return":
                    result = KeywordToken(ReturnKeyword);
                    break;
                case "if":
                    result = KeywordToken(IfKeyword);
                    break;
                case "else":
                    result = KeywordToken(ElseKeyword);
                    break;
                case "while":
                    result = KeywordToken(WhileKeyword);
                    break;
                case "for":
                    result = KeywordToken(ForKeyword);
                    break;
                case "break":
                    result = KeywordToken(BreakKeyword);
                    break;
                case "continue":
                    result = KeywordToken(ContinueKeyword);
                    break;
                case "struct":
                    result = KeywordToken(StructKeyword);
                    break;
                case "trait":
                    result = KeywordToken(TraitKeyword);
                    break;
                default:
                    result = null;
                    return false;
            }

            return true;
        }
    }
}
