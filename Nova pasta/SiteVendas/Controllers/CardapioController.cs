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
            try
            {
                var listaprodutos = context.tb_produto.Where(x => x.pd_disponivel == true && x.pd_tipo == "Pizza")
                        .OrderBy(x => x.pd_tipo).ToList();

                ViewData["ListaProdutosSelecionado"] = null;
                ViewData["ListaProdutosSelecionado"] = listaprodutos;

                return View(listaprodutos);
            }
            catch (Exception x)
            {
                Console.WriteLine($"teste {x.ToString()}");

                return View();
            }
        }

        public IActionResult ListaProdutosBebida()
        {
            var listaProdutosBebida = context.tb_produto.Where(x => x.pd_disponivel == true && x.pd_tipo == "Bebida").ToList();

            ViewData["ListaProdutosSelecionado"] = null;
            ViewData["ListaProdutosSelecionado"] = listaProdutosBebida;

            return View("~/Views/Cardapio/ListaProdutos.cshtml", listaProdutosBebida);
        }

        public IActionResult BuscarProduto(string pesquisa)
        {
            var produto = context.tb_produto.Where(x => x.pd_nome.Contains(pesquisa)).ToList();

            ViewData["ListaProdutosSelecionado"] = null;
            ViewData["ListaProdutosSelecionado"] = produto;

            return View("~/Views/Cardapio/ListaProdutos.cshtml", produto);
        }
    }
}