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
            var provider = await _context.Providers
            .Include(i => i.ProvidedDrinks).ThenInclude(i => i.Drink)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (provider == null)
            {
                return NotFound();
            }
            PopulateProvidedDrinkData(provider);
            return View(provider);

        }
        private void PopulateProvidedDrinkData(Provider provider)
        {
            var allDrinks = _context.Drinks;
            var providerDrinks = new HashSet<int>(provider.ProvidedDrinks.Select(c => c.DrinkID));
            var viewModel = new List<ProvidedDrinkData>();
            foreach (var drink in allDrinks)
            {
                viewModel.Add(new ProvidedDrinkData
                {
                    DrinkID = drink.ID,
                    Name = drink.Name,
                    IsProvided = providerDrinks.Contains(drink.ID)
                });
            }
            ViewData["Drinks"] = viewModel;
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedDrinks)
        {
            if (id == null)
            {
                return NotFound();
            }
            var providerToUpdate = await _context.Providers
            .Include(i => i.ProvidedDrinks)
            .ThenInclude(i => i.Drink)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Provider>(providerToUpdate, "", i => i.ProviderName, i => i.Site))
            {
                UpdateProvidedDrink(selectedDrinks, providerToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateProvidedDrink(selectedDrinks, providerToUpdate);
            PopulateProvidedDrinkData(providerToUpdate);
            return View(providerToUpdate);
        }
        private void UpdateProvidedDrink(string[] selectedDrinks, Provider providerToUpdate)
        {
            if (selectedDrinks == null)
            {
                providerToUpdate.ProvidedDrinks = new List<ProvidedDrink>();
                return;
            }
            var selectedDrinksHS = new HashSet<string>(selectedDrinks);
            var providedDrinks = new HashSet<int>
            (providerToUpdate.ProvidedDrinks.Select(c => c.Drink.ID));
            foreach (var drink in _context.Drinks)
            {
                if (selectedDrinksHS.Contains(drink.ID.ToString()))
                {
                    if (!providedDrinks.Contains(drink.ID))
                    {
                        providerToUpdate.ProvidedDrinks.Add(new ProvidedDrink
                        {
                            ProviderID =
                       providerToUpdate.ID,
                            DrinkID = drink.ID
                        });
                    }
                }
                else
                {
                    if (providedDrinks.Contains(drink.ID))
                    {
                        ProvidedDrink drinkToRemove = providerToUpdate.ProvidedDrinks.FirstOrDefault(i
                       => i.DrinkID == drink.ID);
                        _context.Remove(drinkToRemove);
                    }
                }
            }
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
