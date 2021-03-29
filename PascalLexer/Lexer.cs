using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PascalLexer
{
    public class Lexer
    {
        private int Pos;
        private bool Eof;
        private StringReader Reader;

        public Lexer(string input)
        {
            Reader = new StringReader(input);
        }

        public Token Lex()
        {
            if (Eof) return new Token(TokenType.Eof, Pos, Pos, null);
            // ' ' OR '\n'
            while (Reader.Peek() == ' ' || Reader.Peek() == '\n') ReadAndIncrement();
            int c = Reader.Peek();
            // EOF
            if (c == -1)
            {
                Eof = true;
                Reader.Close();
                return new Token(TokenType.Eof, Pos, Pos, null);
            }
            // SEMI
            if (c == ';')
            {
                ReadAndIncrement();
                return new Token(TokenType.Eof, Pos-1, Pos, ";");
            }
            // IDENT
            if (PascalDefaults.IdentStart.Contains((char) c)) return LexIdentifier();
            // COMMENT
            if (PascalDefaults.ComStart.Contains((char) c)) return LexComment();
            // NUMBER
            if (PascalDefaults.NumbStart.Contains((char) c)) return LexNumber();
            // CHARACTER STRING
            if (PascalDefaults.StrStart.Contains((char) c)) return LexCharString();
            // UNKNOWN
            return LexUnknown();
        }

        public List<Token> LexAll()
        {
            List<Token> tokens = new List<Token>();
            Token token = Lex();
            while (token.Type != TokenType.Eof)
            {
                tokens.Add(token);
                token = Lex();
            }
            tokens.Add(token);
            return tokens;
        }

        private int ReadAndIncrement()
        {
            Pos++;
            return Reader.Read();
        }

        private Token LexIdentifier()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            while (PascalDefaults.IdentChars.Contains((char) Reader.Peek()))
            {
                sb.Append((char) ReadAndIncrement());
            }
            int c = Reader.Peek();
            if (c == ' ' || c == '\n' || c == -1 || PascalDefaults.ComStart.Contains((char) c))
            {
                return new Token(TokenType.Ident, beg, Pos, sb.ToString());
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }

        private Token LexComment()
        {
            switch (Reader.Peek())
            {
                case (int) PascalDefaults.Chars.Line:
                    return LexLineComment();
                case (int) PascalDefaults.Chars.LSimple:
                    return LexMulSimpleComment();
                case (int) PascalDefaults.Chars.LCurly:
                    return LexMulCurlyComment();
            }
            return new Token(TokenType.Unknown, Pos-1, Pos, (char) ReadAndIncrement());
        }

        private Token LexLineComment()
        {
            int beg = Pos;
            int fst = ReadAndIncrement();
            int snd = Reader.Peek();
            if (snd != (int) PascalDefaults.Chars.Line)
            {
                return new Token(TokenType.Unknown, beg, Pos, (char) fst);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append('/');
            while (Reader.Peek() != '\n' && Reader.Peek() != -1)
            {
                sb.Append((char) ReadAndIncrement());
            }
            return new Token(TokenType.LComment, beg, Pos, sb.ToString());
        }

        private Token LexMulSimpleComment()
        {
            int beg = Pos;
            int fst = ReadAndIncrement();
            int snd = Reader.Peek();
            if (snd != (int) PascalDefaults.Chars.Star)
            {
                return new Token(TokenType.Unknown, beg, Pos, (char) fst);
            }
            int ok = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append((char) fst); // (
            sb.Append((char) ReadAndIncrement()); // *
            int c = ReadAndIncrement();
            while (c != -1)
            {
                sb.Append((char) c);
                if (ok == 0 && c == (int) PascalDefaults.Chars.Star)
                {
                    ok++;
                }
                else if (ok == 1 && c == (int) PascalDefaults.Chars.RSimple)
                {
                    ok++;
                }
                else
                {
                    ok = 0;
                }
                if (ok == 2)
                {
                    return new Token(TokenType.MLComment, beg, Pos, sb.ToString());
                }
                c = ReadAndIncrement();
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }

        private Token LexMulCurlyComment()
        {
            int beg = Pos;
            int c = ReadAndIncrement();
            StringBuilder sb = new StringBuilder();
            while (c != -1)
            {
                sb.Append((char) c);
                if (c == (int) PascalDefaults.Chars.RCurly)
                {
                    return new Token(TokenType.MLComment, beg, Pos, sb.ToString());
                }
                c = ReadAndIncrement();
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }

        private Token LexNumber()
        {
            
            return null;
        }


        private Token LexCharString()
        {
            int beg = Pos;
            bool ok = true;
            StringBuilder sb = new StringBuilder();
            while (ok)
            {
                switch (Reader.Peek())
                {
                    case (char) PascalDefaults.Chars.Jail:
                        ok = LexControlString(sb);
                        break;
                    case (char) PascalDefaults.Chars.Quote:
                        ok = LexQuotedString(sb);
                        break;
                    default:
                        return new Token(TokenType.CharStr, beg, Pos, sb.ToString());
                }
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }

        private bool LexControlString(StringBuilder sb)
        {
            bool ok = true;
            int c = ReadAndIncrement(); // #
            sb.Append((char) c);
            c = ReadAndIncrement();
            if (!PascalDefaults.Digits.Contains((char) c))
            {
                ok = false;
            }
            while (c != ' ' && c != '\n' && c != -1)
            {
                sb.Append((char) c);
                if (ok && !PascalDefaults.Digits.Contains((char) c))
                {
                    ok = false;
                }
                c = ReadAndIncrement();
            }
            return ok;
        }

        private bool LexQuotedString(StringBuilder sb)
        {
            int c = ReadAndIncrement(); // #
            sb.Append((char) c);
            c = ReadAndIncrement();
            while (c != ' ' && c != '\n' && c != -1)
            {
                sb.Append((char) c);
                if (c == (char) PascalDefaults.Chars.Quote)
                {
                    return true;
                }
                c = ReadAndIncrement();
            }
            return false;
        }

        private Token LexUnknown()
        {
            StringBuilder sb = new StringBuilder();
            int beg = Pos;
            int c = ReadAndIncrement();
            while (c != ' ' && c != '\n' && c != -1)
            {
                sb.Append((char) c);
                c = ReadAndIncrement();
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }
    }
}
