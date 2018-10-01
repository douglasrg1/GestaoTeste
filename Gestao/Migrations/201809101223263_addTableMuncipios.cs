namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableMuncipios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Municipios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 200),
                        Uf = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Municipios");
        }
    }
}
