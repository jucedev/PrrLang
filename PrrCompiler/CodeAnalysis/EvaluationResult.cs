namespace PrrCompiler.CodeAnalysis;

public sealed class EvaluationResult
{
    public IReadOnlyList<string> Diagnostics { get; }
    public object Value { get; }
    
    public EvaluationResult(IEnumerable<string> diagnostics, object value)
    {
        Value = value;
        Diagnostics = diagnostics.ToArray();
    }

}