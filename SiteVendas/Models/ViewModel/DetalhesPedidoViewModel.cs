using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteVendas.Models.ViewModel
{
    public class DetalhesPedidoViewModel
    {
        public tb_cadastro_cliente cliente { get; set; }

        public tb_pedido pedido { get; set; }

        public tb_nota_fiscal notaFiscal { get; set; }

        public tb_produto produto { get; set; }

        public tb_endereco endereco { get; set; }

        public string usuario { get; set; }

    }
   
}