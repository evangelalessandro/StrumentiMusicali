using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

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

        protected override DbEntityValidationResult ValidateEntity(
            System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
            if (entityEntry.Entity is Fattura && (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified))
            {
                CheckFattura(entityEntry.Entity as Fattura, result);
            }
            if (entityEntry.Entity is Utente)
            {
                foreach (var item in CheckUtenti(entityEntry.Entity as Utente, entityEntry.State))
                {
                    result.ValidationErrors.Add(item);
                }
            }
            else if (entityEntry.Entity is Deposito)
            {
                var deposito = entityEntry.Entity as Deposito;
                if (deposito.Principale)
                {
                    using (var uof = new UnitOfWork())
                    {
                        var firstDepPrin = uof.DepositoRepository.Find(a => a.Principale && a.ID != deposito.ID).FirstOrDefault();
                        if (firstDepPrin != null)
                        {
                            firstDepPrin.Principale = false;
                            uof.DepositoRepository.Update(firstDepPrin);
                            uof.Commit();
                        }
                    }
                }
            }
            else if (entityEntry.Entity is Articolo)
            {
                var articolo = entityEntry.Entity as Articolo;
                if (articolo.ID == 0)
                {
                    articolo.UpdateTitolo = "";
                }
                if (articolo.CategoriaID == 0)
                {
                    result.ValidationErrors.Add(
                              new System.Data.Entity.Validation.DbValidationError(
                                  "Categoria",
                              "Occorre specificare la categoria "));
                }
            }
            else if (entityEntry.Entity is SettingBackupFtp)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    var newValue = entityEntry.Entity as SettingBackupFtp;

                    var old = Core.Settings.SettingBackupFtpValidator.ReadSetting();
                    /*se è cambiato il percorso locale del backup lo salvo nei device del db*/
                    if (old != null
                        && newValue.BackupSetting.FolderLocalServer != null
                        && old.BackupSetting.FolderLocalServer != newValue.BackupSetting.FolderLocalServer
                        && newValue.BackupSetting.FolderLocalServer.Length > 0)
                    {
                        AddDeviceBackup(this.Database.Connection.ConnectionString, newValue);
                    }
                }
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
        private static void AddDeviceBackup(string connectionString, SettingBackupFtp a)
        {

            /*se è cambiata la cartella locale per il backup aggiorno il device backup*/

            if (a.BackupSetting.FolderLocalServer.Length > 0)
            {
                if (!a.BackupSetting.FolderLocalServer.EndsWith(@"\"))
                {
                    a.BackupSetting.FolderLocalServer += @"\";
                }
                 
                using (var connection = new SqlConnection(connectionString))
                {
                    var command = new SqlCommand(Properties.Resource1.AddDeviceBackup, connection);
                    command.Parameters.AddWithValue("@p0", "BACKUP_NegozioSM");
                    command.Parameters.AddWithValue("@p1", a.BackupSetting.FolderLocalServer);
                    connection.Open();

                    command.ExecuteNonQuery();

                    var command1 = new SqlCommand(Properties.Resource1.SpCheckExists, connection);
                    if ((int)command1.ExecuteScalar()==0)
                    {
                        var command2 = new SqlCommand(Properties.Resource1.SpBackup, connection);
                        command2.CommandType = System.Data.CommandType.Text;
                        command2.ExecuteNonQuery();
                    }

                }

                

            }


        }
        public static List<DbValidationError> CheckUtenti(Utente utente, EntityState state)
        {
            var list = new List<DbValidationError>();

            using (var uof = new UnitOfWork())
            {
                var count = uof.UtentiRepository.Find(a => a.ID != utente.ID &&
                a.NomeUtente == utente.NomeUtente
                ).Count();

                if (count > 0 && state != EntityState.Deleted)
                {
                    list.Add(
                           new System.Data.Entity.Validation.DbValidationError("NomeUtente",
                           "Deve essere univoco il 'nome utente'. Questo Nome è già usato "));

                }

                count = uof.UtentiRepository.Find(a => a.ID != utente.ID && a.AdminUtenti == true).Count();
                if (count + (state != EntityState.Deleted && utente.AdminUtenti ? 1 : 0) == 0)
                {
                    list.Add(
                        new System.Data.Entity.Validation.DbValidationError("NomeUtente",
                        "Deve esserci almeno un amministratore degli utenti"));
                }
            }

            return list;
        }

        private void CheckFattura(Fattura fattura, DbEntityValidationResult result)
        {
            using (var uof = new UnitOfWork())
            {
                var count = uof.FatturaRepository.Find(a => a.ID != fattura.ID && a.Codice == fattura.Codice
                && (a.TipoDocumento == EnTipoDocumento.FatturaDiCortesia || a.TipoDocumento == EnTipoDocumento.RicevutaFiscale)).Count();

                if (count > 0)
                {
                    result.ValidationErrors.Add(
                            new System.Data.Entity.Validation.DbValidationError("Codice",
                            "Deve essere univoco il codice. Questo codice è già usato " + fattura.Codice));
                }
                if ((fattura.TipoDocumento == EnTipoDocumento.FatturaDiCortesia || fattura.TipoDocumento == EnTipoDocumento.RicevutaFiscale) && string.IsNullOrEmpty(fattura.Pagamento))
                {
                    result.ValidationErrors.Add(
                            new System.Data.Entity.Validation.DbValidationError("Pagamento",
                            "Occorre specificare il tipo pagamento"));
                }
                if (fattura.ClienteID == 0)
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
            modelBuilder.Entity<Pagamento>().ToTable("Pagamenti");
            modelBuilder.Entity<Deposito>().ToTable("Depositi");
            modelBuilder.Entity<Magazzino>().ToTable("Magazzino");
            modelBuilder.Entity<Articolo>().ToTable("Articoli");
            modelBuilder.Entity<Categoria>().ToTable("Categorie");


            modelBuilder.Entity<FotoArticolo>().ToTable("FotoArticoli");

            modelBuilder.Entity<Fattura>().ToTable("Fatture");
            modelBuilder.Entity<FatturaRiga>().ToTable("FattureRighe");

            modelBuilder.Entity<Utente>().ToTable("Utenti");
            //modelBuilder.Entity<SettingSito>().ToTable("SettingSito");
            modelBuilder.Entity<SettingBackupFtp>().ToTable("SettingBackupFtp");



            modelBuilder.Entity<Cliente>().ToTable("Clienti");
            modelBuilder.Entity<FattureGenerateInvio>().ToTable("FattureGenerate");
            modelBuilder.Entity<SettingDocumentiPagamenti>().ToTable("SettingDocumentiPagamenti");
            modelBuilder.Entity<Pagamento>().Property(e => e.ImportoRata).HasPrecision(19, 2);
            modelBuilder.Entity<Pagamento>().Property(e => e.ImportoResiduo).HasPrecision(19, 2);
            modelBuilder.Entity<Pagamento>().Property(e => e.ImportoTotale).HasPrecision(19, 2);
            modelBuilder.Entity<Articolo>().Property(e => e.Prezzo).HasPrecision(19, 2);
            modelBuilder.Entity<Articolo>().Property(e => e.PrezzoBarrato).HasPrecision(19, 2);

            modelBuilder.Entity<Categoria>()
           .HasIndex(b => new { b.Nome, b.Codice, b.Reparto })
           .IsUnique();
        }

        public virtual DbSet<Deposito> Depositi { get; set; }
        public virtual DbSet<Magazzino> Magazzino { get; set; }
        public virtual DbSet<Articolo> Articoli { get; set; }

        public virtual DbSet<Fattura> Fatture { get; set; }
        public virtual DbSet<FatturaRiga> FattureRighe { get; set; }

        //public virtual DbSet<DDt> DDT { get; set; }
        //public virtual DbSet<DDTRiga> DDTRighe { get; set; }
        public virtual DbSet<FattureGenerateInvio> FattureGenerate { get; set; }
        public virtual DbSet<SettingBackupFtp> SettingBackupFtp { get; set; }


        public virtual DbSet<Cliente> Clienti { get; set; }

        public virtual DbSet<Utente> Utenti { get; set; }
        public virtual DbSet<SettingSito> SettingSito { get; set; }
        public virtual DbSet<DatiIntestazioneStampaFattura> DatiIntestazioneStampaFattura { get; set; }
        public virtual DbSet<DatiMittente> DatiMittenteFattura { get; set; }

        public virtual DbSet<Categoria> Categorie { get; set; }
        public virtual DbSet<EventLog> LogEventi { get; set; }
        public virtual DbSet<FotoArticolo> FotoArticoli { get; set; }

        public virtual DbSet<PagamentoDocumenti> PagamentoDocumenti { get; set; }
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