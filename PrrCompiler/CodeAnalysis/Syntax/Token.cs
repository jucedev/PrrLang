using System.Runtime.InteropServices;

namespace PrrCompiler.CodeAnalysis.Syntax;

public class Token : Node
{
    public override TokenType Type { get; }
    public override IEnumerable<Node> GetChildren()
    {
        return Enumerable.Empty<Node>();
    }

    public int Position { get; }
    public string Value { get; }
    public object? Result { get; }
    public TextSpan Span => new TextSpan(Position, Value.Length);

    public Token(TokenType type, int position, string value, object? result)
    {
        Type = type;
        Position = position;
        Value = value;
        Result = result;
    }
}