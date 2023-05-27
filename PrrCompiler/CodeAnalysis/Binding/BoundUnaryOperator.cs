using PrrCompiler.CodeAnalysis.Syntax;

namespace PrrCompiler.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    public TokenType TokenType { get; }
    public BoundUnaryOperatorType Type { get; }
    public Type OperandType { get; }
    public Type? ResultType { get; }

    
    public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorType type, Type operandType, Type? resultType)
    {
        TokenType = tokenType;
        Type = type;
        OperandType = operandType;
        ResultType = resultType;
    }

    public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorType type, Type operandType)
        : this(tokenType, type, operandType, operandType)
    {
    }

    private static readonly BoundUnaryOperator[] Operators =
    {
        new BoundUnaryOperator(TokenType.Bang, BoundUnaryOperatorType.LogicalNegation, typeof(bool)),
        new BoundUnaryOperator(TokenType.Plus, BoundUnaryOperatorType.Identity, typeof(int)),
        new BoundUnaryOperator(TokenType.Minus, BoundUnaryOperatorType.Negation, typeof(int)),
    };

    public static BoundUnaryOperator? Bind(TokenType tokenType, Type operandType)
    {
        return Operators.FirstOrDefault(op => op.TokenType == tokenType && op.OperandType == operandType);
    }
}