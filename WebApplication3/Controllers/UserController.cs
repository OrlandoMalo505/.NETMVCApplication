using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{


   [Route("api/[controller]")]
    public class UserController : Controller
    {
        [Authorize(Roles = "USER")]
        [HttpGet("/api/users/{id}")]
        public IActionResult GetSingleUser(int id)
        {
            var session = HttpContext.Session.GetInt32("CurrentRole");

            if ((Role)session == Role.USER)
            {
                return View("~/Views/Home/ShowUser.cshtml", new User().getUserById(id));
            }

            return View("~/Views/Login/Login.cshtml");

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("/api/newUser")]
        public IActionResult NewUser()
        {

            var session = HttpContext.Session.GetInt32("CurrentRole");

            if ((Role)session == Role.ADMIN)
            {

                return View("~/Views/Home/New.cshtml");
            }
            return View("~/Views/Login/Login.cshtml");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("/api/newUser")]
        public IActionResult CreateUser(User umodel)
        {
            if (ModelState.IsValid)
            {
                if (umodel.CheckUser(umodel.Username) == 1)
                {
                    ModelState.AddModelError("username", "Duplicate Username.");
                }
                else
                {
                    User user = new User();
                    int result = user.SaveDetails(umodel);
                    if (result > 0)
                    {
                        return RedirectToAction("AllUsers");
                    }
                }
            }

            return View("~/Views/Home/New.cshtml", umodel);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("/api/users/")]
        public IActionResult AllUsers()
        {
            var roleSession = HttpContext.Session.GetInt32("CurrentRole");

            if (roleSession != null)
            {
                Role role = (Role) roleSession;
                if (role == Role.ADMIN)
                {
                    return View("~/Views/Home/Index.cshtml", new User().GetAllUsers());
                }
                
            }
            return RedirectToAction("Login", "Login");
        }        

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("/api/users/edit/{id}")]
        public IActionResult EditUserById(int id)
        {
            var session = HttpContext.Session.GetInt32("CurrentRole");

            if (session == (int)Role.USER)
            {
                return View("~/Views/Home/EditUserRole.cshtml", new User().getUserById(id));
            }
            else if (session == (int)Role.ADMIN)
            {
                return View("~/Views/Home/EditUser.cshtml", new User().getUserById(id));
            }

            return View("~/Views/Login/Login.cshtml");
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpPost("/api/users/edit/{id}")]
        public IActionResult EditUser(int id, User umodel)
        {
            

                    var session = HttpContext.Session.GetInt32("CurrentRole");
                    if ((Role)session == Role.ADMIN)
                    {
                        User user = new User();

                        int result = user.EditUser(umodel, Role.ADMIN);
                        return RedirectToAction("AllUsers");
                    }
                    else if ((Role)session == Role.USER)
                    {
                        User user = new User();
                        user.EditUser(umodel, Role.USER);


                        return RedirectToAction("GetSingleUser", "user", new { id = id });
                    }
                
            
            return View("~/Views/Home/EditUser.cshtml");
        }
        
        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet("/api/users/delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            User user = new User();
            user.DeleteUserById(id);
            var session = HttpContext.Session.GetInt32("CurrentRole");

            if ((Role)session == Role.ADMIN)
            {
                return View("~/Views/Home/Index.cshtml", new User().GetAllUsers());
            }
            else
            {
                return RedirectToAction("Logout", "Login");

            }
        }

        [AllowAnonymous]
        [HttpGet("/api/signup")]
        public IActionResult Signup()
        {
            return View("~/Views/Home/Signup.cshtml");

        }

        [AllowAnonymous]
        [HttpPost("/api/signup")]
        public IActionResult SignupUser(User user)
        {

            if (user.CheckUser(user.Username) == 1)
            {
                ModelState.AddModelError("username", "Duplicate Username.");
            }
            else
            {
                user.Role = 2;
                int result = user.SaveDetails(user);
                if (result > 0)
                {
                    ViewBag.Error = "Your account is created. Please log in!";
                    return View("~/Views/Login/Success.cshtml");
                }
            }
            return View("~/Views/Home/Signup.cshtml", user);
        }

    }
}