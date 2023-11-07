using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components.shared;
public static class Levenshtein
{
    public static int[,] Calculate(string strA, string strB)
    {
        if (string.IsNullOrEmpty(strA))
            return new int[0,0];

        if (string.IsNullOrEmpty(strB))
            return new int[0, 0];

        var matrix = new int[strA.Length+1, strB.Length+1];

        matrix[0, 0] = 0;
        for (var i = 0; i < strA.Length; i++)
        {
            matrix[i+1, 0] = i + 1;
        }

        for (var i = 0; i < strB.Length; i++)
        {
            matrix[0, i+1] = i + 1;
        }

        var rowIndex = 0;
        for (var a = 0; a < strA.Length; a++)
        {
            rowIndex++;
            var colIndex = 0;
            for (var b = 0; b < strB.Length; b++)
            {
                colIndex++;
                var deletion = matrix[rowIndex, colIndex-1];
                var insertion = matrix[rowIndex-1, colIndex];
                var substitution = matrix[rowIndex-1, colIndex-1] + strA[a] != strB[b] ? 1 : 0;
                matrix[rowIndex, colIndex] = Math.Min(deletion, insertion);
                matrix[rowIndex, colIndex] = Math.Min(substitution, matrix[rowIndex, colIndex]);
                Console.WriteLine(matrix[a,b]);
            }
        }

        return matrix;
    }
}
