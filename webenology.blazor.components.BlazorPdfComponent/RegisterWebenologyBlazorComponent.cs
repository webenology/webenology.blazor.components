using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace webenology.blazor.components.BlazorPdfComponent;
public static class RegisterWebenologyBlazorComponent
{
    public static void AddBlazorPdf(this IServiceCollection service)
    {
        service.TryAddTransient<IHtmlToPdfManager, HtmlToPdfManager>();
        service.TryAddTransient<IExecuteProcess, ExecuteProcess>();
        service.TryAddTransient<IWFileWriter, WFileWriter>();
        service.TryAddTransient<IBlazorPdf, BlazorPdf>();
    }
}
