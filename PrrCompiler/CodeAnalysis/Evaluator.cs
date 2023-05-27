using PrrCompiler.CodeAnalysis.Binding;

namespace PrrCompiler.CodeAnalysis;

internal class Evaluator
{
    private readonly BoundExpression _root;
    public Evaluator(BoundExpression root)
    {
        _root = root;
    }
    
    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }
    
    private static object EvaluateExpression(BoundExpression node)
    {
        switch (node)
        {
            case BoundBinaryExpression b:
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                return b.Operator.Type switch
                {
                    BoundBinaryOperatorType.Addition => (int) left + (int) right,
                    BoundBinaryOperatorType.Subtraction => (int) left - (int) right,
                    BoundBinaryOperatorType.Multiplication => (int) left * (int) right,
                    BoundBinaryOperatorType.Division => (int) left / (int) right,
                    BoundBinaryOperatorType.LogicalAnd => (bool) left && (bool) right,
                    BoundBinaryOperatorType.LogicalOr => (bool) left || (bool) right,
                    _ => throw new Exception($"Unexpected binary operator {b.Operator.Type}")
                };
            }
            
            case BoundLiteralExpression n:
                return n.Value;
                
            case BoundUnaryExpression u:
                var operand = EvaluateExpression(u.Operand);
                return u.Operator.Type switch
                {
                    BoundUnaryOperatorType.Identity => (int) operand,
                    BoundUnaryOperatorType.Negation => -(int) operand,
                    BoundUnaryOperatorType.LogicalNegation => !(bool) operand,
                    _ => throw new Exception($"Unexpected unary operator {u.Operator.Type}")
                };
            
            default:
                throw new Exception($"Unexpected node {node.Type}");
        }
    }
}
