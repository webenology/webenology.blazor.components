using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.VisualBasic;

namespace webenology.blazor.components.shared;

public static class SearchHelper
{
    public class SearchHighlight<TValue>
    {
        public SearchHighlight(TValue val)
        {
            Result = val;
        }
        public TValue Result { get; set; } = default!;
        public List<string> HighlightedWords { get; set; } = new();
    }
    public static List<SearchHighlight<TValue>> LevenshteinSearch<TValue>(this IEnumerable<TValue> tIn, string searchTerm, Func<TValue, string> expression)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return (List<SearchHighlight<TValue>>)tIn.Select(x => new SearchHighlight<TValue>(x));

        var first = true;
        IEnumerable<TValue> results = null;
        foreach (var s in searchTerm.ToSearchBreakout(false))
        {
            var numOfTypos = s.Length < 4 ? 1 : s.Length < 8 ? 2 : 3;

            if (first)
            {
                results = tIn.Where(x => IsResult(expression(x), s, numOfTypos));
            }
            else
            {
                results = results.Where(x => IsResult(expression(x), s, numOfTypos));
            }

            first = false;
        }

        return new List<SearchHighlight<TValue>>();
        //return results.ToList() ?? new List<TValue>();
    }

    private static bool IsResult(string strIn, string searchTerm, int typos)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return true;

        var strSplit = strIn.Split(" ").Select(x => x.ToLower()).ToList();
        if (strSplit.Any(x => x.StartsWith("protein")))
        {
            foreach (var s in strSplit)
            {
                var distance = Fastenshtein.Levenshtein.Distance(s.Substring(0, Math.Min(s.Length, searchTerm.Length)), searchTerm.ToLower());
            }
            var d = "";
        }

        return strSplit.Any(str =>
            Fastenshtein.Levenshtein.Distance(str.Substring(0, Math.Min(str.Length, searchTerm.Length)), searchTerm.ToLower()) <= typos);

    }

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

    public static IEnumerable<string> ToSearchBreakout(this string searchTerm, bool includeSubstitution = true)
    {
        if (string.IsNullOrEmpty(searchTerm))
            yield break;

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
                    searchString.Append(Regex.Escape($"{v}"));
                }
            }

            yield return searchString.ToString();
        }
    }

}