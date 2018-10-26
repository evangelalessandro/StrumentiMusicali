using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Base;
using StrumentiMusicali.Library.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Repo
{
	public class Repository<T> : IRepository<T>, IDisposable where T : class
	{
		protected ModelSm dbContext;

		//DbSet usable with any of our entity types
		protected DbSet<T> dbSet;

		//constructor taking the database context and getting the appropriately typed data set from it
		public Repository(ModelSm context)
		{
			dbContext = context;
			dbSet = context.Set<T>();
		}

		public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return dbSet.Where(predicate);
		}


		//Implementation of IRepository methods
		public virtual IEnumerable<T> DataSet { get { return dbSet; } }

		public virtual void AddRange(List<T> entityList)
		{
			int indexEl = 0;
			foreach (var item in entityList)
			{
				Add(item);
				indexEl++;
			}
		}

		public virtual void Add(T entity)
		{
			if (entity is BaseEntity)
			{
				var item = (entity as BaseEntity);
				item.DataUltimaModifica = DateTime.Now;
				item.DataCreazione = DateTime.Now;
			}
			if (entity is Articolo)
			{
				if (string.IsNullOrEmpty((entity as Articolo).ID))
				{
					(entity as Articolo).ID = "Luc_" + DateTime.Now.Ticks.ToString();
				}

			}
			FixDateNull(entity);

			dbSet.Add(entity);
		}

		public virtual T GetById(Guid id)
		{
			return dbSet.Find(id);
		}

		public virtual void Update(T entity)
		{
			if (entity is BaseEntity)
			{
				var item = (entity as BaseEntity);
				item.DataUltimaModifica = DateTime.Now;
			}
			FixDateNull(entity);

			dbContext.Entry(entity).State = EntityState.Modified;
		}

		private void FixDateNull(T entity)
		{
			var list = GetProperties(entity);
			foreach (var item in list.Where(a => a.PropertyType.ToString().Contains("DateTime")))
			{
				var val = item.GetValue(entity);
				if ((val!=null) && (((DateTime)val).Year < 1800))
				{
					item.SetValue(entity, null);
				}
			}
		}

		private static IEnumerable<PropertyInfo> GetProperties(Object obj)
		{
			Type t = obj.GetType();

			return t.GetProperties()
				.Where(p => (p.Name != "EntityKey" && p.Name != "EntityState"))
				.Select(p => p).ToList();
		}
		public virtual void Delete(T entity)
		{
			dbSet.Remove(entity);
		}

		//IDisposable implementation
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
