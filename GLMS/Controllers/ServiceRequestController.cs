using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    // This controller will handle service request pages
    public class ServiceRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
