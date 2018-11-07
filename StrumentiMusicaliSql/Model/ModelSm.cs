using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;

namespace StrumentiMusicali.Library.Model
{
	public partial class ModelSm : DbContext
	{
		public ModelSm()
			: base("name=ModelSm")
		{
			this.Configuration.LazyLoadingEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
			Database.SetInitializer<ModelSm>(new MigrateDatabaseToLatestVersion<ModelSm, Migrations.Configuration>());
		}
		protected  override DbEntityValidationResult ValidateEntity(
			System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
		{
			var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
			if (entityEntry.Entity is Fattura && (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified))
			{ 
				CheckFattura(entityEntry.Entity as Fattura, result);
			}

			if (result.ValidationErrors.Count > 0)
			{
				return result;
			}
			else
			{
				return base.ValidateEntity(entityEntry, items);
			}
		}
		private void CheckFattura(Fattura fattura, DbEntityValidationResult result)
		{
			using (var uof = new UnitOfWork())
			{
				var count = uof.FatturaRepository.Find(a => a.ID != fattura.ID && a.Codice == fattura.Codice
				&& a.TipoDocumento == EnTipoDocumento.Fattura).Count();

				if (count > 0)
				{
					result.ValidationErrors.Add(
							new System.Data.Entity.Validation.DbValidationError("Codice",
							"Deve essere univoco il codice. Questo codice � gi� usato " + fattura.Codice));
				}
				if (fattura.TipoDocumento == EnTipoDocumento.Fattura && string.IsNullOrEmpty(fattura.Pagamento))
				{
					result.ValidationErrors.Add(
							new System.Data.Entity.Validation.DbValidationError("Pagamento",
							"Occorre specificare il tipo pagamento"));
				}
				if (fattura.ClienteID==0)
				{
					result.ValidationErrors.Add(
							new System.Data.Entity.Validation.DbValidationError("ClienteID",
							"Occorre specificare il cliente"));
				}
			}
		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			modelBuilder.Entity<Deposito>().ToTable("Depositi");
			modelBuilder.Entity<Magazzino>().ToTable("Magazzino");
			modelBuilder.Entity<Articolo>().ToTable("Articoli");
			modelBuilder.Entity<Categoria>().ToTable("Categorie");
			modelBuilder.Entity<FotoArticolo>().ToTable("FotoArticoli");

			modelBuilder.Entity<Fattura>().ToTable("Fatture");
			modelBuilder.Entity<FatturaRiga>().ToTable("FattureRighe");

			//modelBuilder.Entity<DDt>().ToTable("DDT");
			//modelBuilder.Entity<DDTRiga>().ToTable("DDTRighe");

			modelBuilder.Entity<Cliente>().ToTable("Clienti");
			modelBuilder.Entity<FattureGenerateInvio>().ToTable("FattureGenerate");

			modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
			modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);
		}

		public virtual DbSet<Deposito> Depositi { get; set; }
		public virtual DbSet<Magazzino> Magazzino { get; set; }
		public virtual DbSet<Articolo> Articoli { get; set; }

		public virtual DbSet<Fattura> Fatture { get; set; }
		public virtual DbSet<FatturaRiga> FattureRighe { get; set; }

		//public virtual DbSet<DDt> DDT { get; set; }
		//public virtual DbSet<DDTRiga> DDTRighe { get; set; }
		public virtual DbSet<FattureGenerateInvio> FattureGenerate { get; set; }

		public virtual DbSet<Cliente> Clienti { get; set; }

		public virtual DbSet<Categoria> Categorie { get; set; }
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