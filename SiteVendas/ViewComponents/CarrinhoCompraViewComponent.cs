using AngleSharp.Dom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.ViewComponents;

public class CarrinhoCompraViewComponent : ViewComponent
{
    private SiteVendasContext context = new SiteVendasContext();


[HttpGet]
[Authorize]
    public IViewComponentResult Invoke()
    {
        string usuario = HttpContext.Session.GetString("usuario");

        var quantidadePedidos = context.tb_cadastro_cliente.Join(context.tb_pedido, cliente => cliente.id_cadastro_cliente,
            pedido => pedido.fk_cadastro_cliente, (cliente, pedido) => new
            {
                Cliente = cliente,
                Pedido = pedido,
            }).Join(context.tb_produto, pedido => pedido.Pedido.fk_produto, produto => produto.id_produto, (pedido1, produto) => new
            {
                Pedido = pedido1.Pedido,
                Produto = produto,
                Cliente = pedido1.Cliente
            })
            .Where(x => x.Cliente.cc_email == usuario && x.Pedido.pd_confirmado == false)
            .Select(x => new CarrinhoViewModel
            {
                idPedido = x.Pedido.id_pedido,
                idProduto = x.Produto.id_produto,
                numeroPedido = x.Pedido.pd_numero_pedido,
                quantidade = x.Pedido.pd_quantidade,
                valorTotal = x.Pedido.pd_valor,
                imagem = x.Produto.pd_imagem,
                nome = x.Produto.pd_nome,
                tamanho = x.Produto.pd_tamanho,
                valorUnitario = x.Produto.pd_preco
            }).ToList();

        ViewData["quantidadePedidos"] = null;

        ViewData["quantidadePedidos"] = quantidadePedidos;

        return View();
    }
}