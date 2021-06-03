using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CasusBlok4.Controllers
{
#if DEBUG
    public class GodModeController : Controller
    {
        public async Task<IActionResult> SignInAsync([FromQuery] bool isEmployee = true)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Fritz Gerald"),
                new Claim(ClaimTypes.Email, "fritzel@example.com"),
            };

            if (isEmployee)
            {
                claims.Add(new Claim(ClaimTypes.Role, "employee"));
            }

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProps = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProps);
            return Json(true);
        }

        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(true);
        }
    }
#endif
}
