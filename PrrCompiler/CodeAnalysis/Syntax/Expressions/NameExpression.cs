namespace PrrCompiler.CodeAnalysis.Syntax.Expressions;

public class NameExpression : Expression
{
    public Token IdentifierToken { get; }
    public override TokenType Type => TokenType.NameExpression;

    public NameExpression(Token identifierToken)
    {
        IdentifierToken = identifierToken;
    }
    
    public override IEnumerable<Node> GetChildren()
    {
        yield return IdentifierToken;
    }
}