using System.ComponentModel.DataAnnotations;
using SiteVendas.Class;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class ProdutoViewModel
    {
        [Required(ErrorMessage = "Esta informação é necessária.")]
        public tb_produto produto { get; set; }

        [Required(ErrorMessage = "Esta informação é necessária.")]
        public tb_pedido pedido { get; set; }
    }
}