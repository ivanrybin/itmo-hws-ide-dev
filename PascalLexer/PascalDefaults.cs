using System.Collections.Generic;

namespace PascalLexer
{
    public class PascalDefaults
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string Letters = Alphabet + Alphabet.ToUpper();
        public static readonly string HexLetters = Alphabet.Substring(0, 7) + 
                                                   Alphabet.Substring(0, 7).ToUpper();
        public static readonly string Digits = "0123456789";
        public static readonly string Special = "’+-*/=<>[].,():^@{}$#&%";
        

        // Starts
        public static readonly HashSet<char> StrStart = new() { '\'', '#' };
        public static readonly HashSet<char> ComStart = new() { '/', '(', '{' };
        public static readonly HashSet<char> NumbStart = new((Digits + "$%&").ToCharArray());
        public static readonly HashSet<char> IdentStart = new((Letters + "_").ToCharArray());

        // 1.1 Symbols
        public static readonly HashSet<char> LettersChs = new(Letters.ToCharArray());
        public static readonly HashSet<char> DigitChs = new(Digits.ToCharArray());
        public static readonly HashSet<char> HexDigitChs = new((HexLetters + Digits).ToCharArray());
        public static readonly HashSet<char> SpecialChs = new(Special.ToCharArray());

        public static readonly HashSet<char> IdentChs = new((Letters + "_" + Digits).ToCharArray());


        public static readonly Dictionary<char, HashSet<char>> NumbStarts = new()
        {
            {'%', new("01".ToCharArray())}, // bin
            {'&', new("01234567".ToCharArray())}, // octal
            {'$', HexDigitChs}, // hex
        }; 

        public enum Chars
        {
            Slash = '/',
            LCurly = '{',
            RCurly = '}',
            LSimple = '(',
            RSimple = ')',
            Star = '*',
            Control = '#',
            Quote = '\'',
            Semi = ';',
            Ampersand = '&',
            Dollar = '$',
            Percent = '%',
            Minus = '-',
            Plus = '+'
        }
    }
}
