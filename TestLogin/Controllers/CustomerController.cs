using Microsoft.AspNetCore.Mvc;

namespace TestLogin.Controllers
{
	public class CustomerController : Controller
	{
		[Route("Customer")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
