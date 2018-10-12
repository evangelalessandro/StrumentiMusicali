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
			this.Configuration.LazyLoadingEnabled = true;
			Database.SetInitializer<ModelSm>(new MigrateDatabaseToLatestVersion<ModelSm, Migrations.Configuration>());
		}

      

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Articolo>().ToTable("Articoli");
			modelBuilder.Entity<Categorie>().ToTable("Categorie");
			modelBuilder.Entity<FotoArticolo>().ToTable("FotoArticoli");
			modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
            modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);
                
        }
		public virtual DbSet<Articolo> Articoli { get; set; }
		public virtual DbSet<Categorie> Categorie { get; set; }
		public virtual DbSet<EventLog> LogEventi { get; set; }
		public virtual DbSet<FotoArticolo> FotoArticoli { get; set; }
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
