namespace PrrCompiler.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private readonly List<string> _diagnostics = new();
    private char CurrentChar => 
        _position >= _text.Length ? '\0' : _text[_position];
    
    public IEnumerable<string> Diagnostic => _diagnostics;

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

            if (!int.TryParse(text, out var value))
                _diagnostics.Add($"ERROR: bad number '{text}' cannot be represented by an Int32");
            
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

        if (char.IsLetter(CurrentChar))
        {
            var start = _position;
            
            while (char.IsLetter(CurrentChar))
                Next();
            
            var length = _position - start;
            var text = _text.Substring(start, length);
            var type = SyntaxFacts.GetKeywordType(text);
            return new Token(type, _position, text, null);
        }

        var returnToken = CurrentChar switch
        {
            '+' => new Token(TokenType.Plus, _position++, "+", null),
            '-' => new Token(TokenType.Minus, _position++, "-", null),
            '*' => new Token(TokenType.Star, _position++, "*", null),
            '/' => new Token(TokenType.ForwardSlash, _position++, "/", null),
            '(' => new Token(TokenType.OpenParenthesis, _position++, "(", null),
            ')' => new Token(TokenType.CloseParenthesis, _position++, ")", null),
            _ => new Token(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null)
        };

        if (returnToken.Type == TokenType.BadToken) 
            _diagnostics.Add($"ERROR: bad token: '{returnToken.Value}'");
        
        return returnToken;
    }

    private void Next() => _position++;
}