namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterNullAbleForStatusDuplicataPagar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DuplicatasPagar", "statusDuplicata", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DuplicatasPagar", "statusDuplicata", c => c.String());
        }
    }
}
