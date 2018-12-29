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
    public class SheduleDaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SheduleDaysController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SheduleDays
        public async Task<IActionResult> Index()
        {
            return View(await _context.SheduleDay.ToListAsync());
        }

        // GET: SheduleDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleDay = await _context.SheduleDay.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleDay == null)
            {
                return NotFound();
            }

            return View(sheduleDay);
        }

        // GET: SheduleDays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SheduleDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DayOfWeek")] SheduleDay sheduleDay)
        {
            if (ModelState.IsValid)
            {
                sheduleDay.HourRanges.Add(new SheduleHourRange());

                _context.Add(sheduleDay);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sheduleDay);
        }

        // GET: SheduleDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleDay = await _context.SheduleDay.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleDay == null)
            {
                return NotFound();
            }
            return View(sheduleDay);
        }

        // POST: SheduleDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DayOfWeek")] SheduleDay sheduleDay)
        {
            if (id != sheduleDay.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sheduleDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheduleDayExists(sheduleDay.ID))
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
            return View(sheduleDay);
        }

        // GET: SheduleDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleDay = await _context.SheduleDay.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleDay == null)
            {
                return NotFound();
            }

            return View(sheduleDay);
        }

        // POST: SheduleDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sheduleDay = await _context.SheduleDay.SingleOrDefaultAsync(m => m.ID == id);
            _context.SheduleDay.Remove(sheduleDay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SheduleDayExists(int id)
        {
            return _context.SheduleDay.Any(e => e.ID == id);
        }
    }
}
