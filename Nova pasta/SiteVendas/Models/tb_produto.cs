﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_produto
    {
        public tb_produto()
        {
            tb_pedido = new HashSet<tb_pedido>();
        }

        [Key]
        public int id_produto { get; set; }
        [Required]
        [StringLength(200)]
        [Unicode(false)]
        public string pd_tipo { get; set; }
        [Required]
        [StringLength(200)]
        [Unicode(false)]
        public string pd_nome { get; set; }
        [Required]
        [StringLength(200)]
        [Unicode(false)]
        public string pd_descricao { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string pd_tamanho { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal pd_preco { get; set; }
        public bool pd_disponivel { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime pd_data { get; set; }
        [Column(TypeName = "image")]
        public byte[] pd_imagem { get; set; }

        [InverseProperty("fk_produtoNavigation")]
        public virtual ICollection<tb_pedido> tb_pedido { get; set; }
    }
}