namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        TipoCliente = c.String(nullable: false, maxLength: 10),
                        CnpjCpf = c.String(maxLength: 20),
                        Rua = c.String(nullable: false, maxLength: 100),
                        Numero = c.String(nullable: false, maxLength: 15),
                        Bairro = c.String(nullable: false, maxLength: 100),
                        cidade = c.String(nullable: false, maxLength: 100),
                        Estado = c.String(nullable: false, maxLength: 2),
                        Telefone1 = c.String(nullable: false, maxLength: 20),
                        Telefone2 = c.String(maxLength: 20),
                        Email = c.String(),
                        DataCadastro = c.DateTime(nullable: false),
                        DataUtimaCompra = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clientes");
        }
    }
}
