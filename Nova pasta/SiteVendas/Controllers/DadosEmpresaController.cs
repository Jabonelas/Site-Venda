using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using Microsoft.AspNetCore.Authorization;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.Controllers
{
    public class DadosEmpresaController : Controller
    {

        private SiteVendasContext context = new SiteVendasContext();

        [HttpGet]
        [Authorize(Roles = "Admin@hotmail.com")]
        public IActionResult Cadastrar()
        {
            BuscarDadosEmpresa();

            return View();
        }



        public void BuscarDadosEmpresa()
        {
           var listaDadosEmpresa = context.tb_endereco.Join(context.tb_dados_empresa, endereco => endereco.id_endereco, empresa => empresa.fk_endereco,
                       (endereco, empresa) => new DadosEmpresaViewModel
                       {
                           endereco = endereco,
                           dadosEmpresa = empresa,

                       }).Where(x => x.dadosEmpresa.id_dados_empresa.Equals(1)).ToList();

            ViewData["DadosEmpresa"] = null;
            ViewData["DadosEmpresa"] = listaDadosEmpresa;
        }

        [HttpPost]
        [Authorize(Roles = "Admin@hotmail.com")]
        public IActionResult CadastrarDados(DadosEmpresaViewModel _dadosEmpresa)
        {
            if (!ModelState.IsValid)
            {
                foreach (var stat in ModelState.Values)
                {
                    foreach (var erro in stat.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erro.ErrorMessage);
                    }
                }

                return View("Cadastrar", _dadosEmpresa);
            }

            var endereco = context.tb_endereco.Where(x => x.id_endereco.Equals(1));

            foreach (var item in endereco)
            {
                item.ed_tipo = _dadosEmpresa.endereco.ed_tipo;
                item.ed_cep = _dadosEmpresa.endereco.ed_cep;
                item.ed_estado = _dadosEmpresa.endereco.ed_estado;
                item.ed_logradouro = _dadosEmpresa.endereco.ed_logradouro;
                item.ed_numero = _dadosEmpresa.endereco.ed_numero;
                item.ed_complemento = _dadosEmpresa.endereco.ed_complemento;
                item.ed_bairro = _dadosEmpresa.endereco.ed_bairro;
                item.ed_cidade = _dadosEmpresa.endereco.ed_cidade;
            }

            context.SaveChanges();

            var dadosEmpresa = context.tb_dados_empresa.Where(x => x.id_dados_empresa.Equals(1));

            foreach (var item in dadosEmpresa)
            {
                item.de_cnpj = _dadosEmpresa.dadosEmpresa.de_cnpj;
                item.de_horario_fechamento = _dadosEmpresa.dadosEmpresa.de_horario_fechamento;
                item.de_horario_inicio = _dadosEmpresa.dadosEmpresa.de_horario_inicio;
                item.de_nome_fantasia = _dadosEmpresa.dadosEmpresa.de_nome_fantasia;
            }

            context.SaveChanges();

            BuscarDadosEmpresa();

            string mensagem = "Operação realizada com sucesso!";
            TempData["mensagem"] = mensagem;

            return View("Cadastrar");
        }






    }
}

