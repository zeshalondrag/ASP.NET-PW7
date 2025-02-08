using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class OrdersController : Controller
    {
        private SorapcContext _soradb;

        public OrdersController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> Orders()
        {
            return View(await _soradb.Orders.ToListAsync());
        }

        public IActionResult CreateOrders()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrders(Order orders)
        {
            if (ModelState.IsValid)
            {
                _soradb.Orders.Add(orders);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("Orders", "AdminPanel");
            }
            return View(orders);
        }

        public async Task<IActionResult> EditOrders(int? id)
        {
            if (id == null)
                return NotFound();

            var orders = await _soradb.Orders.FindAsync(id);
            if (orders == null)
                return NotFound();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrders(int id, Order orders)
        {
            if (id != orders.IdOrder)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(orders);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.Orders.Any(e => e.IdOrder == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Orders", "AdminPanel");
            }
            return View(orders);
        }

        [HttpGet]
        [ActionName("DeleteOrders")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var orders = await _soradb.Orders.FindAsync(id);
            if (orders == null)
                return NotFound();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrders(int id)
        {
            var orders = await _soradb.Orders.FindAsync(id);
            if (orders != null)
            {
                _soradb.Orders.Remove(orders);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("Orders", "AdminPanel");
        }

        public async Task<IActionResult> DetailsOrders(int? id)
        {
            if (id == null)
                return NotFound();

            var orders = await _soradb.Orders.FindAsync(id);
            if (orders == null)
                return NotFound();

            return View(orders);
        }
    }
}