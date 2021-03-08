namespace IDE_HW1
{
    public interface IExpressionVisitor
    {
        public void Visit(Literal expression, bool normal = false);
        public void Visit(Variable expression, bool normal = false);
        public void Visit(BinaryExpression expression, bool normal = false);
        public void Visit(ParenExpression expression, bool normal = false);
    }
}
