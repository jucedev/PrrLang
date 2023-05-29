namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public override BoundNodeType Type => BoundNodeType.BinaryExpression;
    public override Type ExpressionType => Operator.ResultType;
    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public BoundBinaryOperator Operator { get; }

    public BoundBinaryExpression(BoundExpression left, BoundExpression right, BoundBinaryOperator @operator)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }
}