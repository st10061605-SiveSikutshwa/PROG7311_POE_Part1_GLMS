using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    // This controller handles the home page and general pages
    public class HomeController : Controller
    {
        // Show the landing page
        public IActionResult Index()
        {
            return View();
        }

        // Show the references page
        public IActionResult References()
        {
            return View();
        }
    }
}