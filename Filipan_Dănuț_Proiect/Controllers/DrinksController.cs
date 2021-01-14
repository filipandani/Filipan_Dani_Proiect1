using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Filipan_Dănuț_Proiect.Data;
using Filipan_Dănuț_Proiect.Models;

namespace Filipan_Dănuț_Proiect.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ShopContext _context;

        public DrinksController(ShopContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var drinks = from b in _context.Drinks
                         select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                drinks = drinks.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    drinks = drinks.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    drinks = drinks.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    drinks = drinks.OrderByDescending(b => b.Price);
                    break;
                default:
                    drinks = drinks.OrderBy(b => b.Name);
                    break; 
            }
                    int pageSize = 4;
                    return View(await PaginatedList<Drink>.CreateAsync(drinks.AsNoTracking(), pageNumber ?? 1, pageSize));
           
            return View(await drinks.AsNoTracking().ToListAsync());
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink   = await _context.Drinks
                                        .Include(s => s.Orders)
                                        .ThenInclude(e => e.Customer)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.ID == id);
            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Brand,Price,Liters")] Drink drink)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(drink);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(drink);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Drinks.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Drink>(
            studentToUpdate,
            "",
            s => s.Brand, s => s.Name, s => s.Price, s=>s.Liters))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var drink = await _context.Drinks
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (drink == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.ID == id);
        }
    }
}
