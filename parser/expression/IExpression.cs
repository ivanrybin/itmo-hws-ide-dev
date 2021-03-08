namespace IDE_HW1
{
    public interface IExpression
    {
        void Accept(IExpressionVisitor visitor, bool normal = false);
    }
}