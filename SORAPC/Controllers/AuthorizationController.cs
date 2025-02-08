using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using SORAPC.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace SORAPC.Controllers
{
    public class AuthorizationController : Controller
    {
        private SorapcContext _soradb;

        public AuthorizationController(SorapcContext context)
        {
            _soradb = context;
        }

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorization(string login, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = _soradb.Users.FirstOrDefault(u => u.Logins == login && u.Passwords == hashedPassword);
            if (user != null)
            {
                var claim = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.IdUsers.ToString()),
                    new Claim(ClaimTypes.Email, user.Logins),
                    new Claim(ClaimTypes.Role, user.RoleId == 2 ? "Администратор" : "Клиент")
                };

                var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Неверный логин или пароль!";
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                return RedirectToAction("Authorization");
            }

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (userIdClaim == null)
            {
                return RedirectToAction("Authorization");
            }

            var userId = int.Parse(userIdClaim);
            var user = _soradb.Users.FirstOrDefault(u => u.IdUsers == userId);
            if (user == null)
            {
                return RedirectToAction("Authorization");
            }

            return View(user); 
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authorization");
        }
    }
}