namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTabelaFuncionario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Funcionario",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 100),
                        cpfcnpj = c.String(nullable: false, maxLength: 20),
                        salario = c.Decimal(precision: 18, scale: 2),
                        rua = c.String(nullable: false, maxLength: 100),
                        numero = c.String(nullable: false, maxLength: 100),
                        bairro = c.String(nullable: false, maxLength: 100),
                        estado = c.String(nullable: false, maxLength: 2),
                        cidade = c.String(nullable: false, maxLength: 100),
                        telefone1 = c.String(nullable: false, maxLength: 20),
                        telefone2 = c.String(maxLength: 20),
                        email = c.String(maxLength: 100),
                        dataEmissao = c.DateTime(nullable: false),
                        dataDemissao = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Funcionario");
        }
    }
}
