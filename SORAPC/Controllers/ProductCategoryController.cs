using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class ProductCategoryController : Controller
    {
        private SorapcContext _soradb;

        public ProductCategoryController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> ProductCategory()
        {
            return View(await _soradb.ProductCategories.ToListAsync());
        }

        public IActionResult CreateProductCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                _soradb.ProductCategories.Add(category);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("ProductCategory", "AdminPanel");
            }
            return View(category);
        }

        public async Task<IActionResult> EditProductCategory(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _soradb.ProductCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductCategory(int id, ProductCategory category)
        {
            if (id != category.IdProductCategory)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(category);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.ProductCategories.Any(e => e.IdProductCategory == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("ProductCategory", "AdminPanel");
            }
            return View(category);
        }

        [HttpGet]
        [ActionName("DeleteProductCategory")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _soradb.ProductCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var category = await _soradb.ProductCategories.FindAsync(id);
            if (category != null)
            {
                _soradb.ProductCategories.Remove(category);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("ProductCategory", "AdminPanel");
        }

        public async Task<IActionResult> DetailsProductCategory(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _soradb.ProductCategories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }
    }
}