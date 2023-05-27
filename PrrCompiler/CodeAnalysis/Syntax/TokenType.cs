namespace PrrCompiler.CodeAnalysis.Syntax;

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