using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IDE_HW1.parser
{
    public class Parser
    {
        private static readonly List<char> Letters = new List<char>()
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        private static readonly List<char> Digits = new List<char>() {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private static readonly List<char> Ops = new List<char>() {'+', '-', '/', '*'};
        private static readonly Dictionary<char, int> Priority = new Dictionary<char, int>
        {
            {'+', 0},
            {'-', 0},
            {'*', 1},
            {'/', 1}
        };

        private static readonly Dictionary<char, List<char>> Next = new Dictionary<char, List<char>>
        {
            {'(', Digits.Concat(Letters).Concat(new List<char> {'('}).ToList()},
            {')', Ops.Concat(new List<char> {')'}).ToList()}
        };
        
        static Parser()
        {
            for (int i = 0; i < Letters.Count; i++)
            {
                Next[Letters[i]] = Ops.Concat(new List<char> {')'}).ToList();
            }
            for (int i = 0; i < Digits.Count; i++)
            {
                Next[Digits[i]] = Ops.Concat(new List<char> {')'}).ToList();
            }
            for (int i = 0; i < Ops.Count; i++)
            {
                Next[Ops[i]] = Digits.Concat(Letters).Concat(new List<char> {'('}).ToList();
            }
        }

        public static IExpression Parse(string s)
        {
            List<char> chars = new List<char>();
            if (!ParseInputString(s, chars))
            {
                return null;
            }
            return BuildExpression(chars);
        }

        private static bool ParseInputString(string s, List<char> chars)
        {
            int lbs = 0;
            int rbs = 0;
            char prev = '0';
            bool isFirst = true;
            foreach (var c in s)
            {
                if (c != ' ')
                {
                    if (!Next.Keys.Contains(c))
                    {
                        return false;
                    }
                    if (isFirst)
                    {
                        if (Ops.Contains(c) || ')' == c)
                        {
                            return false;
                        }
                        prev = c;
                        isFirst = false;
                        chars.Add(c);
                    }
                    else if (Next[prev].Contains(c))
                    {
                        chars.Add(c);
                        prev = c;
                    }
                    else
                    {
                        return false;
                    }
                    
                    if (c == '(')
                    {
                        lbs++;
                    } else if (c == ')')
                    {
                        rbs++;
                    }
                }
            }

            if (chars.Count == 1 && (Ops.Contains(chars.First()) || chars.First() == ')' || chars.First() == '('))
            {
                return false;
            }
            if (Ops.Contains(prev) || prev == '(')
            {
                return false;
            }
            return lbs == rbs;
        }

        private static IExpression BuildExpression(List<char> chars)
        {
            IExpression expr = null;
            Stack<char> ops = new Stack<char>();
            Stack<IExpression> ast = new Stack<IExpression>();
            
            foreach (var c in chars)
            {
                switch (c)
                {
                    case '(':
                        ops.Push(c);
                        break;
                    case ')':
                        while (ops.Count != 0)
                        {
                            char p = ops.Pop();
                            if (p == '(')
                            {
                                ast.Push(new ParenExpression(ast.Pop()));
                                break;
                            }
                            IExpression r = ast.Pop();
                            IExpression l = ast.Pop();
                            ast.Push(new BinaryExpression(l, r, p.ToString()));
                        }
                        break;
                    default:
                        if (Ops.Contains(c))
                        {
                            while (ops.Count != 0 && Ops.Contains(ops.Peek()))
                            {
                                if (Priority[c] <= Priority[ops.Peek()])
                                {
                                    IExpression r = ast.Pop();
                                    IExpression l = ast.Pop();
                                    ast.Push(
                                        new BinaryExpression(l, r, ops.Pop().ToString())
                                    );
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ops.Push(c);
                        }
                        else
                        {
                            if (Letters.Contains(c))
                            {
                                ast.Push(new Variable(c.ToString()));
                            }
                            else
                            {
                                ast.Push(new Literal(c.ToString()));
                            }
                        }
                        break;
                }
            }

            while (ops.Count != 0)
            {
                IExpression r = ast.Pop();
                IExpression l = ast.Pop();
                ast.Push(
                    new BinaryExpression(l, r, ops.Pop().ToString())
                );
            }
            return ast.Pop();
        }

        private static IExpression CreateLiteralOrVariable(char c) =>
            Digits.Contains(c) ? new Literal(c.ToString()) : new Variable(c.ToString());
    }
}
