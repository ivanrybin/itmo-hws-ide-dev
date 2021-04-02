using System.Collections.Generic;
using NUnit.Framework;

namespace PascalLexer.Test
{
    public class LexerStringsTest
    {
        [Test, Timeout(1000)]
        public void CharStringsBasics()
        {
            string expression = "'This is a pascal string'\n" +
                                "''\n" +
                                "'a'\n" +
                                "'A tabulator character: '#9' is easy to embed'\n" +
                                "'the string starts here'#13#10'   and continues here'\n" + 
                                "'the string starts here'#10'   and continues here'";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.CharStr, "'This is a pascal string'"),
                    new(TokenType.CharStr, "''"),
                    new(TokenType.CharStr, "'a'"),
                    new(TokenType.CharStr, "'A tabulator character: '#9' is easy to embed'"),
                    new(TokenType.CharStr, "'the string starts here'#13#10'   and continues here'"),
                    new(TokenType.CharStr, "'the string starts here'#10'   and continues here'"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
        
        [Test, Timeout(1000)]
        public void BadCharStrings()
        {
            string expression = "'the string starts here\n" +
                                "and continues here'\n" +
                                "#'not byte - just special'";
            expression.Check(
                new List<Token>()  {
                    new(TokenType.BadCharStr, "'the string starts here"),
                    new(TokenType.Ident, "and"),
                    new(TokenType.Ident, "continues"),
                    new(TokenType.Ident, "here"),
                    new(TokenType.BadCharStr, "'"),
                    new(TokenType.Special, "#"),
                    new(TokenType.CharStr, "'not byte - just special'"),
                    new(TokenType.Eof, null)
                },
                expression.Lex()
            );
        }
    }
}
