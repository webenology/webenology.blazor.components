using System.Text;
using System.Text.RegularExpressions;

namespace webenology.blazor.components.shared;

public static class SearchHelper
{
    /// <summary>
    /// Searched through a list and returns the items it finds
    /// </summary>
    /// <param name="tIn">Your list</param>
    /// <param name="searchTerm">The term you are searching for</param>
    /// <param name="expression">The properties in the list that make up the search parameters</param>
    /// <param name="includeSubstitution">By default: true, will substitute simple characters, for example: a with e,i,o</param>
    /// <typeparam name="TValue">List of items</typeparam>
    /// <returns>The filtered results</returns>
    public static IEnumerable<TValue> Search<TValue>(this IEnumerable<TValue> tIn, string searchTerm,
        Func<TValue, string> expression, bool includeSubstitution = true)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return tIn;

        try
        {
            IEnumerable<TValue> results = null;
            var first = true;
            foreach (var s in searchTerm.ToSearchBreakout(includeSubstitution))
            {
                try
                {
                    if (first)
                        results = tIn.Where(x => Regex.IsMatch(expression(x), s,
                            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)));

                    results = results.Where(x => Regex.IsMatch(expression(x), s,
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)));
                }
                catch (Exception)
                {
                    results = tIn;
                }

                first = false;
            }

            return results?.AsEnumerable() ?? tIn;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.ToString());
            return tIn;
        }
    }

    public static string[] ToSearchBreakout(this string searchTerm, bool includeSubstitution = true)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return null;
        
        var splitData = searchTerm.Split(" ");
        var results = new List<string>();
        foreach (var s in splitData)
        {
            if (string.IsNullOrEmpty(s))
                continue;

            var searchString = new StringBuilder(searchTerm.Length * 2);

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

            results.Add(searchString.ToString());
        }

        return results.ToArray();
    }
}