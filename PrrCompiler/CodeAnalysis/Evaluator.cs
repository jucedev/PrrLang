using PrrCompiler.CodeAnalysis.Binding;

namespace PrrCompiler.CodeAnalysis;

internal class Evaluator
{
    private readonly BoundExpression _root;
    private readonly Dictionary<VariableSymbol, object> _variables;

    public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
    {
        _root = root;
        _variables = variables;
    }
    
    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }
    
    private object EvaluateExpression(BoundExpression node)
    {
        switch (node)
        {
            case BoundLiteralExpression n:
                return n.Value;
            
            case BoundVariableExpression v:
                return _variables[v.Variable];
            
            case BoundAssignmentExpression a:
                var value = EvaluateExpression(a.BoundExpression);
                _variables[a.Variable] = value;
                return value;
            
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
                    BoundBinaryOperatorType.Equals => Equals(left, right),
                    BoundBinaryOperatorType.NotEquals => !Equals(left, right),
                    BoundBinaryOperatorType.LogicalAnd => (bool) left && (bool) right,
                    BoundBinaryOperatorType.LogicalOr => (bool) left || (bool) right,
                    _ => throw new Exception($"Unexpected binary operator {b.Operator.Type}")
                };
            }
                
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
