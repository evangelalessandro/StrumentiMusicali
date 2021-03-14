using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Altro;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Ecomm;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace StrumentiMusicali.Library.Repo
{
    public class UnitOfWork : IDisposable
    {
        //Our database context
        public ModelSm dbContext = new ModelSm();

        //Private members corresponding to each concrete repository
        private Repository<Categoria> _CategorieRepository;

        public void OptimizeIndex()
        {
            var query = @"

                declare @db varchar(50) 

                select @db =DB_NAME()

                EXECUTE dbo.IndexOptimize
                @Databases = @db,
                @FragmentationLow = NULL,
                @FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
                @FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
                @FragmentationLevel1 = 5,
                @FragmentationLevel2 = 30

select @db
"
            ;
            object[] parameters = new List<object>().ToArray();
            var dato = this.dbContext.Database.SqlQuery<string>(query, parameters);
            dato.ToList();

        }

        public string ServerName()
        {
            object[] parameters = new List<object>().ToArray();
            var dato = this.dbContext.Database.SqlQuery<string>("select @@SERVERNAME", parameters);
            var serverName = dato.First();
            if (serverName.Contains(@"\"))
            {
                serverName = serverName.Split(@"\".ToCharArray()).First();
            }
            return serverName;
        }

        public IRepository<Categoria> CategorieRepository {
            get {
                if (_CategorieRepository == null)
                {
                    _CategorieRepository = new Repository<Categoria>(dbContext);
                }
                return _CategorieRepository;
            }
        }
        private Repository<TipiPagamentoScontrino> _TipiPagamentoScontrinoRepository;

        public IRepository<TipiPagamentoScontrino> TipiPagamentoScontrinoRepository
        {
            get
            {
                if (_TipiPagamentoScontrinoRepository == null)
                {
                    _TipiPagamentoScontrinoRepository = new Repository<TipiPagamentoScontrino>(dbContext);
                }
                return _TipiPagamentoScontrinoRepository;
            }
        }

        private Repository<CategoriaWeb> _CategorieWebRepository;

        public IRepository<CategoriaWeb> CategorieWebRepository {
            get {
                if (_CategorieWebRepository == null)
                {
                    _CategorieWebRepository = new Repository<CategoriaWeb>(dbContext);
                }
                return _CategorieWebRepository;
            }
        }
        private Repository<ArticoloImportato> _ArticoloImportatoWebRepository;

        public IRepository<ArticoloImportato> ArticoloImportatoWebRepository {
            get {
                if (_ArticoloImportatoWebRepository == null)
                {
                    _ArticoloImportatoWebRepository = new Repository<ArticoloImportato>(dbContext);
                }
                return _ArticoloImportatoWebRepository;
            }
        }

        
        private Repository<RepartoWeb> _RepartoWebRepository;

        public IRepository<RepartoWeb> RepartoWebRepository {
            get {
                if (_RepartoWebRepository == null)
                {
                    _RepartoWebRepository = new Repository<RepartoWeb>(dbContext);
                }
                return _RepartoWebRepository;
            }
        }

        private Repository<DatiMittente> _DatiMittenteRepository;

        public IRepository<DatiMittente> DatiMittenteRepository {
            get {
                if (_DatiMittenteRepository == null)
                {
                    _DatiMittenteRepository = new Repository<DatiMittente>(dbContext);
                }
                return _DatiMittenteRepository;
            }
        }

        public void EseguiBackup()
        {
            using (var connection = new SqlConnection(this.dbContext.Database.Connection.ConnectionString))
            {
                var command = new SqlCommand("dbo.sp_Backup", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        private Repository<DatiIntestazioneStampaFattura> _DatiIntestazioneStampaFatturaRepository;

        public IRepository<DatiIntestazioneStampaFattura> DatiIntestazioneStampaFatturaRepository {
            get {
                if (_DatiIntestazioneStampaFatturaRepository == null)
                {
                    _DatiIntestazioneStampaFatturaRepository = new Repository<DatiIntestazioneStampaFattura>(dbContext);
                }
                return _DatiIntestazioneStampaFatturaRepository;
            }
        }

        private Repository<SettingScontrino> _SettingScontrino;

        public IRepository<SettingScontrino> SettingScontrino
        {
            get
            {
                if (_SettingScontrino == null)
                {
                    _SettingScontrino = new Repository<SettingScontrino>(dbContext);
                }
                return _SettingScontrino;
            }
        }

        private Repository<SettingSito> _SettingSitoRepository;

        public IRepository<SettingSito> SettingSitoRepository {
            get {
                if (_SettingSitoRepository == null)
                {
                    _SettingSitoRepository = new Repository<SettingSito>(dbContext);
                }
                return _SettingSitoRepository;
            }
        }

        private Repository<SettingDocumentiPagamenti> _SettingDocumentiPagamentiRepository;

        public IRepository<SettingDocumentiPagamenti> SettingDocumentiPagamentiRepository {
            get {
                if (_SettingDocumentiPagamentiRepository == null)
                {
                    _SettingDocumentiPagamentiRepository = new Repository<SettingDocumentiPagamenti>(dbContext);
                }
                return _SettingDocumentiPagamentiRepository;
            }
        }

        private Repository<SettingBackupFtp> _SettingBackupFtpRepository;

        public IRepository<SettingBackupFtp> SettingBackupFtpRepository {
            get {
                if (_SettingBackupFtpRepository == null)
                {
                    _SettingBackupFtpRepository = new Repository<SettingBackupFtp>(dbContext);
                }
                return _SettingBackupFtpRepository;
            }
        }

        private Repository<FattureGenerateInvio> _FattureGenerateInvioRepository;

        public IRepository<FattureGenerateInvio> FattureGenerateInvioRepository {
            get {
                if (_FattureGenerateInvioRepository == null)
                {
                    _FattureGenerateInvioRepository = new Repository<FattureGenerateInvio>(dbContext);
                }
                return _FattureGenerateInvioRepository;
            }
        }

        private Repository<Utente> _Utenti;

        public IRepository<Utente> UtentiRepository {
            get {
                if (_Utenti == null)
                {
                    _Utenti = new Repository<Utente>(dbContext);
                }
                return _Utenti;
            }
        }

        private Repository<PagamentoDocumenti> _PagamentoDocumentiRepository;

        public IRepository<PagamentoDocumenti> PagamentoDocumentiRepository {
            get {
                if (_PagamentoDocumentiRepository == null)
                {
                    _PagamentoDocumentiRepository = new Repository<PagamentoDocumenti>(dbContext);
                }
                return _PagamentoDocumentiRepository;
            }
        }
         
        private Repository<Pagamento> _PagamentoRepository;

        public IRepository<Pagamento> PagamentoRepository {
            get {
                if (_PagamentoRepository == null)
                {
                    _PagamentoRepository = new Repository<Pagamento>(dbContext);
                }
                return _PagamentoRepository;
            }
        }

        private Repository<Deposito> _DepositoRepository;

        public IRepository<Deposito> DepositoRepository {
            get {
                if (_DepositoRepository == null)
                {
                    _DepositoRepository = new Repository<Deposito>(dbContext);
                }
                return _DepositoRepository;
            }
        }

        private Repository<Magazzino> _MagazzinoRepository;

        public IRepository<Magazzino> MagazzinoRepository {
            get {
                if (_MagazzinoRepository == null)
                {
                    _MagazzinoRepository = new Repository<Magazzino>(dbContext);
                }
                return _MagazzinoRepository;
            }
        }

        private Repository<AggiornamentoWebArticolo> _AggiornamentoWebArticoloRepository;

        public IRepository<AggiornamentoWebArticolo> AggiornamentoWebArticoloRepository {
            get {
                if (_AggiornamentoWebArticoloRepository == null)
                {
                    _AggiornamentoWebArticoloRepository = new Repository<AggiornamentoWebArticolo>(dbContext);
                }
                return _AggiornamentoWebArticoloRepository;
            }
        }

        private Repository<Articolo> _ArticoliRepository;

        public IRepository<Articolo> ArticoliRepository {
            get {
                if (_ArticoliRepository == null)
                {
                    _ArticoliRepository = new Repository<Articolo>(dbContext);
                }
                return _ArticoliRepository;
            }
        }

        private Repository<Fattura> _FatturaRepository;

        public IRepository<Fattura> FatturaRepository {
            get {
                if (_FatturaRepository == null)
                {
                    _FatturaRepository = new Repository<Fattura>(dbContext);
                }
                return _FatturaRepository;
            }
        }

        //private Repository<DDt> _DDTRepository;

        //public IRepository<DDt> DDTRepository {
        //	get {
        //		if (_DDTRepository == null)
        //		{
        //			_DDTRepository = new Repository<DDt>(dbContext);
        //		}
        //		return _DDTRepository;
        //	}
        //}

        private Repository<Cliente> _ClientiRepository;

        public IRepository<Cliente> ClientiRepository {
            get {
                if (_ClientiRepository == null)
                {
                    _ClientiRepository = new Repository<Cliente>(dbContext);
                }
                return _ClientiRepository;
            }
        }

        private Repository<FatturaRiga> _FattureRigheRepository;

        public IRepository<FatturaRiga> FattureRigheRepository {
            get {
                if (_FattureRigheRepository == null)
                {
                    _FattureRigheRepository = new Repository<FatturaRiga>(dbContext);
                }
                return _FattureRigheRepository;
            }
        }

        //private Repository<DDTRiga> _DDTRigheRepository;

        //public IRepository<DDTRiga> DDTRigheRepository {
        //	get {
        //		if (_DDTRigheRepository == null)
        //		{
        //			_DDTRigheRepository = new Repository<DDTRiga>(dbContext);
        //		}
        //		return _DDTRigheRepository;
        //	}
        //}

        private Repository<SchedulerJob> _SchedulerJobRepository;

        public IRepository<SchedulerJob> SchedulerJobRepository {
            get {
                if (_SchedulerJobRepository == null)
                {
                    _SchedulerJobRepository = new Repository<SchedulerJob>(dbContext);
                }
                return _SchedulerJobRepository;
            }
        }

        private Repository<FotoArticolo> _FotoArticoloRepository;

        public IRepository<FotoArticolo> FotoArticoloRepository {
            get {
                if (_FotoArticoloRepository == null)
                {
                    _FotoArticoloRepository = new Repository<FotoArticolo>(dbContext);
                }
                return _FotoArticoloRepository;
            }
        }

        private Repository<EventLog> _EventLogRepository;

        //Accessors for each private repository, creates repository if null
        public IRepository<EventLog> EventLogRepository {
            get {
                if (_EventLogRepository == null)
                {
                    _EventLogRepository = new Repository<EventLog>(dbContext);
                }
                return _EventLogRepository;
            }
        }

        //Method to save all changes to repositories
        public void Commit()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                throw new MessageException(ex.EntityValidationErrors.First().ValidationErrors.Select(a => a.ErrorMessage).ToList());
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                if (ex.InnerException.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains(
                        "Cannot insert duplicate key row in object 'dbo.Fatture' with unique index 'IX_Codice'. The duplicate key value is ("))
                    {
                        throw new MessageException("Il campo codice della documento è già presente");
                    }
                    throw new Exception(ex.InnerException.InnerException.ToString());
                }
                throw new Exception(ex.InnerException.ToString());
            }
        }

        public void ChiamaSp()
        {
        }

        //IDisposible implementation
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}