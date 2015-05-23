using System;
using System.Diagnostics;

namespace RoslynCodeAnalysis.Domain.Analysis
{
    [DebuggerDisplay("{Name} CC={CyclomaticComplexity}")]
    public class MethodAnalysis
    {
        public String Name { get; set; }
        public Int32 CyclomaticComplexity { get; set; }
    }
}