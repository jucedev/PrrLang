namespace PrrCompiler.Expressions;

public sealed class UnaryExpression : Expression
{
    public override TokenType Type => TokenType.UnaryExpression;
    public Expression Operand { get; }
    public Token Operator { get; }
    
    public override IEnumerable<Node> GetChildren()
    {
        yield return Operator;
        yield return Operand;
    }

    public UnaryExpression(Token operatorToken, Expression operand)
    {
        Operator = operatorToken;
        Operand = operand;
    }
}