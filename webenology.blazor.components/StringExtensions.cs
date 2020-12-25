using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    internal static class StringExtensions
    {
        public static string IfNullOrEmpty(this string str, string other)
        {
            if (string.IsNullOrEmpty(str))
                return other;

            return str;
        }
    }
}
