using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webenology.blazor.ss.authentication.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IAuthUser> _signInManager;
        private readonly IAuthOptions _authOptions;

        public LogoutModel(SignInManager<IAuthUser> signInManager, IAuthOptions authOptions)
        {
            _signInManager = signInManager;
            _authOptions = authOptions;
        }
        public async Task<ActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            return Redirect(_authOptions.AfterLogoutUrl);
        }
    }
}
