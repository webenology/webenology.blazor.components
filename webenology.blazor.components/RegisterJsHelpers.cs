using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace webenology.blazor.components
{
    public static class RegisterJsHelpers
    {
        public static void AddWebenologyJsHelpers(this IServiceCollection service)
        {
            service.TryAddScoped<IComboBoxJsHelper, ComboBoxJsHelper>();
            service.TryAddScoped<IDateTimerPickerJsHelper, DateTimerPickerJsHelper>();
            service.TryAddScoped<IOutsideClickJsHelper, OutsideClickJsHelper>();
            service.TryAddScoped<IModalJsHelper, ModalJsHelper>();
            service.TryAddScoped<IToastrJsHelper, ToastrJsHelper>();
        }
    }
}
