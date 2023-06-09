﻿namespace PrrCompiler.CodeAnalysis.Binding;

internal class BoundUnaryExpression : BoundExpression
{
    public override BoundNodeType Type => BoundNodeType.UnaryExpression;
    public override Type ExpressionType => Operator.ResultType!;
    public BoundUnaryOperator Operator { get; }
    public BoundExpression Operand { get; }

    public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand)
    {
        Operator = @operator;
        Operand = operand;
    }
}