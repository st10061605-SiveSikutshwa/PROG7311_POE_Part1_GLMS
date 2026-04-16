using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    // This controller will handle contract pages
    public class ContractController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
