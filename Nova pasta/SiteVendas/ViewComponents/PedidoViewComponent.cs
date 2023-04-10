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
    [Authorize(Roles = "Admin@hotmail.com")]
    public IViewComponentResult Invoke()
    {
        var pedidos = (from pedido in context.tb_pedido
                       where pedido.pd_entregue == null && pedido.pd_confirmado == true
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