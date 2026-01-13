using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using system_gestion_isga.Infrastructure.Repositories.Users;
using system_gestion_isga.Infrastructure.Utils;
using systeme_gestion_isga.Features.Auth.ViewModels;

namespace system_gestion_isga.Features.Auth.Controllers
{
    public class AuthController : Controller
    {

        private readonly IUserRepository _users;
        public AuthController()
        {
            _users = new UserRepository();
        }

        public AuthController(IUserRepository users)
        {
            _users = users;
        }
        // GET: login
        public ActionResult Login()
        {
            return View();
        }

        // POST: login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var hash = PasswordHasher.Hash(model.Password);
            var user = _users.Login(model.Username, hash);
            if(user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }
            //Session["UserId"] = user.Id;
            //Session["Username"] = user.Username;
            //Session["Role"] = user.Role.ToString();
            var expiration = model.RememberMe
                ? DateTime.Now.AddDays(14)   // 2 weeks
                : DateTime.Now.AddMinutes(30);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                expiration,
                model.RememberMe,
                user.Role.ToString()
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = Request.IsSecureConnection

            };
            if (model.RememberMe)
            {
                cookie.Expires = ticket.Expiration;
            }

            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Dashboard");
            

        }

        // Post: logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}