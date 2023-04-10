namespace SiteVendas.Models.ViewModel
{
    public class CarrinhoViewModel
    {
        public int idPedido { get; set; }
        public int idProduto { get; set; }

        public int? numeroPedido { get; set; }
        public int quantidade { get; set; }
        public decimal valorTotal { get; set; }
        public byte[] imagem { get; set; }
        public string nome { get; set; }
        public string tamanho { get; set; }
        public decimal valorUnitario { get; set; }
    }
}