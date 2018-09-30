using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using StrumentiMusicaliSql.Entity;

namespace StrumentiMusicaliSql.Model
{
    
    public partial class ModelSm : DbContext
    {
        public ModelSm()
            : base("name=ModelSm")
        {
        }

        public virtual DbSet<Articolo> Articoli { get; set; }
        public virtual DbSet<Categorie> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ModelSm>(new DropCreateDatabaseIfModelChanges<ModelSm>());
            modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
            modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);
                
        }
        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
    
}
