using System.Collections.Generic;
using System.Text;

namespace PascalLexer
{
    public class Lexer
    {
        private int Pos;
        private Token Eof;
        private string input;

        public Lexer(string input)
        {
            this.input = input;
        }

        public Token Lex()
        {
            // EOF
            if (Eof != null)
            {
                return Eof;
            }
            // skip whitespaces
            while (Pos < input.Length && (input[Pos] == ' ' || input[Pos] == '\t' || input[Pos] == '\n'))
            {
                ReadAndIncrement();
            }
            // EOF
            if (Pos == input.Length)
            {
                Eof = new Token(TokenType.Eof, Pos, Pos, null);
                return Eof;
            }
            switch (input[Pos])
            {
                // ;
                case (char) PascalDefaults.Chars.Semi:
                    return new Token(TokenType.Semi, Pos - 1, Pos, ReadAndIncrement());
                // /
                case (char) PascalDefaults.Chars.Slash:
                    // //
                    if (Pos + 1 < input.Length && input[Pos + 1] == (char) PascalDefaults.Chars.Slash)
                        return LexLineComment();
                    // others
                    return LexSpecial();
                // {
                case (char) PascalDefaults.Chars.LCurly:
                    return LexMulCurlyComment();
                // (
                case (char) PascalDefaults.Chars.LSimple:
                    // (*
                    if (Pos + 1 < input.Length && input[Pos + 1] == (char) PascalDefaults.Chars.Star)
                        return LexMulSimpleComment();
                    // others
                    return LexSpecial();
                // '
                case (char) PascalDefaults.Chars.Quote:
                    return LexCharString();
                // #
                case (char) PascalDefaults.Chars.Control:
                    // #[0..9]+
                    if (Pos + 1 < input.Length && PascalDefaults.Digits.Contains(input[Pos + 1]))
                        return LexCharString();
                    return LexSpecial();
            }
            // - +
            if (input[Pos] == (char) PascalDefaults.Chars.Minus || input[Pos] == (char) PascalDefaults.Chars.Plus)
            {
                Token number = TryLexSignedNumber();
                if (number != null) 
                    return number;
                return LexSpecial();
            }
            
            // % & $ 
            if (PascalDefaults.NumbStarts.ContainsKey(input[Pos])) {
                Token number = TryLexNumberSystems();
                if (number != null) 
                    return number;
                return LexSpecial();
            }
            // DECIMAL
            if (PascalDefaults.Digits.Contains(input[Pos])) {
                return LexDecimal();
            }
            // IDENT
            if (PascalDefaults.IdentStart.Contains(input[Pos]))
            {
                return LexIdentifier();
            }
            if (PascalDefaults.SpecialChs.Contains(input[Pos]))
            {
                return LexSpecial();
            }
            // UNKNOWN
            return LexUnknown();
        }

        private Token TryLexSignedNumber()
        {
            // no sign
            if (input[Pos] != (char) PascalDefaults.Chars.Minus && input[Pos] != (char) PascalDefaults.Chars.Plus)
                return null;
            if (Pos + 1 >= input.Length)
                return null;
            // (+|-)[0..9]*
            if (PascalDefaults.Digits.Contains(input[Pos + 1]))
                return LexDecimal();
            // bin / octal / hex
            if (PascalDefaults.NumbStarts.ContainsKey(input[Pos + 1]))
            {
                if (Pos + 2 < input.Length &&
                    PascalDefaults.NumbStarts[input[Pos + 1]].Contains(input[Pos + 2]))
                {
                    return LexNumberSystem();
                }
            }
            return null;
        }

        private Token TryLexNumberSystems()
        {
            if (Pos + 1 >= input.Length)
            {
                return null;
            }
            if (PascalDefaults.NumbStarts.ContainsKey(input[Pos]) && 
                PascalDefaults.NumbStarts[input[Pos]].Contains(input[Pos + 1]))
            {
                return LexNumberSystem();
            }
            return null;
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

        private char ReadAndIncrement()
        {
            return input[Pos++];
        }

        private Token LexIdentifier()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            while (Pos < input.Length && PascalDefaults.IdentChs.Contains(input[Pos]))
            {
                sb.Append(ReadAndIncrement());
            }
            return new Token(TokenType.Ident, beg, Pos, sb.ToString());
        }

        private Token LexLineComment()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            while (Pos < input.Length && input[Pos] != '\n')
            {
                sb.Append(ReadAndIncrement());
            }
            return new Token(TokenType.LComment, beg, Pos, sb.ToString());
        }

        private Token LexMulSimpleComment()
        {
            int ok = 0;
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            sb.Append(ReadAndIncrement()); // (
            sb.Append(ReadAndIncrement()); // *
            while (Pos < input.Length)
            {
                char c = ReadAndIncrement();
                sb.Append(c);
                if (ok == 0 && c == (char) PascalDefaults.Chars.Star)
                {
                    ok++;
                }
                else if (ok == 1 && c == (char) PascalDefaults.Chars.RSimple)
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
            }
            return new Token(TokenType.BadMLComment, beg, Pos, sb.ToString());
        }

