using PrrCompiler.CodeAnalysis.Syntax;

namespace PrrCompiler.Tests.CodeAnalysis.Syntax;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokensData))]
    public void Lexer_Lexes_Token(TokenType type, string text)
    {
        var tokens = SyntaxTree.ParseTokens(text);

        var token = Assert.Single(tokens);
        Assert.Equal(type, token.Type);
        Assert.Equal(text, token.Value);
    }
    
    [Theory]
    [MemberData(nameof(GetTokenPairsData))] 
    public void Lexer_Lexes_TokenPairs(TokenType t1Type, string t1Text, TokenType t2Type, string t2Text)
    {
        var text = t1Text + t2Text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();

        Assert.Equal(2, tokens.Length);
        Assert.Equal(t1Type, tokens[0].Type);
        Assert.Equal(t1Text, tokens[0].Value);
        Assert.Equal(t2Type, tokens[1].Type);
        Assert.Equal(t2Text, tokens[1].Value);
    }
    
    [Theory]
    [MemberData(nameof(GetTokenPairsWithWhitespaceData))] 
    public void Lexer_Lexes_TokenPairsWithWhitespace(TokenType t1Type, string t1Text, TokenType separatorType, string separatorText, TokenType t2Type, string t2Text)
    {
        var text = t1Text + separatorText + t2Text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();

        Assert.Equal(3, tokens.Length);
        Assert.Equal(t1Type, tokens[0].Type);
        Assert.Equal(t1Text, tokens[0].Value);
        Assert.Equal(separatorType, tokens[1].Type);
        Assert.Equal(separatorText, tokens[1].Value);
        Assert.Equal(t2Type, tokens[2].Type);
        Assert.Equal(t2Text, tokens[2].Value);
    }

    public static IEnumerable<object[]> GetTokensData() =>
        GetTokens().Concat(GetWhiteSpace())
            .Select(t => new object[] {t.type, t.text});
    
    public static IEnumerable<object[]> GetTokenPairsData() =>
        GetTokenPairs().Select(t => new object[] {t.t1Type, t.t1Text, t.t2Type, t.t2Text});
    
    public static IEnumerable<object[]> GetTokenPairsWithWhitespaceData() =>
        GetTokenPairsWithWhiteSpace().Select(t => 
            new object[] {t.t1Type, t.t1Text, t.separatorType, t.separatorText, t.t2Type, t.t2Text});


    private static bool RequiresSeparator(TokenType t1Type, TokenType t2Type)
    {
        var t1IsKeyword = t1Type.ToString().EndsWith("Keyword");
        var t2IsKeyword = t2Type.ToString().EndsWith("Keyword");

        return (t1Type == TokenType.Identifier && t2Type == TokenType.Identifier) ||
               (t1Type == TokenType.Number && t2Type == TokenType.Number) ||
               (t1Type == TokenType.Equals && t2Type == TokenType.Equals) ||
               (t1Type == TokenType.Equals && t2Type == TokenType.EqualsEquals) ||
               (t1Type == TokenType.Bang && t2Type == TokenType.Equals) ||
               (t1Type == TokenType.Bang && t2Type == TokenType.EqualsEquals) ||
               (t1Type == TokenType.Identifier && t2IsKeyword) ||
               (t1IsKeyword && t2Type == TokenType.Identifier) ||
               (t1IsKeyword && t2IsKeyword);
    }

    private static IEnumerable<(TokenType t1Type, string t1Text, TokenType t2Type, string t2Text)> GetTokenPairs()
    {
        return 
            from t1 in GetTokens() 
            from t2 in GetTokens() 
            where !RequiresSeparator(t1.type, t2.type) 
            select (t1.type, t1.text, t2.type, t2.text);
    }
    
    private static IEnumerable<(TokenType t1Type, string t1Text, TokenType separatorType, string separatorText, TokenType t2Type, string t2Text)> GetTokenPairsWithWhiteSpace()
    {
        return 
            from t1 in GetTokens() 
            from t2 in GetTokens() 
            where RequiresSeparator(t1.type, t2.type) 
            from s in GetWhiteSpace() 
            select (t1.type, t1.text, s.type, s.text, t2.type, t2.text);
    }

    private static IEnumerable<(TokenType type, string text)> GetTokens() =>
        new[]
        {
            (TokenType.Plus, "+"),
            (TokenType.Minus, "-"),
            (TokenType.Star, "*"),
            (TokenType.ForwardSlash, "/"),
            (TokenType.Bang, "!"),
            (TokenType.BangEquals, "!="),
            (TokenType.Equals, "="),
            (TokenType.EqualsEquals, "=="),
            (TokenType.AmpersandAmpersand, "&&"),
            (TokenType.PipePipe, "||"),
            (TokenType.OpenParenthesis, "("),
            (TokenType.CloseParenthesis, ")"),
            (TokenType.FalseKeyword, "false"),
            (TokenType.TrueKeyword, "true"),

            (TokenType.Identifier, "a"),
            (TokenType.Identifier, "abc"),
            (TokenType.Number, "1"),
            (TokenType.Number, "1234"),
        };
    
    private static IEnumerable<(TokenType type, string text)> GetWhiteSpace() =>
        new[]
        {
            (TokenType.WhiteSpace, " "),
            (TokenType.WhiteSpace, "  "),
            (TokenType.WhiteSpace, "\r"),
            (TokenType.WhiteSpace, "\n"),
            (TokenType.WhiteSpace, "\r\n"),
        };
}