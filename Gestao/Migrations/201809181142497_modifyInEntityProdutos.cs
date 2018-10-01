namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyInEntityProdutos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clientes", newName: "Cliente");
            RenameTable(name: "dbo.Empresas", newName: "Empresa");
            RenameTable(name: "dbo.Fornecedors", newName: "Fornecedor");
            RenameTable(name: "dbo.Produtoes", newName: "Produto");
            AlterColumn("dbo.Produto", "valorFrete", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "despesasAdicionaisdeCompra", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "despesasAdicionaisdeVenda", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "desconto", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.Produto", "nome", unique: true, name: "nome");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Produto", "nome");
            AlterColumn("dbo.Produto", "desconto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "despesasAdicionaisdeVenda", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "despesasAdicionaisdeCompra", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Produto", "valorFrete", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            RenameTable(name: "dbo.Produto", newName: "Produtoes");
            RenameTable(name: "dbo.Fornecedor", newName: "Fornecedors");
            RenameTable(name: "dbo.Empresa", newName: "Empresas");
            RenameTable(name: "dbo.Cliente", newName: "Clientes");
        }
    }
}
