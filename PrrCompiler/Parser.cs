namespace PrrCompiler;

public class Parser
{
    private readonly Token[] _tokens;
    private int _position;
    
    public Parser(string text)
    {
        List<Token> tokens = new();

        var lexer = new Lexer(text);
        Token token;

        do
        {
            token = lexer.NextToken();

            if (token.Type != TokenType.WhiteSpace && token.Type != TokenType.EndOfFile)
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
}