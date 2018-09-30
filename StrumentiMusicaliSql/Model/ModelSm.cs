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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ModelSm>(new DropCreateDatabaseIfModelChanges<ModelSm>());
            modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
            modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);

        }
    }
}
