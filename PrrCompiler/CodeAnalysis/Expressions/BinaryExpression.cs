namespace PrrCompiler.Expressions;

public sealed class BinaryExpression : Expression
{
    public override TokenType Type => TokenType.BinaryExpression;
    public override IEnumerable<Node> GetChildren()
    {
        yield return Left;
        yield return Operator;
        yield return Right;
    }

    public Expression Left { get; }
    public Expression Right { get; }
    public Token Operator { get; }
    public BinaryExpression(Expression left, Expression right, Token @operator)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }
}