using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoslynCodeAnalysis.Domain.Analysis
{
    [DebuggerDisplay("{Name} Methods={Methods.Count}, CC={CyclomaticComplexity}")]
    public class ClassAnalysis
    {
        public ClassAnalysis()
        {
            Methods = new List<MethodAnalysis>();
        }

        public String Name { get; set; }

        public Int32 CyclomaticComplexity
        {
            get { return Methods.Sum(m => m.CyclomaticComplexity); }
        }

        public ICollection<MethodAnalysis> Methods { get; private set; }
    }
}