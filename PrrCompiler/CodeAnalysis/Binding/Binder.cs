using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly DiagnosticCollection _diagnostics = new();
    public DiagnosticCollection Diagnostics => _diagnostics;

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
        
        _diagnostics.ReportUndefinedBinaryOperator(syntax.Operator.Span, syntax.Operator.Value, left.ExpressionType, right.ExpressionType);
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
        
        _diagnostics.ReportUndefinedUnaryOperator(syntax.Operator.Span, syntax.Operator.Value, boundOperand.ExpressionType);
        return boundOperand;
    }
}