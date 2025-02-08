using Microsoft.AspNetCore.Mvc;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class RegistrationController : Controller
    {
        private SorapcContext _soradb;

        public RegistrationController(SorapcContext context)
        {
            _soradb = context;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(string userSurname, string userName, string userMiddleName, string email, string phone, string logins, string passwords)
        {
            if (_soradb.Users.Any(u => u.Logins == logins))
            {
                ViewBag.ErrorMessage = "Такой логин уже существует!";
                return View();
            }

            string hashedPassword = HashPassword(passwords);

            var users = new User
            {
                UserSurname = userSurname,
                UserName = userName,
                UserMiddlename = userMiddleName,
                Email = email,
                Phone = phone,
                Logins = logins,
                Passwords = hashedPassword,
                RoleId = 2
            };

            _soradb.Users.Add(users);
            await _soradb.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}