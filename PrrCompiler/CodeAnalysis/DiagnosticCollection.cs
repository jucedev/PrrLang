using System.Collections;
using PrrCompiler.CodeAnalysis.Syntax;

namespace PrrCompiler.CodeAnalysis;

internal sealed class DiagnosticCollection : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();
    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    private void Report(TextSpan span, string message)
    {
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }

    public void AddRange(DiagnosticCollection diagnostics)
    {
        _diagnostics.AddRange(diagnostics);
    }
    
    public void ReportInvalidNumber(TextSpan span, string text, Type type)
    {
        var message = $"The number {text} isn't a valid {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(int position, char currentChar)
    {
        var message = $"Bad character: '{currentChar}'.";
        var span = new TextSpan(position, 1);
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, TokenType currentType, TokenType type)
    {
        var message = $"Unexpected token <{currentType}>, expected <{type}>.";
        Report(span, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorValue, Type expressionType)
    {
        var message = $"Unary operator '{operatorValue}' is not defined for type {expressionType}";
        Report(span, message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string operatorValue, Type leftExpressionType, Type rightExpressionType)
    {
        var message = $"Binary operator '{operatorValue}' is not defined for types {leftExpressionType} and {rightExpressionType}";
        Report(span, message);
    }
}