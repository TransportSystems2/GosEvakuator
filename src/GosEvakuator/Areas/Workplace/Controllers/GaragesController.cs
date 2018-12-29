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
    public class GaragesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public GaragesController(ApplicationDbContext context)
        {
            dbContext = context;    
        }

        // GET: Garages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = dbContext.Garages.Include(g => g.City).Include(g => g.Dispatcher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Garages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await dbContext.Garages.Include(g => g.City).Include(g => g.Dispatcher).SingleOrDefaultAsync(m => m.ID == id);
            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        // GET: Garages/Create
        public IActionResult Create()
        {
            ViewData["CityID"] = new SelectList(dbContext.City, "ID", "Name");
            ViewData["DispatcherID"] = new SelectList(dbContext.Dispatchers, "ID", "FullName");

            return View();
        }

        // POST: Garages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CityID,DispatcherID,IsMaster,Name")] Garage garage)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(garage.Name))
                {
                    var city = await dbContext.City.SingleOrDefaultAsync(c => c.ID.Equals(garage.CityID));
                    garage.Name = city.Name;
                    garage.Pricelist = new Pricelist();
                }

                dbContext.Add(garage);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["CityID"] = new SelectList(dbContext.City, "ID", "ID", garage.CityID);
            ViewData["DispatcherID"] = new SelectList(dbContext.Dispatchers, "ID", "ID", garage.DispatcherID);
            ViewData["PricelistID"] = new SelectList(dbContext.Pricelist, "ID", "ID", garage.PricelistID);

            return View(garage);
        }

        // GET: Garages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await dbContext.Garages.Include(g => g.City).Include(g => g.Dispatcher).Include(g => g.Pricelist).SingleOrDefaultAsync(m => m.ID == id);
            if (garage == null)
            {
                return NotFound();
            }

            ViewData["CityID"] = new SelectList(dbContext.City, "ID", "Name", garage.CityID);
            ViewData["DispatcherID"] = new SelectList(dbContext.Dispatchers, "ID", "FullName", garage.DispatcherID);
            ViewData["PricelistID"] = new SelectList(dbContext.Pricelist, "ID", "ID", garage.PricelistID);

            return View(garage);
        }

        // POST: Garages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CityID,DispatcherID,IsMaster,Name,PhoneNumber,PhoneNumberMask, PricelistID")] Garage garage)
        {
            if (id != garage.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(garage);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarageExists(garage.ID))
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

            ViewData["CityID"] = new SelectList(dbContext.City, "ID", "Name", garage.CityID);
            ViewData["DispatcherID"] = new SelectList(dbContext.Dispatchers, "ID", "FullName", garage.DispatcherID);
            ViewData["PricelistID"] = new SelectList(dbContext.Pricelist, "ID", "ID", garage.PricelistID);

            return View(garage);
        }

        // GET: Garages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var garage = await dbContext.Garages.SingleOrDefaultAsync(m => m.ID == id);
            if (garage == null)
            {
                return NotFound();
            }

            return View(garage);
        }

        // POST: Garages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var garage = await dbContext.Garages.SingleOrDefaultAsync(m => m.ID == id);
            dbContext.Garages.Remove(garage);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool GarageExists(int id)
        {
            return dbContext.Garages.Any(e => e.ID == id);
        }
    }
}
