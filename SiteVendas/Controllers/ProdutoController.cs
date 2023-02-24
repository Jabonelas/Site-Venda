using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Resources;
using SiteVendas.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Web;

namespace SiteVendas.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Cadastrar()
        {
            return View();
        }
    }
}