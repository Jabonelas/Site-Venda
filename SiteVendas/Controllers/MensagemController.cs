using Microsoft.AspNetCore.Authorization;
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

            DateTime dataAtual = DateTime.Now;

            var mensagem = new tb_mensagens()
            {
                mg_nome = _mensagem.mg_nome,
                mg_email = _mensagem.mg_email,
                mg_servico = _mensagem.mg_servico,
                mg_mensagem = _mensagem.mg_mensagem,
                mg_verificado = false,
                mg_exibir = false,
                mg_data = dataAtual,
            };


            context.tb_mensagens.Add(mensagem);
            context.SaveChanges();


            return View("~/Home/Index.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "Admin@hotmail.com")]
        // [Route("Mensagem/MarcarVisualizacaoMensagem/{_idCliente}")]
        public void MarcarVisualizacaoMensagem(List<int> _idMensagem)
        {
            foreach (var item in _idMensagem)
            {

                var mensagem = context.tb_mensagens.Where(x => x.id_mensagem.Equals(item)).First();

                mensagem.mg_verificado = true;

                context.SaveChanges();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin@hotmail.com")]
        [Route("Mensagem/ExibirDetalhesMensagem/{_idMensagem}")]
        public IActionResult ExibirDetalhesMensagem(int _idMensagem)
        {
            var mensagem = context.tb_mensagens.Where(x => x.id_mensagem.Equals(_idMensagem)).ToList();

ViewData["Mensagem"] = mensagem;


            return View();
        }
    }
}