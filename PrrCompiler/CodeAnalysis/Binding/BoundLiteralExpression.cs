namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundLiteralExpression : BoundExpression
{
    public override BoundNodeType Type => BoundNodeType.LiteralExpression;
    public override Type ExpressionType => Value.GetType();
    public object Value { get; }

    public BoundLiteralExpression(object value)
    {
        Value = value;
    }
}