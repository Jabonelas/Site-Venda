using Correios.CorreiosServiceReference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.Controllers
{
    public class PedidosController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        [Authorize]
        public IActionResult MeusPedidos()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            List<PedidosViewModel> listaPedidos = new List<PedidosViewModel>();

            if (usuario == "Admin@hotmail.com")
            {
                listaPedidos =
                   (from pedido in context.tb_pedido
                    join produto in context.tb_produto
                         on pedido.fk_produto equals produto.id_produto
                    join cliente in context.tb_cadastro_cliente
                         on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
                    orderby pedido.pd_data
                    select new PedidosViewModel
                    {
                        produto = produto,
                        pedido = pedido,
                        cliente = cliente
                    }).ToList();
            }
            else
            {
                listaPedidos =
                   (from pedido in context.tb_pedido
                    join produto in context.tb_produto
                        on pedido.fk_produto equals produto.id_produto
                    join cliente in context.tb_cadastro_cliente
                        on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
                    where cliente.cc_email == usuario
                    orderby pedido.pd_data
                    select new PedidosViewModel
                    {
                        produto = produto,
                        pedido = pedido,
                        cliente = cliente
                    }).ToList();
            }

            return View(listaPedidos);
        }
    }
}