using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Filipan_Dănuț_Proiect.Data;
using Filipan_Dănuț_Proiect.Models;
using Filipan_Dănuț_Proiect.Models.ShopViewModels.ViewModel;

namespace Filipan_Dănuț_Proiect.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly ShopContext _context;

        public ProvidersController(ShopContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index(int? id, int? drinkID)
        {
            var viewModel = new ProviderIndexData();
            viewModel.Providers = await _context.Providers
                                                 .Include(i => i.ProvidedDrinks)
                                                 .ThenInclude(i => i.Drink)
                                                 .ThenInclude(i => i.Orders)
                                                 .ThenInclude(i => i.Customer)
                                                 .AsNoTracking()
                                                 .OrderBy(i => i.ProviderName)
                                                 .ToListAsync();
            if (id != null)
            {
                ViewData["ProviderID"] = id.Value;
                Provider provider = viewModel.Providers.Where( i => i.ID == id.Value).Single();
                viewModel.Drinks = provider.ProvidedDrinks.Select(s => s.Drink);
            }
            if (drinkID != null)
            {
                ViewData["DrinkID"] = drinkID.Value;
                viewModel.Orders = viewModel.Drinks.Where(x => x.ID == drinkID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProviderName,Site")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(provider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProviderName,Site")] Provider provider)
        {
            if (id != provider.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderExists(provider.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.ID == id);
        }
    }
}
