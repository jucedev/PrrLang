namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundVariableExpression : BoundExpression
{
    public string Name { get; }
    public override Type ExpressionType { get; }
    public override BoundNodeType Type => BoundNodeType.NameExpression;

    public BoundVariableExpression(string name, Type type)
    {
        Name = name;
        ExpressionType = type;
    }
}