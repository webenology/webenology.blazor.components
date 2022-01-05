using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Primitives;

namespace webenology.blazor.components.Helpers
{
    public static class InputMaskHelper
    {
        public static string Mask(this string itm, string mask)
        {
            var str = new StringBuilder();
            var currentIndex = 0;
            foreach (var t in mask)
            {
                var isIn = false;
                if (t == '#' || t == '*' || t == 'A')
                {
                    isIn = true;
                    if (itm.Length > currentIndex &&
                        ((t == '#' && IsNumber(itm[currentIndex]))
                         || t == 'A' && IsLetter(itm[currentIndex].ToString())
                         || t == '*'))
                    {
                        str.Append(itm[currentIndex]);
                    }
                    else
                    {
                        str.Append("_");
                    }
                    currentIndex++;
                }

                if (!isIn)
                {
                    str.Append(t);
                }
            }
            return str.ToString();
        }

        private static bool IsNumber(char n)
        {
            var str = "0123456789";

            return str.Contains(n);
        }

        private static bool IsLetter(string l)
        {
            var str = "abcdefghijklmnopqrstuvwxyz";

            return str.Contains(l.ToLower());
        }
    }

}
