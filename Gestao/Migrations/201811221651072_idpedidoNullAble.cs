namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idpedidoNullAble : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DuplicatasReceber", new[] { "idPedido" });
            AlterColumn("dbo.DuplicatasReceber", "idPedido", c => c.Int());
            CreateIndex("dbo.DuplicatasReceber", "idPedido");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DuplicatasReceber", new[] { "idPedido" });
            AlterColumn("dbo.DuplicatasReceber", "idPedido", c => c.Int(nullable: false));
            CreateIndex("dbo.DuplicatasReceber", "idPedido");
        }
    }
}
