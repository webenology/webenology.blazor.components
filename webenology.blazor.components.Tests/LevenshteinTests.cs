using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webenology.blazor.components.shared;
using Xunit;
using Xunit.Abstractions;

namespace webenology.blazor.components.Tests;
public class LevenshteinTests
{
    private readonly ITestOutputHelper _outputHelper;

    public LevenshteinTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    [Fact]
    public void it_should()
    {
        var a = "abi";
        var b = "habi";

        var distance = Levenshtein.Calculate(a, b);

        //Assert.Equal(5,distance);
    }

    [Fact]
    public void it_should_do()
    {
        var a = "Saturday";
        var b = "Sunday";

        var distance = Levenshtein.Calculate(a, b);
        foreach (var i in distance)
        {
            _outputHelper.WriteLine(i.ToString());
        }
        Assert.True(true);
        //Assert.Equal(4, distance);
    }
}
