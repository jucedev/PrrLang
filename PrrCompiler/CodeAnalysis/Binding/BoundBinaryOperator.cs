using PrrCompiler.CodeAnalysis.Syntax;

namespace PrrCompiler.CodeAnalysis.Binding;

public class BoundBinaryOperator
{
    
    public TokenType TokenType { get; }
    public BoundBinaryOperatorType Type { get; }
    public Type LeftType { get; }
    public Type RightType { get; }
    public Type ResultType { get; }

    
    public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorType type, Type leftType, Type rightType, Type resultType)
    {
        TokenType = tokenType;
        Type = type;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
    }

    public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorType operatorType, Type type)
        : this(tokenType, operatorType, type, type, type)
    {
    }

    private static readonly BoundBinaryOperator[] Operators =
    {
        new BoundBinaryOperator(TokenType.Plus, BoundBinaryOperatorType.Addition, typeof(int)),
        new BoundBinaryOperator(TokenType.Minus, BoundBinaryOperatorType.Subtraction, typeof(int)),
        new BoundBinaryOperator(TokenType.Star, BoundBinaryOperatorType.Multiplication, typeof(int)),
        new BoundBinaryOperator(TokenType.ForwardSlash, BoundBinaryOperatorType.Division, typeof(int)),
        new BoundBinaryOperator(TokenType.AmpersandAmpersand, BoundBinaryOperatorType.LogicalAnd, typeof(bool)),
        new BoundBinaryOperator(TokenType.PipePipe, BoundBinaryOperatorType.LogicalOr, typeof(bool)),
    };

    public static BoundBinaryOperator? Bind(TokenType tokenType, Type leftType, Type rightType)
    {
        return Operators.FirstOrDefault(op =>
            op.TokenType == tokenType && 
            op.LeftType == leftType &&
            op.RightType == rightType);
    }
}