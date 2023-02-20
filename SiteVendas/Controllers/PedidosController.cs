using Correios.CorreiosServiceReference;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Context;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.Controllers
{
    public class PedidosController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        public IActionResult MeusPedidos()
        {
            var listaPedidos = context.tb_pedido.Join(context.tb_produto, pedido => pedido.fk_produto,
                produto => produto.id_produto, (pedido, produto) => new PedidosViewModel
                {
                    pedido = pedido,
                    produto = produto,
                }).Where(x => x.pedido.fk_produto == x.produto.id_produto).ToList();

            return View(listaPedidos);
        }
    }
}