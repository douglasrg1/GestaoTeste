namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableProdutos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Produtoes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 100),
                        valorCompra = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorFrete = c.Decimal(nullable: false, precision: 18, scale: 2),
                        despesasAdicionaisdeCompra = c.Decimal(nullable: false, precision: 18, scale: 2),
                        valorVenda = c.Decimal(nullable: false, precision: 18, scale: 2),
                        despesasAdicionaisdeVenda = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dataCadastro = c.DateTime(nullable: false),
                        dataUltimaEntrada = c.DateTime(),
                        dataUltimaSaida = c.DateTime(),
                        desconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        condigoReferencia = c.String(maxLength: 100),
                        quantidadeEstoque = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observacoes = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Produtoes");
        }
    }
}
