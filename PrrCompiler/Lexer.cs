namespace PrrCompiler;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private char CurrentChar => _position >= _text.Length ? '\0' : _text[_position];
    
    public Lexer(string text)
    {
        _text = text;
    }

    public Token NextToken()
    {
        // implemented:
        // whitespace
        // + - * / ( )
        // numbers

        if (_position >= _text.Length)
            return new Token(TokenType.EndOfFile, _position, "\0", null);

        if (char.IsDigit(CurrentChar))
        {
            var start = _position;
            
            while (char.IsDigit(CurrentChar))
                Next();
            
            var length = _position - start;
            var text = _text.Substring(start, length);

            int.TryParse(text, out var value);
            
            return new Token(TokenType.Number, start, text, value);
        }

        if (char.IsWhiteSpace(CurrentChar))
        {
            var start = _position;
            
            while (char.IsWhiteSpace(CurrentChar))
                Next();
            
            var length = _position - start;
            var text = _text.Substring(start, length);
            
            return new Token(TokenType.WhiteSpace, start, text, null);
        }

        return CurrentChar switch
        {
            '+' => new Token(TokenType.Plus, _position++, "+", null),
            '-' => new Token(TokenType.Minus, _position++, "-", null),
            '*' => new Token(TokenType.Star, _position++, "*", null),
            '/' => new Token(TokenType.ForwardSlash, _position++, "/", null),
            '(' => new Token(TokenType.OpenParenthesis, _position++, "(", null),
            ')' => new Token(TokenType.CloseParenthesis, _position++, ")", null),
            _ => new Token(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null)
        };
    }

    private void Next() => _position++;
}

internal enum TokenType
{
    Number,
    WhiteSpace,
    Plus,
    Minus,
    Star,
    ForwardSlash,
    OpenParenthesis,
    CloseParenthesis,
    EndOfFile,
    BadToken,
}

internal class Token
{
    public TokenType Type { get; }
    public int Position { get; }
    public string Value { get; }
    public object? Result { get; }

    public Token(TokenType type, int position, string value, object? result)
    {
        Type = type;
        Position = position;
        Value = value;
        Result = result;
    }
}