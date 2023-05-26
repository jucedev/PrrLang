namespace PrrCompiler;

public enum TokenType
{
    // Tokens
    BadToken,
    WhiteSpace,
    EndOfFile,
    Number,
    
    // Operators
    Plus,
    Minus,
    Star,
    ForwardSlash,
    OpenParenthesis,
    CloseParenthesis,
    
    // Expressions
    BinaryExpression,
    UnaryExpression,
    ParenthesisExpression,
    LiteralExpression,
}