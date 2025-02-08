using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;

namespace SORAPC.Controllers
{
    public class AdminPanelController : Controller
    {
        private SorapcContext _soradb;

        public AdminPanelController(SorapcContext context)
        {
            _soradb = context;
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult Cart()
        {
            var cart = _soradb.Carts.ToList(); 
            return View(cart);
        }

        public IActionResult OrderPosition()
        {
            var orderposition = _soradb.OrderPositions.ToList(); 
            return View(orderposition);
        }

        public IActionResult Orders()
        {
            var orders = _soradb.Orders.ToList();
            return View(orders);
        }

        public IActionResult OrderStatus()
        {
            var orderstatus = _soradb.OrderStatuses.ToList(); 
            return View(orderstatus);
        }

        public IActionResult ProductCategory()
        {
            var productcategory = _soradb.ProductCategories.ToList(); 
            return View(productcategory);
        }

        public IActionResult Products()
        {
            var product = _soradb.Products.ToList(); 
            return View(product);
        }

        public IActionResult ProductStatus()
        {
            var productstatus = _soradb.ProductStatuses.ToList(); 
            return View(productstatus);
        }

        public IActionResult Roles()
        {
            var roles = _soradb.Roles.ToList(); 
            return View(roles);
        }

        public IActionResult Users()
        {
            var users = _soradb.Users.ToList(); 
            return View(users);
        }

    }
}