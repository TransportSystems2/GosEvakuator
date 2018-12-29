using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

using GosEvakuator.Data;
using GosEvakuator.Models;
using GosEvakuator.Services;
using System.Security.Claims;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class MembershipsController : Controller, IMembershipsService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public MembershipsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        
        public async Task<Membership> GetCurrentMembership(ClaimsPrincipal user)
        {
            //var userID = userManager.GetUserId(httpContext.User);

            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return await context.Membership.SingleOrDefaultAsync(m => m.ApplicationUserId.Equals(userId));
        }

        public async Task<Member> GetCurrentMember(ClaimsPrincipal user)
        {
            var membership = await GetCurrentMembership(user);

            if (membership.DispatcherID != null)
            {
                return await context.Dispatchers.SingleOrDefaultAsync(d => d.ID.Equals(membership.DispatcherID));
            }

            if (membership.DriverID != null)
            {
                return await context.Driver.SingleOrDefaultAsync(d => d.ID.Equals(membership.DriverID));
            }

            if (membership.CustomerID != null)
            {
                return await context.Customer.SingleOrDefaultAsync(c => c.ID.Equals(membership.CustomerID));
            }

            return null;
        }

        // GET: Memberships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = context.Membership.Include(m => m.ApplicationUser).Include(m => m.Customer).Include(m => m.Driver);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await context.Membership.SingleOrDefaultAsync(m => m.ID == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(context.Users, "Id", "Id");
            ViewData["CustomerID"] = new SelectList(context.Customer, "ID", "ID");
            ViewData["DriverID"] = new SelectList(context.Driver, "ID", "ID");
            return View();
        }

        // POST: Memberships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ApplicationUserId,CustomerID,DispatcherID,DriverID,Status")] Membership membership)
        {
            if (ModelState.IsValid)
            {
                context.Add(membership);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["ApplicationUserId"] = new SelectList(context.Users, "Id", "Id", membership.ApplicationUserId);
            ViewData["CustomerID"] = new SelectList(context.Customer, "ID", "ID", membership.CustomerID);
            ViewData["DriverID"] = new SelectList(context.Driver, "ID", "ID", membership.DriverID);

            return View(membership);
        }

        // GET: Memberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await context.Membership.SingleOrDefaultAsync(m => m.ID == id);
            if (membership == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(context.Users, "Id", "Id", membership.ApplicationUserId);
            ViewData["CustomerID"] = new SelectList(context.Customer, "ID", "ID", membership.CustomerID);
            ViewData["DriverID"] = new SelectList(context.Driver, "ID", "ID", membership.DriverID);

            return View(membership);
        }

        // POST: Memberships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApplicationUserId,CustomerID,DispatcherID,DriverID,Status")] Membership membership)
        {
            if (id != membership.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(membership);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.ID))
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
            ViewData["ApplicationUserId"] = new SelectList(context.Users, "Id", "Id", membership.ApplicationUserId);
            ViewData["CustomerID"] = new SelectList(context.Customer, "ID", "ID", membership.CustomerID);
            ViewData["DriverID"] = new SelectList(context.Driver, "ID", "ID", membership.DriverID);

            return View(membership);
        }

        // GET: Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await context.Membership.SingleOrDefaultAsync(m => m.ID == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membership = await context.Membership.SingleOrDefaultAsync(m => m.ID == id);
            context.Membership.Remove(membership);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool MembershipExists(int id)
        {
            return context.Membership.Any(e => e.ID == id);
        }
    }
}
