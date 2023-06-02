using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly DiagnosticCollection _diagnostics = new();
    private readonly Dictionary<VariableSymbol, object> _variables = new();

    public DiagnosticCollection Diagnostics => _diagnostics;
    public Dictionary<VariableSymbol, object> Variables => _variables;

    public Binder(Dictionary<VariableSymbol, object> variables)
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

        var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
        if (existingVariable != null)
            _variables.Remove(existingVariable);

        var variable = new VariableSymbol(name, boundExpression.ExpressionType);
        _variables[variable] = null!;
        
        return new BoundAssignmentExpression(variable, boundExpression);
    }

    private BoundExpression BindNameExpression(NameExpression syntax)
    {
        var name = syntax.IdentifierToken.Value;
        var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

        if (variable != null) 
            return new BoundVariableExpression(variable, variable.Type);
        
        _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
        return new BoundLiteralExpression(0);
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