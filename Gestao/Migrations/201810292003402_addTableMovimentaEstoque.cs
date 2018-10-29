namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableMovimentaEstoque : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovimentaEstoque",
                c => new
                    {
                        idMovimentacao = c.Int(nullable: false, identity: true),
                        datamovimentacao = c.DateTime(nullable: false),
                        tipoMovimentacao = c.String(nullable: false),
                        nrDocumento = c.String(nullable: false),
                        totalMovimentacao = c.Decimal(precision: 18, scale: 2),
                        valorFrete = c.Decimal(precision: 18, scale: 2),
                        valorIpi = c.Decimal(precision: 18, scale: 2),
                        valorIcms = c.Decimal(precision: 18, scale: 2),
                        cfop = c.String(),
                        idFornecefor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idMovimentacao)
                .ForeignKey("dbo.Fornecedor", t => t.idFornecefor)
                .Index(t => t.idFornecefor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimentaEstoque", "idFornecefor", "dbo.Fornecedor");
            DropIndex("dbo.MovimentaEstoque", new[] { "idFornecefor" });
            DropTable("dbo.MovimentaEstoque");
        }
    }
}
