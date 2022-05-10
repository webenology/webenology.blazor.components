using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.Helpers
{
    public static class KpmSearchHelper
    {
        public static IEnumerable<T> Search<T>(this IEnumerable<T> inList, string searchTerm, Func<T, string> expression)
        {
            if (string.IsNullOrEmpty(searchTerm.Trim()))
                return inList;

            IEnumerable<T> results = null;
            var firstIteration = true;
            foreach (var term in searchTerm.Split(" "))
            {
                if (string.IsNullOrEmpty(term.Trim()))
                    continue;

                results = firstIteration
                    ? inList.Where(x => Search(term, expression(x)))
                    : results.Where(x => Search(term, expression(x)));

                firstIteration = false;
            }

            return results.Any() ? results : new List<T>();
        }
        
        private static bool Search(string pat, string txt, bool ignoreCase = true)
        {
            if (pat.Length == 0)
                return false;

            if (ignoreCase)
            {
                pat = pat.ToLower();
                txt = txt.ToLower();
            }

            int M = pat.Length;
            int N = txt.Length;

            // create lps[] that will hold the longest 
            // prefix suffix values for pattern 
            int[] lps = new int[M];
            int j = 0; // index for pat[] 

            // Preprocess the pattern (calculate lps[] 
            // array) 
            lPSArray(pat, M, lps);

            int i = 0; // index for txt[] 
            while (i < N)
            {
                if (SharedHelper.SubstituteChars.ContainsKey(pat[j]))
                {
                    if (SharedHelper.SubstituteChars[pat[j]].Any(x => x == txt[i]))
                    {
                        j++;
                        i++;
                    }
                }
                else if (pat[j] == txt[i])
                {
                    j++;
                    i++;
                }

                if (j == M)
                {
                    return true;
                    //j = lps[j - 1];
                    //results.Add(i - j);
                }// mismatch after j matches 

                if (i < N && pat[j] != txt[i])
                {
                    if (SharedHelper.SubstituteChars.ContainsKey(pat[j]))
                    {
                        if (SharedHelper.SubstituteChars[pat[j]].Any(x => x == txt[i]))
                        {
                            continue;
                        }
                    }
                    // Do not match lps[0..lps[j-1]] characters, 
                    // they will match anyway 
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i += 1;
                }
            }

            return false;
        }

        private static void lPSArray(string pat, int M, int[] lps)
        {
            // length of the previous longest prefix suffix 
            int len = 0;
            int i = 1;
            lps[0] = 0; // lps[0] is always 0 

            // the loop calculates lps[i] for i = 1 to M-1 
            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len]) 
                {
                    // This is tricky. Consider the example. 
                    // AAACAAAA and i = 7. The idea is similar 
                    // to search step. 
                    if (len != 0)
                    {
                        len = lps[len - 1];

                        // Also, note that we do not increment 
                        // i here 
                    }
                    else // if (len == 0) 
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }
    }
}
