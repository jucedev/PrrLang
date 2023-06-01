using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly List<string> _diagnostics = new();
    public IEnumerable<string> Diagnostics => _diagnostics;

    public BoundExpression BindExpression(Expression syntax)
    {
        return syntax.Type switch
        {
            TokenType.BinaryExpression => BindBinaryExpression((BinaryExpression) syntax),
            TokenType.LiteralExpression => BindLiteralExpression((LiteralExpression) syntax),
            TokenType.UnaryExpression => BindUnaryExpression((UnaryExpression) syntax),
            TokenType.ParenthesisExpression => BindExpression(((ParenthesisExpression) syntax).ParenthesizedExpression),
            _ => throw new Exception($@"Unexpected syntax {syntax.Type}")
        };
    }
    
    private BoundExpression BindBinaryExpression(BinaryExpression syntax)
    {
        var left = BindExpression(syntax.Left);
        var right = BindExpression(syntax.Right);
        var boundOperator = BoundBinaryOperator.Bind(syntax.Operator.Type, left.ExpressionType, right.ExpressionType);

        if (boundOperator != null) 
            return new BoundBinaryExpression(left, right, boundOperator);
        
        _diagnostics.Add($@"Binary operator {syntax.Operator.Value} is not defined for types {left.ExpressionType} and {right.ExpressionType}");
        return left;
    }

    private BoundExpression BindLiteralExpression(LiteralExpression syntax)
    {
        var value = syntax.Value ?? 0;
        return new BoundLiteralExpression(value);
    }
    
    private BoundExpression BindUnaryExpression(UnaryExpression syntax)
    {
        var boundOperand = BindExpression(syntax.Operand);
        var boundOperator = BoundUnaryOperator.Bind(syntax.Operator.Type, boundOperand.ExpressionType);

        if (boundOperator != null) 
            return new BoundUnaryExpression(boundOperator, boundOperand);
        
        _diagnostics.Add($"Unary operator '{syntax.Operator.Value}' is not defined for type {boundOperand.ExpressionType}");
        return boundOperand;
    }
}