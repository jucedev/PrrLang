using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly DiagnosticCollection _diagnostics = new();
    private readonly Dictionary<string, object> _variables = new();

    public DiagnosticCollection Diagnostics => _diagnostics;
    public Dictionary<string, object> Variables => _variables;

    public Binder(Dictionary<string, object> variables)
    {
        _variables = variables;
    }
    
    public BoundExpression BindExpression(Expression syntax)
    {
        return syntax.Type switch
        {
            TokenType.ParenthesisExpression => BindParenthesizedExpression((ParenthesisExpression) syntax),
            TokenType.LiteralExpression => BindLiteralExpression((LiteralExpression) syntax),
            TokenType.NameExpression => BindNameExpression((NameExpression) syntax),
            TokenType.AssignmentExpression => BindAssignmentExpression((AssignmentExpression) syntax),
            TokenType.UnaryExpression => BindUnaryExpression((UnaryExpression) syntax),
            TokenType.BinaryExpression => BindBinaryExpression((BinaryExpression) syntax),
            _ => throw new Exception($@"Unexpected syntax {syntax.Type}")
        };
    }

    private BoundExpression BindAssignmentExpression(AssignmentExpression syntax)
    {
        var name = syntax.IdentifierToken.Value;
        var boundExpression = BindExpression(syntax.Expression);

        // TODO: clean this shit up
        var defaultValue =
            boundExpression.ExpressionType == typeof(int)
                ? (object) 0
                : boundExpression.ExpressionType == typeof(bool)
                    ? false 
                    : null;

        if (defaultValue == null)
            throw new Exception($"Unsupported variable type: {boundExpression.ExpressionType}");
        
        return new BoundAssignmentExpression(name, boundExpression);
    }

    private BoundExpression BindNameExpression(NameExpression syntax)
    {
        var name = syntax.IdentifierToken.Value;
        if (!_variables.TryGetValue(name, out var value))
        {
            _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
            return new BoundLiteralExpression(0);
        }

        var type = value?.GetType() ?? typeof(object);
        return new BoundVariableExpression(name, type);
    }

    private BoundExpression BindParenthesizedExpression(ParenthesisExpression syntax)
    {
        return BindExpression(syntax.ParenthesizedExpression);
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