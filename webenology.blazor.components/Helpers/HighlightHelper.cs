using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace webenology.blazor.components.Helpers
{
    public static class HighlightHelper
    {
        private record HighlightObject
        {
            public int Index { get; set; }
            public int Length { get; set; }
            public string Color { get; set; }
        }

        public static string Highlight(this string item, string searchTerm, bool colorize = false)
        {
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(item))
                return item;

            try
            {
                var highlighted = new List<HighlightObject>();
                var colorIndex = 0;

                foreach (var s in searchTerm.Split(" "))
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    var searchString = new StringBuilder();

                    foreach (var v in s)
                    {
                        if (SharedHelper.SubstituteChars.ContainsKey(v))
                        {
                            var chars = string.Join("", SharedHelper.SubstituteChars[v]);
                            searchString.Append($"[{chars}]");
                        }
                        else
                        {
                            searchString.Append(v);
                        }
                    }

                    var pattern =
                        $"{Regex.Escape(s)}{(string.IsNullOrEmpty(searchString.ToString()) ? "" : $"|{Regex.Escape(searchString.ToString())}")}";

                    var index = Regex.Matches(item, pattern, RegexOptions.IgnoreCase);

                    foreach (Match match in index)
                    {
                        highlighted.Add(new HighlightObject
                        {
                            Index = match.Index, Length = s.Length,
                            Color = SharedHelper.Colors[colorIndex % SharedHelper.Colors.Length]
                        });
                    }

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