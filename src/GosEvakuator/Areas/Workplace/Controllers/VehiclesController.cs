using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using GosEvakuator.Data;
using GosEvakuator.Models;
using Microsoft.AspNetCore.Identity;
using GosEvakuator.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GosEvakuator.Services;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMembershipsService memberships;

        public VehiclesController(IMembershipsService memberships, ApplicationDbContext context)
        {
            this.memberships = memberships;
            dbContext = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = dbContext.Vehicle.Include(v => v.Garage).ThenInclude(g => g.City).Include(v => v.Shedule);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await dbContext.Vehicle.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public async Task<IActionResult> Create()
        {
            var garages = await GetGaragesForCurrentMember();
            ViewData["GarageID"] = new SelectList(garages, "ID", "Name");

            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GarageID,Name,RegistrationNumber,SheduleID")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                var currentMembership = await memberships.GetCurrentMembership(HttpContext.User);
                vehicle.MembershipID = currentMembership.ID;

                dbContext.Add(vehicle);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["GarageID"] = new SelectList(dbContext.Garages.Include(g => g.City), "ID", "FullName", vehicle.GarageID);
            ViewData["SheduleID"] = new SelectList(dbContext.Shedule, "ID", "ID", vehicle.SheduleID);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await dbContext.Vehicle.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["GarageID"] = new SelectList(dbContext.Garages, "ID", "ID", vehicle.GarageID);
            ViewData["SheduleID"] = new SelectList(dbContext.Shedule, "ID", "ID", vehicle.SheduleID);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GarageID,Name,RegistrationNumber,SheduleID")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(vehicle);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.ID))
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
            ViewData["GarageID"] = new SelectList(dbContext.Garages, "ID", "ID", vehicle.GarageID);
            ViewData["SheduleID"] = new SelectList(dbContext.Shedule, "ID", "ID", vehicle.SheduleID);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await dbContext.Vehicle.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await dbContext.Vehicle.SingleOrDefaultAsync(m => m.ID == id);
            dbContext.Vehicle.Remove(vehicle);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VehicleExists(int id)
        {
            return dbContext.Vehicle.Any(e => e.ID == id);
        }

        private async Task<List<Garage>> GetGaragesForCurrentMember()
        {
            var result = new List<Garage>();
            var currentMember = await memberships.GetCurrentMember(HttpContext.User);

            if (currentMember == null)
            {
                return result;
            }

            var driver = currentMember as Driver;
            if (driver != null)
            {
                var garage = await dbContext.Garages.SingleOrDefaultAsync(g => g.ID.Equals(driver.GarageID));
                result.Add(garage);
            }

            var dispatcher = currentMember as Dispatcher;
            if (dispatcher != null)
            {
                dispatcher = await dbContext.Dispatchers.Include(d => d.Garages).SingleOrDefaultAsync(d => d.ID.Equals(dispatcher.ID));
                result.AddRange(dispatcher.Garages);
            }

            return result;
        }
    }
}