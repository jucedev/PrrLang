using Microsoft.VisualBasic.FileIO;
using PrrCompiler.CodeAnalysis;
using PrrCompiler.CodeAnalysis.Syntax;

namespace Prr;

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
            var compiler = new Compiler(syntaxTree);
            var result = compiler.Evaluate();

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

    private static void PrintDiagnostics(IEnumerable<Diagnostic> diagnostics, string input)
    {

        foreach (var diagnostic in diagnostics)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(diagnostic);
            Console.ResetColor();
            
            var prefix = input.Substring(0, diagnostic.Span.Start);
            var error = input.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
            var suffix = input.Substring(diagnostic.Span.End);
            
            Console.Write("    ");
            Console.Write(prefix);
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(error);
            Console.ResetColor();
            
            Console.Write(suffix);
            Console.WriteLine();
        }
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