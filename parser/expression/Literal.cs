namespace IDE_HW1
{
    public class Literal : IExpression
    {
        public readonly string value;

        public Literal(string v)
        {
            this.value = v;
        }
        
        public void Accept(IExpressionVisitor visitor, bool normal = false)
        {
            visitor.Visit(this, normal);
        }
    }
}