using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
