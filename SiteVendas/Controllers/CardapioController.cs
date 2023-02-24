using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;

namespace SiteVendas.Controllers
{
    public class CardapioController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        [HttpGet]
        public IActionResult ListaProdutos()
        {
            var listaprodutos = context.tb_produto.Where(x => x.pd_disponivel.Equals(true)).ToList();

            return View(listaprodutos);
        }
    }
}