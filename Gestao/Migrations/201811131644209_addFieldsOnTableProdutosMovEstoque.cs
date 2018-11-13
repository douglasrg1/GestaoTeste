namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldsOnTableProdutosMovEstoque : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProdutosMovimentacao", "observacao", c => c.String());
            AddColumn("dbo.ProdutosMovimentacao", "porcdesconto", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProdutosMovimentacao", "porcdesconto");
            DropColumn("dbo.ProdutosMovimentacao", "observacao");
        }
    }
}
