using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public class LexerNumberTest
    {
        
        [Test, Timeout(1000)]
        public void UnsignedBinBasics()
        {
            string expression = "%1\n" +
                                "%10\n" +
                                "%1010\n" +
                                "%10101";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "%1"),
                    new(TokenType.Number, 0, 0, "%10"),
                    new(TokenType.Number, 0, 0, "%1010"),
                    new(TokenType.Number, 0, 0, "%10101"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void SignedBinBasics()
        {
            string expression = "+%1\n" +
                                "-%10\n" +
                                "+%1010\n" +
                                "-%10101";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "+%1"),
                    new(TokenType.Number, 0, 0, "-%10"),
                    new(TokenType.Number, 0, 0, "+%1010"),
                    new(TokenType.Number, 0, 0, "-%10101"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void UnsignedOctalBasics()
        {
            string expression = "&0\n" +
                                "&1\n" +
                                "&123\n" +
                                "&01234567";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "&0"),
                    new(TokenType.Number, 0, 0, "&1"),
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Number, 0, 0, "&01234567"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void SignedOctalBasics()
        {
            string expression = "+&0\n" +
                                "-&1\n" +
                                "+&123\n" +
                                "-&01234567";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "+&0"),
                    new(TokenType.Number, 0, 0, "-&1"),
                    new(TokenType.Number, 0, 0, "+&123"),
                    new(TokenType.Number, 0, 0, "-&01234567"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void UnsignedDecimalBasics()
        {
            string expression = "1\n" +
                                "12\n" +
                                "123\n" +
                                "12.34";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1"),
                    new(TokenType.Number, 0, 0, "12"),
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "12.34"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void SignedDecimalBasics()
        {
            string expression = "+1\n" +
                                "-12\n" +
                                "+123\n" +
                                "-12.34";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "+1"),
                    new(TokenType.Number, 0, 0, "-12"),
                    new(TokenType.Number, 0, 0, "+123"),
                    new(TokenType.Number, 0, 0, "-12.34"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void RealDecimalBasics()
        {
            string expression = "+1.0\n" +
                                "-12.1234567\n" +
                                "+123.0\n" +
                                "-12.000";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "+1.0"),
                    new(TokenType.Number, 0, 0, "-12.1234567"),
                    new(TokenType.Number, 0, 0, "+123.0"),
                    new(TokenType.Number, 0, 0, "-12.000"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void UnsignedDecimalExponent()
        {
            string expression = "1E1\n" +
                                "12E345\n" +
                                "123E123\n" +
                                "1.23e50\n" + 
                                "0.23e1234567";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1E1"),
                    new(TokenType.Number, 0, 0, "12E345"),
                    new(TokenType.Number, 0, 0, "123E123"),
                    new(TokenType.Number, 0, 0, "1.23e50"),
                    new(TokenType.Number, 0, 0, "0.23e1234567"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void SignedDecimalExponent()
        {
            string expression = "1E+1\n" +
                                "+12E+1\n" +
                                "-123E-123\n" +
                                "1.23E-50\n" + 
                                "+0.23e-1234567";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1E+1"),
                    new(TokenType.Number, 0, 0, "+12E+1"),
                    new(TokenType.Number, 0, 0, "-123E-123"),
                    new(TokenType.Number, 0, 0, "1.23E-50"),
                    new(TokenType.Number, 0, 0, "+0.23e-1234567"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void NoExponent()
        {
            string expression = "1E+\n" +
                                "+12E+\n" +
                                "1.23e-EEEE\n" +
                                "1E";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1"),
                    new(TokenType.Ident, 0, 0, "E"),
                    new(TokenType.Special, 0, 0, "+"),
                    new(TokenType.Number, 0, 0, "+12"),
                    new(TokenType.Ident, 0, 0, "E"),
                    new(TokenType.Special, 0, 0, "+"),
                    new(TokenType.Number, 0, 0, "1.23"),
                    new(TokenType.Ident, 0, 0, "e"),
                    new(TokenType.Special, 0, 0, "-"),
                    new(TokenType.Ident, 0, 0, "EEEE"),
                    new(TokenType.Number, 0, 0, "1"),
                    new(TokenType.Ident, 0, 0, "E"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void NoReal()
        {
            string expression = "123.";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Special, 0, 0, "."),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }

        [Test, Timeout(1000)]
        public void UnsignedHexBasics()
        {
            string expression = "$1\n" +
                                "$F\n" +
                                "$FA\n" +
                                "$123FF123\n" + 
                                "$123456789ABCDEF";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "$1"),
                    new(TokenType.Number, 0, 0, "$F"),
                    new(TokenType.Number, 0, 0, "$FA"),
                    new(TokenType.Number, 0, 0, "$123FF123"),
                    new(TokenType.Number, 0, 0, "$123456789ABCDEF"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void SignedHexBasics()
        {
            string expression = "-$1\n" +
                                "-$F\n" +
                                "+$FA\n" +
                                "-$123FF123\n" + 
                                "+$123456789ABCDEF";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "-$1"),
                    new(TokenType.Number, 0, 0, "-$F"),
                    new(TokenType.Number, 0, 0, "+$FA"),
                    new(TokenType.Number, 0, 0, "-$123FF123"),
                    new(TokenType.Number, 0, 0, "+$123456789ABCDEF"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void DifferentDecimalOneLine()
        {
            string expression = "1+2.34-9.5e-150";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1"),
                    new(TokenType.Number, 0, 0, "+2.34"),
                    new(TokenType.Number, 0, 0, "-9.5e-150"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void DifferentAllOneLine()
        {
            string expression = "1+%101-9.5e-150+&1234567-$FFFFFF";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "1"),
                    new(TokenType.Number, 0, 0, "+%101"),
                    new(TokenType.Number, 0, 0, "-9.5e-150"),
                    new(TokenType.Number, 0, 0, "+&1234567"),
                    new(TokenType.Number, 0, 0, "-$FFFFFF"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void NotBinAfterBin()
        {
            string expression = "%1019999%101";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "%101"),
                    new(TokenType.Number, 0, 0, "9999"),
                    new(TokenType.Number, 0, 0, "%101"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void NotOctalAfterOctal()
        {
            string expression = "&1239999&123";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Number, 0, 0, "9999"),
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void MixOneLine()
        {
            string expression = "123%0101234&123$123321FF";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "%0101"),
                    new(TokenType.Number, 0, 0, "234"),
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Number, 0, 0, "$123321FF"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void MixDiffLines()
        {
            string expression = "123\n%0101\n234\n&123\n$123321FF";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "%0101"),
                    new(TokenType.Number, 0, 0, "234"),
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Number, 0, 0, "$123321FF"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void MixSpecialSymbolsOneLine()
        {
            string expression = "123/%0101*234-(&123-$123321FF)";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Special, 0, 0, "/"),
                    new(TokenType.Number, 0, 0, "%0101"),
                    new(TokenType.Special, 0, 0, "*"),
                    new(TokenType.Number, 0, 0, "234"),
                    new(TokenType.Special, 0, 0, "-"),
                    new(TokenType.Special, 0, 0, "("),
                    new(TokenType.Number, 0, 0, "&123"),
                    new(TokenType.Number, 0, 0, "-$123321FF"),
                    new(TokenType.Special, 0, 0, ")"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(10000)]
        public void DecimalSemiRandom()
        {
            string expression = "";
            List<Token> expected = new List<Token>();
            for (int i = 0; i < 10_000; i += 1)
            {
                string number = i.ToString();
                if (i % 5 == 0)
                {
                    number += "." + i;
                }
                if (i % 3 == 0)
                {
                    if (i % 2 == 0)
                    {
                        number += "e+" + i;
                    }
                    else
                    {
                        number += "e-" + i;
                    }
                }
                if (i % 4 == 0)
                {
                    number = "-" + number;
                }
                expression += number + "\n";
                expected.Add(new Token(TokenType.Number, 0, 0, number));
            }
            expected.Add(new Token(TokenType.Eof, 0, 0, null));
            expression.Check(expected, expression.Lex());
        }
        
        [Test, Timeout(10000)]
        public void UnsignedBinStress()
        {
            string expression = "";
            List<Token> expected = new List<Token>();
            for (int i = 1; i < 10_000; i += 100)
            {
                string bin = "%" + Convert.ToString(i, 2);
                expression += bin + "\n";
                expected.Add(new Token(TokenType.Number, 0, 0, bin));
            }
            expected.Add(new Token(TokenType.Eof, 0, 0, null));
            expression.Check(expected, expression.Lex());
        }
        
        [Test, Timeout(10000)]
        public void SignedBinStress()
        {
            string expression = "";
            List<Token> expected = new List<Token>();
            for (int i = 10; i < 10_000; i += 100)
            {
                string sign = i % 2 == 0 ? "-" : "+";
                string bin = "%" + Convert.ToString(i, 2);
                expression += sign + bin + "\n";
                Console.WriteLine(sign + bin);
                expected.Add(new Token(TokenType.Number, 0, 0, sign + bin));
            }
            expected.Add(new Token(TokenType.Eof, 0, 0, null));
            expression.Check(expected, expression.Lex());
        }
    }
}
