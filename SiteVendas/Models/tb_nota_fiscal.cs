﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_nota_fiscal
    {
        [Key]
        public int id_nota_fiscal { get; set; }
        public int fk_pedido { get; set; }

        [ForeignKey("fk_pedido")]
        [InverseProperty("tb_nota_fiscal")]
        public virtual tb_pedido fk_pedidoNavigation { get; set; }
    }
}