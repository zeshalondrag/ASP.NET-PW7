using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class UsersController : Controller
    {
        private SorapcContext _soradb;

        public UsersController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> Users()
        {
            return View(await _soradb.Users.ToListAsync());
        }

        public IActionResult CreateUsers()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsers(User user)
        {
            if (ModelState.IsValid)
            {
                user.Passwords = HashPassword(user.Passwords);

                _soradb.Users.Add(user);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("Users", "AdminPanel");
            }
            return View(user);
        }

        public async Task<IActionResult> EditUsers(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _soradb.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsers(int id, User user)
        {
            if (id != user.IdUsers)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(user);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.Users.Any(e => e.IdUsers == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Users", "AdminPanel");
            }
            return View(user);
        }

        [HttpGet]
        [ActionName("DeleteUsers")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _soradb.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var user = await _soradb.Users.FindAsync(id);
            if (user != null)
            {
                _soradb.Users.Remove(user);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("Users", "AdminPanel");
        }

        public async Task<IActionResult> DetailsUsers(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _soradb.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
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
