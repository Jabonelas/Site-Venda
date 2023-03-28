using AngleSharp.Dom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.ViewComponents;

public class PedidoViewComponent : ViewComponent
{
    private SiteVendasContext context = new SiteVendasContext();


    [HttpGet]
    // [Authorize(Roles = "Admin@hotmail.com")]
    public IViewComponentResult Invoke()
    {

        // List<PedidosViewModel> listaPedidos = new List<PedidosViewModel>();

        //  listaPedidos = (from pedido in context.tb_pedido
        //                             join cliente in context.tb_cadastro_cliente
        //                             on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
        //                             where pedido.pd_confirmado == true
        //                             group pedido by pedido.pd_numero_pedido into pedidosGroup
        //                             orderby pedidosGroup.Key descending
        //                             select new PedidosViewModel
        //                             {
        //                                 pedido = pedidosGroup.First(),
        //                                 valorTotal = pedidosGroup.Sum(p => p.pd_valor)
        //                             }).ToList();


        var pedidos = (from pedido in context.tb_pedido
                       where pedido.pd_entregue == null
                       group pedido by pedido.pd_numero_pedido into pedidosGroup
                       orderby pedidosGroup.Key descending
                       select new PedidosViewModel
                       {
                           pedido = pedidosGroup.First(),
                           valorTotal = pedidosGroup.Sum(p => p.pd_valor)
                       }).ToList();


        return View(pedidos);

    }
}