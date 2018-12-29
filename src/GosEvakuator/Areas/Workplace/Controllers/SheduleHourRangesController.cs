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
    public class SheduleHourRangesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SheduleHourRangesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SheduleHourRanges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SheduleHourRange.Include(s => s.Driver);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SheduleHourRanges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleHourRange = await _context.SheduleHourRange.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleHourRange == null)
            {
                return NotFound();
            }

            return View(sheduleHourRange);
        }

        // GET: SheduleHourRanges/Create
        public IActionResult Create()
        {
            ViewData["DriverID"] = new SelectList(_context.Driver, "ID", "ID");
            return View();
        }

        // POST: SheduleHourRanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Beginning,DriverID,End")] SheduleHourRange sheduleHourRange)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sheduleHourRange);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DriverID"] = new SelectList(_context.Driver, "ID", "ID", sheduleHourRange.DriverID);
            return View(sheduleHourRange);
        }

        // GET: SheduleHourRanges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleHourRange = await _context.SheduleHourRange.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleHourRange == null)
            {
                return NotFound();
            }
            ViewData["DriverID"] = new SelectList(_context.Driver, "ID", "ID", sheduleHourRange.DriverID);
            return View(sheduleHourRange);
        }

        // POST: SheduleHourRanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Beginning,DriverID,End")] SheduleHourRange sheduleHourRange)
        {
            if (id != sheduleHourRange.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sheduleHourRange);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheduleHourRangeExists(sheduleHourRange.ID))
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
            ViewData["DriverID"] = new SelectList(_context.Driver, "ID", "ID", sheduleHourRange.DriverID);
            return View(sheduleHourRange);
        }

        // GET: SheduleHourRanges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheduleHourRange = await _context.SheduleHourRange.SingleOrDefaultAsync(m => m.ID == id);
            if (sheduleHourRange == null)
            {
                return NotFound();
            }

            return View(sheduleHourRange);
        }

        // POST: SheduleHourRanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sheduleHourRange = await _context.SheduleHourRange.SingleOrDefaultAsync(m => m.ID == id);
            _context.SheduleHourRange.Remove(sheduleHourRange);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SheduleHourRangeExists(int id)
        {
            return _context.SheduleHourRange.Any(e => e.ID == id);
        }
    }
}
