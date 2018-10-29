using Gestao.Models.Adicionais;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Gestao.Models
{
    public class ApplicationDbContext : DbContext
    {
        public void AplicationDbContext()
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Pluralizing
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Non delete cascade
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<ProdutoPedido> ProdutoPedido { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<MovimentaEstoque> movimentaEstoque { get; set; }

    }

    
}