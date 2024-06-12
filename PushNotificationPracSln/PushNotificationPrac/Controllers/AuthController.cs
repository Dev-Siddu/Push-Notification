using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using DataAccess;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PushNotificationPrac.Controllers
{
    [Route("[Controller]/[Action]")]
    public class AuthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignOut()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInModel signInData,string ReturnUrl = "Home/index")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            CommonDataAccess dal = new CommonDataAccess();
            int RS = dal.AuthenticateUser(signInData);

            if (RS == 0) { ViewBag.Message = "Incorrect Credentials"; return View(); }

            // Setting the cookies

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, signInData.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // configure additional properties of the cookie, such as expiration later
                ExpiresUtc = null,
            };

            // Sign in the user with the claims and authentication properties
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("~/" + ReturnUrl);
            //return View(signInData);
        }
    }
}
