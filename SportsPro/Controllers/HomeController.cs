using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using System.Threading;

namespace SportsPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}