using System.ComponentModel.DataAnnotations;
using SiteVendas.Class;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class CadastroClienteViewModel
    {
        public tb_cadastro_cliente cliente { get; set; }

        public tb_endereco endereco { get; set; }

        public string confirmarSenha { get; set; }
    }
}