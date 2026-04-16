using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    // This controller will handle client pages
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
