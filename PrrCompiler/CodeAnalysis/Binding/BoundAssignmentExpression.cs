namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundAssignmentExpression : BoundExpression
{
    public VariableSymbol Variable { get; }
    public BoundExpression BoundExpression { get; }
    public override BoundNodeType Type => BoundNodeType.AssignmentExpression;
    public override Type ExpressionType => BoundExpression.ExpressionType;
    
    public BoundAssignmentExpression(VariableSymbol variable, BoundExpression boundExpression)
    {
        Variable = variable;
        BoundExpression = boundExpression;
    }
}