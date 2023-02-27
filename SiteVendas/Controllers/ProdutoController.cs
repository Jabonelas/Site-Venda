using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Resources;
using SiteVendas.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Web;
using SiteVendas.Class;
using System.Net;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using SiteVendas.Class;
using SiteVendas.Models;

using SiteVendas.Models.ViewModel;
using static SiteVendas.Controllers.ProdutoController;
using Microsoft.JSInterop;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System;

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

        public async Task<IActionResult> teste()
        {
            return View();
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

                    var dadosProduto = new tb_produto()
                    {
                        pd_preco = _produto.pd_preco,
                        pd_tipo = _produto.pd_tipo,
                        pd_tamanho = _produto.pd_tamanho,
                        pd_descricao = _produto.pd_descricao,
                        pd_disponivel = _produto.pd_disponivel,
                        pd_imagem = image.Dados,
                        pd_nome = _produto.pd_nome
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

        public async Task<IActionResult> TodosProdutos()
        {
            var listaProdutos = context.tb_produto.Select(x => x).ToList();

            ViewData["ListaProdutos"] = listaProdutos;
            return View();
        }

        public async Task<IActionResult> EditarProduto(int _idProduto)
        {
            BuscarProduto(_idProduto);

            return View();
        }

        private List<tb_produto> BuscarProduto(int _idProduto)
        {
            var produto = context.tb_produto.Select(x => x).Where(x => x.id_produto.Equals(_idProduto)).ToList();

            ViewData["EditarProduto"] = produto;

            return produto;
        }

        [HttpPost]
        public async Task<IActionResult> EditarProduto(tb_produto _produto, IFormFile imagem)
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

                    BuscarProduto(_produto.id_produto);

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

                    var listaProdutos = context.tb_produto.Select(x => x).ToList();

                    ViewData["ListaProdutos"] = listaProdutos;

                    return View("~/Views/Produto/TodosProdutos.cshtml");
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

        public IActionResult Exibir()
        {
            var image = context.tb_produto.Where(x => x.id_produto == 4).ToList();
            //.Select(x => x.pd_imagem);

            //List<tb_produto> teste = new List<tb_produto>();

            //foreach (var item in image)
            //{
            //    teste.Add(new tb_produto
            //    {
            //        pd_tipo = item.pd_tipo,
            //        pd_nome = item.pd_nome,
            //        pd_descricao = item.pd_descricao,
            //        pd_tamanho = item.pd_tamanho,
            //        pd_preco = item.pd_preco,
            //        pd_disponivel = item.pd_disponivel,
            //        pd_imagem = item.pd_imagem
            //    });
            //}

            ////return View(teste);

            //foreach (var VARIABLE in teste)
            //{
            //    return File(VARIABLE.pd_imagem, "teste");
            //}

            return View(image);

            //var image1 = new Image
            //{
            //    Nome = imagem.FileName,
            //    Tipo = imagem.ContentType,
            //    Dados = image
            //};

            ////Converter imgaem em byte
            //using var stream = new MemoryStream();
            //await imagem.CopyToAsync(stream);
            //image.Dados = stream.ToArray();

            //ViewData["ListaProduto"] = image;

            //return File(image.Dados, image.Tipo);
        }
    }
}