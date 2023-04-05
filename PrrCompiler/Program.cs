namespace PrrCompiler;

internal static class Program
{
    static void Main(string[] args)
    {
        Console.Write("> ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return;
        
        var lexer = new Lexer(input);

        var token = lexer.NextToken();
        while (token.Type != TokenType.EndOfFile)
        {
            Console.WriteLine($"{token.Type}: '{token.Value}'");
            if (token.Result != null)
                Console.WriteLine($"Value: {token.Result}");
            
            token = lexer.NextToken();
        } 
    }
}