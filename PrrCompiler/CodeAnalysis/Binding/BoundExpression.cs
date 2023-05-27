namespace PrrCompiler.CodeAnalysis.Binding;

public abstract class BoundExpression : BoundNode
{
    public abstract Type ExpressionType { get; }
}