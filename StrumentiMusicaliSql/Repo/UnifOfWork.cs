using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Model;
using System;
using System.Linq;

namespace StrumentiMusicali.Library.Repo
{
	public class UnitOfWork : IDisposable
	{
		//Our database context
		private ModelSm dbContext = new ModelSm();

		//Private members corresponding to each concrete repository
		private Repository<Categorie> _CategorieRepository;

		public IRepository<Categorie> CategorieRepository {
			get {
				if (_CategorieRepository == null)
				{
					_CategorieRepository = new Repository<Categorie>(dbContext);
				}
				return _CategorieRepository;
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

		private Repository<DDt> _DDTRepository;

		public IRepository<DDt> DDTRepository {
			get {
				if (_DDTRepository == null)
				{
					_DDTRepository = new Repository<DDt>(dbContext);
				}
				return _DDTRepository;
			}
		}

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

		private Repository<DDTRiga> _DDTRigheRepository;

		public IRepository<DDTRiga> DDTRigheRepository {
			get {
				if (_DDTRigheRepository == null)
				{
					_DDTRigheRepository = new Repository<DDTRiga>(dbContext);
				}
				return _DDTRigheRepository;
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
					throw new Exception(ex.InnerException.InnerException.ToString());
				}
				throw new Exception(ex.InnerException.ToString());
			}
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