using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
