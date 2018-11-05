using PropertyChanged;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
	[AddINotifyPropertyChangedInterface]
	public abstract class BaseControllerGeneric<TEntity, TBaseItem> : BaseController, IMenu, IDisposable,ICloseSave //, INotifyPropertyChanged
		where TEntity : BaseEntity, new()
		where TBaseItem : BaseItem<TEntity>, new()
	{
		public BaseControllerGeneric(enAmbienti ambiente, enAmbienti ambienteDettaglio)
		{
			AmbienteDettaglio = ambienteDettaglio;

			Ambiente = ambiente;

			Init();

			TestoRicerca = ReadSetting(ambiente).LastStringaRicerca;

		}


		~BaseControllerGeneric()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		public enAmbienti Ambiente { get; private set; }
		public enAmbienti AmbienteDettaglio { get; private set; }

		public string TestoRicerca { get; set; } = "";
		public abstract void RefreshList(UpdateList<TEntity> obj);


		public TEntity EditItem { get; set; } = new TEntity();

	  
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


		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected new virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				var dato = ReadSetting(Ambiente);
				dato.LastStringaRicerca = TestoRicerca;
				if (string.IsNullOrEmpty(dato.LastStringaRicerca))
				{
					dato.LastStringaRicerca = "";
				}
				SaveSetting(Ambiente, dato);

				// free managed resources
				EventAggregator.Instance().UnSbscribe(_updateList);
				EventAggregator.Instance().UnSbscribe(_selectItemSub);
				if (DataSource != null)
					DataSource.Clear();
				DataSource = null;
			}
			// free native resources if there are any.
		}

		internal void RiselezionaSelezionato()
		{
			var item = (TEntity)SelectedItem;
			EventAggregator.Instance().Publish<UpdateList<TEntity>>(new UpdateList<TEntity>());
			EventAggregator.Instance().Publish<ItemSelected<TBaseItem, TEntity>>(
				new ItemSelected<TBaseItem, TEntity>(new TBaseItem()
				{
					ID = item.ID,
					Entity = item
				}));
		}
		public TEntity SelectedItem { get; set; }
		public void ShowEditView()
		{
			using (var view = new GenericSettingView(this.SelectedItem))
			{
				view.OnSave += (a, b) =>
				{
					view.Validate();
					EventAggregator.Instance().Publish<Save<TEntity>>
					(new Save<TEntity>());
				};
				ShowView(view, AmbienteDettaglio);
			}
		}
		public MySortableBindingList<TBaseItem> DataSource { get; set; } = new MySortableBindingList<TBaseItem>();

		internal void UpdateDataSource()
		{
			RefreshList(null);
		}
		private MenuTab _menuTab = null;

	 

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandi();

			}
			return _menuTab;
		}
		private RibbonMenuButton ribCerca;

		private RibbonMenuButton ribDelete;
		 

		private RibbonMenuButton ribEdit;

		public event EventHandler<EventArgs> OnSave;
		public event EventHandler<EventArgs> OnClose;

		public enAmbienti AmbienteMenu { get; set; }

		private void AggiungiComandi()
		{

			var tabArticoli = _menuTab.Add(TestoAmbiente(AmbienteMenu));
			var panelComandi = tabArticoli.Add("Comandi");
			UtilityView.AddButtonSaveAndClose(panelComandi, this, false);

			var ribCrea = panelComandi.Add("Crea", Properties.Resources.Add);
			ribCrea.Tag = MenuTab.TagAdd;

			ribEdit = panelComandi.Add(@"Vedi\Modifica", Properties.Resources.Edit, true);
			ribEdit.Tag = MenuTab.TagEdit;

			ribDelete = panelComandi.Add("Cancella", Properties.Resources.Delete, true);
			ribDelete.Tag = MenuTab.TagRemove;
			 
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCerca = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);
			ribCerca.Tag = MenuTab.TagCerca;

			ribCrea.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Add<TEntity>());
			};
			ribDelete.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Remove<TEntity>());
			};
			ribEdit.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Edit<TEntity>());
			};
			ribCerca.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new ViewRicerca<TEntity>());
			};
		}

		public void RaiseSave() => OnSave(this, new EventArgs());


		public void RaiseClose() => OnClose(this, new EventArgs());
	}
}