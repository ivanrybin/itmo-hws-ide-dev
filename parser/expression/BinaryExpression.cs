namespace IDE_HW1
{
    public class BinaryExpression : IExpression
    {
        public IExpression FirstOperand;
        public IExpression SecondOperand;
        public string Operator;

        public BinaryExpression(string @operator)
        {
            Operator = @operator;
        }
        
        public BinaryExpression(IExpression firstOperand, IExpression secondOperand, string @operator)
        {
            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operator = @operator;
        }

        public void Accept(IExpressionVisitor visitor, bool normal = false)
        {
            visitor.Visit(this, normal);
        }
    }
}