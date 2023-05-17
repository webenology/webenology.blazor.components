using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.ss.authentication;

public interface IAuthOptions
{
    string AfterLogoutUrl { get; set; }
    string Title { get; set; }
}

public class AuthOptions : IAuthOptions
{
    public string AfterLogoutUrl { get; set; }
    public string Title { get; set; }

    public AuthOptions()
    {
        AfterLogoutUrl = "/";
    }
}