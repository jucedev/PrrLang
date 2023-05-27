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
            _ => throw new Exception($@"Unexpected syntax {syntax.Type}")
        };
    }
    
    private BoundExpression BindBinaryExpression(BinaryExpression syntax)
    {
        var left = BindExpression(syntax.Left);
        var right = BindExpression(syntax.Right);
        var operatorType = BindBinaryOperatorType(syntax.Operator.Type, left.ExpressionType, right.ExpressionType);

        if (operatorType != null) 
            return new BoundBinaryExpression(left, right, (BoundBinaryOperatorType) operatorType);
        
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
        var operand = BindExpression(syntax.Operand);
        var operatorType = BindUnaryOperatorType(syntax.Operator.Type, operand.ExpressionType);

        if (operatorType != null) 
            return new BoundUnaryExpression((BoundUnaryOperatorType)operatorType, operand);
        
        _diagnostics.Add($"Unary operator '{syntax.Operator.Value}' is not defined for type {operand.ExpressionType}");
        return operand;
    }

    private static BoundBinaryOperatorType? BindBinaryOperatorType(TokenType operatorType, Type leftType, Type rightType)
    {
        // if left/right value doesn't parse to int, return null
        if (leftType != typeof(int) || rightType != typeof(int))
            return null;
        
        return operatorType switch
        {
            TokenType.Plus => BoundBinaryOperatorType.Addition,
            TokenType.Minus => BoundBinaryOperatorType.Subtraction,
            TokenType.Star => BoundBinaryOperatorType.Multiplication,
            TokenType.ForwardSlash => BoundBinaryOperatorType.Division,
            _ => throw new Exception($@"Unexpected binary operator {operatorType}")
        };
    }

    private static BoundUnaryOperatorType? BindUnaryOperatorType(TokenType operatorType, Type operandType)
    {
        if (operandType != typeof(int))
            return null;
        
        return operatorType switch
        {
            TokenType.Plus => BoundUnaryOperatorType.Identity,
            TokenType.Minus => BoundUnaryOperatorType.Negation,
            _ => throw new Exception($@"Unexpected unary operator {operatorType}")
        };
    }
}