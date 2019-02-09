namespace zoa.compiler.Syntax {
    public enum SyntaxKind {
        None = 0,
        IntegerLiteral,
        RealLiteral,
        CharLiteral,
        StringLiteral,

        Identifier,

        KnownKeywordStart,

        PlusToken = KnownKeywordStart, // +
        MinusToken, // -
        AsteriskToken, // *
        SlashToken, // /
        PercentToken, // %
        GreaterToken, // >
        LessToken, // <
        GreaterEqualToken, // >=
        LessEqualToken, // <=
        EqualToken, // ==
        NotEqualToken, // !=
        // LShiftToken, // <<
        // RShiftToken, // >>
        // ExclamationToken, // !
        // TildeToken, // ~
        // VBarToken, // |
        // AmperToken, // &
        // DoubleVBarToken, // ||
        // DoubleAmperToken, // &&
        // CaretToken, // ^
        AssignToken, // =
        ColonToken, // :
        LParenToken, // (
        RParenToken, // )
        LBracketToken, // [
        RBracketToken, // ]
        LBraceToken, // {
        RBraceToken, // }
        // LTypeToken == LessToken ?
        LTypeToken, // <
        RTypeToken, // >
        RArrowToken, // ->
        DollarToken, // $
        TrueKeyword, // true
        FalseKeyword, // false
        DotToken, // .

        LetKeyword,
        ImportKeyword,
        ModuleKeyword,
        ReturnKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        ForKeyword,
        BreakKeyword,
        ContinueKeyword,
        StructKeyword,
        TraitKeyword,

        KnownKeywordEnd = TraitKeyword,
    }
}
