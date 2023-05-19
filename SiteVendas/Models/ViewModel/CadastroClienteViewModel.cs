using System.ComponentModel.DataAnnotations;
using SiteVendas.Class;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class CadastroClienteViewModel
    {
        [Required(ErrorMessage = "Esta informação é necessária.")]
        public tb_cadastro_cliente cliente { get; set; }

        [Required(ErrorMessage = "Esta informação é necessária.")]
        public tb_endereco endereco { get; set; }

        [Required(ErrorMessage = "Esta informação é necessária.")]
        public string confirmarSenha { get; set; }
    }
}