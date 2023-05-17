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
    public static IEnumerable<TValue> Search<TValue>(this IEnumerable<TValue> tIn, string searchTerm,
        Func<TValue, string> expression)
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

                var pattern =
                    $"{Regex.Escape(s)}{(string.IsNullOrEmpty(searchString.ToString()) ? "" : $"|{Regex.Escape(searchString.ToString())}")}";

                if (first)
                    results = tIn.Where(x => Regex.IsMatch(expression(x), pattern, RegexOptions.IgnoreCase));

                results = results.Where(x => Regex.IsMatch(expression(x), pattern, RegexOptions.IgnoreCase));
                
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