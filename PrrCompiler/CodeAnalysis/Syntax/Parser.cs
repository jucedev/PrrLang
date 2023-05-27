using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Syntax;

internal static class SyntaxFacts
{
    internal static int GetBinaryOperatorPrecedence(this TokenType type)
    {
        switch (type)
        {
            case TokenType.Star:
            case TokenType.ForwardSlash:
                return 4;
            case TokenType.Plus:
            case TokenType.Minus:
                return 3;
            case TokenType.AmpersandAmpersand:
                return 2;
            case TokenType.PipePipe:
                return 1;
            default:
                return 0;
        }
    }
    
    internal static int GetUnaryOperatorPrecedence(this TokenType type)
    {
        switch (type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Bang:
                return 5;
            
            default:
                return 0;
        }
    }

    public static TokenType GetKeywordType(string text)
    {
        return text switch
        {
            "true" => TokenType.TrueKeyword,
            "false" => TokenType.FalseKeyword,
            _ => TokenType.Identifier
        };
    }
}

internal sealed class Parser
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

    private Token MatchToken(TokenType type)
    {
        if (Current.Type == type)
            return NextToken();
        
        _diagnostics.Add($"ERROR: Unexpected token <{Current.Type}>, expected <{type}>");
        return new Token(type, Current.Position, null, null);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var endOfFileToken = MatchToken(TokenType.EndOfFile);
        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private Expression ParseExpression(int parentPrecedence = 0)
    {
        Expression left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();

        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operandToken = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpression(operatorToken, operandToken);   
        }
        else
        {
            left = ParsePrimary();
        }
        
        while (true)
        {
            var precedence = Current.Type.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;
            
            var op = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpression(left, right, op);
        }
 
        return left;
    }

    private Expression ParsePrimary()
    {
        switch (Current.Type)
        {
            case TokenType.OpenParenthesis:
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(TokenType.CloseParenthesis);
                return new ParenthesisExpression(left, expression, right);
            
            case TokenType.TrueKeyword:
            case TokenType.FalseKeyword:
                var keywordToken = NextToken();
                var value = keywordToken.Type == TokenType.TrueKeyword;
                return new LiteralExpression(keywordToken, value);
            
            default:
                var numberToken = MatchToken(TokenType.Number);
                return new LiteralExpression(numberToken);
        }
    }
}