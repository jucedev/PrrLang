namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public override BoundNodeType Type => BoundNodeType.BinaryExpression;
    public override Type ExpressionType => Left.ExpressionType;
    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public BoundBinaryOperatorType OperatorType { get; }

    public BoundBinaryExpression(BoundExpression left, BoundExpression right, BoundBinaryOperatorType operatorType)
    {
        Left = left;
        Right = right;
        OperatorType = operatorType;
    }
}