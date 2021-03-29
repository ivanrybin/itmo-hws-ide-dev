using System.Collections.Generic;

namespace PascalLexer
{
    public class PascalDefaults
    {
        public static readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static readonly string Digits = "0123456789";

        public static readonly HashSet<char> StrStart = new() { '\'', '#' };
        public static readonly HashSet<char> ComStart = new() { '/', '(', '{' };
        public static readonly HashSet<char> NumbStart = new(Digits.ToCharArray());
        public static readonly HashSet<char> IdentStart = new((Letters + "_").ToCharArray());
        public static readonly HashSet<char> IdentChars = new((Letters + "_" + Digits).ToCharArray());

        public enum Chars
        {
            Line = '/',
            LCurly = '{',
            RCurly = '}',
            LSimple = '(',
            RSimple = ')',
            Star = '*',
            Jail = '#',
            Quote = '\''
        }
    }
}
