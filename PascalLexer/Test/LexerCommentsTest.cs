using System.Collections.Generic;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public class LexerCommentsTest
    {
       [Test, Timeout(1000)]
        public void LineCommentSimple()
        {
            string expression = "// hello, Pascal!";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.LComment, 0, 0, "// hello, Pascal!"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentIdentSameLine()
        {
            string expression = "  x0 // 1hello, Pascal!\n" +
                                "x1  // 2hello, Pascal!\n" +
                                "x2 // 3hello, Pascal!\n" +
                                "x3// 4hello, Pascal!";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.Ident, "x0"),
                    new(TokenType.LComment, "// 1hello, Pascal!"),
                    new(TokenType.Ident, "x1"),
                    new(TokenType.LComment, "// 2hello, Pascal!"),
                    new(TokenType.Ident, "x2"),
                    new(TokenType.LComment, "// 3hello, Pascal!"),
                    new(TokenType.Ident, "x3"),
                    new(TokenType.LComment, "// 4hello, Pascal!"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentIdentBeforeAfterNewLine()
        {
            string expression = "abracadabra\n" +
                                "// hello, Pascal!\n" +
                                "abracadabra";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.Ident, "abracadabra"),
                    new(TokenType.LComment, "// hello, Pascal!"),
                    new(TokenType.Ident, "abracadabra"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void LineCommentComment()
        {
            string expression = "// hello, Pascal! // inner comment";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.LComment, "// hello, Pascal! // inner comment"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void LineCommentMany()
        {
            string expression = "//\n" +
                                "//a\n" +
                                "// b\n" +
                                "// hello, Pascal!\n" +
                                "// next comment\n" +
                                "///\n" +
                                "////";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.LComment, "//"),
                    new(TokenType.LComment, "//a"),
                    new(TokenType.LComment, "// b"),
                    new(TokenType.LComment, "// hello, Pascal!"),
                    new(TokenType.LComment, "// next comment"),
                    new(TokenType.LComment, "///"),
                    new(TokenType.LComment, "////"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentUnknown()
        {
            string expression = "/";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.Special, "/"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentOneLine()
        {
            string expression = "(* multiline one line *)";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.MLComment, expression),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void MultiSimpleCommentManyLines()
        {
            string expression = "(* multiline \n" +
                                "comment\n" +
                                "third line *)";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.MLComment, expression),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentIdentBeforeAfter()
        {
            string expression = "ident (* multiline \n" +
                                "comment\n" +
                                "third line *) ident";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.Ident, "ident"),
                    new(TokenType.MLComment, "(* multiline \ncomment\nthird line *)"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void MultiCurlyCommentOneLine()
        {
            string expression = "{ multiline one line }";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.MLComment, expression),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void MultiCurlyCommentBasics()
        {
            string expression = "{}\n" +
                                "{\n" +
                                "}\n" +
                                "{*x*}\n" +
                                "ident{ multiline one line }ident\n" +
                                "ident {multiline\n" +
                                "} ident";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.MLComment, "{}"),
                    new(TokenType.MLComment, "{\n}"),
                    new(TokenType.MLComment, "{*x*}"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.MLComment, "{ multiline one line }"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Ident, "ident"),
                    new (TokenType.MLComment, "{multiline\n}"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void AllIsBadSimpleComment()
        {
            string expression = "{ it's\n" +
                                "all one\n" +
                                "one big bad comment\n" +
                                "x:=x + 2;";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.BadMLComment, "{ it's\nall one\none big bad comment\nx:=x + 2;"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void AllIsBadCurlyComment()
        {
            string expression = "(* it's\n" +
                                "all one\n" +
                                "one big bad comment\n" +
                                "x:=x + 2;";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.BadMLComment, "(* it's\nall one\none big bad comment\nx:=x + 2;"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
    }
}
