using Microsoft.AspNetCore.Mvc;

namespace SiteVendas.Controllers
{
    public class DadosEmpresaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}