using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SORAPC.Controllers
{
    public class OrdersPositionController : Controller
    {
        private SorapcContext _soradb;

        public OrdersPositionController(SorapcContext context)
        {
            _soradb = context;
        }

        public async Task<IActionResult> OrdersPosition()
        {
            return View(await _soradb.OrderPositions.ToListAsync());
        }

        public IActionResult CreateOrdersPosition()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrdersPosition(OrderPosition orderPosition)
        {
            if (ModelState.IsValid)
            {
                _soradb.OrderPositions.Add(orderPosition);
                await _soradb.SaveChangesAsync();
                return RedirectToAction("OrdersPosition", "AdminPanel");
            }
            return View(orderPosition);
        }

        public async Task<IActionResult> EditOrdersPosition(int? id)
        {
            if (id == null)
                return NotFound();

            var orderPosition = await _soradb.OrderPositions.FindAsync(id);
            if (orderPosition == null)
                return NotFound();

            return View(orderPosition);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrdersPosition(int id, OrderPosition orderPosition)
        {
            if (id != orderPosition.IdOrderPosition)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _soradb.Update(orderPosition);
                    await _soradb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_soradb.OrderPositions.Any(e => e.IdOrderPosition == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("OrdersPosition", "AdminPanel");
            }
            return View(orderPosition);
        }

        [HttpGet]
        [ActionName("DeleteOrdersPosition")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var orderPosition = await _soradb.OrderPositions.FindAsync(id);
            if (orderPosition == null)
                return NotFound();

            return View(orderPosition);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrdersPosition(int id)
        {
            var orderPosition = await _soradb.OrderPositions.FindAsync(id);
            if (orderPosition != null)
            {
                _soradb.OrderPositions.Remove(orderPosition);
                await _soradb.SaveChangesAsync();
            }
            return RedirectToAction("OrdersPosition", "AdminPanel");
        }

        public async Task<IActionResult> DetailsOrdersPosition(int? id)
        {
            if (id == null)
                return NotFound();

            var orderPosition = await _soradb.OrderPositions.FindAsync(id);
            if (orderPosition == null)
                return NotFound();

            return View(orderPosition);
        }
    }
}