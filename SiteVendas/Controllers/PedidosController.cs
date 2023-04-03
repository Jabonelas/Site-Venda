using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.Controllers
{
    public class PedidosController : Controller
    {
        private int idPedido = 0;
        private int idUsuario = 0;
        private int idProduto = 0;
        private int quantidade = 0;
        private decimal valor = 0;

        private SiteVendasContext context = new SiteVendasContext();

        [HttpGet]
        // [Authorize]
        public IActionResult MeusPedidos()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            List<PedidosViewModel> listaPedidos = new List<PedidosViewModel>();

            if (usuario == "Admin@hotmail.com")
            {
                listaPedidos = (from pedido in context.tb_pedido
                                join cliente in context.tb_cadastro_cliente
                                on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
                                where pedido.pd_confirmado == true
                                group pedido by pedido.pd_numero_pedido into pedidosGroup
                                orderby pedidosGroup.Key descending
                                select new PedidosViewModel
                                {
                                    pedido = pedidosGroup.First(),
                                    valorTotal = pedidosGroup.Sum(p => p.pd_valor)
                                }).ToList();

            }
            else
            {
                listaPedidos = (from pedido in context.tb_pedido
                                join cliente in context.tb_cadastro_cliente
                                on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
                                where cliente.cc_email == usuario && pedido.pd_confirmado == true
                                group pedido by pedido.pd_numero_pedido into pedidosGroup
                                orderby pedidosGroup.Key descending
                                select new PedidosViewModel
                                {
                                    pedido = pedidosGroup.First(),
                                    valorTotal = pedidosGroup.Sum(p => p.pd_valor)
                                }).ToList();


            }

            return View(listaPedidos);
        }

        [HttpPost]
        [Authorize]
        public IActionResult BuscarPedidosCarrinho(int _idProduto, string _valor, int _quantidade)
        {
            idProduto = _idProduto;

            quantidade = _quantidade;

            valor = Convert.ToDecimal(_valor.Replace(".", ","));

            BuscarIdUsuario();

            bool IsCarrinhoExiste = VerificarExistenciaPedido();

            if (IsCarrinhoExiste == true)
            {
                AdicionarPedidosNoCarrinhoJaExistente(idPedido);
            }
            else
            {
                AdicionarPedidoPrimeiroProdutoNoCarrinho();
            }

            return Ok();
        }

        private void AdicionarPedidoPrimeiroProdutoNoCarrinho()
        {
            var pedido = new tb_pedido()
            {
                pd_quantidade = quantidade,
                pd_data = DateTime.Today,
                pd_valor = valor,
                pd_confirmado = false,
                fk_cadastro_cliente = idUsuario,
                fk_produto = idProduto,
            };

            context.tb_pedido.Add(pedido);
            context.SaveChanges();

            int idPedido = pedido.id_pedido;

            InserirNumeroPedido(idPedido);
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
                fk_cadastro_cliente = idUsuario,
                fk_produto = idProduto,
            };

            context.tb_pedido.Add(pedido);
            context.SaveChanges();
        }

        private void BuscarIdUsuario()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            var idUsuario = context.tb_cadastro_cliente.Where(x => x.cc_email.Equals(usuario)).Select(x => x.id_cadastro_cliente).FirstOrDefault();

            this.idUsuario = idUsuario;
        }

        private bool VerificarExistenciaPedido()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            var buscarProdutosCarrinho = context.tb_cadastro_cliente.Join(context.tb_pedido, cliente => cliente.id_cadastro_cliente,
                pedido => pedido.fk_cadastro_cliente, (cliente, pedido) => new
                {
                    Cliente = cliente,
                    Pedido = pedido,
                }).Where(x => x.Cliente.id_cadastro_cliente == idUsuario && x.Pedido.pd_confirmado == false).Select(x => x.Pedido.pd_numero_pedido).FirstOrDefault();

            if (buscarProdutosCarrinho != null)
            {
                idPedido = Convert.ToInt32(buscarProdutosCarrinho);

                return true;
            }

            return false;
        }

        [HttpDelete]
        [Authorize]
        public IActionResult ExcluirPedido(int _idProduto)
        {
            var pedidoExcluir = context.tb_pedido.Where(x => x.id_pedido.Equals(_idProduto)).FirstOrDefault();

            context.tb_pedido.Remove(pedidoExcluir);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult CarrinhoCompra()
        {
            //string usuario = "Admin@hotmail.com";
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

            ViewData["quantidadePedidos1"] = null;

            ViewData["quantidadePedidos1"] = quantidadePedidos;

            BuscarChaviPix();

            return View();
        }

        private void BuscarChaviPix()
        {
            var chavePix = context.tb_dados_empresa.Select(x => x.de_cnpj).FirstOrDefault();

            ViewData["chavePix"] = null;

            ViewData["chavePix"] = chavePix;
        }

        // public IActionResult FinalizarPedido(int _numeroPedido, string _tipoPagamento, string _troco)
        public void FinalizarPedido(int _numeroPedido, string _tipoPagamento, string _troco)
        {
            // if (!ModelState.IsValid)
            // {
            //     foreach (var stat in ModelState.Values)
            //     {
            //         foreach (var erro in stat.Errors)
            //         {
            //             ModelState.AddModelError(string.Empty, erro.ErrorMessage);
            //         }
            //     }

            //     return RedirectToAction("CarrinhoCompra");
            // }

            var finalizarPedido = context.tb_pedido.Where(x => x.pd_numero_pedido.Equals(_numeroPedido)).ToList();

            decimal valorTroco = 0;

            if (_troco != null)
            {
                valorTroco = Convert.ToDecimal(_troco.Replace("R$ ", ""));
            }

            DateTime dataHoraAtual = DateTime.Now;
            string formato = "dd/MM/yyyy HH:mm:ss"; // Formato desejado
            string dataHoraAtualFormatada = dataHoraAtual.ToString(formato);

            DateTime dataAtual = DateTime.Now;

            foreach (var item in finalizarPedido)
            {
                item.pd_confirmado = true;
                item.pd_data = dataAtual;
                item.pd_tipo_pagamento = _tipoPagamento;
                item.pd_troco_para = valorTroco;

                var inserirNF = new tb_nota_fiscal()
                {
                    fk_pedido = item.id_pedido
                };

                context.tb_nota_fiscal.Add(inserirNF);
                context.SaveChanges();
            }
        }

        [HttpGet]
        [Authorize]
        // [Authorize(Roles = "Admin@hotmail.com")]
        [Route("Pedidos/DetalhePedido/{_numeroPedido}")]
        public IActionResult DetalhePedido(int _numeroPedido)
        {
            List<DetalhesPedidoViewModel> listaDetalhePedido = new List<DetalhesPedidoViewModel>();

            listaDetalhePedido.Clear();

            listaDetalhePedido =
                           (from pedido in context.tb_pedido
                            join produto in context.tb_produto
                                on pedido.fk_produto equals produto.id_produto
                            join cliente in context.tb_cadastro_cliente
                                on pedido.fk_cadastro_cliente equals cliente.id_cadastro_cliente
                            join notaFiscal in context.tb_nota_fiscal
                                on pedido.id_pedido equals notaFiscal.fk_pedido
                            join endereco in context.tb_endereco
                                on cliente.fk_endereco equals endereco.id_endereco
                            where pedido.pd_numero_pedido == _numeroPedido
                            orderby pedido.pd_data
                            select new DetalhesPedidoViewModel
                            {
                                produto = produto,
                                pedido = pedido,
                                cliente = cliente,
                                notaFiscal = notaFiscal,
                                endereco = endereco
                            }).ToList();

            return View(listaDetalhePedido);
        }

        [HttpPost]
        [Authorize(Roles = "Admin@hotmail.com")]
        [Route("Pedidos/DetalhePedido/{_numeroPedido}")]
        public IActionResult EntregaRealizada(int _numeroPedido)
        {
            var pedidoEntregue = context.tb_pedido.Where(x => x.pd_numero_pedido.Equals(_numeroPedido)).ToList();

            DateTime dataHoraAtual = DateTime.Now;
            string formato = "dd/MM/yyyy HH:mm:ss"; // Formato desejado
            string dataHoraAtualFormatada = dataHoraAtual.ToString(formato);

            DateTime dataAtual = DateTime.Now;

            foreach (var item in pedidoEntregue)
            {
                item.pd_entregue = dataAtual;
            }

            context.SaveChanges();

            return RedirectToAction("MeusPedidos");
        }

    }
}