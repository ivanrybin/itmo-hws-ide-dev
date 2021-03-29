using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PascalLexer
{
    public class LexerTests
    {
        public void Check(List<Token> expected, List<Token> actual, string expression)
        {

            if (expected.Count != actual.Count)
            {
                foreach (var token in actual)
                {
                    Console.WriteLine(token);
                }
            }
            Assert.AreEqual(expected.Count, actual.Count, "tokens size test");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Type, actual[i].Type, "bad types for expression: \"" + expression + "\" \n" +
                                                                            "\texpected: " + expected[i] + "\n" + 
                                                                            "\tactual  : " + actual[i]);
                Assert.AreEqual(expected[i].Literal, actual[i].Literal, "bad literal for expression: \"" + expression + "\"\n" +
                                                                        "\texpected: " + expected[i] + "\n" + 
                                                                        "\tactual  : " + actual[i]);
            }
        }

        private List<Token> Lex(string input)
        {
            return new Lexer(input).LexAll();
        }


        [Test, Timeout(1000)]
        public void EmptyFile()
        {
            string expression = "";
            Check(
                new List<Token>()
                {
                    new(TokenType.Eof, 0, 0, null)
                },
                Lex(expression), expression
            );
        }

        [Test, Timeout(1000)]
        public void IdentBasics()
        {
            string expression = "_\n" +
                                "a\n" +
                                "a1\n" +
                                "_a\n" +
                                "_a1\n" +
                                "_1a2\n" +
                                "_12AAAA3\n" +
                                "_123\n" +
                                "ПЛОХОЙ\n" +
                                "_____1__F__________2_______F_________B__________3\n";
            Check(
                new List<Token>()
                {
                    new(TokenType.Ident, "_"),
                    new(TokenType.Ident, "a"),
                    new(TokenType.Ident, "a1"),
                    new(TokenType.Ident, "_a"),
                    new(TokenType.Ident, "_a1"),
                    new(TokenType.Ident, "_1a2"),
                    new(TokenType.Ident, "_12AAAA3"),
                    new(TokenType.Ident, "_123"),
                    new(TokenType.Unknown, "ПЛОХОЙ"),
                    new(TokenType.Ident, "_____1__F__________2_______F_________B__________3"),
                    new(TokenType.Eof, null)
                    
                },
                Lex(expression), expression
            );  
        }
        

        [Test, Timeout(1000)]
        public void LineCommentSimple()
        {
            string expression = "// hello, Pascal!";
            Check(
                new List<Token>()
                {
                    new(TokenType.LComment, 0, 0, "// hello, Pascal!"),
                    new(TokenType.Eof, 0, 0, null)
                },
                Lex(expression), expression
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentIdentSameLine()
        {
            string expression = "  x0 // 1hello, Pascal!\n" +
                                "x1  // 2hello, Pascal!\n" +
                                "x2 // 3hello, Pascal!\n" +
                                "x3// 4hello, Pascal!";
            Check(
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
                Lex(expression), expression
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentIdentBeforeAfterNewLine()
        {
            string expression = "abracadabra\n" +
                                "// hello, Pascal!\n" +
                                "abracadabra";
            Check(
                new List<Token>()  {
                    new(TokenType.Ident, "abracadabra"),
                    new(TokenType.LComment, "// hello, Pascal!"),
                    new(TokenType.Ident, "abracadabra"),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
        
        [Test, Timeout(1000)]
        public void LineCommentComment()
        {
            string expression = "// hello, Pascal! // inner comment";
            Check(
                new List<Token>()  {
                    new(TokenType.LComment, "// hello, Pascal! // inner comment"),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
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
            Check(
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
                Lex(expression), expression
            );
        }

        [Test, Timeout(1000)]
        public void LineCommentUnknown()
        {
            string expression = "/";
            Check(
                new List<Token>()  {
                    new(TokenType.Unknown, "/"),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentOneLine()
        {
            string expression = "(* multiline one line *)";
            Check(
                new List<Token>()  {
                    new(TokenType.MLComment, expression),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentBasics()
        {
            string expression = "(**)\n" +
                                "(*\n" +
                                "*)\n" +
                                "(*x*)\n" +
                                "ident(* multiline one line *)ident\n" +
                                "ident (*multiline\n" +
                                "*) ident";
            Check(
                new List<Token>()  {
                    new(TokenType.MLComment, "(**)"),
                    new(TokenType.MLComment, "(*\n*)"),
                    new(TokenType.MLComment, "(*x*)"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.MLComment, "(* multiline one line *)"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Ident, "ident"),
                    new (TokenType.MLComment, "(*multiline\n*)"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentManyLines()
        {
            string expression = "(* multiline \n" +
                                "comment\n" +
                                "third line *)";
            Check(
                new List<Token>()  {
                    new(TokenType.MLComment, expression),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
        
        [Test, Timeout(1000)]
        public void MultiSimpleCommentIdentBeforeAfter()
        {
            string expression = "ident (* multiline \n" +
                                "comment\n" +
                                "third line *) ident";
            Check(
                new List<Token>()  {
                    new(TokenType.Ident, "ident"),
                    new(TokenType.MLComment, "(* multiline \ncomment\nthird line *)"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Eof, null)
                },
                Lex(expression), expression
            );
        }
    }
}
