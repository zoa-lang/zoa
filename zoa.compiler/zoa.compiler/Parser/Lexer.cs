using System;
using System.Collections.Generic;
using System.Text;
using zoa.compiler.Syntax;

namespace zoa.compiler.Parser {
    public class Lexer {
        readonly string code;
        public Lexer(string code) {
            this.code = code;
        }

        public bool IsEOF => throw new NotImplementedException();

        public SyntaxToken Lex() {
            throw new NotImplementedException();
        }
    }
}
