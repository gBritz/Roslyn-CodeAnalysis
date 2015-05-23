using RoslynCodeAnalysis.Domain.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace RoslynCodeAnalysis.Domain
{
    public class SolutionSourceService
    {
        private readonly IFileSystem fileSystem;

        public SolutionSourceService(IFileSystem fileSystem)
        {
            if (fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            this.fileSystem = fileSystem;
        }

        public IEnumerable<FileSource> GetCSharpSourceCodesFrom(SolutionSource solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }

            return GetCSharpSourceCodesFromInternal(solution);
        }

        private IEnumerable<FileSource> GetCSharpSourceCodesFromInternal(SolutionSource solution)
        {
            var sources = fileSystem.Directory.EnumerateFiles(solution.Directory, "*.cs", SearchOption.AllDirectories);

            foreach (var source in sources)
            {
                yield return new FileSource
                {
                    Name = Path.GetFileName(source),
                    FullPath = source,
                    Source = fileSystem.File.ReadAllText(source)
                };
            }
        }
    }
}