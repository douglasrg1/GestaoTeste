namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class naosei1 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Teste");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Teste",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
    }
}
