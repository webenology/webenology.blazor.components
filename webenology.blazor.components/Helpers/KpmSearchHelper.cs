using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.Helpers
{
    public static class KpmSearchHelper
    {
        public static List<T> Search<T>(this List<T> inList, string searchTerm, Func<T, string> expression)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return inList;

            var results = new List<T>();
            var firstIteration = true;
            foreach (var term in searchTerm.Split(" "))
            {
                if (string.IsNullOrEmpty(term.Trim()))
                    continue;

                results = firstIteration
                    ? inList.Where(x => Search(term, expression(x)).Count > 0).ToList()
                    : results.Where(x => Search(term, expression(x)).Count > 0).ToList();

                firstIteration = false;
            }

            return results;
        }

        private static readonly Dictionary<string, List<char>> _substituteChars = new Dictionary<string, List<char>>
        {
            {"a", new List<char> {'o','a','e'}},
            {"A", new List<char> {'O','A','E'}},
            {"o", new List<char> {'o', 'a'}},
            {"O", new List<char> {'O', 'A'}},
            {"e", new List<char> {'i','e','a'}},
            {"E", new List<char> {'E','I','A'}},
            {"i", new List<char> {'e', 'i', 'y'}},
            {"I", new List<char> {'E', 'I', 'Y'}},
            {"y", new List<char> {'y', 'i', 'e'}},
            {"Y", new List<char> {'Y', 'I', 'E'}},
        };



        public static List<int> Search(string pat, string txt, bool ignoreCase = true)
        {
            var results = new List<int>();
            if (pat.Length == 0)
                return results;

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
                if (_substituteChars.ContainsKey(pat[j].ToString()))
                {
                    if (_substituteChars[pat[j].ToString()].Any(x => x == txt[i]))
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
                    results.Add(i - j);
                    j = lps[j - 1];
                }// mismatch after j matches 
                else if (i < N && pat[j] != txt[i])
                {
                    if (_substituteChars.ContainsKey(pat[j].ToString()))
                    {
                        if (_substituteChars[pat[j].ToString()].Any(x => x == txt[i]))
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

            return results;
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
