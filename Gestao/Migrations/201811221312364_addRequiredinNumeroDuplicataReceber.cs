namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredinNumeroDuplicataReceber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DuplicatasReceber", "numeroDuplicata", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DuplicatasReceber", "numeroDuplicata", c => c.String());
        }
    }
}
