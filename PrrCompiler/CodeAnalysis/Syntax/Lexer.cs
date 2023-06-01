namespace PrrCompiler.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;
    private int _position;
    private readonly DiagnosticCollection _diagnosticses = new();
    private char CurrentChar => Peek();
    private char NextChar => Peek(1);
    
    public DiagnosticCollection Diagnostics => _diagnosticses;

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

        var start = _position;
        
        if (char.IsDigit(CurrentChar))
        {
            
            while (char.IsDigit(CurrentChar))
                Next();
            
            var length = _position - start;
            var text = _text.Substring(start, length);

            if (!int.TryParse(text, out var value))
                _diagnosticses.ReportInvalidNumber(new TextSpan(start, length), text, typeof(int));
            
            return new Token(TokenType.Number, start, text, value);
        }

        if (char.IsWhiteSpace(CurrentChar))
        {   
            while (char.IsWhiteSpace(CurrentChar))
                Next();
            
            var length = _position - start;
            var text = _text.Substring(start, length);
            
            return new Token(TokenType.WhiteSpace, start, text, null);
        }

        if (char.IsLetter(CurrentChar))
        {
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
            case '&':
                if (NextChar == '&')
                {
                    _position += 2;
                    return new Token(TokenType.AmpersandAmpersand, start, "&&", null);
                }
                break;
            case '|':
                if (NextChar == '|')
                {
                    _position += 2;
                    return new Token(TokenType.PipePipe, start, "||", null);
                }
                break;
            case '=':
                if (NextChar == '=')
                {
                    _position += 2;
                    return new Token(TokenType.EqualsEquals, start, "==", null);
                }
                break;
            case '!':
                if (NextChar == '=')
                {
                    _position += 2;
                    return new Token(TokenType.BangEquals, start, "!=", null);
                }
                _position++;
                return new Token(TokenType.Bang, start, "!", null);
        }
        
        _diagnosticses.ReportBadCharacter(_position, CurrentChar);
        return new Token(TokenType.BadToken, _position++, _text.Substring(_position - 1, 1), null);;
    }
}