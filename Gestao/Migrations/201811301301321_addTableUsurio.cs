namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableUsurio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        idUsuario = c.Int(nullable: false, identity: true),
                        cnpj = c.String(nullable: false),
                        razaoSocial = c.String(nullable: false),
                        endereco = c.String(nullable: false),
                        bairro = c.String(nullable: false),
                        numero = c.String(nullable: false),
                        estado = c.String(nullable: false),
                        cidade = c.String(nullable: false),
                        email = c.String(nullable: false),
                        telefoneContato1 = c.String(nullable: false),
                        telefoneContato2 = c.String(),
                        senha = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idUsuario);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuario");
        }
    }
}
