using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    internal static class SearchHelper
    {
        public static string HighlightSearchResult(string searchTerm, string item)
        {
            if (item == null)
            {
                return "";
            }
            var s = searchTerm ?? string.Empty;
            var highlightStartIndex = item.IndexOf(s, StringComparison.InvariantCultureIgnoreCase);
            if (highlightStartIndex < 0)
                return item;

            var results = item.Substring(0, highlightStartIndex);
            results += $"<mark><b>{item.Substring(highlightStartIndex, s.Length)}</b></mark>";
            results += item.Substring(highlightStartIndex + s.Length);

            return results;
        }
    }
}
