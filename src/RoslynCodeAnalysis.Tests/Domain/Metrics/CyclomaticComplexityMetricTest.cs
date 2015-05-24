using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynCodeAnalysis.Domain.Metrics;
using System;

namespace RoslynCodeAnalysis.Tests.Domain.Metrics
{
    [TestClass]
    public class CyclomaticComplexityMetricTest
    {
        private CyclomaticComplexityMetric metric;

        [TestInitialize]
        public void CreateMethodInstance()
        {
            metric = new CyclomaticComplexityMetric(new ConditionsCounterMetric());
        }

        [TestMethod]
        public void Given_method_with_if_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public void CalculoMaluco(int i)
{
    var res = 5;
    if (i > 10)
        i += 15;
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_while_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int WhileMethod(int i)
{
    while (i > 20)
        i -= 10;
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_for_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int ForMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_foreach_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int ForeachMethod(int[] arr)
{
    var res = 0;
    foreach (var v in arr)
    {
        res += v;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_switch_case_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int SwitchCaseMethod(int i)
{
    switch (i)
    {
        case 20:
            i += 10;
            break;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_for_continue_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int ForContinueMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
        continue;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_foreach_continue_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int ForeachContinueMethod(int[] arr)
{
    var res = 0;
    foreach (var v in arr)
    {
        res += v;
        continue;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_goto_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int GotoMethod(int i)
{
    goto teste;

teste:
    i = 10;

    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(1);
        }

        [TestMethod]
        public void Given_method_with_try_catch_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int TryCatchMethod(int i, int j)
{
    try
    {
        i = i / j;
    }
    catch (Exception ex)
    {
        i = 0;
    }

    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_ternary_operator_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int TernaryOperatorMethod(int i)
{
    i = i > 20 ? 10 : 20;
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_null_coalescing_operator_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int NullCoalescingOperatorMethod(object i)
{
    i = i ?? 10;
    return Convert.ToInt32(i);
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        private static SyntaxNode ParseMethodBlock(String code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            return tree.GetCompilationUnitRoot();
        }
    }
}