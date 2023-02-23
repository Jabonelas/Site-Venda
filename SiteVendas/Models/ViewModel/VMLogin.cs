using System.ComponentModel.DataAnnotations;

namespace SiteVendas.Models.ViewModel
{
    public class VMLogin
    {
        [Required(ErrorMessage = "Informe um E-mail Válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe uma Senha Válida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        public bool ManterConectado { get; set; }
    }
}