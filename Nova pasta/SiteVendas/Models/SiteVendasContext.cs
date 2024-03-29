﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SiteVendas.Models
{
    public partial class SiteVendasContext : DbContext
    {
        public SiteVendasContext()
        {
        }

        public SiteVendasContext(DbContextOptions<SiteVendasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<tb_cadastro_cliente> tb_cadastro_cliente { get; set; }
        public virtual DbSet<tb_dados_empresa> tb_dados_empresa { get; set; }
        public virtual DbSet<tb_endereco> tb_endereco { get; set; }
        public virtual DbSet<tb_mensagens> tb_mensagens { get; set; }
        public virtual DbSet<tb_nota_fiscal> tb_nota_fiscal { get; set; }
        public virtual DbSet<tb_pedido> tb_pedido { get; set; }
        public virtual DbSet<tb_produto> tb_produto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ISRAEL\\SQLEXPRESS;Initial Catalog=SiteVendas;User ID=sa;Password=12345;TrustServerCertificate=True", x => x.UseDateOnlyTimeOnly());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tb_cadastro_cliente>(entity =>
            {
                entity.HasOne(d => d.fk_enderecoNavigation)
                    .WithMany(p => p.tb_cadastro_cliente)
                    .HasForeignKey(d => d.fk_endereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_cadastro_cliente_tb_endereco");
            });

            modelBuilder.Entity<tb_dados_empresa>(entity =>
            {
                entity.HasOne(d => d.fk_enderecoNavigation)
                    .WithMany(p => p.tb_dados_empresa)
                    .HasForeignKey(d => d.fk_endereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_dados_empresa_tb_endereco");
            });

            modelBuilder.Entity<tb_nota_fiscal>(entity =>
            {
                entity.HasOne(d => d.fk_pedidoNavigation)
                    .WithMany(p => p.tb_nota_fiscal)
                    .HasForeignKey(d => d.fk_pedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_nota_fiscal_tb_pedido");
            });

            modelBuilder.Entity<tb_pedido>(entity =>
            {
                entity.HasOne(d => d.fk_cadastro_clienteNavigation)
                    .WithMany(p => p.tb_pedido)
                    .HasForeignKey(d => d.fk_cadastro_cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_pedido_tb_cadastro_cliente");

                entity.HasOne(d => d.fk_produtoNavigation)
                    .WithMany(p => p.tb_pedido)
                    .HasForeignKey(d => d.fk_produto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_pedido_tb_produto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}