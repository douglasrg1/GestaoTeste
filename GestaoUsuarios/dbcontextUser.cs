using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using GestaoUsuarios.Usuarios;

namespace GestaoUsuarios
{
    public class dbcontextUser : DbContext
    {
        public dbcontextUser()
            : base("UserGestao")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Pluralizing
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Non delete cascade
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usuarios.Usuarios> usuario { get; set; }
    }
}
