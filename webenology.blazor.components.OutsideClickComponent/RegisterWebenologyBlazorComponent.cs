using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace webenology.blazor.components.OutsideClickComponent;
public static partial class RegisterWebenologyBlazorComponent
{
    public static void RegisterWebenologyOutsideClickComponents(this IServiceCollection service)
    {
        service.TryAddTransient<IOutsideClickJsHelper, OutsideClickJsHelper>();
    }
}
