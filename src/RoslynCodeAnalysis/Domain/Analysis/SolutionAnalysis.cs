using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoslynCodeAnalysis.Domain.Analysis
{
    [DebuggerDisplay("{Name} TotalLines={LogicalCodeLinesCount}")]
    public class SolutionAnalysis
    {
        public SolutionAnalysis()
        {
            Projects = new List<ProjectAnalysis>();
        }

        public String Name { get; set; }

        public Int32 LogicalCodeLinesCount
        {
            get { return Projects.Sum(p => p.LogicalCodeLinesCount); }
        }

        public ICollection<ProjectAnalysis> Projects { get; private set; }
    }
}