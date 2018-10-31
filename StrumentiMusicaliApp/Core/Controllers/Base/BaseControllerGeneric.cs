using PropertyChanged;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
	[AddINotifyPropertyChangedInterface]
	public abstract class BaseControllerGeneric<TEntity, TBaseItem> : BaseController, IDisposable
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		public BaseControllerGeneric()
		{
			Init();
		}

		public abstract void RefreshList(UpdateList<TEntity> obj);

		private Subscription<UpdateList<TEntity>> _updateList;
		private Subscription<ItemSelected<TBaseItem, TEntity>> _selectItemSub;

		public void Init()
		{
			_updateList = EventAggregator.Instance().Subscribe<UpdateList<TEntity>>(RefreshList);

			_selectItemSub = EventAggregator.Instance().Subscribe<ItemSelected<TBaseItem, TEntity>>(
				(a) =>
				{
					if (a.ItemSelected != null)
						SelectedItem = a.ItemSelected.Entity;
				}
				);
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~BaseControllerGeneric()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected new virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				EventAggregator.Instance().UnSbscribe(_updateList);
				EventAggregator.Instance().UnSbscribe(_selectItemSub);
				if (DataSource != null)
					DataSource.Clear();
				DataSource = null;
			}
			// free native resources if there are any.
		}

		public BaseEntity SelectedItem { get; set; }

		[AlsoNotifyFor("DataSource")]
		public MySortableBindingList<TBaseItem> DataSource { get; set; } = new MySortableBindingList<TBaseItem>();

		internal void UpdateDataSource()
		{
			RefreshList(null);
		}
	}
}