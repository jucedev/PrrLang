namespace PrrCompiler;

internal static class Program
{
    private static bool _showTree;

    private static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || 
                EvaluateCommands(input))
                continue;

            var syntaxTree = SyntaxTree.Parse(input);
            if (_showTree)
                Print(syntaxTree.Root);

            if (!syntaxTree.Diagnostics.Any())
            {
                var evaluator = new Evaluator(syntaxTree.Root);
                var result = evaluator.Evaluate();
                Console.WriteLine(result);
            }
            else
            {
                PrintDiagnostics(syntaxTree.Diagnostics);
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
            Print(child, indent, child == lastChild);
    }

    private static void PrintDiagnostics(IEnumerable<string> diagnostics)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        
        foreach (var diagnostic in diagnostics)
            Console.WriteLine(diagnostic);

        Console.ResetColor();
    }

    private static bool EvaluateCommands(string input)
    {
        switch (input)
        {
            case "clear":
                Console.Clear();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            case "tree":
                _showTree = !_showTree;
                Console.WriteLine($"Tree visibile: {_showTree}");
                break;
            default:
                return false;
        }
        
        return true;
    }
}