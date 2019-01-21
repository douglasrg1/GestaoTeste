namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableCaixa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Caixa",
                c => new
                    {
                        CaixaId = c.Int(nullable: false, identity: true),
                        TipoMovimentacao = c.String(nullable: false, maxLength: 10),
                        ValorMovimentacao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataMovimentacao = c.DateTime(nullable: false),
                        descricao = c.String(maxLength: 100),
                        DuplicataId = c.Int(),
                        PedidoId = c.Int(),
                        FuncionarioId = c.Int(),
                    })
                .PrimaryKey(t => t.CaixaId)
                .ForeignKey("dbo.DuplicatasReceber", t => t.DuplicataId)
                .ForeignKey("dbo.Funcionario", t => t.FuncionarioId)
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .Index(t => t.DuplicataId)
                .Index(t => t.PedidoId)
                .Index(t => t.FuncionarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Caixa", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.Caixa", "FuncionarioId", "dbo.Funcionario");
            DropForeignKey("dbo.Caixa", "DuplicataId", "dbo.DuplicatasReceber");
            DropIndex("dbo.Caixa", new[] { "FuncionarioId" });
            DropIndex("dbo.Caixa", new[] { "PedidoId" });
            DropIndex("dbo.Caixa", new[] { "DuplicataId" });
            DropTable("dbo.Caixa");
        }
    }
}
