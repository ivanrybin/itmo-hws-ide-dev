using System.Collections.Generic;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public class LexerMixTest
    {
        [Test, Timeout(1000)]
        public void EmptyFile()
        {
            string expression = "";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void TokensMixOneLine()
        {
            string expression = "ident:=123+%0101;/// my comment";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, 0, 0, "ident"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "+%0101"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.LComment, 0, 0, "/// my comment"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void UnknownMixOneLine()
        {
            string expression = "good ПЛОХОЙ good";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, 0, 0, "good"),
                    new(TokenType.Unknown, 0, 0, "ПЛОХОЙ"),
                    new(TokenType.Ident, 0, 0, "good"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void TokensMixManyLines()
        {
            string expression = "ident:=123+%0101;{ my comment\n" +
                                "it's comment end } ident:=ident * 2;";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, 0, 0, "ident"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "+%0101"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.MLComment, 0, 0, "{ my comment\nit's comment end }"),
                    new(TokenType.Ident, 0, 0, "ident"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Ident, 0, 0, "ident"),
                    new(TokenType.Special, 0, 0, "*"),
                    new(TokenType.Number, 0, 0, "2"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void ExpressionsOneLine()
        {
            string expression = "ident:=123+%0101;x:=x * 2;            y:=y*2.0E-15;";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, 0, 0, "ident"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Number, 0, 0, "123"),
                    new(TokenType.Number, 0, 0, "+%0101"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.Ident, 0, 0, "x"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Ident, 0, 0, "x"),
                    new(TokenType.Special, 0, 0, "*"),
                    new(TokenType.Number, 0, 0, "2"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.Ident, 0, 0, "y"),
                    new(TokenType.Special, 0, 0, ":"),
                    new(TokenType.Special, 0, 0, "="),
                    new(TokenType.Ident, 0, 0, "y"),
                    new(TokenType.Special, 0, 0, "*"),
                    new(TokenType.Number, 0, 0, "2.0E-15"),
                    new(TokenType.Semi, 0, 0, ";"),
                    new(TokenType.Eof, 0, 0, null)
                },
                expression.Lex()
            );
        }
    }
}