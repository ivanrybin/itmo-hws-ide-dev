using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public static class Checker
    {
        public static void Check(this string expression, List<Token> expected, List<Token> actual)
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

        public static List<Token> Lex(this string input)
        {
            return new Lexer(input).LexAll();
        }
    }
}
