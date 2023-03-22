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
        // [Authorize(Roles = "Admin@hotmail.com")]
        // [Route("Mensagem/MarcarVisualizacaoMensagem/{idMensagem}")]
        [Route("Mensagem/MarcarVisualizacaoMensagem/{idMensagem}")]
        public IActionResult MarcarVisualizacaoMensagem(int idMensagem)
        {
            // if (idMensagem != null)
            // {
            //         var mensagem = context.tb_mensagens.Where(x => x.id_mensagem.Equals(idMensagem)).First();

            //         mensagem.mg_verificado = true;

            //         context.SaveChanges();
            // }


            return View("~/Home/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin@hotmail.com")]
        [Route("Mensagem/ExibirDetalhesMensagem/{_idMensagem}")]
        public IActionResult ExibirDetalhesMensagem(int _idMensagem)
        {
            var mensagem = context.tb_mensagens.Where(x => x.id_mensagem.Equals(_idMensagem)).ToList();

            ViewData["Mensagem"] = mensagem;

            return View();
            // return PartialView(mensagem);
        }

        //Pegar dados do cliente para gerar e-mail
        // [HttpPost]
        public IActionResult EnviarEmail(string destinatario, string nomeCliente)
        {
            // string destinatario = destinatario;
            string assunto = "Reserva Pizza House";
            string corpo = $"{nomeCliente}      Agredecemos a preferencia, estamos de aguardando!";

            string mailtoLink = string.Format("mailto:{0}?subject={1}&body={2}", destinatario, assunto, corpo);
            return Redirect(mailtoLink);
        }
    }
}