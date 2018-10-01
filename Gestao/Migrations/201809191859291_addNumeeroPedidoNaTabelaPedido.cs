namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNumeeroPedidoNaTabelaPedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "numeroPedido", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "numeroPedido");
        }
    }
}
