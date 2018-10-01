namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFornecedor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fornecedors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        razaoSocial = c.String(nullable: false, maxLength: 100),
                        nomeFantasia = c.String(nullable: false, maxLength: 100),
                        cnpj = c.String(nullable: false, maxLength: 30),
                        inscricaoEstadual = c.String(maxLength: 30),
                        inscricaoMunicipal = c.String(maxLength: 30),
                        rua = c.String(nullable: false, maxLength: 100),
                        numero = c.String(nullable: false, maxLength: 20),
                        bairro = c.String(nullable: false, maxLength: 100),
                        complemento = c.String(maxLength: 100),
                        estado = c.String(nullable: false, maxLength: 2),
                        cidade = c.String(nullable: false, maxLength: 100),
                        telefone1 = c.String(nullable: false),
                        telefone2 = c.String(),
                        email = c.String(),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fornecedors");
        }
    }
}
