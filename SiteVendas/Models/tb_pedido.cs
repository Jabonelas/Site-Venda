﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_pedido
    {
        public tb_pedido()
        {
            tb_nota_fiscal = new HashSet<tb_nota_fiscal>();
        }

        [Key]
        public int id_pedido { get; set; }
        public int? pd_numero_pedido { get; set; }
        public int pd_quantidade { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime pd_data { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal pd_valor { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string pd_tipo_pagamento { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? pd_troco_para { get; set; }
        public bool pd_confirmado { get; set; }
        public int fk_cadastro_cliente { get; set; }
        public int fk_produto { get; set; }

        [ForeignKey("fk_cadastro_cliente")]
        [InverseProperty("tb_pedido")]
        public virtual tb_cadastro_cliente fk_cadastro_clienteNavigation { get; set; }
        [ForeignKey("fk_produto")]
        [InverseProperty("tb_pedido")]
        public virtual tb_produto fk_produtoNavigation { get; set; }
        [InverseProperty("fk_pedidoNavigation")]
        public virtual ICollection<tb_nota_fiscal> tb_nota_fiscal { get; set; }
    }
}