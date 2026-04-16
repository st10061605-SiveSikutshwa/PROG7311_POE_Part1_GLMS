using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    // This controller handles all contract CRUD actions
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;

        // Inject the database context into the controller
        public ContractController(AppDbContext context)
        {
            _context = context;
        }

        // Show all contracts with their linked client
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();

            return View(contracts);
        }

        // Show the create form
        public async Task<IActionResult> Create()
        {
            // Load all clients into a dropdown
            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "Id", "Name");
            return View();
        }

        // Save a new contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload the dropdown if validation fails
            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "Id", "Name", contract.ClientId);
            return View(contract);
        }

        // Show the edit form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "Id", "Name", contract.ClientId);
            return View(contract);
        }

        // Update an existing contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
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

            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "Id", "Name", contract.ClientId);
            return View(contract);
        }

        // Show the delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // Delete the selected contract
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a contract exists
        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(c => c.Id == id);
        }
    }
}