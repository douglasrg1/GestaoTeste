namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableMovimentaestoque : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MovimentaEstoque", name: "idFornecefor", newName: "idFornecedor");
            RenameIndex(table: "dbo.MovimentaEstoque", name: "IX_idFornecefor", newName: "IX_idFornecedor");
            AlterColumn("dbo.MovimentaEstoque", "tipoMovimentacao", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.MovimentaEstoque", "nrDocumento", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovimentaEstoque", "nrDocumento", c => c.String(nullable: false));
            AlterColumn("dbo.MovimentaEstoque", "tipoMovimentacao", c => c.String(nullable: false));
            RenameIndex(table: "dbo.MovimentaEstoque", name: "IX_idFornecedor", newName: "IX_idFornecefor");
            RenameColumn(table: "dbo.MovimentaEstoque", name: "idFornecedor", newName: "idFornecefor");
        }
    }
}
