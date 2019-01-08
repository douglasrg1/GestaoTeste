using GestaoUsuarios.Usuarios;
using System;
using System.Linq;

namespace GestaoUsuarios
{
    public class Principal
    {
        dbcontextUser context = new dbcontextUser();
        public bool verificaCnpjCadastrado(string cnpj)
        {
            bool resposta = false;

            if (context.usuario.Where(c => c.cnpj == cnpj).ToList().Count > 0)
                resposta = true;

            return resposta;
            
        }
        public void Criar(Usuarios.Usuarios usuario)
        {
            context.usuario.Add(usuario);
            var resp = context.SaveChanges();
        }
        public Usuarios.Usuarios RetornarUser(int id)
        {
            return context.usuario.Find(id);
        }
        public void Editar(Usuarios.Usuarios usuario)
        {
            context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
