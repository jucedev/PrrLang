namespace PrrCompiler;

public class Token : Node
{
    public override TokenType Type { get; }
    public override IEnumerable<Node> GetChildren()
    {
        return Enumerable.Empty<Node>();
    }

    public int Position { get; }
    public object? Value { get; }
    public object? Result { get; }

    public Token(TokenType type, int position, string? value, object? result)
    {
        Type = type;
        Position = position;
        Value = value;
        Result = result;
    }
}