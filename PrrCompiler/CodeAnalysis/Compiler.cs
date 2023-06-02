﻿using PrrCompiler.CodeAnalysis.Syntax;
using PrrCompiler.CodeAnalysis.Binding;

namespace PrrCompiler.CodeAnalysis;

public class Compiler
{
    public SyntaxTree SyntaxTree { get; }

    public Compiler(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }
    
    public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
    {
        var binder = new Binder(variables);
        var boundExpression = binder.BindExpression(SyntaxTree.Root);
        
        var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
        if (diagnostics.Any())
            return new EvaluationResult(diagnostics, null!);
        
        
        var evaluator = new Evaluator(boundExpression, variables);
        var value = evaluator.Evaluate();
        return new EvaluationResult(Array.Empty<Diagnostic>(), value);
    }
}