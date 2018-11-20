namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removrequiredFornecedor : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MovimentaEstoque", new[] { "idFornecedor" });
            AlterColumn("dbo.MovimentaEstoque", "idFornecedor", c => c.Int());
            CreateIndex("dbo.MovimentaEstoque", "idFornecedor");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MovimentaEstoque", new[] { "idFornecedor" });
            AlterColumn("dbo.MovimentaEstoque", "idFornecedor", c => c.Int(nullable: false));
            CreateIndex("dbo.MovimentaEstoque", "idFornecedor");
        }
    }
}
