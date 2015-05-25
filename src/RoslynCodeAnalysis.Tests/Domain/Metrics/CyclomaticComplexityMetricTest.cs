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

        #region Simple basic

        [TestMethod]
        public void Given_method_with_if_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public void IfMethod(int i)
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
        public void Given_method_with_goto_of_1_condition_when_metric_measure_should_be_1()
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

        #endregion

        #region Two consecutive statements

        [TestMethod]
        public void Given_method_with_2_ifs_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public void IfMethod(int i)
{
    var res = 5;
    if (i > 10)
        i += 15;
    if (i > 20)
        i += 5;
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_while_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int WhileMethod(int i)
{
    while (i > 20)
        i -= 10;
    while (i > 10)
        i -= 5;
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_for_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
    }
    for (var e = 0; e < i+5; e++)
    {
        i -= 5;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_foreach_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForeachMethod(int[] arr)
{
    var res = 0;
    foreach (var v in arr)
    {
        res += v;
    }
    foreach (var v in arr)
    {
        res += v;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_switch_2_cases_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int SwitchCaseMethod(int i)
{
    switch (i)
    {
        case 20:
            i += 10;
            break;
        case 50:
            i += 5;
            break;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_switch_cases_of_1_condition_when_metric_measure_should_be_3()
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

    switch (i)
    {
        case 50:
            i += 15;
            break;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_for_continue_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForContinueMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
        continue;
    }
    for (var j = 0; j < i; j++)
    {
        i += 10;
        continue;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_foreach_continue_of_1_condition_when_metric_measure_should_be_3()
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

    foreach (var v in arr)
    {
        res += v;
        continue;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_goto_of_1_condition_when_metric_measure_should_be_1()
        {
            var code = @"
public int GotoMethod(int i)
{
    goto teste;

teste:
    i = 10;
    goto teste2;

teste2:
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(1);
        }

        [TestMethod]
        public void Given_method_with_try_2_catch_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int TryCatchMethod(int i, int j)
{
    try
    {
        i = i / j;
    }
    catch (ArithmeticException ex)
    {
        i = 0;
    }
    catch (Exception ex)
    {
        i = 0;
    }

    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        #endregion

        #region Two nested statements

        [TestMethod]
        public void Given_method_with_2_nested_ifs_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public void IfMethod(int i)
{
    var res = 5;
    if (i > 10)
    {
        i += 15;
        if (i > 20)
            i += 5;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_while_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int WhileMethod(int i)
{
    while (i > 20)
    {
        i -= 10;
        while (i > 10)
            i -= 5;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_for_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
        for (var e = 0; e < i+5; e++)
        {
            i -= 5;
        }
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_foreach_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForeachMethod(int[] arr)
{
    var res = 0;
    foreach (var v in arr)
    {
        res += v;
        foreach (var j in arr)
        {
            res += j;
        }
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_switch_cases_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int SwitchCaseMethod(int i)
{
    switch (i)
    {
        case 20:
        {
            i += 10;

            switch (i)
            {
                case 50:
                    i += 15;
                    break;
            }

            break;
        }
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_for_continue_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForContinueMethod(int i)
{
    for (var j = 0; j < i; j++)
    {
        i += 10;
        for (var e = 0; e < i; e++)
        {
            i += 10;
            continue;
        }
        continue;
    }
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_foreach_continue_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int ForeachContinueMethod(int[] arr, int[] arr2)
{
    var res = 0;
    foreach (var v in arr)
    {
        res += v;
        foreach (var j in arr2)
        {
            res += j;
            continue;
        }
        continue;
    }
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_try_2_nested_catch_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int TryCatchMethod(int i, int j)
{
    try
    {
        try
        {
            i = i / j;
        }
        catch (ArithmeticException ex)
        {
            i = i / j;
        }        
    }
    catch (Exception ex)
    {
        i = 0;
    }

    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        #endregion

        #region Operators

        #region Ternary (?:) operator

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
        public void Given_method_with_2_ternary_operator_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int TernaryOperatorMethod(int i)
{
    i = i > 20 ? 10 : 20;
    i = i < 10 ? 0 : 20;
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_ternary_operator_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int TernaryOperatorMethod(int i)
{
    i = i > 20 ? 10 : i < 10 ? 20 : 50;
    return i;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        #endregion

        #region null coalescing (??) operator

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

        [TestMethod]
        public void Given_method_with_2_null_coalescing_operator_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int NullCoalescingOperatorMethod(object i)
{
    i = i ?? null;
    i = i ?? 50;
    return Convert.ToInt32(i);
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        [TestMethod]
        public void Given_method_with_2_nested_null_coalescing_operator_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public int NullCoalescingOperatorMethod(object i, object e)
{
    i = i ?? e ?? 50;
    return Convert.ToInt32(i);
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        #endregion

        #region Mixed

        [TestMethod]
        public void Given_method_with_ternary_and_null_coalescing_operator_of_1_condition_when_metric_measure_should_be_2()
        {
            var code = @"
public int TernaryOperatorMethod(object i)
{
    var res = Convert.ToInt32(i) > 10 ? 10 : (int?)null ?? 50;
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(2);
        }

        [TestMethod]
        public void Given_method_with_ternary_and_2_null_coalescing_operator_of_1_condition_when_metric_measure_should_be_3()
        {
            var code = @"
public Object TernaryOperatorMethod(int i, object j)
{
    Object res = i > 10 ? 10 : null ?? j ?? 50;
    return res;
}";
            var root = ParseMethodBlock(code);

            metric.Measure(root).Should().Be(3);
        }

        #endregion

        #endregion

        private static SyntaxNode ParseMethodBlock(String code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            return tree.GetCompilationUnitRoot();
        }
    }
}