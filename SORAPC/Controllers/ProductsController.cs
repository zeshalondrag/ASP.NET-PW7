using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class ProductsController : Controller
    {
        private SorapcContext _soradb;

        public ProductsController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> Products()
        {
            return View(await _soradb.Products.ToListAsync());
        }

        public IActionResult CreateProducts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductStatusId == null || product.ProductCategoryId == null)
                {
                    ModelState.AddModelError("", "Статус и категория товара обязательны для заполнения.");
                    return View(product);
                }

                _soradb.Products.Add(product);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("Products", "AdminPanel");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> EditProducts(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _soradb.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProducts(int id, Product product)
        {
            if (id != product.IdProduct)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(product);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.Products.Any(e => e.IdProduct == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Products", "AdminPanel");
            }
            return View(product);
        }

        [HttpGet]
        [ActionName("DeleteProducts")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _soradb.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var product = await _soradb.Products.FindAsync(id);
            if (product != null)
            {
                _soradb.Products.Remove(product);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("Products", "AdminPanel");
        }

        public async Task<IActionResult> DetailsProducts(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _soradb.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}