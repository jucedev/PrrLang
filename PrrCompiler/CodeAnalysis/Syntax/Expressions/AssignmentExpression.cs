namespace PrrCompiler.CodeAnalysis.Syntax.Expressions;

public class AssignmentExpression : Expression
{
    public Token IdentifierToken { get; }
    public Token EqualsToken { get; }
    public Expression Expression { get; }

    public override TokenType Type => TokenType.AssignmentExpression;

    public AssignmentExpression(Token identifierToken, Token equalsToken, Expression expression)
    {
        IdentifierToken = identifierToken;
        EqualsToken = equalsToken;
        Expression = expression;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return IdentifierToken;
    }
}