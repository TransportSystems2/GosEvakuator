using GosEvakuator.Data;
using GosEvakuator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    [Authorize(Roles = Consts.Roles.Dispatcher)]
    [Authorize(Policy = "AcceptedMembership")]
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public DriversController(ApplicationDbContext context)
        {
            dbContext = context;    
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = dbContext.Driver.Include(d => d.Garage).ThenInclude(g => g.City)
                .Include(d => d.Membership).ThenInclude(m => m.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await dbContext.Driver.Include(d => d.Garage).ThenInclude(g => g.City).SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            } 

            return View(driver);
        }

        public async Task<IActionResult> Shedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await dbContext.Driver.SingleOrDefaultAsync(d => d.ID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return NotFound();
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await dbContext.Driver.SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            }

            ViewData["GarageID"] = new SelectList(dbContext.Garages.Include(g => g.City), "ID", "FullName", driver.GarageID);
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,GarageID,LastName,PhoneNumber")] Driver driver)
        {
            if (id != driver.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(driver);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.ID))
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

            ViewData["GarageID"] = new SelectList(dbContext.Garages.Include(g => g.City), "ID", "FullName", driver.GarageID);
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await dbContext.Driver.SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await dbContext.Driver.SingleOrDefaultAsync(m => m.ID == id);
            dbContext.Driver.Remove(driver);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool DriverExists(int id)
        {
            return dbContext.Driver.Any(e => e.ID == id);
        }
    }
}
