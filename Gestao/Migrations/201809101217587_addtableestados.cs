namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableestados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoUf = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Uf = c.String(nullable: false, maxLength: 2),
                        Regiao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Estados");
        }
    }
}
