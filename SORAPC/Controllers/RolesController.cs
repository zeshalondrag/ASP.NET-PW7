using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class RolesController : Controller
    {
        private SorapcContext _soradb;

        public RolesController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> Roles()
        {
            return View(await _soradb.Roles.ToListAsync());
        }

        public IActionResult CreateRoles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoles(Role role)
        {
            if (ModelState.IsValid)
            {
                _soradb.Roles.Add(role);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("Roles", "AdminPanel");
            }
            return View(role);
        }

        public async Task<IActionResult> EditRoles(int? id)
        {
            if (id == null)
                return NotFound();

            var role = await _soradb.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(int id, Role role)
        {
            if (id != role.IdRole)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(role);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.Roles.Any(e => e.IdRole == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Roles", "AdminPanel");
            }
            return View(role);
        }

        [HttpGet]
        [ActionName("DeleteRoles")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var role = await _soradb.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoles(int id)
        {
            var role = await _soradb.Roles.FindAsync(id);
            if (role != null)
            {
                _soradb.Roles.Remove(role);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("Roles", "AdminPanel");
        }

        public async Task<IActionResult> DetailsRoles(int? id)
        {
            if (id == null)
                return NotFound();

            var role = await _soradb.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            return View(role);
        }
    }
}