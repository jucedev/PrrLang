using Microsoft.VisualBasic.FileIO;
using PrrCompiler.CodeAnalysis;
using PrrCompiler.CodeAnalysis.Syntax;

namespace Prr;

internal class Program
{
    private static bool _showTree;
    private static void Main(string[] args)
    {
        var variables = new Dictionary<VariableSymbol, object>();
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input) || 
                EvaluateCommands(input))
                continue;
            
            var syntaxTree = SyntaxTree.Parse(input);
            var compiler = new Compiler(syntaxTree);
            var result = compiler.Evaluate(variables);

            var diagnostics = result.Diagnostics;
            
            if (_showTree)
                Print(syntaxTree.Root);

            if (!diagnostics.Any())
            {
                Console.WriteLine(result.Value);
            }
            else
            {
                PrintDiagnostics(diagnostics, input);
            }
        }
    }

    private static void Print(Node node, string indent = "", bool isLast = true)
    {
        var marker = isLast ? "└──" : "├──";
        
        Console.Write(indent);
        Console.Write(marker);
        Console.Write($"{node.Type}");

        if (node is Token {Value: not null} token)
        {
            Console.Write($" {token.Value}");
        }

        Console.WriteLine();
        indent += isLast ? "    " : "│   ";

        var lastChild = node.GetChildren().LastOrDefault();
        
        foreach (var child in node.GetChildren())
            Print(child, indent, child == lastChild);
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

    private static void PrintDiagnostics(IEnumerable<Diagnostic> diagnostics, string input)
    {

        foreach (var diagnostic in diagnostics)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(diagnostic);
            Console.ResetColor();
        }
    }
}