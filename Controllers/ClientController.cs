using Microsoft.AspNetCore.Mvc;

namespace WFSDev.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
