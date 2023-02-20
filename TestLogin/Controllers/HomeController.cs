using Microsoft.AspNetCore.Mvc;

namespace TestLogin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
