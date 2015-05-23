using System;
using System.Diagnostics;

namespace RoslynCodeAnalysis.Domain
{
    [DebuggerDisplay("{Name}")]
    public struct FileSource
    {
        public String Name { get; set; }

        public String FullPath { get; set; }

        public String Source { get; set; }
    }
}