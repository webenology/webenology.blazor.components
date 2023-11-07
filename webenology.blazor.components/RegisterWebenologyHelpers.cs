using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using webenology.blazor.components.BlazorPdfComponent;
using webenology.blazor.components.MailMerge;

namespace webenology.blazor.components
{
    public static class RegisterWebenologyHelpers
    {
        /// <summary>
        /// Register All the Webenology Helpers
        /// </summary>
        /// <param name="service">Service Collection</param>
        public static void AddWebenologyHelpers(this IServiceCollection service, string? metabaeSecretKey = null)
        {
            service.TryAddScoped<IComboBoxJsHelper, ComboBoxJsHelper>();
            service.TryAddScoped<IDateTimerPickerJsHelper, DateTimerPickerJsHelper>();
            service.TryAddScoped<IOutsideClickJsHelper, OutsideClickJsHelper>();
            service.TryAddScoped<IModalJsHelper, ModalJsHelper>();
            service.TryAddScoped<IToastrJsHelper, ToastrJsHelper>();
            service.TryAddScoped<IWebPdfViewerJsHelper, WebPdfViewerJsHelper>();
            service.TryAddScoped<IWebTextInputJsHelper, WebTextInputJsHelper>();
            service.TryAddTransient<IHtmlToPdfManager, HtmlToPdfManager>();
            service.TryAddTransient<IExecuteProcess, ExecuteProcess>();
            service.TryAddTransient<IWFileWriter, WFileWriter>();
            service.TryAddTransient<IBlazorPdf, BlazorPdf>();
            service.TryAddScoped<IMailMergeManager, MailMergeManager>();
            service.TryAddScoped<IMetabaseHelper>(x => new MetabaseHelper(metabaeSecretKey));
        }
    }
}