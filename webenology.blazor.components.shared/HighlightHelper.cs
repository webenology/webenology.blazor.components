using System;
using System.Buffers;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace webenology.blazor.components.shared
{
    public static class HighlightHelper
    {
        public static string Highlight(this string item, string searchTerm, bool includeSubstitution = true)
        {
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(item))
                return item;

            try
            {
                var isFirst = true;
                var searchString = new StringBuilder();

                foreach (var s in searchTerm.Split(" "))
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    if (!isFirst)
                        searchString.Append("|");

                    searchString.Append("(?:");
                    foreach (var v in s)
                    {
                        if (includeSubstitution && SharedHelper.SubstituteChars.TryGetValue(v, out var c))
                        {
                            searchString.Append($"[{c}]");
                        }
                        else
                        {
                            searchString.Append(Regex.Escape(v.ToString()));
                        }
                    }

                    searchString.Append(")");
                    isFirst = false;
                }


                return Regex.Replace(item, searchString.ToString(), "<mark>$&</mark>",
                    RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return item;
            }
        }
    }
}