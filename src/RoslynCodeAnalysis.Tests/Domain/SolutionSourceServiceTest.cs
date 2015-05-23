using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynCodeAnalysis.Domain;
using RoslynCodeAnalysis.Domain.Analysis;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

namespace RoslynCodeAnalysis.Tests.Domain
{
    [TestClass]
    public class SolutionSourceServiceTest
    {
        [TestMethod]
        public void When_new_instance_with_null_arguments_should_be_throw_ArgumentNullException()
        {
            Action method = () => new SolutionSourceService(null);

            method.ShouldThrow<ArgumentNullException>().And
                .ParamName.Should().Be("fileSystem");
        }

        [TestMethod]
        public void When_get_csharp_codes_with_null_project_method_should_be_throw_ArgumentNullException()
        {
            var service = new SolutionSourceService(new MockFileSystem());

            Action method = () => service.GetCSharpSourceCodesFrom(null);

            method.ShouldThrow<ArgumentNullException>().And
                .ParamName.Should().Be("solution");
        }

        [TestMethod]
        public void Given_solution_folder_with_3_files_when_get_csharp_codes_result_should_be_1_files()
        {
            var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\img.png", new MockFileData(String.Empty) },
                { @"c:\Class1.cs", new MockFileData(GetSimpleSnippetClass()) },
                { @"c:\Class1.vb", new MockFileData(String.Empty) }
            });
            var service = new SolutionSourceService(fileSystemMock);

            var files = service.GetCSharpSourceCodesFrom(CreateSolution(@"c:\"));

            files.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(1).And
                .HaveElementAt(0, new FileSource
                {
                    Name = "Class1.cs",
                    FullPath = @"c:\Class1.cs",
                    Source = GetSimpleSnippetClass()
                });
        }

        [TestMethod]
        public void Given_empty_solution_folder_when_get_csharp_codes_result_should_be_empty()
        {
            var emptyFolder = @"c:\";

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(emptyFolder);

            var service = new SolutionSourceService(fileSystem);

            var files = service.GetCSharpSourceCodesFrom(CreateSolution(emptyFolder));

            files.Should()
                .NotBeNull().And
                .BeEmpty();
        }

        private static SolutionSource CreateSolution(String directory)
        {
            return new SolutionSource
            {
                Directory = directory,
                Name = String.Empty,
                SolutionFileName = String.Empty
            };
        }

        private static String GetSimpleSnippetClass()
        {
            return @"
namespace RoslynCodeAnalysis
{
    class Class1
    {
    }
}";
        }
    }
}