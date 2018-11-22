namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statusDuplicataIsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DuplicatasReceber", "statusDuplicata", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DuplicatasReceber", "statusDuplicata", c => c.String());
        }
    }
}
