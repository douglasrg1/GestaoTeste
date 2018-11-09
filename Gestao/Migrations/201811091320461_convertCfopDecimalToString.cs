namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class convertCfopDecimalToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProdutosMovimentacao", "CFOP", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProdutosMovimentacao", "CFOP", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
