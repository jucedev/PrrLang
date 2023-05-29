namespace PrrCompiler.CodeAnalysis.Syntax;

public enum TokenType
{
    // Tokens
    BadToken,
    WhiteSpace,
    EndOfFile,
    EqualsEquals,
    BangEquals,
    AmpersandAmpersand,
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