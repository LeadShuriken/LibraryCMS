using F84396_LibraryCMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace F84396_LibraryCMS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
