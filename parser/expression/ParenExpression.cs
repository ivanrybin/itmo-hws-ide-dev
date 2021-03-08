namespace IDE_HW1
{
    public class ParenExpression : IExpression
    {
        public IExpression Operand;

        public ParenExpression(IExpression operand)
        {
            Operand = operand;
        }

        public void Accept(IExpressionVisitor visitor, bool normal = false)
        {
            visitor.Visit(this, normal);
        }
    }
}