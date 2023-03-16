using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;

namespace SiteVendas.Controllers
{
    public class MensagemController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        [HttpPost]
        public IActionResult InserirMensagem(tb_mensagens _mensagem)
        {
            var mensagem = new tb_mensagens()
            {
                mg_nome = _mensagem.mg_nome,
                mg_email = _mensagem.mg_email,
                mg_servico = _mensagem.mg_servico,
                mg_mensagem = _mensagem.id_mensagem,
                mg_verificado = false,
                mg_exibir = false,
            };

            string mensagemSucesso = "Operação realizada com sucesso!";
            TempData["mensagemSucesso"] = mensagemSucesso;

            // context.tb_mensagens.Add(mensagem);
            // context.SaveChanges();

            // return RedirectResult("~/Views/Home/Index.cshtml");

            return View("~/Home/Index.cshtml");
            // return (IAsyncResult)View("~/Views/Home/Index.cshtml");

        }
    }
}