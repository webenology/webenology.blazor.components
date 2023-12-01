using Microsoft.Extensions.DependencyInjection;

using webenology.blazor.components.OutsideClickComponent;

namespace webenology.blazor.components.calendar;
public static partial class RegisterWebenologyBlazorComponent
{
    public static void RegisterWebenologyCalendarComponents(this IServiceCollection service)
    {
        service.RegisterWebenologyOutsideClickComponents();
    }
}
