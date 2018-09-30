using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrumentiMusicaliSql.Entity;
using StrumentiMusicaliSql.Model;

namespace StrumentiMusicaliSql.Repo
{
     
        public class UnitOfWork : IDisposable
        {
            //Our database context 
            private ModelSm dbContext = new ModelSm();

        //Private members corresponding to each concrete repository
        private Repository<Categorie> _CategorieRepository;

        public IRepository<Categorie> CategorieRepository
        {
            get
            {
                if (_CategorieRepository == null)
                {
                    _CategorieRepository = new Repository<Categorie>(dbContext);
                }
                return _CategorieRepository;
            }

        }

        private Repository<Articolo> _ArticoliRepository;
            
            public IRepository<Articolo> ArticoliRepository
            {
                get
                {
                    if (_ArticoliRepository == null)
                    {
                        _ArticoliRepository = new Repository<Articolo>(dbContext);
                    }
                    return _ArticoliRepository;
                }

            }
        private Repository<EventLog> _EventLogRepository;

        //Accessors for each private repository, creates repository if null
        public IRepository<EventLog> EventLogRepository
            {
                get
                {
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
                dbContext.SaveChanges();
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
