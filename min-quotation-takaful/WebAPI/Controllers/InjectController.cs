using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class InjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
