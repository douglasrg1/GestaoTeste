namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableProdutoMovimentacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdutosMovimentacao",
                c => new
                    {
                        idProdutosMovimentacao = c.Int(nullable: false, identity: true),
                        idMovimentacaoEstoque = c.Int(nullable: false),
                        idProduto = c.Int(nullable: false),
                        quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        aliqICMS = c.Decimal(precision: 18, scale: 2),
                        CFOP = c.Decimal(precision: 18, scale: 2),
                        valorICMS = c.Decimal(precision: 18, scale: 2),
                        cst_cson = c.String(),
                        aliqIPI = c.Decimal(precision: 18, scale: 2),
                        valorIPI = c.Decimal(precision: 18, scale: 2),
                        valorBaseICMSST = c.Decimal(precision: 18, scale: 2),
                        valorICMSST = c.Decimal(precision: 18, scale: 2),
                        valorDesconto = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.idProdutosMovimentacao)
                .ForeignKey("dbo.MovimentaEstoque", t => t.idMovimentacaoEstoque)
                .ForeignKey("dbo.Produto", t => t.idProduto)
                .Index(t => t.idMovimentacaoEstoque)
                .Index(t => t.idProduto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutosMovimentacao", "idProduto", "dbo.Produto");
            DropForeignKey("dbo.ProdutosMovimentacao", "idMovimentacaoEstoque", "dbo.MovimentaEstoque");
            DropIndex("dbo.ProdutosMovimentacao", new[] { "idProduto" });
            DropIndex("dbo.ProdutosMovimentacao", new[] { "idMovimentacaoEstoque" });
            DropTable("dbo.ProdutosMovimentacao");
        }
    }
}
