namespace IDE_HW1
{
    public class Variable : IExpression
    {
        public readonly string value;
        
        public Variable(string value)
        {
            this.value = value;
        }

        public void Accept(IExpressionVisitor visitor, bool normal = false)
        {
            visitor.Visit(this, normal);
        }
    }
}