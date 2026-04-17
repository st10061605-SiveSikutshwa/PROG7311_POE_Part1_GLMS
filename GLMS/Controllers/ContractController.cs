using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using GLMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    // This controller handles all contract CRUD actions
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FileService _fileService;

        // Inject the database context and file service
        public ContractController(AppDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // Show all contracts with optional filtering
        public async Task<IActionResult> Index(string? status, DateTime? startDateFrom, DateTime? endDateTo)
        {
            // Start with all contracts and include the linked client
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            // Filter by status if the user selected one
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // Filter contracts that start on or after the selected start date
            if (startDateFrom.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDateFrom.Value);
            }

            // Filter contracts that end on or before the selected end date
            if (endDateTo.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDateTo.Value);
            }

            // Put the results into the view model
            var viewModel = new ContractFilterViewModel
            {
                Status = status,
                StartDateFrom = startDateFrom,
                EndDateTo = endDateTo,
                Contracts = await query.ToListAsync()
            };

            return View(viewModel);
        }

        // Show the create form
        public async Task<IActionResult> Create()
        {
            ViewBag.ClientId = new SelectList(await _context.Clients.ToListAsync(), "Id", "Name");
            return View();
        }

        // Save a new contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? signedAgreementFile)
        {
            // Validate the uploaded file if one was selected
            if (signedAgreementFile != null)
            {
                if (!_fileService.IsPdf(signedAgreementFile))
                {
                    ModelState.AddModelError("", "Only PDF files are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                // Save the PDF and store the path
                if (signedAgreementFile != null)
                {
                    contract.SignedAgreementPath = await _fileService.SavePdfAsync(signedAgreementFile);
                }

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
        public async Task<IActionResult> Edit(int id, Contract contract, IFormFile? signedAgreementFile)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            // Keep the old file path if no new file is uploaded
            var existingContract = await _context.Contracts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existingContract == null)
            {
                return NotFound();
            }

            contract.SignedAgreementPath = existingContract.SignedAgreementPath;

            if (signedAgreementFile != null)
            {
                if (!_fileService.IsPdf(signedAgreementFile))
                {
                    ModelState.AddModelError("", "Only PDF files are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (signedAgreementFile != null)
                    {
                        contract.SignedAgreementPath = await _fileService.SavePdfAsync(signedAgreementFile);
                    }

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

        // Download the uploaded PDF
        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
            {
                return NotFound();
            }

            string relativePath = contract.SignedAgreementPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(fileBytes, "application/pdf", "SignedAgreement.pdf");
        }

        // Helper method to check if a contract exists
        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(c => c.Id == id);
        }
    }
}