using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.Helpers
{
    internal static class SharedHelper
    {
        public static readonly Dictionary<char, List<char>> SubstituteChars = new Dictionary<char, List<char>>
        {
            {'a', new List<char> {'o','a','e'}},
            {'A', new List<char> {'O','A','E'}},
            {'o', new List<char> {'o', 'a'}},
            {'O', new List<char> {'O', 'A'}},
            {'e', new List<char> {'i','e','a'}},
            {'E', new List<char> {'E','I','A'}},
            {'i', new List<char> {'e', 'i', 'y'}},
            {'I', new List<char> {'E', 'I', 'Y'}},
            {'y', new List<char> {'y', 'i', 'e'}},
            {'Y', new List<char> {'Y', 'I', 'E'}},
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
