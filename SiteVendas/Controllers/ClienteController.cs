using Correios;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models.ViewModel;
using Correios.NET;

using Correios.CorreiosServiceReference;
using Exception = Correios.CorreiosServiceReference.Exception;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using SiteVendas.Class;
using SiteVendas.Models;

namespace SiteVendas.Controllers
{
    public class ClienteController : Controller
    {
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
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
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

            return View();
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

        [HttpGet]
        public IActionResult BuscarCadastro()
        {
            string usuario = HttpContext.Session.GetString("usuario");

            var dadosCliente = context.tb_cadastro_cliente.Join(context.tb_endereco, cliente => cliente.fk_endereco, endereco => endereco.id_endereco, (cliente, endereco) => new CadastroClienteViewModel
            {
                cliente = cliente,
                endereco = endereco
            }).Where(x => x.cliente.cc_email.Equals(usuario)).ToList();

            //var teste = new CadastroClienteViewModel();

            //foreach (var VARIABLE in dadosCliente)
            //{
            //    teste.cliente.cc_nome = VARIABLE.cliente.cc_nome;
            //    teste.cliente.cc_cpf = VARIABLE.cliente.cc_cpf;
            //    teste.cliente.cc_rg = VARIABLE.cliente.cc_rg;
            //    teste.cliente.cc_data_nascimento = VARIABLE.cliente.cc_data_nascimento;
            //    teste.cliente.cc_email = VARIABLE.cliente.cc_email;
            //    teste.cliente.cc_celular = VARIABLE.cliente.cc_celular;
            //    teste.cliente.cc_telefone = VARIABLE.cliente.cc_telefone;

            //    teste.endereco.ed_tipo = VARIABLE.endereco.ed_tipo;
            //    teste.endereco.ed_cep = VARIABLE.endereco.ed_cep;
            //    teste.endereco.ed_estado = VARIABLE.endereco.ed_estado;
            //    teste.endereco.ed_logradouro = VARIABLE.endereco.ed_logradouro;
            //    teste.endereco.ed_numero = VARIABLE.endereco.ed_numero;
            //    teste.endereco.ed_complemento = VARIABLE.endereco.ed_complemento;
            //    teste.endereco.ed_bairro = VARIABLE.endereco.ed_bairro;
            //    teste.endereco.ed_cidade = VARIABLE.endereco.ed_cidade;
            //}

            ViewData["ListaCliente"] = dadosCliente;

            return View();
        }

        [HttpPost]
        public IActionResult BuscarCadastro(CadastroClienteViewModel _dadosCliente)
        {
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
    }
}