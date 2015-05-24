using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace RoslynCodeAnalysis.Domain.Metrics
{
    public class ConditionsCounterMetric : CSharpSyntaxVisitor, IMetric
    {
        private int countConditions = 0;

        public Int32 Measure(SyntaxNode node)
        {
            countConditions = 0;
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (IsLogicalExpression(node))
            {
                Visit(node.Left);
            }

            countConditions++;
        }

        private static Boolean IsLogicalExpression(SyntaxNode node)
        {
            var kind = node.Kind();

            return kind == SyntaxKind.LogicalAndExpression ||
                   kind == SyntaxKind.LogicalOrExpression ||
                   kind == SyntaxKind.LogicalNotExpression;
        }
    }
}