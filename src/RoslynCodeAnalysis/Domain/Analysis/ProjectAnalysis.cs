using System;
using System.Collections.Generic;
using System.Linq;

namespace RoslynCodeAnalysis.Domain.Analysis
{
    public class ProjectAnalysis
    {
        public String AssemblyName { get; set; }

        public String DefaultNamespace { get; set; }

        public Int32 ClassesCount
        {
            get { return Files.Sum(f => f.ClassesCount); }
        }

        public Int32 MethodsCount
        {
            get { return Files.Sum(f => f.MethodsCount); }
        }

        public Int32 FieldsCount
        {
            get { return Files.Sum(f => f.FieldsCount); }
        }

        public Int32 PropertiesCount
        {
            get { return Files.Sum(f => f.PropertiesCount); }
        }

        public Int32 FilesCount
        {
            get { return Files.Count(); }
        }

        public Int32 LogicalCodeLinesCount
        {
            get { return Files.Sum(f => f.LogicalCodeLinesCount); }
        }

        public ICollection<FileAnalysis> Files { get; set; }
    }
}