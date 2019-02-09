using System;
using System.Collections.Generic;
using System.Text;

namespace zoa.compiler.Syntax {
    internal class SyntaxTokenIdentifier : SyntaxToken {
        protected readonly string _value;
        internal SyntaxTokenIdentifier(SyntaxKind type, string name)
            // Text가 아직 설정되지 않은 상태에서 base 생성자에서는 Text.Length 를 사용해서
            : base(type, name.Length) {
            _value = name;
        }

        public override string Text => _value;
        public override object Value => _value;
    }
}
