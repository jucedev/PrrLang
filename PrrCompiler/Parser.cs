namespace PrrCompiler;

public class Parser
{
    private readonly Token[] _tokens;
    private int _position;
    private List<string> _diagnostics = new();

    public IEnumerable<string> Diagnostics => _diagnostics;
    public Parser(string text)
    {
        List<Token> tokens = new();
        var lexer = new Lexer(text);
        Token token;

        do
        {
            token = lexer.NextToken();

            if (token.Type != TokenType.WhiteSpace && 
                token.Type != TokenType.BadToken)
                tokens.Add(token);

        } while (token.Type != TokenType.EndOfFile);
        
        _tokens = tokens.ToArray();
    }
    
    private Token Peek(int offset = 0)
    {
        var index = _position + offset;
        return index >= _tokens.Length ? _tokens[^1] : _tokens[index];
    }

    private Token Current => Peek();

    private Token NextToken()
    {
        var current = Current;
        _position++;
        return current;   
    }

    private Token Match(TokenType type)
    {
        if (Current.Type == type)
            return NextToken();
        
        _diagnostics.Add($"ERROR: Unexpected token <{Current.Type}>, expected <{type}>");
        return new Token(type, Current.Position, null, null);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var endOfFileToken = Match(TokenType.EndOfFile);
        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private Expression ParseExpression()
    {
        var left = ParsePrimary();
        
        while (Current.Type == TokenType.Plus || 
               Current.Type == TokenType.Minus)
        {
            var op = NextToken();
            var right = ParsePrimary();
            left = new BinaryExpression(left, right, op);
        }

        return left;
    }

    private Expression ParsePrimary()
    {
        var numberToken = Match(TokenType.Number);
        return new NumberExpression(numberToken);
    }
}