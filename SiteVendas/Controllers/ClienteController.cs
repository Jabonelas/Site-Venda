using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models.ViewModel;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using SiteVendas.Class;
using SiteVendas.Models;

namespace SiteVendas.Controllers
{
    public class ClienteController : Controller
    {
        private string cpf = string.Empty;
        private string rg = string.Empty;
        private SiteVendasContext context = new SiteVendasContext();

        [HttpGet]
        public IActionResult Inserir()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            if (usuario != null)
            {
                var dadosCliente = context.tb_cadastro_cliente.Join(context.tb_endereco, cliente => cliente.fk_endereco, endereco => endereco.id_endereco, (cliente, endereco) => new
                {
                    cliente,
                    endereco
                }).Where(x => x.cliente.cc_email.Equals(usuario)).ToList();

                return View(dadosCliente);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Inserir(CadastroClienteViewModel _cliente)
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

                return View(_cliente);
            }

            try
            {
                context.tb_endereco.Add(_cliente.endereco);
                context.SaveChanges();

                _cliente.cliente.fk_endereco = _cliente.endereco.id_endereco;

                context.tb_cadastro_cliente.Add(_cliente.cliente);
                context.SaveChanges();
            }
            catch (System.Exception)
            {
                return BadRequest("Erro ao Inserir Dados Cliente!");
            }

            string mensagem = "Operação realizada com sucesso!";
            TempData["mensagem"] = mensagem;

            return View();
        }

        [HttpPost]
        public JsonResult VerificarCPFExistente(string CPF)
        {
            string usuario = HttpContext.Session.GetString("usuario");

            if (usuario != null)
            {
                VerificarRGeCPFUsuarioLogado(usuario);
            }

            if (cpf.Equals(CPF))
            {
                return Json(false);
            }

            bool CPFExiste = context.tb_cadastro_cliente.Where(x => x.cc_cpf == CPF).Any();

            return Json(CPFExiste);
        }

        [HttpPost]
        public JsonResult VerificarRGExistente(string RG)
        {
            string usuario = HttpContext.Session.GetString("usuario");

            if (usuario != null)
            {
                VerificarRGeCPFUsuarioLogado(usuario);
            }

            if (rg.Equals(RG))
            {
                return Json(false);
            }

            bool RGExiste = context.tb_cadastro_cliente.Where(x => x.cc_rg == RG).Any();

            return Json(RGExiste);

        }

        public void VerificarRGeCPFUsuarioLogado(string usuario)
        {
            var verificarRGeCPF = context.tb_cadastro_cliente
            .Where(x => x.cc_email == usuario)
            .Select(x => new { x.cc_rg, x.cc_cpf })
            .FirstOrDefault();

            cpf = verificarRGeCPF.cc_cpf;
            rg = verificarRGeCPF.cc_rg;
        }

  

        [HttpPost]
        public JsonResult VerificarEmailExistente(string Email)
        {
            string usuario = HttpContext.Session.GetString("usuario");

            if (Email == usuario)
            {
                return Json(false);
            }

            bool emailExiste = context.tb_cadastro_cliente.Where(x => x.cc_email == Email).Any();

            return Json(emailExiste);
        }

        [HttpGet]
        [Route("Cliente/GetCep/{cep}")]
        public IActionResult GetCep(string cep)
        {
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString($"https://viacep.com.br/ws/{cep.Replace("-", "")}/json/");
                var cepModel = JsonConvert.DeserializeObject<CorreiosResponse>(json);
                return Ok(cepModel);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult AlterarCadastro()
        {
            BuscarDadosClienteCadastrado();

            return View();
        }

        [Authorize]
        private IActionResult BuscarDadosClienteCadastrado()
        {
            try
            {
                string usuario = HttpContext.Session.GetString("usuario");

                var dadosCliente = context.tb_cadastro_cliente.Join(context.tb_endereco, cliente => cliente.fk_endereco, endereco => endereco.id_endereco, (cliente, endereco) => new CadastroClienteViewModel
                {
                    cliente = cliente,
                    endereco = endereco
                }).Where(x => x.cliente.cc_email.Equals(usuario)).ToList();

                ViewData["ListaCliente"] = null;

                ViewData["ListaCliente"] = dadosCliente;

                return View();
            }
            catch (System.Exception e)
            {
                TempData["errorMessage"] = e.Message;

                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult BuscarCadastro(CadastroClienteViewModel _dadosCliente)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }

                BuscarDadosClienteCadastrado();

                return View();
            }

            string usuario = HttpContext.Session.GetString("usuario");

            var dadosCadastroCliente = context.tb_cadastro_cliente.Where(x => x.cc_email.Equals(usuario)).ToList();

            foreach (var item in dadosCadastroCliente)
            {
                item.cc_nome = _dadosCliente.cliente.cc_nome;
                item.cc_cpf = _dadosCliente.cliente.cc_cpf;
                item.cc_telefone = _dadosCliente.cliente.cc_telefone;
                item.cc_celular = _dadosCliente.cliente.cc_celular;
                item.cc_email = _dadosCliente.cliente.cc_email;
                item.cc_senha = _dadosCliente.cliente.cc_senha;
                item.cc_data_nascimento = _dadosCliente.cliente.cc_data_nascimento;
            }

            var dadosEnderecoCliente = context.tb_endereco.Where(x => x.id_endereco.Equals(_dadosCliente.endereco.id_endereco)).ToList();

            foreach (var item in dadosEnderecoCliente)
            {
                item.ed_cep = _dadosCliente.endereco.ed_cep;
                item.ed_logradouro = _dadosCliente.endereco.ed_logradouro;
                item.ed_numero = _dadosCliente.endereco.ed_numero;
                item.ed_complemento = _dadosCliente.endereco.ed_complemento;
                item.ed_bairro = _dadosCliente.endereco.ed_bairro;
                item.ed_cidade = _dadosCliente.endereco.ed_cidade;
                item.ed_estado = _dadosCliente.endereco.ed_estado;
            }

            context.SaveChanges();

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        [Authorize(Roles = "Admin@hotmail.com")]
        public IActionResult ExibirClientes()
        {
            var clientes = context.tb_cadastro_cliente.Select(x => x).ToList();

            return View(clientes);
        }

        [HttpGet]
        [Authorize(Roles = "Admin@hotmail.com")]
        [Route("Cliente/DetalheCliente/{_idCliente}")]
        public IActionResult DetalheCliente(int _idCliente)
        {
            var detalheCliente = context.tb_cadastro_cliente.Join(context.tb_endereco, cliente => cliente.fk_endereco,
            endereco => endereco.id_endereco, (cliente, endereco) =>
            new DetalhesClienteViewModel { cliente = cliente, endereco = endereco })
            .Where(x => x.cliente.id_cadastro_cliente == _idCliente).ToList();

            return View(detalheCliente);
        }
    }
}