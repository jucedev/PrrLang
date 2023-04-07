namespace PrrCompiler;

public sealed class SyntaxTree
{
    public IEnumerable<string> Diagnostics { get; }
    public Expression Root { get; }
    public Token EndOfFileToken { get; }
    public SyntaxTree(IEnumerable<string> diagnostics, Expression root, Token endOfFileToken)
    {
        Diagnostics = diagnostics;
        Root = root;
        EndOfFileToken = endOfFileToken;
    }
}