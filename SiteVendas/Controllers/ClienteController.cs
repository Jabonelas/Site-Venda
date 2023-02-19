using Correios;
using Microsoft.AspNetCore.Mvc;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;
using Correios.NET;
using Correios.CorreiosServiceReference;
using Exception = Correios.CorreiosServiceReference.Exception;
using Newtonsoft.Json;
using System.Net;
using SiteVendas.Class;

namespace SiteVendas.Controllers
{
    public class ClienteController : Controller
    {
        private SiteVendasContext db = new SiteVendasContext();

        [HttpGet]
        public IActionResult Inserir()
        {
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
                db.tb_endereco.Add(_cliente.endereco);
                db.SaveChanges();
                _cliente.cliente.fk_endereco = _cliente.endereco.id_endereco;
                db.tb_cadastro_cliente.Add(_cliente.cliente);
                db.SaveChanges();
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
    }
}