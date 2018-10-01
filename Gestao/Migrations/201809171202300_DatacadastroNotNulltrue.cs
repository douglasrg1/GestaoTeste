namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatacadastroNotNulltrue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Fornecedors", "dataCadastro", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Fornecedors", "dataCadastro", c => c.DateTime(nullable: false));
        }
    }
}
