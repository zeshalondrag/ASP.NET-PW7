using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class ProductStatusController : Controller
    {
        private SorapcContext _soradb;

        public ProductStatusController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> ProductStatus()
        {
            return View(await _soradb.ProductStatuses.ToListAsync());
        }

        public IActionResult CreateProductStatus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductStatus(ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                _soradb.ProductStatuses.Add(productStatus);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("ProductStatus", "AdminPanel");
            }
            return View(productStatus);
        }

        public async Task<IActionResult> EditProductStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var status = await _soradb.ProductStatuses.FindAsync(id);
            if (status == null)
                return NotFound();

            return View(status);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductStatus(int id, ProductStatus productStatus)
        {
            if (id != productStatus.IdProductStatus)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(productStatus);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.ProductStatuses.Any(e => e.IdProductStatus == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("ProductStatus", "AdminPanel");
            }
            return View(productStatus);
        }

        [HttpGet]
        [ActionName("DeleteProductStatus")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var status = await _soradb.ProductStatuses.FindAsync(id);
            if (status == null)
                return NotFound();

            return View(status);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductStatus(int id)
        {
            var status = await _soradb.ProductStatuses.FindAsync(id);
            if (status != null)
            {
                _soradb.ProductStatuses.Remove(status);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("ProductStatus", "AdminPanel");
        }

        public async Task<IActionResult> DetailsProductStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var status = await _soradb.ProductStatuses.FindAsync(id);
            if (status == null)
                return NotFound();

            return View(status);
        }
    }
}