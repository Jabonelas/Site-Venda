using Microsoft.AspNetCore.Mvc;

namespace SiteVendas.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}