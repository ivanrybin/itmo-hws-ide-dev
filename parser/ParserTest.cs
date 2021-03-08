using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace  IDE_HW1.parser
{
    public class ParserTest
    {
        private const string Lb = "(";
        private const string Rb = ")";
        private string[] ops = {"+", "-", "*", "/"};
        private string[] lits = {"1", "2", "3", "4", "a", "b", "c", "d"};
        private Visitor v;

        private string GenRandomExp(int n)
        {
            string exp = "";
            Random r = new Random();
            while (n != 0)
            {
                string op1 = ops[r.Next(0, 4)];
                string op2 = ops[r.Next(0, 4)];
                string leftOp = lits[r.Next(0, 8)];
                string rightOp = lits[r.Next(0, 8)];
                bool isBrackets = r.Next(0, 100) % 2 == 0;
                bool isBracketsFull = r.Next(0, 500) % 15 == 0;
                if (isBrackets)
                {
                    exp += Lb + leftOp + op1 + rightOp + Rb; // (a `op` b)
                }
                else
                {
                    exp += leftOp + op1 + rightOp; // a op b
                }
                if (isBracketsFull)
                {
                    exp = Lb + exp + Rb; // (exp)
                }
                if (n != 1) // if n == 1 => no next two operands => no connection `op`
                {
                    exp += op2; // exp `op` 
                }
                --n;
            }
            return exp;
        }
        
        [Test]
        public void Basics()
        {
            string[] exps =
            {
                "1", "a", "(1)", "((1))", 
                "1+2", "(1+2)", "(1)+(2)", 
                "(1+2)+3", "(1)+(2+3)", "(1+2+3)", "(1+(2+3))", "((1+2)+3)",
                "(1+2)+3+4", "(1)+(2+3)+(4)", "(1+2+3)+4", "(1+(2+3)+4)", "(1+2)+(3+4)", "1+2+3+4"
            };
            for (int i = 0; i < exps.Length; i++)
            {
                Visitor v = new Visitor();
                IExpression e = Parser.Parse(exps[i]);
                e.Accept(v, true);
                Assert.AreEqual(exps[i], v.Dump());
            }
        }

        [Test]
        public void BadExps()
        {
            string[] exps =
            {
                "+", ")", "(", "+/", "+)", "(+)", ")*(", "*(", ")(",
                "1+", "+1", "(1", "1)",
                "1++", "++1", "1++2", "1+2)", "1+(2",
                "((()))", "()", "(())",
                "!", "%", "$", "@", "1=2", "a=2", 
                "123", "321", "abcdefg"
            };
            for (int i = 0; i < exps.Length; i++)
            {
                IExpression e = Parser.Parse(exps[i]);
                Assert.IsNull(e);
            }
        }
        
        [Test]
        public void Stress()
        {
            Random r = new Random();
            for (int i = 0; i < 50000; i++)
            {
                string exp = GenRandomExp(r.Next(1, 100)); // N operators in expression
                Visitor v = new Visitor();
                IExpression e = Parser.Parse(exp);
                e.Accept(v, true);
                Assert.AreEqual(exp, v.Dump());
            }
        }
    }
}