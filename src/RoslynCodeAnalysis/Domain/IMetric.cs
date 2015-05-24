using Microsoft.CodeAnalysis;
using System;

namespace RoslynCodeAnalysis.Domain
{
    public interface IMetric
    {
        Int32 Measure(SyntaxNode node);
    }
}