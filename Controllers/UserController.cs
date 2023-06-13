using BookManagementSystem_BMS.Data;
using BookManagementSystem_BMS.Models;
using BookManagementSystem_BMS.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Cryptography;
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

        [HttpGet]
        public ActionResult LoginPage()
        {
            var userView = new UserViewModel();
            return View("~/Views/User/Login.cshtml", userView);
        }

        //--------------------------------------------------------
        [HttpPost]
        public ActionResult LoginPage(UserViewModel userViewModel)
        {
            ViewBag.Roles = _dbContext.Roles.ToList();
            // Validate the login credentials
            var user = _dbContext.Users.FirstOrDefault(u => u.EmailAddress == userViewModel.EmailAddress);

            if (user == null)
            {
                ModelState.AddModelError("loginError", "The email or password is incorrect");
                return View("~/Views/User/Login.cshtml", userViewModel);
            }
            bool passwordMatch = BCryptNet.Verify(userViewModel.Password, user.PasswordHash);
            if (!passwordMatch) 
            {
                ModelState.AddModelError("loginError", "The email or password is incorrect");
                return View("~/Views/User/Login.cshtml", userViewModel);
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, userViewModel.EmailAddress),
                new Claim(ClaimTypes.Role, (user.RoleID).ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = userViewModel.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
            return RedirectToAction("Index", "Book");
        }
        //--------------------------------------------------------
        public ActionResult SignupPage()
        {
            var roles = _dbContext.Roles.ToList();
            ViewBag.Roles = roles;
            var userView = new UserViewModel()
            {
                Roles = roles,
                SelectedRoleId = roles.First().RoleID,
            };
            return View("~/Views/User/Signup.cshtml", userView);
        }


        //_________________________________________________________________
        [HttpPost]
        public ActionResult SignupPage(UserViewModel userViewModel)
        {
            //return Ok("wajid");
            if (ModelState.IsValid)
            {
                try
                {
                    if (userViewModel.Password != userViewModel.ConfirmPassword || userViewModel.Password == "" || userViewModel.ConfirmPassword == "") return Ok("User Password does not match");
                    // Check if the username is already taken
                    if (_dbContext.Users.Any(u => u.EmailAddress == userViewModel.EmailAddress))
                    {
                        ModelState.AddModelError("EmailAddress", "Email Address is already taken.");
                        return Ok("Your email is already registered with another account");
                    }


                    string salt = BCryptNet.GenerateSalt();
                    // Hash the password with the salt
                    string hashedPassword = BCryptNet.HashPassword(userViewModel.Password, salt);

                    // Create a new user
                    var user = new User
                    {
                        Username = userViewModel.Username,
                        PasswordHash = hashedPassword,
                        EmailAddress = userViewModel.EmailAddress,
                        Salt = salt,
                        RoleID = userViewModel.SelectedRoleId,

                    };

                    // Save the user to the database
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();

                    return View("~/Views/User/Login.cshtml", userViewModel);
                }
                catch (Exception ex)
                {

                }
            }
            // Model is not valid, add validation errors to the ModelState
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }
            userViewModel.Roles = _dbContext.Roles.ToList();
            return View("~/Views/User/Signup.cshtml", userViewModel);
        }
        //_________________________________________________________________
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
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, username),
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

            return Ok("loggedout");
        }


        // POST: User/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Signup(string signupUsername, string signupEmail, int userRole, string signupPassword, string confirmPassword)
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

                
                // find the role id associated with the role
                //int roleId = _dbContext.Roles.FirstOrDefault(r => r.RoleName.ToLower() == userRole.ToLower()).RoleID;
                string salt = BCryptNet.GenerateSalt();
                SHA256 mySha256 = SHA256.Create();
                // Hash the password with the salt
                string hashedPassword = BCryptNet.HashPassword(signupPassword, salt);

                // Create a new user
                var user = new User
                {
                    Username = signupUsername,
                    PasswordHash = hashedPassword,
                    EmailAddress = signupEmail,
                    Salt = salt,
                    RoleID = userRole,
                    
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
