using System.ComponentModel.DataAnnotations;
using SiteVendas.Class;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class CadastroClienteViewModel
    {
        [Required]
        public tb_cadastro_cliente cliente { get; set; }

        [Required]
        public tb_endereco endereco { get; set; }

        [Required]
        public string confirmarSenha { get; set; }
    }
}