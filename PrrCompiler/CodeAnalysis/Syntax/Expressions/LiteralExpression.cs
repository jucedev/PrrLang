namespace PrrCompiler.CodeAnalysis.Syntax.Expressions;

public sealed class LiteralExpression : Expression
{
    public override TokenType Type => TokenType.LiteralExpression;
    public Token LiteralToken { get; }
    public object Value { get; }

    public LiteralExpression(Token token) 
        : this(token, token.Result!)
    {
    }
    
    public LiteralExpression(Token token, object value)
    {
        LiteralToken = token;
        Value = value;
    }
    

    public override IEnumerable<Node> GetChildren()
    {
        yield return LiteralToken;
    }
}