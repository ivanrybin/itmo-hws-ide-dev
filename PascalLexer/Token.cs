namespace PascalLexer
{
    public class Token
    {

        public int Beg = -1;
        public int End = -1;
        public TokenType Type;
        public string Literal = null;

        public Token(TokenType type, int beg, int end, string literal)
        {
            Type = type;
            Beg = beg;
            End = end;
            Literal = literal;
        }
        
        public Token(TokenType type, string literal)
        {
            Type = type;
            Literal = literal;
        }
        
        public Token(TokenType type, int beg, int end, char literal) 
            : this(type, beg, end, literal.ToString()) {}

        public override string ToString()
        {
            return "[" + Beg + ":" + End + "]" + " <" + Type + "> \"" + Literal + "\"";
        }
    }
}
