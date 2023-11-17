using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using webenology.blazor.components;

namespace webenology.blazor.components.OutsideClickComponent;
public static class RegisterWebenologyBlazorComponent
{
    public static void AddBlazorOutsideClick(this IServiceCollection service)
    {
        service.TryAddTransient<IOutsideClickJsHelper, OutsideClickJsHelper>();
    }
}
