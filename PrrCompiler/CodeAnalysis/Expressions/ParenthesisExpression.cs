namespace PrrCompiler.Expressions;

public class ParenthesisExpression : Expression
{
    public Expression ParenthesizedExpression { get; }
    public Token OpenParenthesis { get; }
    public Token CloseParenthesis { get; }
    public override TokenType Type => TokenType.ParenthesisExpression;
    
    public ParenthesisExpression(Token openParenthesis, Expression expression, Token closeParenthesis)
    {
        OpenParenthesis = openParenthesis;
        ParenthesizedExpression = expression;
        CloseParenthesis = closeParenthesis;
    }
    
    public override IEnumerable<Node> GetChildren()
    {
        yield return OpenParenthesis;
        yield return ParenthesizedExpression;
        yield return CloseParenthesis;
    }
}