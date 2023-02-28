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
            var listaprodutos = context.tb_produto.Where(x => x.pd_disponivel == true && x.pd_tipo == "Pizza").OrderBy(x => x.pd_tipo).ToList();

            ViewData["ListaProdutosSelecionado"] = listaprodutos;

            return View(listaprodutos);
        }

        public IActionResult ListaProdutosBebida()
        {
            var listaProdutosBebida =
                context.tb_produto.Where(x => x.pd_disponivel == true && x.pd_tipo == "Bebida").ToList();

            ViewData["ListaProdutosSelecionado"] = listaProdutosBebida;

            return View("~/Views/Cardapio/ListaProdutos.cshtml", listaProdutosBebida);
        }

        public IActionResult BuscarProduto(string _nome)
        {
            var produto = context.tb_produto.Where(x => x.pd_nome.Contains(_nome)).ToList();

            ViewData["ListaProdutosSelecionado"] = produto;

            return View("~/Views/Cardapio/ListaProdutos.cshtml", produto);
        }
    }
}