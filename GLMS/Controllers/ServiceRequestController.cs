using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    // This controller handles service request CRUD actions
    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ContractValidationService _contractValidationService;

        // Inject the database context and validation service
        public ServiceRequestController(AppDbContext context, ContractValidationService contractValidationService)
        {
            _context = context;
            _contractValidationService = contractValidationService;
        }

        // Show all service requests with linked contract data
        public async Task<IActionResult> Index()
        {
            var serviceRequests = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return View(serviceRequests);
        }

        // Show the create form
        public async Task<IActionResult> Create()
        {
            ViewBag.ContractId = new SelectList(await _context.Contracts.Include(c => c.Client).ToListAsync(), "Id", "ContractDisplayName");
            return View();
        }

        // Save a new service request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            // Find the linked contract
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "The selected contract could not be found.");
            }
            else
            {
                // Apply the business rule from the POE
                if (!_contractValidationService.CanCreateServiceRequest(contract))
                {
                    ModelState.AddModelError("", "A service request cannot be created for a contract that is Expired or On Hold.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ContractId = new SelectList(await _context.Contracts.Include(c => c.Client).ToListAsync(), "Id", "Id", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // Show the edit form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            ViewBag.ContractId = new SelectList(await _context.Contracts.Include(c => c.Client).ToListAsync(), "Id", "Id", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // Update an existing service request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.Id))
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

            ViewBag.ContractId = new SelectList(await _context.Contracts.Include(c => c.Client).ToListAsync(), "Id", "Id", serviceRequest.ContractId);
            return View(serviceRequest);
        }

        // Show the delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // Delete the selected service request
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a service request exists
        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(sr => sr.Id == id);
        }
    }
}