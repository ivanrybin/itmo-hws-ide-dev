using System.Collections.Immutable;
using System.Text;

namespace IDE_HW1
{
    public class Visitor : IExpressionVisitor
    {
        private readonly StringBuilder builder;

        public Visitor()
        {
            builder = new StringBuilder();
        }

        public void Visit(Literal expression, bool normal = false)
        {
            if (normal)
            {
                builder.Append(expression.value);
            }
            else
            {
                builder.Append("Lit(" + expression.value + ")");
            }
        }

        public void Visit(Variable expression, bool normal = false)
        {
            if (normal)
            {
                builder.Append(expression.value);
            }
            else
            {
                builder.Append("Var(" + expression.value + ")");
            }
        }

        public void Visit(BinaryExpression expression, bool normal = false)
        {
            if (normal)
            {
                expression.FirstOperand.Accept(this, normal);
                builder.Append(expression.Operator);
                expression.SecondOperand.Accept(this, normal);
            }
            else
            {
                builder.Append("BinExp(");
                expression.FirstOperand.Accept(this);
                builder.Append(expression.Operator);
                expression.SecondOperand.Accept(this);
                builder.Append(')');
            }
        }

        public void Visit(ParenExpression expression, bool normal = false)
        {
            if (normal)
            {
                builder.Append("(");
                expression.Operand.Accept(this, normal);
                builder.Append(')');
            }
            else
            {
                builder.Append("Paren(");
                expression.Operand.Accept(this);
                builder.Append(')');
            }
        }

        public string Dump()
        {
            return builder.ToString();
        }
    }
}
