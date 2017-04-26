using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KudosNetCore.Model;
using Microsoft.AspNetCore.Http;

namespace KudosNetCore.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        readonly Repository m_repository;

        public UsersController(Repository repository) => m_repository = repository;

        [AuthenticatedUser, HttpGet("")]
        public IActionResult Index()
        {
            var users = m_repository.Users.ToArray();
            return View(users);
        }

        [AuthenticatedUser, HttpGet("Create")]
        public IActionResult Create() => View();

        [AuthenticatedUser, HttpPost("Create")]
        public IActionResult Create(ViewModels.EditUser userViewModel)
        {
            var user = new User();
            userViewModel.ApplyTo(user);
            m_repository.Users.Add(user);

            try {
                m_repository.SaveChanges(); // throws
                return RedirectToAction(nameof(Index));
            } catch(Exception e) {
                ViewData["Exception"] = e;
                return View(user); // can't save, spit back the view
            }
        }

        [AuthenticatedUser, HttpGet("{id}")]
        public IActionResult Edit(int id) => View(new ViewModels.EditUser(m_repository.Users.Find(id)));

        [AuthenticatedUser, HttpPost("{id}")]
        public IActionResult Edit(ViewModels.EditUser userViewModel)
        {
            var user = m_repository.Users.Find(userViewModel.Id);
            userViewModel.ApplyTo(user);

            try {
                m_repository.SaveChanges(); // throws
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e) {
                ViewData["Exception"] = e;
                return View(user); // can't save, spit back the view
            }
        }

        [AuthenticatedUser, HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = m_repository.Users.Find(id);
            m_repository.Users.Remove(user);

            try {
                m_repository.SaveChanges(); // throws
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e) {
                ViewData["Exception"] = e;
                return View(nameof(Edit), user); // can't delete, spit back the view
            }
        }

        [HttpGet("Login")] // ANONYMOUS
        public IActionResult Login() =>  View();

        [HttpPost("Login")] // ANONYMOUS
        public IActionResult Login(string username, string password)
        {
            User user;
            if (username == "orion" && password == "pants") { // gotta get the ball rolling somehow
                user = m_repository.Users.First(u => u.Username == "orion");
            }
            else {
                user = m_repository.Users.First(u => u.Username == username);
                if (!Pbkdf2.ValidatePassword(password, user.PasswordSalt, user.PasswordHash)) {
                    throw new Exception("Login failed!");
                }
            }
            Response.Cookies.Append("auth", user.Id.ToString(), new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(1) });
            return RedirectToAction(nameof(Index));
        }
    }
}

namespace KudosNetCore.ViewModels
{
    public class EditUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public EditUser() { }

        public EditUser(User model)
        {
            if(model == null) {
                return;
            }
            Id = model.Id;
            FullName = model.FullName;
            Username = model.Username;
            // we can't extract the password, it's hashed
            IsAdmin = model.Flags.HasFlag(UserFlags.Admin);
        }

        public void ApplyTo(User model)
        {
            // writing back the ID doesn't make sense
            model.FullName = FullName;
            model.Username = Username;
            if(Password != null) {
                (model.PasswordSalt, model.PasswordHash) = Pbkdf2.HashPassword(Password);
            }
            model.Flags = IsAdmin ? UserFlags.Admin : UserFlags.None;
        }
    }
}
