namespace PrrCompiler.CodeAnalysis.Binding;

internal class BoundUnaryExpression : BoundExpression
{
    public override BoundNodeType Type => BoundNodeType.UnaryExpression;
    public override Type ExpressionType => Operand.ExpressionType;
    public BoundUnaryOperatorType OperatorType { get; }
    public BoundExpression Operand { get; }

    public BoundUnaryExpression(BoundUnaryOperatorType operatorType, BoundExpression operand)
    {
        OperatorType = operatorType;
        Operand = operand;
    }
}