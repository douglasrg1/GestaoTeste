namespace Gestao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class popularestados : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (12, 'Acre', 'AC', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (27, 'Alagoas', 'AL', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (16, 'Amapá', 'AP', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (13, 'Amazonas', 'AM', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (29, 'Bahia', 'BA', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (23, 'Ceará', 'CE', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (53, 'Distrito Federal', 'DF', 5)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (32, 'Espírito Santo', 'ES', 3)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (52, 'Goiás', 'GO', 5)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (21, 'Maranhão', 'MA', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (51, 'Mato Grosso', 'MT', 5)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (50, 'Mato Grosso do Sul', 'MS', 5)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (31, 'Minas Gerais', 'MG', 3)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (15, 'Pará', 'PA', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (25, 'Paraíba', 'PB', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (41, 'Paraná', 'PR', 4)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (26, 'Pernambuco', 'PE', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (22, 'Piauí', 'PI', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (33, 'Rio de Janeiro', 'RJ', 3)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (24, 'Rio Grande do Norte', 'RN', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (43, 'Rio Grande do Sul', 'RS', 4)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (11, 'Rondônia', 'RO', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (14, 'Roraima', 'RR', 1)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (42, 'Santa Catarina', 'SC', 4)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (35, 'São Paulo', 'SP', 3)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (28, 'Sergipe', 'SE', 2)");
            Sql("Insert into Estados (CodigoUf, Nome, Uf, Regiao) values (17, 'Tocantins', 'TO', 1)");
        }
        
        public override void Down()
        {
        }
    }
}
