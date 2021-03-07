using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IDE_HW1.parser
{
    public class Parser
    {
        private static List<char> letters = new List<char>()
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        private static List<char> digits = new List<char>() {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        private static List<char> ops = new List<char>() {'+', '-', '/', '*'};

        private static Dictionary<char, List<char>> next = new Dictionary<char, List<char>>
        {
            {'(', digits.Concat(letters).Concat(new List<char> {'('}).ToList()},
            {')', ops.Concat(new List<char> {')'}).ToList()}
        };
        
        static Parser()
        {
            for (int i = 0; i < letters.Count; i++)
            {
                next[letters[i]] = ops.Concat(new List<char> {')'}).ToList();
            }
            for (int i = 0; i < digits.Count; i++)
            {
                next[digits[i]] = ops.Concat(new List<char> {')'}).ToList();
            }
            for (int i = 0; i < ops.Count; i++)
            {
                next[ops[i]] = digits.Concat(letters).Concat(new List<char> {'('}).ToList();
            }
        }

        public static IExpression Parse(string s)
        {
            Stack<char> stack = new Stack<char>();
            if (!ParseInputString(s, stack))
            {
                return null;
            }
            return BuildExpression(stack);
        }

        private static bool ParseInputString(string s, Stack<char> stack)
        {
            int lbs = 0;
            int rbs = 0;
            char prev = '0';
            bool isFirst = true;
            foreach (var c in s)
            {
                if (c != ' ')
                {
                    if (!next.Keys.Contains(c))
                    {
                        return false;
                    }
                    else
                    {
                        if (isFirst)
                        {
                            if (ops.Contains(c) || ')' == c)
                            {
                                return false;
                            }
                            prev = c;
                            isFirst = false;
                            stack.Push(c);
                        }
                        else if (next[prev].Contains(c))
                        {
                            stack.Push(c);
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
            }

            if (stack.Count == 1 && (ops.Contains(stack.Peek()) || stack.Peek() == ')' || stack.Peek() == '('))
            {
                return false;
            }
            if (ops.Contains(prev) || prev == '(')
            {
                return false;
            }
            return lbs == rbs;
        }

        private static IExpression BuildExpression(Stack<char> stack, bool isInner = false)
        {
            IExpression expr = null;
            while (stack.Count > 0)
            {
                char c = stack.Pop();
                if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    expr = new BinaryExpression(BuildExpression(stack, true), expr, c.ToString());
                } else if (c == ')')
                {
                    expr = new ParenExpression(BuildExpression(stack));
                }
                else if (c == '(')
                {
                    return expr;
                }
                else
                {
                    expr = CreateLiteralOrVariable(c);
                }
                if (expr != null && isInner)
                {
                    return expr;
                }
            }
            return expr;
        }

        private static IExpression CreateLiteralOrVariable(char c) =>
            digits.Contains(c) ? new Literal(c.ToString()) : new Variable(c.ToString());
    }
}
