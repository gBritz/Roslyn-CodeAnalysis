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

                case SyntaxKind.EqualsToken:
                    // Identificar se é ?: ou ??
                    // TODO: Create parsers to ternary operations
                    if (token.Parent is AssignmentExpressionSyntax || token.Parent is EqualsValueClauseSyntax)
                    {
                        var str = token.Parent.GetText().ToString();
                        if (str.Contains("??"))
                        {
                            var countNull = CountSubIn(str, "??");
                            cc += (Int32)countNull;
                        }
                        else if (str.Contains("?") && str.Contains(":"))
                        {
                            var countTernary = str.Count(s => s == '?' || s == ':') / 2;
                            cc += (Int32)countTernary;
                        }
                    }
                    break;

                /*
SyntaxKind.QuestionToken === ?
SyntaxKind.ColonToken === :
SyntaxKind.SemicolonToken === ;
                */
            }
        }

        //TODO: refactor, using regex.
        private static Int32 CountSubIn(String expr, String sub)
        {
            var count = 0;
            var lastIndex = 0;
            while ((lastIndex = expr.IndexOf(sub, lastIndex)) > -1)
            {
                count += 1;
                lastIndex += 1;
            }
            return count;
        }
    }
}