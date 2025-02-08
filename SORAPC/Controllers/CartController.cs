using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class CartController : Controller
    {
        private SorapcContext _soradb;

        public CartController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> Cart()
        {
            return View(await _soradb.Carts.ToListAsync());
        }

        public IActionResult CreateCart()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(Cart cart)
        {
            if (ModelState.IsValid)
            {
                _soradb.Carts.Add(cart);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("Cart", "AdminPanel");
            }
            return View(cart);
        }

        public async Task<IActionResult> EditCart(int? id)
        {
            if (id == null)
                return NotFound();

            var cart = await _soradb.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> EditCart(int id, Cart cart)
        {
            if (id != cart.IdCart)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(cart);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.Carts.Any(e => e.IdCart == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Cart", "AdminPanel");
            }
            return View(cart);
        }

        [HttpGet]
        [ActionName("DeleteCart")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var cart = await _soradb.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _soradb.Carts.FindAsync(id);
            if (cart != null)
            {
                _soradb.Carts.Remove(cart);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("Cart", "AdminPanel");
        }

        public async Task<IActionResult> DetailsCart(int? id)
        {
            if (id == null)
                return NotFound();

            var cart = await _soradb.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }
    }
}