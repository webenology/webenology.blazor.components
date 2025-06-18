using System.Text;
using System.Text.RegularExpressions;

namespace webenology.blazor.components.shared
{
    public static class HighlightHelper
    {
        public static string Highlight(this string item, string searchTerm, bool includeSubstitution = true, List<KeyValuePair<string, string>>? searchSubstitute = null)
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

                    var search = Regex.Replace(s, @"[^\d\w]", "");


                    if (!isFirst)
                        searchString.Append("|");

                    searchString.Append("(?:");


                    foreach (var v in search)
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
                    if (searchSubstitute != null)
                    {
                        searchSubstitute.Where(x => x.Key.Equals(search, StringComparison.OrdinalIgnoreCase)).ToList().ForEach(
                            x =>
                            {
                                searchString.Append($"|(?:{x.Value})");
                            });
                    }
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