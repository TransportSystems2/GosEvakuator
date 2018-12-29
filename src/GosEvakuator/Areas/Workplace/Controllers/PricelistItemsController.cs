using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GosEvakuator.Data;
using GosEvakuator.Models;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class PricelistItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PricelistItemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: PricelistItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.PricelistItem.ToListAsync());
        }

        // GET: PricelistItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelistItem = await _context.PricelistItem
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pricelistItem == null)
            {
                return NotFound();
            }

            return View(pricelistItem);
        }

        // GET: PricelistItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PricelistItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PricelistID,Alias,Name,Discount,LockedWheel,LockedSteeringWheel,LoadingVehicle,PerKilometer")] PricelistItem pricelistItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pricelistItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pricelistItem);
        }

        // GET: PricelistItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelistItem = await _context.PricelistItem.SingleOrDefaultAsync(m => m.ID == id);
            if (pricelistItem == null)
            {
                return NotFound();
            }
            return View(pricelistItem);
        }

        // POST: PricelistItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PricelistID,Alias,Name,Discount,LockedWheel,LockedSteeringWheel,LoadingVehicle,PerKilometer")] PricelistItem pricelistItem)
        {
            if (id != pricelistItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricelistItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricelistItemExists(pricelistItem.ID))
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
            return View(pricelistItem);
        }

        // GET: PricelistItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricelistItem = await _context.PricelistItem
                .SingleOrDefaultAsync(m => m.ID == id);
            if (pricelistItem == null)
            {
                return NotFound();
            }

            return View(pricelistItem);
        }

        // POST: PricelistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pricelistItem = await _context.PricelistItem.SingleOrDefaultAsync(m => m.ID == id);
            _context.PricelistItem.Remove(pricelistItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PricelistItemExists(int id)
        {
            return _context.PricelistItem.Any(e => e.ID == id);
        }
    }
}
