namespace PrrCompiler.CodeAnalysis.Syntax;

public abstract class Node
{
    public abstract TokenType Type { get; }
    public abstract IEnumerable<Node> GetChildren();
}