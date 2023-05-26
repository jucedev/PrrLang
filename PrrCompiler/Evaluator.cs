using PrrCompiler.Expressions;

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
    
    private static int EvaluateExpression(Expression node)
    {
        switch (node)
        {
            case LiteralExpression n:
                if (int.TryParse(n.LiteralToken.Value as string, out var result))
                    return result;
                
                throw new Exception("Number expected");
            
            case UnaryExpression u:
                 var operand = EvaluateExpression(u.Operand);
                    return u.Operator switch
                    {
                        {Type: TokenType.Plus} => operand,
                        {Type: TokenType.Minus} => -operand,
                        _ => throw new Exception($"Unexpected unary operator {u.Operator.Type}")
                    };
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
            case ParenthesisExpression p:
                return EvaluateExpression(p.ParenthesizedExpression);
            default:
                throw new Exception($"Unexpected node {node.Type}");
        }
    }
}

