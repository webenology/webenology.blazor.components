using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Diffing.Strategies.ElementStrategies;

namespace webenology.blazor.components.Helpers;

public static class SearchHelper
{
    public static IEnumerable<TValue> Search<TValue>(this IEnumerable<TValue> tIn, string searchTerm,
        Func<TValue, string> expression)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return tIn;

        try
        {
            var results = new List<TValue>();
            var first = true;
            foreach (var s in searchTerm.Split(" "))
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var searchString = new StringBuilder();

                foreach (var v in s)
                {
                    if (SharedHelper.SubstituteChars.TryGetValue(v, out var c))
                    {
                        var chars = string.Join("", c);
                        searchString.Append($"[{chars}]");
                    }
                    else
                    {
                        searchString.Append(v);
                    }
                }

                var r = new Regex(
                    $"{Regex.Escape(s)}{(string.IsNullOrEmpty(searchString.ToString()) ? "" : $"|{Regex.Escape(searchString.ToString())}")}",
                    RegexOptions.IgnoreCase);

                results = first
                    ? tIn.Where(x => r.IsMatch(expression(x))).ToList()
                    : results.Where(x => r.IsMatch(expression(x))).ToList();

                first = false;
                Debug.WriteLine("call ");
            }

            return results;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.ToString());
            return tIn;
        }
    }
}