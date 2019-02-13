using System;
using System.Collections.Generic;
using System.Text;
using zoa.compiler.Syntax;

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

        public bool IsEOF => idx == length;

        public Exception Error(string message = "Exception from Lexer")
            => new Exception(message);

        public SyntaxToken Lex() {
            if (IsEOF)
                throw Error("Code reached EOF");

            switch (Peek) {
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
                case '.':
                    return LexKeyword();
                    case 
            }
        }

        private SyntaxToken LexKeyword() {

        }
    }
}
