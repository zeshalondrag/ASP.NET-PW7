using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class OrderStatusController : Controller
    {
        private SorapcContext _soradb;

        public OrderStatusController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> OrderStatus()
        {
            return View(await _soradb.OrderStatuses.ToListAsync());
        }

        public IActionResult CreateOrderStatus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderStatus(OrderStatus orderStatus)
        {
            if (ModelState.IsValid)
            {
                _soradb.OrderStatuses.Add(orderStatus);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("OrderStatus", "AdminPanel");
            }
            return View(orderStatus);
        }

        public async Task<IActionResult> EditOrderStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var orderStatus = await _soradb.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
                return NotFound();

            return View(orderStatus);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrderStatus(int id, OrderStatus orderStatus)
        {
            if (id != orderStatus.IdOrderStatus)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(orderStatus);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.OrderStatuses.Any(e => e.IdOrderStatus == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("OrderStatus", "AdminPanel");
            }
            return View(orderStatus);
        }

        [HttpGet]
        [ActionName("DeleteOrderStatus")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var orderStatus = await _soradb.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
                return NotFound();

            return View(orderStatus);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderStatus(int id)
        {
            var orderStatus = await _soradb.OrderStatuses.FindAsync(id);
            if (orderStatus != null)
            {
                _soradb.OrderStatuses.Remove(orderStatus);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("OrderStatus", "AdminPanel");
        }

        public async Task<IActionResult> DetailsOrderStatus(int? id)
        {
            if (id == null)
                return NotFound();

            var orderStatus = await _soradb.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
                return NotFound();

            return View(orderStatus);
        }
    }
}