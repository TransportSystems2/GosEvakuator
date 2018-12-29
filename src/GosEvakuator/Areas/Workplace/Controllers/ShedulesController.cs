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
    public class ShedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShedulesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Shedules
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shedule.ToListAsync());
        }

        // GET: Shedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shedule = await _context.Shedule.SingleOrDefaultAsync(m => m.ID == id);
            if (shedule == null)
            {
                return NotFound();
            }

            return View(shedule);
        }

        // GET: Shedules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Shedule shedule)
        {
            if (ModelState.IsValid)
            {
                InitWeek(shedule);

                _context.Add(shedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shedule);
        }

        // GET: Shedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shedule = await _context.Shedule.SingleOrDefaultAsync(m => m.ID == id);
            if (shedule == null)
            {
                return NotFound();
            }
            return View(shedule);
        }

        // POST: Shedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] Shedule shedule)
        {
            if (id != shedule.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheduleExists(shedule.ID))
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
            return View(shedule);
        }

        // GET: Shedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shedule = await _context.Shedule.SingleOrDefaultAsync(m => m.ID == id);
            if (shedule == null)
            {
                return NotFound();
            }

            return View(shedule);
        }

        // POST: Shedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shedule = await _context.Shedule.SingleOrDefaultAsync(m => m.ID == id);
            _context.Shedule.Remove(shedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SheduleExists(int id)
        {
            return _context.Shedule.Any(e => e.ID == id);
        }

        private void InitWeek(Shedule shedule)
        {
            for (var day = 0; day <= 6; day++)
            {
                var sheduleDay = new SheduleDay();
                sheduleDay.DayOfWeek = (Models.DayOfWeek)day;

                shedule.WorkWeek.Add(sheduleDay);
            }
        }
    }
}
