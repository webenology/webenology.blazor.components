using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace webenology.blazor.ss.authentication;
public static class RegisterWebenologyAuth
{
    /// <summary>
    /// Register All the Webenology Auth Helpers
    /// </summary>
    /// <param name="service">Service Collection</param>
    public static void AddWebenologyAuth(this IServiceCollection service)
    {
        service.AddRouting();
    }
}
