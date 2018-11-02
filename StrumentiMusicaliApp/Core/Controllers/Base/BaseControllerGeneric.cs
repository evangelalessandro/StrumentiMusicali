using PropertyChanged;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
	[AddINotifyPropertyChangedInterface]
	public abstract class BaseControllerGeneric<TEntity, TBaseItem> : BaseController, IMenu, IDisposable//, INotifyPropertyChanged
		where TEntity : BaseEntity,new()
		where TBaseItem : BaseItem<TEntity>
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

//		public event PropertyChangedEventHandler PropertyChanged;

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

		public TEntity SelectedItem { get; set; }
		 
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
		private RibbonMenuButton ribCercaArticolo;

		private RibbonMenuButton ribDeleteArt;

		//private RibbonMenuButton ribDuplicaArt;

		private RibbonMenuButton ribEditArt;

		public enAmbienti AmbienteMenu { get; set; }
		
		private void AggiungiComandi()
		{

			var tabArticoli = _menuTab.Add(TestoAmbiente(AmbienteMenu));
			var panelComandiArticoli = tabArticoli.Add("Comandi");
			var ribCreArt = panelComandiArticoli.Add("Crea", Properties.Resources.Add);
			ribEditArt = panelComandiArticoli.Add(@"Vedi\Modifica", Properties.Resources.Edit,true);
			ribDeleteArt = panelComandiArticoli.Add("Cancella", Properties.Resources.Delete, true);
			//ribDuplicaArt = panelComandiArticoli.Add("Duplica", Properties.Resources.Duplicate);
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCercaArticolo = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);
			ribCercaArticolo.Tag = TagCerca;

			ribCreArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Add<TEntity>());
			};
			ribDeleteArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Remove<TEntity>());
			};
			ribEditArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Edit<TEntity>());
			};
			ribCercaArticolo.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new ViewRicerca<TEntity>());
			};
		}

		private void ShowEditView()
		{
			throw new NotImplementedException();
		}
	}
}