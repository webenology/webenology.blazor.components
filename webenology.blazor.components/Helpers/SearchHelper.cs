using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace webenology.blazor.components.Helpers;

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
            foreach (var s in searchTerm.Split(" "))
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var searchString = new StringBuilder();

                foreach (var v in s)
                {
                    if (includeSubstitution && SharedHelper.SubstituteChars.TryGetValue(v, out var c))
                    {
                        var chars = string.Join("", c);
                        searchString.Append($"[{chars}]");
                    }
                    else
                    {
                        searchString.Append(Regex.Escape(v.ToString()));
                    }
                }

                if (first)
                    results = tIn.Where(x => Regex.IsMatch(expression(x), searchString.ToString(),
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(10)));

                results = results.Where(x => Regex.IsMatch(expression(x), searchString.ToString(),
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(10)));

                first = false;

                Debug.WriteLine("call ");
            }


            return results.AsEnumerable();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.ToString());
            return tIn;
        }
    }
}