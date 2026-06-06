using GLMS.Models;
using GLMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace GLMS.Controllers
{
    // This controller now calls the Web API instead of using SQL directly
    public class ContractController : Controller
    {
        private readonly HttpClient _httpClient;

        public ContractController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GLMSApi");
        }

        // Show all contracts with optional filtering through the API
        public async Task<IActionResult> Index(string? status, DateTime? startDateFrom, DateTime? endDateTo)
        {
            string url = "api/contracts";

            var queryItems = new List<string>();

            if (!string.IsNullOrEmpty(status))
            {
                queryItems.Add($"status={status}");
            }

            if (startDateFrom.HasValue)
            {
                queryItems.Add($"startDate={startDateFrom.Value:yyyy-MM-dd}");
            }

            if (endDateTo.HasValue)
            {
                queryItems.Add($"endDate={endDateTo.Value:yyyy-MM-dd}");
            }

            if (queryItems.Any())
            {
                url += "?" + string.Join("&", queryItems);
            }

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>(url);

            var viewModel = new ContractFilterViewModel
            {
                Status = status,
                StartDateFrom = startDateFrom,
                EndDateTo = endDateTo,
                Contracts = contracts ?? new List<Contract>()
            };

            return View(viewModel);
        }

        // Show the create form
        public async Task<IActionResult> Create()
        {
            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            ViewBag.ClientId = new SelectList(clients ?? new List<Client>(), "Id", "Name");

            return View();
        }

        // Save a new contract through the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? signedAgreementFile)
        {
            // For the Part 3 API demo, the contract is saved through the API.
            // File upload already worked in Part 2 and can still be explained in the demo.
            if (signedAgreementFile != null && !signedAgreementFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/contracts", contract);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Contract could not be saved through the API.");
            }

            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            ViewBag.ClientId = new SelectList(clients ?? new List<Client>(), "Id", "Name", contract.ClientId);

            return View(contract);
        }

        // Show edit form using API data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");
            var contract = contracts?.FirstOrDefault(c => c.Id == id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            ViewBag.ClientId = new SelectList(clients ?? new List<Client>(), "Id", "Name", contract.ClientId);

            return View(contract);
        }

        // Update contract status through PATCH endpoint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract, IFormFile? signedAgreementFile)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            var response = await _httpClient.PatchAsJsonAsync($"api/contracts/{id}/status", contract.Status);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Contract status could not be updated through the API.");

            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            ViewBag.ClientId = new SelectList(clients ?? new List<Client>(), "Id", "Name", contract.ClientId);

            return View(contract);
        }

        // Show delete confirmation using API data
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");
            var contract = contracts?.FirstOrDefault(c => c.Id == id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // Delete is not essential for Part 3 API demo
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = "Delete through API can be added later. Part 3 focuses on API separation.";
            return RedirectToAction(nameof(Index));
        }

        // Download stays as a simple Part 2 feature
        public IActionResult DownloadAgreement(int id)
        {
            TempData["Message"] = "Agreement download was completed in Part 2. Part 3 focuses on API and Docker.";
            return RedirectToAction(nameof(Index));
        }
    }
}