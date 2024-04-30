namespace webenology.blazor.components.shared
{
    internal static class SharedHelper
    {
        public static readonly Dictionary<char, string> SubstituteChars = new Dictionary<char, string>(capacity: 10)
        {
            {'a', "oae"},
            {'A', "OAE"},
            {'o', "oa"},
            {'O', "OA"},
            {'e', "iea"},
            {'E', "EIA"},
            {'i', "eiy"},
            {'I', "EIY"},
            {'y', "yie"},
            {'Y', "YIE"},
            {'s', "'s"}
        };

        public static readonly string[] Colors = new[]
        {
            "#CE517A",
            "#4EED1A",
            "#4DD783",
            "#B912E2",
            "#58B72A",
            "#77ACDC",
            "#809134",
            "#4C78EB",
            "#B0E347"
        };
    }
}
