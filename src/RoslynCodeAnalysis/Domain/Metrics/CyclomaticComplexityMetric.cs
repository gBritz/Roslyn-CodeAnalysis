using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace RoslynCodeAnalysis.Domain.Metrics
{
    /// <summary>
    /// CC of a method is 1 + {the number of following expressions found in the body of the method}:
    /// if | while | for | foreach | case | default | continue | goto | && | || | catch | ternary operator ?: | ??
    /// Following expressions are not counted for CC computation:
    /// else | do | switch | try | using | throw | finally | return | object creation | method call | field access 
    /// </summary>
    public class CyclomaticComplexityMetric : SyntaxWalker, IMetric
    {
        private int cc;
        private readonly IMetric conditions;

        public CyclomaticComplexityMetric(IMetric conditions)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            if (conditions == null)
                throw new ArgumentNullException("conditions");

            this.conditions = conditions;
        }

        public Int32 Measure(SyntaxNode token)
        {
            cc = 1;
            Visit(token);
            return cc;
        }

        protected override void VisitToken(SyntaxToken token)
        {
            base.VisitToken(token);

            var kind = token.Kind();

            switch (kind)
            {
                case SyntaxKind.WhileKeyword:
                case SyntaxKind.IfKeyword:
                    var expressions = token.Parent
                        .ChildNodes()
                        .OfType<BinaryExpressionSyntax>();

                    foreach (var expression in expressions)
                    {
                        cc += conditions.Measure(expression);
                    }

                    break;

                case SyntaxKind.ForKeyword:
                case SyntaxKind.ForEachKeyword:
                case SyntaxKind.CaseKeyword:
                case SyntaxKind.DefaultKeyword:
                //case SyntaxKind.ContinueKeyword:
                //case SyntaxKind.GotoKeyword:
                //case SyntaxKind.AndAssignmentExpression: //case &&
                //case SyntaxKind.OrAssignmentExpression: //case ||
                case SyntaxKind.CatchKeyword:
                    //case ?:
                    //case ??
                    cc++;
                    break;
            }
        }
    }
}