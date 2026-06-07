using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace GLMS.Controllers
{
    // This controller now talks to the Web API instead of the database directly
    public class ServiceRequestController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CurrencyService _currencyService;

        public ServiceRequestController(
            IHttpClientFactory httpClientFactory,
            CurrencyService currencyService)
        {
            _httpClient = httpClientFactory.CreateClient("GLMSApi");
            _currencyService = currencyService;
        }

        // Show all service requests from the API
        public async Task<IActionResult> Index()
        {
            var serviceRequests = await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");

            return View(serviceRequests ?? new List<ServiceRequest>());
        }

        // Show the create form
        public async Task<IActionResult> Create()
        {
            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");

            ViewBag.ContractId = new SelectList(
                contracts ?? new List<Contract>(),
                "Id",
                "ContractDisplayName"
            );

            return View();
        }

        // Save a new service request through the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Currency conversion is still done before sending the request to the API
                    decimal exchangeRate = await _currencyService.GetUsdToZarRateAsync();
                    serviceRequest.CostZAR = _currencyService.ConvertUsdToZar(serviceRequest.CostUSD, exchangeRate);

                    var response = await _httpClient.PostAsJsonAsync("api/servicerequests", serviceRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", errorMessage);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Could not create the service request through the API.");
                }
            }

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");

            ViewBag.ContractId = new SelectList(
                contracts ?? new List<Contract>(),
                "Id",
                "ContractDisplayName",
                serviceRequest.ContractId
            );

            return View(serviceRequest);
        }

        // Show edit form using API data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");
            var serviceRequest = serviceRequests?.FirstOrDefault(sr => sr.Id == id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");

            ViewBag.ContractId = new SelectList(
                contracts ?? new List<Contract>(),
                "Id",
                "ContractDisplayName",
                serviceRequest.ContractId
            );

            return View(serviceRequest);
        }

        // Edit is kept simple for Part 3 because the main focus is API separation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            ModelState.AddModelError("", "Edit through API can be added later. Current Part 3 focus is API decoupling.");

            var contracts = await _httpClient.GetFromJsonAsync<List<Contract>>("api/contracts");

            ViewBag.ContractId = new SelectList(
                contracts ?? new List<Contract>(),
                "Id",
                "ContractDisplayName",
                serviceRequest.ContractId
            );

            return View(serviceRequest);
        }

        // Show delete confirmation using API data
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequests = await _httpClient.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");
            var serviceRequest = serviceRequests?.FirstOrDefault(sr => sr.Id == id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // Delete is kept simple for Part 3
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = "Delete through API can be added later. Current Part 3 focus is API decoupling.";
            return RedirectToAction(nameof(Index));
        }
    }
}