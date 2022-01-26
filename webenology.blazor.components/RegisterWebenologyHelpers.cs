using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using webenology.blazor.components.BlazorPdfComponent;
using webenology.blazor.components.MailMerge;

namespace webenology.blazor.components
{
    public static class RegisterWebenologyHelpers
    {
        public static void AddWebenologyHelpers(this IServiceCollection service)
        {
            service.TryAddScoped<IComboBoxJsHelper, ComboBoxJsHelper>();
            service.TryAddScoped<IDateTimerPickerJsHelper, DateTimerPickerJsHelper>();
            service.TryAddScoped<IOutsideClickJsHelper, OutsideClickJsHelper>();
            service.TryAddScoped<IModalJsHelper, ModalJsHelper>();
            service.TryAddScoped<IToastrJsHelper, ToastrJsHelper>();
            service.TryAddScoped<IWebPdfViewerJsHelper, WebPdfViewerJsHelper>();
            service.TryAddScoped<IWebTextInputJsHelper, WebTextInputJsHelper>();
            service.TryAddScoped<IHtmlToPdfManager, HtmlToPdfManager>();
            service.TryAddScoped<IExecuteProcess, ExecuteProcess>();
            service.TryAddScoped<IWFileWriter, WFileWriter>();
            service.TryAddScoped<IBlazorPdf, BlazorPdf>();
            service.TryAddScoped<IMailMergeManager, MailMergeManager>();
        }
    }
}
