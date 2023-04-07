namespace PrrCompiler;

internal static class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return;

            var parser = new Parser(input);
            var syntaxTree = parser.Parse();

            Print(syntaxTree.Root);

            if (!syntaxTree.Diagnostics.Any())
            {
                var evaluator = new Evaluator(syntaxTree.Root);
                var result = evaluator.Evaluate();
                Console.WriteLine(result);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var diagnostic in syntaxTree.Diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }

                Console.ResetColor();
            }
        }
    }

    private static void Print(Node node, string indent = "", bool isLast = true)
    {
        var marker = isLast ? "└──" : "├──";
        
        Console.Write(indent);
        Console.Write(marker);
        Console.Write($"{node.Type}");

        if (node is Token {Value: { }} token)
        {
            Console.Write($" {token.Value}");
        }

        Console.WriteLine();
        indent += isLast ? "    " : "│   ";

        var lastChild = node.GetChildren().LastOrDefault();
        
        foreach (var child in node.GetChildren())
        {
            // Console.Write(indent);
            Print(child, indent, child == lastChild);
        }
    }
}