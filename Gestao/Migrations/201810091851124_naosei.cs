namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class naosei : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProdutoPedido", "Produto_id", "dbo.Produto");
            DropIndex("dbo.ProdutoPedido", new[] { "Produto_id" });
            AddColumn("dbo.ProdutoPedido", "Produto_id1", c => c.Int());
            AlterColumn("dbo.ProdutoPedido", "Produto_id", c => c.Int(nullable: false));
            CreateIndex("dbo.ProdutoPedido", "Produto_id1");
            AddForeignKey("dbo.ProdutoPedido", "Produto_id1", "dbo.Produto", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutoPedido", "Produto_id1", "dbo.Produto");
            DropIndex("dbo.ProdutoPedido", new[] { "Produto_id1" });
            AlterColumn("dbo.ProdutoPedido", "Produto_id", c => c.Int());
            DropColumn("dbo.ProdutoPedido", "Produto_id1");
            CreateIndex("dbo.ProdutoPedido", "Produto_id");
            AddForeignKey("dbo.ProdutoPedido", "Produto_id", "dbo.Produto", "id");
        }
    }
}
