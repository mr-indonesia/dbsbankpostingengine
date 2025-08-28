using Microsoft.AspNetCore.Mvc;

namespace WebApps.Controllers
{
	public class NotificationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
