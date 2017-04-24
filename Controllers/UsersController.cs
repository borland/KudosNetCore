using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KudosNetCore.Model;
using Microsoft.AspNetCore.Http;

namespace KudosNetCore.Controllers
{
    public class UsersController : Controller
    {
        readonly Repository m_repository;

        public UsersController(Repository repository) => m_repository = repository;

        [AuthenticatedUser]
        public IActionResult Index()
        {
            var users = m_repository.Users.ToArray();
            return View(users);
        }

        [AuthenticatedUser, HttpGet]
        public IActionResult Create() => View();

        [AuthenticatedUser, HttpPost]
        public IActionResult Create(string fullName, string username, string password, bool isAdmin = false)
        {
            (var salt, var hash) = Pbkdf2.HashPassword(password);

            User user = new User {
                FullName = fullName,
                Username = username,
                PasswordSalt = salt,
                PasswordHash = hash,
                Flags = isAdmin ? UserFlags.Admin : UserFlags.None
            };

            var entity = m_repository.Users.Add(user);

            try {
                m_repository.SaveChanges(); // throws
                return RedirectToAction("Index");
            } catch(Exception e) {
                ViewData["Exception"] = e;
                return View(user); // can't save, spit back the view
            }
        }

        [HttpGet] // ANONYMOUS
        public IActionResult Login() =>  View();

        [HttpPost] // ANONYMOUS
        public IActionResult Login(string username, string password)
        {
            if(username == "orion" && password=="pants") { // gotta get the ball rolling somehow
                var orion = m_repository.Users.First(u => u.Username == "orion");

                Response.Cookies.Append("auth", orion.Id.ToString(), new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(1) });
                return View();
            }

            var user = m_repository.Users.First(u => u.Username == username);
            if(!Pbkdf2.ValidatePassword(password, user.PasswordSalt, user.PasswordHash)) {
                throw new Exception("Login failed!");
            }
            return RedirectToAction("Index");
        }
    }
}
