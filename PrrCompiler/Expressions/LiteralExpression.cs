namespace PrrCompiler.Expressions;

public sealed class LiteralExpression : Expression
{
    public override TokenType Type => TokenType.LiteralExpression;
    public Token LiteralToken { get; }
    public LiteralExpression(Token token) => LiteralToken = token;
    public override IEnumerable<Node> GetChildren()
    {
        yield return LiteralToken;
    }
}