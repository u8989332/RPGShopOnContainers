﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            var user = User as ClaimsPrincipal;
            var token = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            foreach(var claim in user.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value : {claim.Value}");
            }

            if(token != null)
            {
                ViewData["access_token"] = token;
            }

            if(idToken != null)
            {
                ViewData["id_token"] = idToken;
            }

            return RedirectToAction(nameof(CatalogController.About), "Catalog");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var homeUrl = Url.Action(nameof(CatalogController.Index), "Catalog");
            return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = homeUrl });
        }
    }
}
