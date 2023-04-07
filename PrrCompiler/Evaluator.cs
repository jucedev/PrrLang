using System.Runtime.InteropServices;

namespace PrrCompiler;

public class Evaluator
{
    private readonly Expression _root;
    public Evaluator(Expression root)
    {
        _root = root;
    }
    
    public int Evaluate()
    {
        return EvaluateExpression(_root);
    }
    
    private int EvaluateExpression(Expression node)
    {
        switch (node)
        {
            case NumberExpression n:
                if (int.TryParse(n.NumberToken.Value as string, out var result))
                    return result;
                
                throw new Exception("Unexpected null");
            
            case BinaryExpression b:
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                return b.Operator.Type switch
                {
                    TokenType.Plus => left + right,
                    TokenType.Minus => left - right,
                    TokenType.Star => left * right,
                    TokenType.ForwardSlash => left / right,
                    _ => throw new Exception($"Unexpected binary operator {b.Operator.Type}")
                };
            }
            default:
                throw new Exception($"Unexpected node {node.Type}");
        }
    }
}

