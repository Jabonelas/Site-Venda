﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SiteVendas.Models
{
    public partial class tb_cadastro_cliente
    {
        [Key]
        public int id_cadastro_cliente { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(200)]
        [Unicode(false)]
        public string cc_nome { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(50)]
        [Unicode(false)]
        public string cc_cpf { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(50)]
        [Unicode(false)]
        public string cc_rg { get; set; }
        public DateOnly cc_data_nascimento { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(200)]
        [Unicode(false)]
        public string cc_email { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(50)]
        [Unicode(false)]
        public string cc_celular { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string cc_telefone { get; set; }
        [Required(ErrorMessage = "Esta informação é necessária.")]
        [StringLength(50)]
        [Unicode(false)]
        public string cc_senha { get; set; }
        public int fk_endereco { get; set; }

        [ForeignKey("fk_endereco")]
        [InverseProperty("tb_cadastro_cliente")]
        public virtual tb_endereco fk_enderecoNavigation { get; set; }
    }
}