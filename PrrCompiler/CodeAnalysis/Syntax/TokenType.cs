namespace PrrCompiler.CodeAnalysis.Syntax;

public enum TokenType
{
    // Tokens
    BadToken,
    WhiteSpace,
    EndOfFile,
    Ampersand,
    AmpersandAmpersand,
    Pipe,
    PipePipe,
    Bang,
    Number,
    Identifier,
    
    // Operators
    Plus,
    Minus,
    Star,
    ForwardSlash,
    OpenParenthesis,
    CloseParenthesis,
    
    // keywords
    FalseKeyword,
    TrueKeyword,
    
    // Expressions
    BinaryExpression,
    UnaryExpression,
    ParenthesisExpression,
    LiteralExpression,
}