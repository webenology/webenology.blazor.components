using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace webenology.blazor.components.shared
{
    public static class HighlightHelper
    {
        public static string Highlight(this string item, string searchTerm, bool includeSubstitution = true, List<KeyValuePair<string, string>>? searchSubstitute = null)
        {
            if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(searchTerm))
                return item;

            try
            {
                var isFirst = true;
                var searchString = new StringBuilder();

                foreach (var s in searchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries).OrderByDescending(x => x.Length))
                {
                    if (!isFirst)
                        searchString.Append("|");

                    searchString.Append("(?:");


                    foreach (var v in s)
                    {
                        if (includeSubstitution && SharedHelper.SubstituteChars.TryGetValue(v, out var c))
                        {
                            searchString.Append("[").Append(Regex.Escape(c)).Append("]");
                        }
                        else
                        {
                            searchString.Append(Regex.Escape(v.ToString()));
                        }
                    }

                    searchString.Append(")");

                    isFirst = false;
                }

                var rg = new Regex(searchString.ToString(), RegexOptions.IgnoreCase);
                var matches = rg.Replace(item, "<mark>$&</mark>");
                return matches;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return item;
            }
        }
    }
}