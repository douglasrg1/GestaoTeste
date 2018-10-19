namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForeingkeypedidoClienteToCliente : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pedido", name: "cliente_Id", newName: "idCliente");
            RenameIndex(table: "dbo.Pedido", name: "IX_cliente_Id", newName: "IX_idCliente");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Pedido", name: "IX_idCliente", newName: "IX_cliente_Id");
            RenameColumn(table: "dbo.Pedido", name: "idCliente", newName: "cliente_Id");
        }
    }
}
