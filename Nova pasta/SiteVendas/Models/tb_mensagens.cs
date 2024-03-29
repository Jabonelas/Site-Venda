﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_mensagens
    {
        [Key]
        public int id_mensagem { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string mg_nome { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string mg_email { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string mg_servico { get; set; }
        [Required]
        [StringLength(200)]
        [Unicode(false)]
        public string mg_mensagem { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime mg_data_recebimento { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? mg_data_retorno { get; set; }
        public bool mg_verificado { get; set; }
        public bool mg_exibir { get; set; }
    }
}