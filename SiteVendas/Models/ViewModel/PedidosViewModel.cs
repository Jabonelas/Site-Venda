using SiteVendas.Class;
using SiteVendas.Context;
using SiteVendas.Controllers;

namespace SiteVendas.Models.ViewModel
{
    public class PedidosViewModel
    {
        public tb_produto produto { get; set; }
        public tb_pedido pedido { get; set; }
    }
}