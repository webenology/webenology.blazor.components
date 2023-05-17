using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webenology.blazor.ss.authentication.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IAuthUser> _signInManager;

        public readonly IAuthOptions _options;

        public LoginModel(SignInManager<IAuthUser> signInManager, IAuthOptions options)
        {
            _signInManager = signInManager;
            _options = options;
        }
        public void OnGet()
        {
        }
    }
}
