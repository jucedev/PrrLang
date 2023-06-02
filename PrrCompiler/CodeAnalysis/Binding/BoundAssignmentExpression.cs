namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundAssignmentExpression : BoundExpression
{
    public string Name { get; }
    public BoundExpression BoundExpression { get; }
    public override BoundNodeType Type => BoundNodeType.AssignmentExpression;
    public override Type ExpressionType => BoundExpression.ExpressionType;
    
    public BoundAssignmentExpression(string name, BoundExpression boundExpression)
    {
        Name = name;
        BoundExpression = boundExpression;
    }
}