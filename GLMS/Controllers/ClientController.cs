using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace GLMS.Controllers
{
    // This controller now talks to the Web API instead of the database directly
    public class ClientController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GLMSApi");
        }

        // Show all clients from the API
        public async Task<IActionResult> Index()
        {
            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");

            if (clients == null)
            {
                clients = new List<Client>();
            }

            return View(clients);
        }

        // Show the empty create form
        public IActionResult Create()
        {
            return View();
        }

        // Send a new client to the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/clients", client);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Client could not be saved through the API.");
            }

            return View(client);
        }

        // For now, edit still loads from the API list
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            var client = clients?.FirstOrDefault(c => c.Id == id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // Update is left simple for Part 3 demo.
        // The main Part 3 requirement is to show MVC is using the API instead of SQL.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            ModelState.AddModelError("", "Edit through API can be added later. Current Part 3 focus is API decoupling.");
            return View(client);
        }

        // Show delete confirmation using API data
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
            var client = clients?.FirstOrDefault(c => c.Id == id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // Delete is left simple for Part 3 demo.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            TempData["Message"] = "Delete through API can be added later. Current Part 3 focus is API decoupling.";
            return RedirectToAction(nameof(Index));
        }
    }
}