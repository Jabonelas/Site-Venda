﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_dados_empresa
    {
        [Key]
        public int id_dados_empresa { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(200)]
        [Unicode(false)]
        public string de_nome_fantasia { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(50)]
        [Unicode(false)]
        public string de_cnpj { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string de_telefone { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string de_email { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string de_horario_inicio { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string de_horario_fechamento { get; set; }
        public int fk_endereco { get; set; }

        [ForeignKey("fk_endereco")]
        [InverseProperty("tb_dados_empresa")]
        public virtual tb_endereco fk_enderecoNavigation { get; set; }
    }
}