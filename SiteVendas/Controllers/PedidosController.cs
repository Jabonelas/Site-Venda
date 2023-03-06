using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public IActionResult AdicionarPedidos(tb_pedido _pedido)
        {
            return View();
        }

        private int idProduto = 0;

        private int quantidade = 0;

        private decimal valor = 0;

        [HttpPost]
        public IActionResult BuscarPedidosCarrinho(int _idProduto, string _valor, int _quantidade)
        {
            idProduto = _idProduto;

            quantidade = _quantidade;

            valor = Convert.ToDecimal(_valor.Replace(".", ","));

            BuscarIdUsuario();

            bool IsCarrinhoExiste = VerificarExistenciaPedido();

            if (IsCarrinhoExiste == true)
            {
                AdicionarPedidosNoCarrinhoJaExistente(IdPedido);
            }
            else
            {
                AdicionarPedidoPrimeiroProdutoNoCarrinho();
            }

            return Ok();

            //return View("~/Views/Cardapio/ListaProdutos.cshtml");
        }

        private void AdicionarPedidoPrimeiroProdutoNoCarrinho()
        {
            var pedido = new tb_pedido()
            {
                pd_quantidade = quantidade,
                pd_data = DateTime.Today,
                pd_valor = valor,
                pd_confirmado = false,
                fk_cadastro_cliente = IdUsuario,
                fk_produto = idProduto,
            };

            //context.tb_pedido.Add(pedido);
            //context.SaveChanges();

            //int idPedido = pedido.id_pedido;

            //InserirNumeroPedido(idPedido);
        }

        private void InserirNumeroPedido(int _idPedido)
        {
            var pedido = context.tb_pedido.Where(x => x.id_pedido.Equals(_idPedido));

            foreach (var item in pedido)
            {
                item.pd_numero_pedido = _idPedido;
            }

            context.SaveChanges();
        }

        private void AdicionarPedidosNoCarrinhoJaExistente(int _numeroPedido)
        {
            var pedido = new tb_pedido
            {
                pd_numero_pedido = _numeroPedido,
                pd_quantidade = quantidade,
                pd_data = DateTime.Today,
                pd_valor = valor,
                pd_confirmado = false,
                fk_cadastro_cliente = IdUsuario,
                fk_produto = idProduto,
            };

            //context.tb_pedido.Add(pedido);
            //context.SaveChanges();
        }

        private int IdUsuario = 0;

        private void BuscarIdUsuario()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            var idUsuario = context.tb_cadastro_cliente.Where(x => x.cc_email.Equals(usuario)).Select(x => x.id_cadastro_cliente).FirstOrDefault();

            IdUsuario = idUsuario;
        }

        private int IdPedido = 0;

        private bool VerificarExistenciaPedido()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            var buscarProdutosCarrinho = context.tb_cadastro_cliente.Join(context.tb_pedido, cliente => cliente.id_cadastro_cliente,
                pedido => pedido.fk_cadastro_cliente, (cliente, pedido) => new
                {
                    Cliente = cliente,
                    Pedido = pedido,
                }).Where(x => x.Cliente.id_cadastro_cliente == IdUsuario && x.Pedido.pd_confirmado == false).Select(x => x.Pedido.pd_numero_pedido);

            if (!buscarProdutosCarrinho.IsNullOrEmpty())
            {
                foreach (var VARIABLE in buscarProdutosCarrinho)
                {
                    IdPedido = VARIABLE.Value;

                    //AdicionarPedidosNoCarrinhoJaExistente(IdPedido);
                }
                return true;
            }

            return false;
            //AdicionarPedidoPrimeiroProdutoNoCarrinho();
        }
    }
}