using SiteVendas.Class;
using SiteVendas.Controllers;
using System.ComponentModel.DataAnnotations;

namespace SiteVendas.Models.ViewModel
{
    public class PedidosViewModel
    {
       
        public tb_pedido pedido { get; set; }

        // public tb_cadastro_cliente cliente { get; set; }

        public decimal valorTotal {get;set;}
    }

}