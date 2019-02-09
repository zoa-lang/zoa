using System;
using System.Collections.Generic;
using System.Text;

namespace zoa.compiler.Syntax {
    internal class SyntaxTokenWithValue<T> : SyntaxToken {
        protected readonly string _text;
        protected readonly T _value;

        internal SyntaxTokenWithValue(SyntaxKind type, T value) : this(type, value.ToString(), value) { }

        internal SyntaxTokenWithValue(SyntaxKind type, string text, T value) : base(type, text.Length) {
            _text = text;
            _value = value;
        }

        public override string Text => _text;
        public override object Value => _value;
    }
}
