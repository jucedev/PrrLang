using PrrCompiler.CodeAnalysis.Syntax.Expressions;

namespace PrrCompiler.CodeAnalysis.Syntax;

public sealed class SyntaxTree
{
    public IEnumerable<Diagnostic> Diagnostics { get; }
    public Expression Root { get; }
    private Token _endOfFileToken;
    public SyntaxTree(IEnumerable<Diagnostic> diagnostics, Expression root, Token endOfFileToken)
    {
        Diagnostics = diagnostics;
        Root = root;
        _endOfFileToken = endOfFileToken;
    }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }
    
    public static IEnumerable<Token> ParseTokens(string text)
    {
        var lexer = new Lexer(text);
        while (true)
        {
            var token = lexer.NextToken();
            if (token.Type == TokenType.EndOfFile)
                break;

            yield return token;
        }
    }
}