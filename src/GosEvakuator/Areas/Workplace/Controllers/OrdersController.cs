using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GosEvakuator.Data;
using GosEvakuator.Models;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.City).Include(o => o.Customer).Include(o => o.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CityID"] = new SelectList(_context.City, "ID", "ID");
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "ID");
            ViewData["VehicleID"] = new SelectList(_context.Vehicle, "ID", "ID");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CityID,CustomerID,Distance,PlaceArrival,PlaceDeparture,VehicleID")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderStatuses.Add(new OrderStatus());

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CityID"] = new SelectList(_context.City, "ID", "ID", order.CityID);
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "ID", order.CustomerID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicle, "ID", "ID", order.VehicleID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CityID"] = new SelectList(_context.City, "ID", "ID", order.CityID);
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "ID", order.CustomerID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicle, "ID", "ID", order.VehicleID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CityID,CustomerID,Distance,PlaceArrival,PlaceDeparture,VehicleID")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CityID"] = new SelectList(_context.City, "ID", "ID", order.CityID);
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "ID", order.CustomerID);
            ViewData["VehicleID"] = new SelectList(_context.Vehicle, "ID", "ID", order.VehicleID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(m => m.ID == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID == id);
        }
    }
}
