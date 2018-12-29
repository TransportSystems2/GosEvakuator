using GosEvakuator.Data;
using GosEvakuator.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class DispatchersController : Controller
    {
        private readonly ApplicationDbContext context;

        public DispatchersController(ApplicationDbContext context)
        {
            this.context = context;    
        }

        // GET: Dispatchers
        public async Task<IActionResult> Index()
        {
            return View(await context.Dispatchers.Include(d => d.Membership).ThenInclude(m => m.ApplicationUser).ToListAsync());
        }

        // GET: Dispatchers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispatcher = await context.Dispatchers.Include(d => d.Membership).ThenInclude(m => m.ApplicationUser).SingleOrDefaultAsync(d => d.ID == id);
            if (dispatcher == null)
            {
                return NotFound();
            }

            return View(dispatcher);
        }

        // GET: Dispatchers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispatcher = await context.Dispatchers.Include(d => d.Membership).ThenInclude(m => m.ApplicationUser).SingleOrDefaultAsync(m => m.ID == id);
            if (dispatcher == null)
            {
                return NotFound();
            }

            return View(dispatcher);
        }

        // POST: Dispatchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] Dispatcher dispatcher)
        {
            if (id != dispatcher.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(dispatcher);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispatcherExists(dispatcher.ID))
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
            return View(dispatcher);
        }

        // GET: Dispatchers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispatcher = await context.Dispatchers.SingleOrDefaultAsync(m => m.ID == id);
            if (dispatcher == null)
            {
                return NotFound();
            }

            return View(dispatcher);
        }

        // POST: Dispatchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dispatcher = await context.Dispatchers.SingleOrDefaultAsync(m => m.ID == id);
            context.Dispatchers.Remove(dispatcher);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DispatcherExists(int id)
        {
            return context.Dispatchers.Any(e => e.ID == id);
        }
    }
}
