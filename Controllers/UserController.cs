using BookManagementSystem_BMS.Data;
using BookManagementSystem_BMS.Models;
using BookManagementSystem_BMS.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Claims;

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
        //public ActionResult Login(string username, string password)
        //{
        //    return Ok("success");
        //    var user = _dbContext.Users.FirstOrDefault(u => u.EmailAddress == username);

        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "Invalid username or password");
        //        return Ok("Your email is not registered");
        //    }
        //    bool passwordMatch = BCryptNet.Verify(password, user.PasswordHash);
        //    if (!passwordMatch) return Ok("Your password or user name is not valid");

        //    return Ok("success");
        //}
        
        public ActionResult Login(string username, string password, string strRemember)
        {
            bool rememberMe = false;
            if (strRemember == "on")
            {
                rememberMe = true;
            }
            // Validate the login credentials
            var user = _dbContext.Users.FirstOrDefault(u => u.EmailAddress == username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return Ok("Your email is not registered");
            }
            bool passwordMatch = BCryptNet.Verify(password, user.PasswordHash);
            if (!passwordMatch) return Ok("Your password or user name is not valid");

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, (user.RoleID).ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) 
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);


            return Ok("success");
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or any other page
            return RedirectToAction("Index", "Book");
        }


        // POST: User/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Signup(string signupUsername, string signupEmail, string userRole, string signupPassword, string confirmPassword)
        {
            //return Ok("wajid");
            try
            {
                if (signupPassword != confirmPassword) return Ok("User Password does not match");
                // Check if the username is already taken
                if (_dbContext.Users.Any(u => u.EmailAddress == signupEmail))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return Ok("Your email is already registered with another account");
                }

                //check if the role is already created
                var role = _dbContext.Roles.FirstOrDefault(r => r.RoleName.ToLower() == userRole.ToLower());
                if (role==null)
                {
                    //create the role
                    role = new()
                    {
                        RoleName = userRole,
                    };
                    //save role
                    _dbContext.Roles.Add(role);
                    _dbContext.SaveChanges();
                }
                
                // find the role id associated with the role
                //int roleId = _dbContext.Roles.FirstOrDefault(r => r.RoleName.ToLower() == userRole.ToLower()).RoleID;
                string salt = BCryptNet.GenerateSalt();
                // Hash the password with the salt
                string hashedPassword = BCryptNet.HashPassword(signupPassword, salt);

                // Create a new user
                var user = new User
                {
                    Username = signupUsername,
                    PasswordHash = hashedPassword,
                    EmailAddress = signupEmail,
                    Salt = salt,
                    RoleID = role.RoleID,
                    
                };

                // Save the user to the database
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

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
