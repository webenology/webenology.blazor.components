using Microsoft.Extensions.DependencyInjection;
using webenology.blazor.components.OutsideClickComponent;

namespace webenology.blazor.components.calendar;
public static class RegisterWebenologyBlazorComponent
{
    public static void AddBlazorCalendar(this IServiceCollection service)
    {
        service.AddBlazorOutsideClick();
    }
}
