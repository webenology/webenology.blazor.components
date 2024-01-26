namespace webenology.blazor.components.shared;

public static class LuhnHelper
{
    public static int GenerateCheckDigit(string number)
    {
        var multiplier = 3;
        var splitData = number.ToArray();
        var total = 0;
        for (var i = 0; i < splitData.Length; i++)
        {
            if (!char.IsDigit(splitData[i]))
                return -1;

            var num = int.Parse(splitData[i].ToString());
            if (i % 2 == 0)
            {
                num *= multiplier;
            }

            total += Reduce(num);
        }

        return 9 - (total % 10);
    }

    public static bool VerifyCheckDigit(string number)
    {
        var data = GenerateCheckDigit(number[..^1]);
        var lastDigit = number[^1..];
        return lastDigit.Equals(data.ToString());
    }

    private static int Reduce(int number)
    {
        if (number > 20)
            return (number - 20) + 2;
        if (number > 10)
            return (number - 10) + 1;
        
        if (number == 10)
            return 1;

        return number;
    }
}