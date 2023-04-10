using System.ComponentModel.DataAnnotations;
using SiteVendas.Class;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class ProdutoViewModel
    {
        [Required]
        public tb_produto produto { get; set; }

        [Required]
        public tb_pedido pedido { get; set; }

    }
}