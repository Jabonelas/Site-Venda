using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace SiteVendas.Controllers
{
    public class ProdutoController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        public struct Image
        {
            public string Nome { get; set; }
            public string Tipo { get; set; }
            public byte[] Dados { get; set; }
        }

        [Authorize(Roles = "Admin@hotmail.com")]
        public async Task<IActionResult> Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin@hotmail.com")]
        public async Task<IActionResult> Cadastrar(tb_produto _produto, IFormFile imagem)
        {
            try
            {
                //Verifica se todos os campos estao preenchidos
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.ErrorMessage);
                        }
                    }

                    return View();
                }

                string formatoImagem = imagem.FileName.Split(".")[1];

                //Verifica se a imagem esta no formato correto
                if (formatoImagem == "jpg" || formatoImagem == "png")
                {
                    var image = new Image
                    {
                        Nome = imagem.FileName,
                        Tipo = imagem.ContentType
                    };

                    //Converter imgaem em byte
                    using var stream = new MemoryStream();
                    await imagem.CopyToAsync(stream);
                    image.Dados = stream.ToArray();

                    //DateOnly dataHoje = DateOnly.FromDateTime(DateTime.Now);

                    var dadosProduto = new tb_produto()
                    {
                        pd_preco = _produto.pd_preco,
                        pd_tipo = _produto.pd_tipo,
                        pd_tamanho = _produto.pd_tamanho,
                        pd_descricao = _produto.pd_descricao,
                        pd_disponivel = _produto.pd_disponivel,
                        pd_imagem = image.Dados,
                        pd_nome = _produto.pd_nome,
                        pd_data = DateTime.Today
                    };

                    context.tb_produto.Add(dadosProduto);
                    context.SaveChanges();

                    string mensagem = "Operação realizada com sucesso!";
                    TempData["mensagem"] = mensagem;

                    return RedirectToAction("Cadastrar");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Imagem em formato invalido!");
                }

                return View();
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;

                return View("~/Views/Home/Index.cshtml");
            }
        }

        private void CarregarListaProdutos()
        {
            var listaProdutos = context.tb_produto.Select(x => x).OrderBy(x => x.pd_data).ToList();

            ViewData["ListaProdutos"] = listaProdutos;
        }

        [Authorize(Roles = "Admin@hotmail.com")]
        public async Task<IActionResult> TodosProdutos()
        {
            CarregarListaProdutos();

            return View();
        }

        [Authorize(Roles = "Admin@hotmail.com")]
        public async Task<IActionResult> EditarProduto(int _idProduto)
        {
            BuscarProduto(_idProduto);

            return View();
        }

        [Authorize(Roles = "Admin@hotmail.com")]
        private List<tb_produto> BuscarProduto(int _idProduto)
        {
            var produto = context.tb_produto.Select(x => x).Where(x => x.id_produto.Equals(_idProduto)).ToList();

            ViewData["EditarProduto"] = null;
            ViewData["EditarProduto"] = produto;

            return produto;
        }

        [HttpPost]
        [Authorize(Roles = "Admin@hotmail.com")]
        public async Task<IActionResult> EditarProduto(tb_produto _produto, IFormFile imagem)
        {
            try
            {
                bool isCampoVazio = false;

                if (!ModelState.IsValid)
                {
                    foreach (var VARIABLE in ModelState)
                    {
                        if (VARIABLE.Key != "imagem" && !ModelState.IsValid)
                        {
                            foreach (var error in VARIABLE.Value.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.ErrorMessage);

                                isCampoVazio = true;
                            }
                        }
                    }

                    if (isCampoVazio == true)
                    {
                        BuscarProduto(_produto.id_produto);

                        return View();
                    }
                }

                if (imagem == null)
                {
                    foreach (var item in BuscarProduto(_produto.id_produto))
                    {
                        item.pd_nome = _produto.pd_nome;
                        item.pd_preco = _produto.pd_preco;
                        item.pd_tipo = _produto.pd_tipo;
                        item.pd_tamanho = _produto.pd_tamanho;
                        item.pd_descricao = _produto.pd_descricao;
                        item.pd_disponivel = _produto.pd_disponivel;
                    }

                    context.SaveChanges();

                    var listaProdutos = context.tb_produto.Select(x => x).ToList();

                    ViewData["ListaProdutos"] = null;
                    ViewData["ListaProdutos"] = listaProdutos;

                    return View("~/Views/Produto/TodosProdutos.cshtml");
                }
                else
                {
                    string formatoImagem = imagem.FileName.Split(".")[1];

                    //Verifica se a imagem esta no formato correto
                    if (formatoImagem == "jpg" || formatoImagem == "png")
                    {
                        var image = new Image
                        {
                            Nome = imagem.FileName,
                            Tipo = imagem.ContentType
                        };

                        //Converter imgaem em byte
                        using var stream = new MemoryStream();
                        await imagem.CopyToAsync(stream);
                        image.Dados = stream.ToArray();

                        foreach (var item in BuscarProduto(_produto.id_produto))
                        {
                            item.pd_nome = _produto.pd_nome;
                            item.pd_preco = _produto.pd_preco;
                            item.pd_tipo = _produto.pd_tipo;
                            item.pd_tamanho = _produto.pd_tamanho;
                            item.pd_descricao = _produto.pd_descricao;
                            item.pd_disponivel = _produto.pd_disponivel;
                            item.pd_imagem = image.Dados;
                        }

                        context.SaveChanges();

                        CarregarListaProdutos();

                        return View("~/Views/Produto/TodosProdutos.cshtml");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Imagem em formato invalido!");
                    }

                    return View();
                }
            }
            catch (Exception e)
            {
                TempData["errorMessage"] = e.Message;

                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin@hotmail.com")]
        public IActionResult DeletarProduto(int _idProduto)
        {
            var produtoDeletar = context.tb_produto.Where(x => x.id_produto.Equals(_idProduto)).FirstOrDefault();

            context.tb_produto.Remove(produtoDeletar);
            context.SaveChanges();

            CarregarListaProdutos();

            return View("~/Views/Produto/TodosProdutos.cshtml");
        }


    }
}