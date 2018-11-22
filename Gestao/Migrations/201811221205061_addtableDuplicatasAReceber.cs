namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableDuplicatasAReceber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DuplicatasReceber",
                c => new
                    {
                        idDuplicataReceber = c.Int(nullable: false, identity: true),
                        numeroDuplicata = c.String(),
                        idCliente = c.Int(nullable: false),
                        dataHemissao = c.DateTime(nullable: false),
                        dataVencimento = c.DateTime(nullable: false),
                        dataPagamento = c.DateTime(),
                        valorDuplicata = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorPago = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorDevedor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorMulta = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorJurosPorDia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observacao = c.String(),
                        statusDuplicata = c.String(),
                        idPedido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idDuplicataReceber)
                .ForeignKey("dbo.Cliente", t => t.idCliente)
                .ForeignKey("dbo.Pedido", t => t.idPedido)
                .Index(t => t.idCliente)
                .Index(t => t.idPedido);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DuplicatasReceber", "idPedido", "dbo.Pedido");
            DropForeignKey("dbo.DuplicatasReceber", "idCliente", "dbo.Cliente");
            DropIndex("dbo.DuplicatasReceber", new[] { "idPedido" });
            DropIndex("dbo.DuplicatasReceber", new[] { "idCliente" });
            DropTable("dbo.DuplicatasReceber");
        }
    }
}
