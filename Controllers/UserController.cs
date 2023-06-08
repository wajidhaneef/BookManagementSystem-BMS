using BookManagementSystem_BMS.Data;
using BookManagementSystem_BMS.Models;
using BookManagementSystem_BMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BookManagementSystem_BMS.Controllers
{
    public class UserController : Controller
    {
        private readonly BMSContext _dbContext;

        public UserController(BMSContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Login
        public string Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.EmailAddress == username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return "Your email is not registered";
            }
            bool passwordMatch = BCryptNet.Verify(password, user.PasswordHash);
            if (!passwordMatch) return "Your password or user name is not valid";

            return "success";
        }

        // POST: User/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Signup(string signupUsername, string emailAddress, string userRole, string signupPassword, string confirmPassword)
        {
            return Ok("wajid");
            try
            {
                if (signupPassword != confirmPassword) return Ok("User Password does not match");
                // Check if the username is already taken
                if (_dbContext.Users.Any(u => u.EmailAddress == emailAddress))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return Ok("Your email is already registered with another account");
                }
                // find the role id associated with the role
                int roleId = _dbContext.Roles.FirstOrDefault(r => r.RoleName.ToLower() == userRole.ToLower()).RoleID;
                string salt = BCryptNet.GenerateSalt();
                // Hash the password with the salt
                string hashedPassword = BCryptNet.HashPassword(signupPassword, salt);

                // Create a new user entity
                var user = new User
                {
                    Username = signupUsername,
                    PasswordHash = hashedPassword,
                    EmailAddress = emailAddress,
                    Salt = salt,
                    RoleID = roleId,
                    // You may need to add more properties to the User entity depending on your requirements
                };

                // Save the user to the database
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                // Redirect to a success page or login page

                // Redirect to a success page or perform additional actions

                return Ok("success");
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
    }
}
