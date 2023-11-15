using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace webenology.blazor.components.Helpers
{
    public static class HighlightHelper
    {
        private record HighlightObject
        {
            public int Index { get; init; }
            public int Length { get; init; }
            public string Color { get; init; }
        }

        public static string Highlight(this string item, string searchTerm, bool colorize = false, bool includeSubstitution = true)
        {
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(item))
                return item;

            try
            {
                var colorIndex = 0;
                var highlighted = new List<HighlightObject>();
                foreach (var s in searchTerm.Split(" "))
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    var searchString = new StringBuilder();

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

                    
                    var index = Regex.Matches(item, searchString.ToString(), RegexOptions.IgnoreCase);
                    highlighted.AddRange(index.Select(match => new HighlightObject
                    {
                        Index = match.Index,
                        Length = s.Length,
                        Color = SharedHelper.Colors[colorIndex % SharedHelper.Colors.Length]
                    }).ToList());

                    colorIndex++;
                }

                var results = new StringBuilder();
                var currentIndex = 0;

                foreach (var highlightObject in highlighted.GroupBy(x => x.Index).OrderBy(x => x.Key))
                {
                    if (currentIndex > highlightObject.Key)
                        continue;

                    var found = highlightObject.OrderByDescending(x => x.Length).First();
                    results.Append(item[currentIndex..found.Index]);
                    var lastIndex = found.Index + found.Length;
                    results.Append(
                        $"<mark{(colorize ? $" style='background-color:{found.Color}'" : "")}>{item[found.Index..lastIndex]}</mark>");
                    currentIndex = lastIndex;
                }

                if (currentIndex < item.Length)
                {
                    results.Append(item.Substring(currentIndex));
                }

                return Regex.Unescape(results.ToString().Replace("</mark><mark>", ""));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return item;
            }
        }
    }
}