namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUniqueKeyInNumeroPedido : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Pedido", "numeroPedido", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pedido", new[] { "numeroPedido" });
        }
    }
}
