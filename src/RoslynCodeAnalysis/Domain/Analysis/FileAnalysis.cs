using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoslynCodeAnalysis.Domain.Analysis
{
    [DebuggerDisplay("{Name} Classes={Classes.Count}, Lines={LogicalCodeLinesCount}")]
    public class FileAnalysis
    {
        public FileAnalysis()
        {
            this.Classes = new List<ClassAnalysis>();
        }

        public String Name { get; set; }

        public Int32 ClassesCount { get; set; }

        public Int32 FieldsCount { get; set; }

        public Int32 PropertiesCount { get; set; }

        public Int32 FilesCount { get; set; }

        public Int32 LogicalCodeLinesCount { get; set; }

        public Int32 MethodsCount
        {
            get { return Classes.SelectMany(c => c.Methods).Count(); }
        }

        public ICollection<ClassAnalysis> Classes { get; private set; }

        public Boolean HasClass(string name)
        {
            return Classes.Any(c => c.Name == name);
        }  
    }
}