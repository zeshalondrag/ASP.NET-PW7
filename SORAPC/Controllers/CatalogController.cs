using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;

namespace SORAPC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly SorapcContext db;

        public CatalogController(SorapcContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Catalog()
        {
            var products = await db.Products.ToListAsync();
            return View(products);
        }
    }
}