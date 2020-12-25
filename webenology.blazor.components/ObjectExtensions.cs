using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    internal static class ObjectExtensions
    {
        public static void IfNullThrow<TType>(this ComponentBase obj, TType modal, string errorMessage = "")
        {
            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = $"Component {nameof(obj)} must be a child of {nameof(modal)}";

            if (modal == null)
            {
                throw new ArgumentNullException(errorMessage);
            }
        }
    }
}
