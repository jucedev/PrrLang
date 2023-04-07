namespace PrrCompiler.Expressions;

public sealed class NumberExpression : Expression
{
    public override TokenType Type => TokenType.NumberExpression;
    public Token NumberToken { get; }
    public NumberExpression(Token token) => NumberToken = token;
    public override IEnumerable<Node> GetChildren()
    {
        yield return NumberToken;
    }
}