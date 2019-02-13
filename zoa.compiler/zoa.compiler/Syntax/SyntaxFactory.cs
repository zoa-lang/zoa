using System;
using System.Collections.Generic;
using System.Text;

namespace zoa.compiler.Syntax {
    public static class SyntaxFactory {
        private static SyntaxKind FirstToken = SyntaxKind.KnownKeywordStart;
        private static SyntaxKind LastToken = SyntaxKind.KnownKeywordEnd;
        private static SyntaxToken[] _tokens;

        static SyntaxFactory() {
            _tokens = new SyntaxToken[LastToken - FirstToken + 1];

            for (var typ = FirstToken; typ <= LastToken; typ++) {
                _tokens[typ - FirstToken] = new SyntaxToken(typ);
            }
        }

        public static string GetText(SyntaxKind type) {
            switch (type) {
                case SyntaxKind.PlusToken:
                    return "+";
                case SyntaxKind.MinusToken:
                    return "-";
                case SyntaxKind.AsteriskToken:
                    return "*";
                case SyntaxKind.SlashToken:
                    return "/";
                case SyntaxKind.PercentToken:
                    return "%";
                case SyntaxKind.GreaterToken:
                    return ">";
                case SyntaxKind.LessToken:
                    return "<";
                case SyntaxKind.GreaterEqualToken:
                    return ">=";
                case SyntaxKind.LessEqualToken:
                    return "<=";
                case SyntaxKind.EqualToken:
                    return "==";
                case SyntaxKind.NotEqualToken:
                    return "!=";
                case SyntaxKind.AssignToken:
                    return "=";
                case SyntaxKind.ColonToken:
                    return ":";
                case SyntaxKind.LParenToken:
                    return "(";
                case SyntaxKind.RParenToken:
                    return ")";
                case SyntaxKind.LBracketToken:
                    return "[";
                case SyntaxKind.RBracketToken:
                    return "]";
                case SyntaxKind.LBraceToken:
                    return "{";
                case SyntaxKind.RBraceToken:
                    return "}";
                case SyntaxKind.RArrowToken:
                    return "->";
                case SyntaxKind.DollarToken:
                    return "$";
                case SyntaxKind.DotToken:
                    return ".";
                case SyntaxKind.TrueKeyword:
                    return "true";
                case SyntaxKind.FalseKeyword:
                    return "false";

                case SyntaxKind.LetKeyword:
                    return "let";
                case SyntaxKind.ImportKeyword:
                    return "import";
                case SyntaxKind.ModuleKeyword:
                    return "mod";
                case SyntaxKind.ReturnKeyword:
                    return "return";
                case SyntaxKind.IfKeyword:
                    return "if";
                case SyntaxKind.ElseKeyword:
                    return "else";
                case SyntaxKind.WhileKeyword:
                    return "while";
                case SyntaxKind.ForKeyword:
                    return "for";
                case SyntaxKind.BreakKeyword:
                    return "break";
                case SyntaxKind.ContinueKeyword:
                    return "continue";
                case SyntaxKind.StructKeyword:
                    return "struct";
                case SyntaxKind.TraitKeyword:
                    return "trait";
                default:
                    throw new ArgumentException("Failed to find syntaxkind from string pool");
            }
        }

        public static SyntaxToken KeywordToken(SyntaxKind type)
            => _tokens[type - FirstToken];

        public static SyntaxToken Literal(int value)
            => SyntaxToken.WithValue(SyntaxKind.IntegerLiteral, value.ToString(), value);

        public static SyntaxToken Literal(string text, int value)
            => SyntaxToken.WithValue(SyntaxKind.IntegerLiteral, text, value);

        public static SyntaxToken Literal(float value)
            => SyntaxToken.WithValue(SyntaxKind.RealLiteral, value.ToString(), value);

        public static SyntaxToken Literal(string text, float value)
            => SyntaxToken.WithValue(SyntaxKind.RealLiteral, text, value);

        public static SyntaxToken Literal(char value)
            => SyntaxToken.WithValue(SyntaxKind.CharLiteral, value.ToString(), value);

        public static SyntaxToken Literal(string text, char value)
            => SyntaxToken.WithValue(SyntaxKind.CharLiteral, text, value);

        public static SyntaxToken Literal(string value)
            => SyntaxToken.WithValue(SyntaxKind.StringLiteral, value, value);

        public static SyntaxToken Identifier(string name)
            => SyntaxToken.Identifier(name);
    }
}
