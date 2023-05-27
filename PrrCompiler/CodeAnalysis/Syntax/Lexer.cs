namespace PrrCompiler.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private readonly List<string> _diagnostics = new();
    private char CurrentChar => Peek();
    private char NextChar => Peek(1);
    
    public IEnumerable<string> Diagnostic => _diagnostics;

    public Lexer(string text)
    {
        _text = text;
    }
    
    private void Next() => _position++;

    private char Peek(int offset = 0)
    {
        var index = _position + offset;
        return index >= _text.Length ? '\0' : _text[index];
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

        switch (CurrentChar)
        {
            case '+':
                return new Token(TokenType.Plus, _position++, "+", null);
            case '-':
                return new Token(TokenType.Minus, _position++, "-", null);
            case '*':
                return new Token(TokenType.Star, _position++, "*", null);
            case '/':
                return new Token(TokenType.ForwardSlash, _position++, "/", null);
            case '(':
                return new Token(TokenType.OpenParenthesis, _position++, "(", null);
            case ')':
                return new Token(TokenType.CloseParenthesis, _position++, ")", null);
            case '!':
                return new Token(TokenType.Bang, _position++, "!", null);
            case '&':
                if (NextChar == '&')
                    return new Token(TokenType.AmpersandAmpersand, _position += 2, "&&", null);
                break;
            case '|':
                if (NextChar == '|')
                    return new Token(TokenType.PipePipe, _position += 2, "||", null);
                break;
        }
        
        _diagnostics.Add($"ERROR: bad token: '{TokenType.BadToken}'");
        return new Token(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null);;
    }
}