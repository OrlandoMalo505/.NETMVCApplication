using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication3.Controllers
{
    
    public class LoginController : Controller
    {
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Login(User user)
        {
            
            user = user.FindUser(user);

            if (user != null)
            {

                HttpContext.Session.SetInt32("CurrentRole", user.Role);
                HttpContext.Session.SetInt32("CurrentId", user.Id);

                ClaimsIdentity identity = null;
                bool isAuthenticate = false;
                if (user.RoleEnum == Role.ADMIN)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Role,"ADMIN")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }


                if (user.RoleEnum == Role.USER)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Role,"USER")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticate = true;
                }

                if (isAuthenticate)
                {
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (user.Role == (int)Role.ADMIN)
                    {
                        return RedirectToAction("AllUsers", "user");
                    }
                    else if (user.Role == (int)Role.USER)
                    {
                        return RedirectToAction("GetSingleUser", "user", new { id = user.Id });
                    }

                }

            }

            ViewBag.Error = "Wrong Username or Password!";
            return View("~/Views/Login/Login.cshtml");

        }




        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentRole");
            HttpContext.Session.Remove("CurrentId");

            return RedirectToAction("Login","Login");
        }





        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.Error = "You are not authorized to view this page.";
            return View();
        }

    }
}