        private Token LexMulCurlyComment()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            sb.Append(ReadAndIncrement()); // {
            while (Pos < input.Length)
            {
                char c = ReadAndIncrement();
                sb.Append(c);
                if (c == (char) PascalDefaults.Chars.RCurly)
                {
                    return new Token(TokenType.MLComment, beg, Pos, sb.ToString());
                }
            }
            return new Token(TokenType.BadMLComment, beg, Pos, sb.ToString());
        }

        private Token LexDecimal()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            // SIGN
            if (Pos < input.Length && (input[Pos] == '+' || input[Pos] == '-'))
            {
                sb.Append(ReadAndIncrement());
            }
            LexDigitSequence(sb);
            // REAL
            if (Pos < input.Length && input[Pos] == '.')
            {
                if (Pos + 1 < input.Length && PascalDefaults.Digits.Contains(input[Pos + 1]))
                {
                    sb.Append(ReadAndIncrement()); // .
                    LexDigitSequence(sb);
                }
                else
                {
                    return new Token(TokenType.Number, beg, Pos, sb.ToString());
                }
            }
            // SCALE FACTOR
            if (Pos < input.Length && (input[Pos] == 'E' || input[Pos] == 'e')) // E
            {
                if (Pos + 1 < input.Length) 
                {
                    if (input[Pos + 1] == '+' || input[Pos + 1] == '-') // sign
                    {
                        if (Pos + 2 < input.Length && PascalDefaults.Digits.Contains(input[Pos + 2]))
                        {
                            sb.Append(ReadAndIncrement()); // E
                            sb.Append(ReadAndIncrement()); // sign
                            LexDigitSequence(sb);
                        }
                    }
                    else if (PascalDefaults.Digits.Contains(input[Pos + 1])) // no sign
                    {
                        sb.Append(ReadAndIncrement()); // E
                        LexDigitSequence(sb);
                    }
                }
            }
            return new Token(TokenType.Number, beg, Pos, sb.ToString());
        }

        private void LexDigitSequence(StringBuilder sb)
        {
            while (Pos < input.Length && PascalDefaults.Digits.Contains(input[Pos]))
            {
                sb.Append(ReadAndIncrement());
            }
        }

        private Token LexNumberSystem()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            if (input[Pos] == (char) PascalDefaults.Chars.Minus || input[Pos] == (char) PascalDefaults.Chars.Plus)
            {
                sb.Append(ReadAndIncrement());
            }
            char system = ReadAndIncrement();
            sb.Append(system);
            while (Pos < input.Length && PascalDefaults.NumbStarts[system].Contains(input[Pos]))
            {
                sb.Append(ReadAndIncrement());
            }
            return new Token(TokenType.Number, beg, Pos, sb.ToString());
        }

        private Token LexSpecial()
        {
            int beg = Pos;
            return new Token(TokenType.Special, beg, beg + 1, ReadAndIncrement());
        }
        
        private Token LexCharString()
        {
            int beg = Pos;
            bool ok = false;
            StringBuilder sb = new StringBuilder();
            while (Pos < input.Length)
            {
                switch (input[Pos])
                {
                    case (char) PascalDefaults.Chars.Control:
                        ok = LexControlString(sb);
                        break;
                    case (char) PascalDefaults.Chars.Quote:
                        ok = LexQuotedString(sb);
                        break;
                    default:
                        if (!ok)
                        {
                            return new Token(TokenType.BadCharStr, beg, Pos, sb.ToString());
                        }
                        return new Token(TokenType.CharStr, beg, Pos, sb.ToString());
                }
            }
            if (!ok)
            {
                return new Token(TokenType.BadCharStr, beg, Pos, sb.ToString());
            }
            return new Token(TokenType.CharStr, beg, Pos, sb.ToString());
        }

        private bool LexControlString(StringBuilder sb)
        {
            bool ok = false;
            sb.Append(ReadAndIncrement()); // #
            while (Pos < input.Length && PascalDefaults.Digits.Contains(input[Pos]))
            {
                sb.Append(ReadAndIncrement());
                ok = true;
            }
            return ok;
        }

        private bool LexQuotedString(StringBuilder sb)
        {
            bool ok = false;
            sb.Append(ReadAndIncrement()); // '
            while (Pos < input.Length && input[Pos] != '\n')
            {
                if (input[Pos] == (char) PascalDefaults.Chars.Quote)
                {
                    sb.Append(ReadAndIncrement());
                    ok = true;
                    break;
                }
                sb.Append(ReadAndIncrement());
            }
            return ok;
        }

        private Token LexUnknown()
        {
            int beg = Pos;
            StringBuilder sb = new StringBuilder();
            char c = ReadAndIncrement();
            while (Pos < input.Length && c != ' ' && c != '\n' && c != -1)
            {
                sb.Append(c);
                c = ReadAndIncrement();
            }
            return new Token(TokenType.Unknown, beg, Pos, sb.ToString());
        }
    }
}
