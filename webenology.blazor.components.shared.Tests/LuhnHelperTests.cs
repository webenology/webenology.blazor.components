using JetBrains.dotMemoryUnit;
using Microsoft.VisualBasic;

namespace webenology.blazor.components.shared.Tests;

public class LuhnHelperTests
{
    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void it_should_generate_pin()
    {
        var number = "1234";
        var data = LuhnHelper.GenerateCheckDigit(number);

        Assert.That(1, Is.EqualTo(data));
    }
    
    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void it_should_generate_pin_2()
    {
        var number = "24";
        var data = LuhnHelper.GenerateCheckDigit(number);

        Assert.That(9, Is.EqualTo(data));
    }

    [Test]
    public void it_should_generate_a_two_factor_number()
    {
        var saltDt = new DateTime(2001, 1, 2, 4, 5, 45, 390, 433);
        var year = DateTime.Now.Year;
        var month = DateTime.Now.Month;
        var day = DateTime.Now.Day;
        var hour = DateTime.Now.Hour;
        var minute = DateTime.Now.Minute;
        minute -= (minute % 10);
        var currentDt = new DateTime(year, month, day, hour, minute, 00);
        var salt = saltDt.Ticks.ToString().ToArray()[^5..];
        var current = currentDt.Ticks.ToString().ToArray()[6..^7];

        var newResult = long.Parse(salt) + long.Parse(current);

        var checkDigit = LuhnHelper.GenerateCheckDigit(newResult.ToString());
        Console.WriteLine($"{newResult}{checkDigit}");
    }

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void it_should_generate_number()
    {
        var number = "1789372997";
        var data = LuhnHelper.GenerateCheckDigit(number);

        Assert.That(7, Is.EqualTo(data));
    }

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void it_should_verify_check_digit()
    {
        var number = "17893729977";
        var data = LuhnHelper.VerifyCheckDigit(number);

        Assert.IsTrue(data);
    }

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void it_should_verify_check_digit_not_pass()
    {
        var number = "17893729979";
        var data = LuhnHelper.VerifyCheckDigit(number);

        Assert.IsFalse(data);
    }
}