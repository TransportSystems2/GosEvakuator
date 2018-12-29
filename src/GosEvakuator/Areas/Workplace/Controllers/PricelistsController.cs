using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GosEvakuator.Data;
using GosEvakuator.Models;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class PricelistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Dictionary<string, string> defaultVehicles = new Dictionary<string, string> {
            { "bike", "Мотоцикл" },
            { "car", "Легковой автомобиль" },
            { "crossover", "Кроссовер" },
            { "suv", "Внедорожник" },
            { "minibus", "Микроавтобус" },
            { "bus", "Автобус" },
            { "truck", "Грузовик" },
        };

        public PricelistsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Pricelists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pricelist.ToListAsync());
        }

        // GET: Pricelists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelist = await _context.Pricelist
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return View(pricelist);
        }

        // GET: Pricelists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pricelists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                AddDefaultItems(pricelist);

                _context.Add(pricelist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pricelist);
        }

        // GET: Pricelists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelist = await _context.Pricelist.SingleOrDefaultAsync(m => m.ID == id);
            if (pricelist == null)
            {
                return NotFound();
            }
            return View(pricelist);
        }

        // POST: Pricelists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] Pricelist pricelist)
        {
            if (id != pricelist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricelist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricelistExists(pricelist.ID))
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
            return View(pricelist);
        }

        // GET: Pricelists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelist = await _context.Pricelist
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pricelist == null)
            {
                return NotFound();
            }

            return View(pricelist);
        }

        // POST: Pricelists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pricelist = await _context.Pricelist.SingleOrDefaultAsync(m => m.ID == id);
            _context.Pricelist.Remove(pricelist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PricelistExists(int id)
        {
            return _context.Pricelist.Any(e => e.ID == id);
        }

        private void AddDefaultItems(Pricelist pricelist)
        {
            foreach (var vehicle in defaultVehicles)
            {
                var pricelistItem = new PricelistItem { Alias = vehicle.Key, Name = vehicle.Value };
                pricelist.Items.Add(pricelistItem);
            }
        }
    }
}
