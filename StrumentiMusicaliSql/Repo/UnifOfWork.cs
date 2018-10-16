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
