namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPedido : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dataPedido = c.DateTime(nullable: false),
                        valorComissao = c.Decimal(precision: 18, scale: 2),
                        observacao = c.String(maxLength: 150),
                        totalPedido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        totalPago = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorDevedor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cliente_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Cliente", t => t.cliente_Id)
                .Index(t => t.cliente_Id);
            
            AddColumn("dbo.ProdutoPedido", "Pedido_id", c => c.Int());
            CreateIndex("dbo.ProdutoPedido", "Pedido_id");
            AddForeignKey("dbo.ProdutoPedido", "Pedido_id", "dbo.Pedido", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutoPedido", "Pedido_id", "dbo.Pedido");
            DropForeignKey("dbo.Pedido", "cliente_Id", "dbo.Cliente");
            DropIndex("dbo.ProdutoPedido", new[] { "Pedido_id" });
            DropIndex("dbo.Pedido", new[] { "cliente_Id" });
            DropColumn("dbo.ProdutoPedido", "Pedido_id");
            DropTable("dbo.Pedido");
        }
    }
}
