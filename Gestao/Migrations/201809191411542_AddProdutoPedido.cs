namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProdutoPedido : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdutoPedido",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        valorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        porcDesconto = c.Decimal(precision: 18, scale: 2),
                        valorDesconto = c.Decimal(precision: 18, scale: 2),
                        quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observacao = c.String(maxLength: 200),
                        Produto_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Produto", t => t.Produto_id)
                .Index(t => t.Produto_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutoPedido", "Produto_id", "dbo.Produto");
            DropIndex("dbo.ProdutoPedido", new[] { "Produto_id" });
            DropTable("dbo.ProdutoPedido");
        }
    }
}
