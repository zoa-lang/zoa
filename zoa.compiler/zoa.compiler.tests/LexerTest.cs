using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zoa.compiler.Parser;
using zoa.compiler.Syntax;
using static zoa.compiler.Syntax.SyntaxFactory;
using static zoa.compiler.Syntax.SyntaxKind;
using T = zoa.compiler.Syntax.SyntaxToken;

namespace zoa.compiler.tests {
    [TestClass]
    public class LexerTest {
        static void AssertLex(string code, params T[] tokens) {
            var lexer = new Lexer(code);
            int i = 0;
            while (!lexer.IsEOF) {
                if (i == tokens.Length)
                    Assert.Fail("Different tokens length");
                Assert.AreEqual(tokens[i++], lexer.Lex());
            }

            Assert.AreEqual(i, tokens.Length, "Different tokens length");
        }

        static void AssertLexError(string code) {
            bool error = false;
            try {
                var lexer = new Lexer(code);
                while (!lexer.IsEOF)
                    lexer.Lex();
            }
            catch {
                error = true;
            }
            Assert.IsTrue(error, "No exception raised");
        }

        static T K(SyntaxKind kind)
            => KeywordToken(kind);

        [TestMethod]
        public void TestLiteral() {
            AssertLex("1", Literal(1));
            AssertLex("0", Literal(0));
            AssertLex("314", Literal(314));
            AssertLex("3.14", Literal("3.14", 3.14f));
            AssertLex("'a'", Literal('a'));
            AssertLex("' '", Literal(' '));
            AssertLex("'\\n'", Literal('\n'));
            AssertLex("'\\\\'", Literal('\\'));
            AssertLex("'\\''", Literal('\''));
            AssertLex("\"\"", Literal(""));
            AssertLex("\"abc\"", Literal("abc"));
            AssertLex("\"h e l l o 3 \"", Literal("h e l l o 3 "));
            AssertLex("\"\\n\\t\\\"\"", Literal("\n\t\""));
        }

        [TestMethod]
        public void TestLiteralError() {
            AssertLexError("1.16.1");
            AssertLexError("1.a");
            AssertLexError("'ab'");
            AssertLexError("''");
            AssertLexError("'\n'");
            AssertLexError("'\r'");
            AssertLexError("'\t'");
            AssertLexError("'\\'");
            AssertLexError("\"\\\"");
            AssertLexError("\"\"\"");
            AssertLexError("\"\n\"");
        }

        [TestMethod]
        public void TestSpecialTokens() {
            AssertLex("+", K(PlusToken));
            AssertLex("-", K(MinusToken));
            AssertLex("*", K(AsteriskToken));
            AssertLex("/", K(SlashToken));
            AssertLex("%", K(PercentToken));
            AssertLex(">", K(GreaterToken));
            AssertLex("<", K(LessToken));
            AssertLex(">=", K(GreaterEqualToken));
            AssertLex("<=", K(LessEqualToken));
            AssertLex("==", K(EqualToken));
            AssertLex("!=", K(NotEqualToken));
            AssertLex("=", K(AssignToken));
            AssertLex(":", K(ColonToken));
            AssertLex("(", K(LParenToken));
            AssertLex(")", K(RParenToken));
            AssertLex("[", K(LBracketToken));
            AssertLex("]", K(RBracketToken));
            AssertLex("{", K(LBraceToken));
            AssertLex("}", K(RBraceToken));
            AssertLex("->", K(RArrowToken));
            AssertLex("$", K(DollarToken));
            AssertLex(".", K(DotToken));
            AssertLex(";", K(SemicolonToken));
            AssertLex("true", K(TrueKeyword));
            AssertLex("false", K(FalseKeyword));
            AssertLex("let", K(LetKeyword));
            AssertLex("import", K(ImportKeyword));
            AssertLex("mod", K(ModuleKeyword));
            AssertLex("return", K(ReturnKeyword));
            AssertLex("if", K(IfKeyword));
            AssertLex("else", K(ElseKeyword));
            AssertLex("while", K(WhileKeyword));
            AssertLex("for", K(ForKeyword));
            AssertLex("break", K(BreakKeyword));
            AssertLex("continue", K(ContinueKeyword));
            AssertLex("struct", K(StructKeyword));
            AssertLex("trait", K(TraitKeyword));
        }

        [TestMethod]
        public void TestSpecialTokenTogether() {
            AssertLex("< = <=", K(LessToken), K(AssignToken), K(LessEqualToken));
            AssertLex("> = >=", K(GreaterToken), K(AssignToken), K(GreaterEqualToken));
            AssertLex("= = ==", K(AssignToken), K(AssignToken), K(EqualToken));
            AssertLex("- > ->", K(MinusToken), K(GreaterToken), K(RArrowToken));
            AssertLex("-><-1", K(RArrowToken), K(LessToken), K(MinusToken), Literal(1));
        }
    }
}
