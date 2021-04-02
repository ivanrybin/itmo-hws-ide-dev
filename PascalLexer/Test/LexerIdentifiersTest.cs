using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public class LexerIdentifiersTest
    {
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
                                "_____1__F__________2_______F_________B__________3\n";
            expression.Check(
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
                    new(TokenType.Ident, "_____1__F__________2_______F_________B__________3"),
                    new(TokenType.Eof, null)
                    
                },
                expression.Lex()
            );  
        }
        
        [Test, Timeout(1000)]
        public void IdentBasicsOneLine()
        {
            string expression = "_a bc d123 _efg123 no\n";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, "_a"),
                    new(TokenType.Ident, "bc"),
                    new(TokenType.Ident, "d123"),
                    new(TokenType.Ident, "_efg123"),
                    new(TokenType.Ident, "no"),
                    new(TokenType.Eof, null)
                    
                },
                expression.Lex()
            );  
        }
        
        [Test, Timeout(1000)]
        public void IdentWithNumbersWithNumbers()
        {
            string expression = "ident123; no ident 123no-no-no\n";
            expression.Check(
                new List<Token>()
                {
                    new(TokenType.Ident, "ident123"),
                    new(TokenType.Semi, ";"),
                    new(TokenType.Ident, "no"),
                    new(TokenType.Ident, "ident"),
                    new(TokenType.Number, "123"),
                    new(TokenType.Ident, "no"),
                    new(TokenType.Special, "-"),
                    new(TokenType.Ident, "no"),
                    new(TokenType.Special, "-"),
                    new(TokenType.Ident, "no"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );  
        }
    }
}