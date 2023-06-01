using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Binding;

namespace PrrCompiler.CodeAnalysis;

public class Compiler
{
    public SyntaxTree SyntaxTree { get; }

    public Compiler(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }
    
    public EvaluationResult Evaluate()
    {
        var binder = new Binder();
        var boundExpression = binder.BindExpression(SyntaxTree.Root);
        
        var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
        if (diagnostics.Any())
            return new EvaluationResult(diagnostics, null!);
        
        
        var evaluator = new Evaluator(boundExpression);
        var value = evaluator.Evaluate();
        return new EvaluationResult(Array.Empty<string>(), value);
    }
}