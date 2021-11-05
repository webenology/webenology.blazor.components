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
        }
        private static readonly Dictionary<char, List<char>> _substituteChars = new Dictionary<char, List<char>>
        {
            {'a', new List<char> {'o','a','e'}},
            {'A', new List<char> {'O','A','E'}},
            {'o', new List<char> {'o', 'a'}},
            {'O', new List<char> {'O', 'A'}},
            {'e', new List<char> {'i','e','a'}},
            {'E', new List<char> {'E','I','A'}},
            {'i', new List<char> {'e', 'i', 'y'}},
            {'I', new List<char> {'E', 'I', 'Y'}},
            {'y', new List<char> {'y', 'i', 'e'}},
            {'Y', new List<char> {'Y', 'I', 'E'}},
        };

        public static string Highlight(this string item, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return item;

            var highlighted = new List<HighlightObject>();

            foreach (var s in searchTerm.Split(" "))
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var searchString = new StringBuilder();

                foreach (var v in s)
                {
                    if (_substituteChars.ContainsKey(v))
                    {
                        var chars = string.Join("", _substituteChars[v]);
                        searchString.Append($"[{chars}]");
                    }
                    else
                    {
                        searchString.Append(v);
                    }
                }

                var r = new Regex($"{s}|{searchString}", RegexOptions.IgnoreCase);

                var index = r.Matches(item);
                foreach (Match match in index)
                {
                    highlighted.Add(new HighlightObject { Index = match.Index, Length = s.Length });

                }
            }

            var results = new StringBuilder();
            for (int i = 0; i < item.Length; i++)
            {
                var found = highlighted.Where(x => x.Index == i).ToList();
                if (found.Any())
                {
                    var first = found.OrderByDescending(x => x.Length).First();
                    results.Append($"<mark>{item.Substring(i, first.Length)}</mark>");
                    i += first.Length - 1;
                }
                else
                {
                    results.Append(item[i]);
                }
            }

            return results.ToString().Replace("</mark><mark>", "");
        }
    }
}
