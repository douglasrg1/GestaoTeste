namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableDuplicatasaPagar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DuplicatasPagar",
                c => new
                    {
                        idDuplicataPagar = c.Int(nullable: false, identity: true),
                        numeroDuplicata = c.String(),
                        idFornecedor = c.Int(nullable: false),
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
                        nrDocumento = c.String(),
                    })
                .PrimaryKey(t => t.idDuplicataPagar)
                .ForeignKey("dbo.Fornecedor", t => t.idFornecedor)
                .Index(t => t.idFornecedor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DuplicatasPagar", "idFornecedor", "dbo.Fornecedor");
            DropIndex("dbo.DuplicatasPagar", new[] { "idFornecedor" });
            DropTable("dbo.DuplicatasPagar");
        }
    }
}
