namespace zoa.compiler.Syntax
{
    public class SyntaxToken
    {
        public SyntaxKind TokenType { get; }
        public int FullWidth { get; private set; }

        internal SyntaxToken(SyntaxKind type)
        {
            TokenType = type;
            FullWidth = Text.Length;
        }

        protected SyntaxToken(SyntaxKind type, int width)
        {
            TokenType = type;
            FullWidth = width;
        }

        public virtual string Text
            => SyntaxFactory.GetText(TokenType);

        public virtual object Value
        {
            get
            {
                switch (TokenType)
                {
                    case SyntaxKind.TrueKeyword:
                        return true;
                    case SyntaxKind.FalseKeyword:
                        return false;
                    default:
                        return Text;
                }
            }
        }

        internal static SyntaxToken WithValue<T>(SyntaxKind type, string text, T value)
            => new SyntaxTokenWithValue<T>(type, text, value);

        internal static SyntaxToken Identifier(string name)
            => new SyntaxTokenIdentifier(SyntaxKind.Identifier, name);

        public override bool Equals(object obj)
        {
            if (!(obj is SyntaxToken tok))
                return false;
            if (tok.TokenType != TokenType)
                return false;
            if (tok.Text != Text)
                return false;
            if (tok.Value.GetType() != Value.GetType())
                return false;
            if (!tok.Value.Equals(Value))
                return false;

            return true;
        }

        public override int GetHashCode()
            => Text.GetHashCode();
    }
}
