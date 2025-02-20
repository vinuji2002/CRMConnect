using Microsoft.AspNetCore.Mvc;

namespace CRMConnect.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
