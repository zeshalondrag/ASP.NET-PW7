using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;

namespace SORAPC.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly SorapcContext db;

        public ProductDetailsController(SorapcContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("ProductDetails/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Reviews).ThenInclude(r => r.Users).FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Route("ProductDetails/AddReview")]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (User.Identity.IsAuthenticated)
            {
                review.UsersId = Convert.ToInt32(User.Identity.Name);
                review.CreatedAt = DateTime.Now;

                db.Reviews.Add(review);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = review.ProductId });
        }
    }
}