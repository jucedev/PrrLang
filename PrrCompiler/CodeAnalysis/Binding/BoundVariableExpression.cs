namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundVariableExpression : BoundExpression
{
    public VariableSymbol Variable { get; }
    public override Type ExpressionType { get; }
    public override BoundNodeType Type => BoundNodeType.NameExpression;

    public BoundVariableExpression(VariableSymbol variable, Type type)
    {
        Variable = variable;
        ExpressionType = type;
    }
}