using StrumentiMusicali.Library.Entity;
using System.Data.Entity;

namespace StrumentiMusicali.Library.Model
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
			modelBuilder.Entity<Deposito>().ToTable("Depositi");
			modelBuilder.Entity<Magazzino>().ToTable("Magazzino");
			modelBuilder.Entity<Articolo>().ToTable("Articoli");
			modelBuilder.Entity<Categorie>().ToTable("Categorie");
			modelBuilder.Entity<FotoArticolo>().ToTable("FotoArticoli");

			modelBuilder.Entity<Fattura>().ToTable("Fatture");
			modelBuilder.Entity<FatturaRiga>().ToTable("FattureRighe");

			modelBuilder.Entity<DDt>().ToTable("DDT");
			modelBuilder.Entity<DDTRiga>().ToTable("DDTRighe");

			modelBuilder.Entity<Cliente>().ToTable("Clienti");
			
			modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
			modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);
		}

		public virtual DbSet<Deposito> Depositi { get; set; }
		public virtual DbSet<Magazzino> Magazzino { get; set; }
		public virtual DbSet<Articolo> Articoli { get; set; }

		public virtual DbSet<Fattura> Fatture { get; set; }
		public virtual DbSet<FatturaRiga> FattureRighe { get; set; }

		public virtual DbSet<DDt> DDT { get; set; }
		public virtual DbSet<DDTRiga> DDTRighe { get; set; }

		public virtual DbSet<Cliente> Clienti { get; set; }
		
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