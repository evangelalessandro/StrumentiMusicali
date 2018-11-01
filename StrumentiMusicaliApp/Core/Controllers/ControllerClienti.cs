using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
	//[AddINotifyPropertyChangedInterface]
	public class ControllerClienti : BaseControllerGeneric<Cliente, ClientiItem>, 
		IDisposable//, INotifyPropertyChanged
	{ 
		private Subscription<Add<ClientiItem, Cliente>> _selectSub;

		private Subscription<Remove<ClientiItem, Cliente>> _subRemove;

		private Subscription<Save<ClientiItem, Cliente>> _subSave;

		//public event PropertyChangedEventHandler PropertyChanged;

		public ControllerClienti()
		{
			 
			SelectedItem = new Cliente();

		 
			_selectSub = EventAggregator.Instance().Subscribe<Add<ClientiItem, Cliente>>((a) =>
			{
				SelectedItem = new Cliente() { RagioneSociale="Nuovo cliente"};
			});
			_subRemove = EventAggregator.Instance().Subscribe<Remove<ClientiItem, Cliente>>((a) =>
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
					return;
				using (var saveManager = new SaveEntityManager())
				{
					var uof = saveManager.UnitOfWork;
					var curItem = (Cliente)SelectedItem;
					if (curItem.ID > 0)
					{
						var item = uof.ClientiRepository.Find(b => b.ID == curItem.ID).First();
						uof.ClientiRepository.Delete(item);

						if (saveManager.SaveEntity(enSaveOperation.OpDelete))
						{
							EventAggregator.Instance().Publish<UpdateList<Cliente>>(new UpdateList<Cliente>());
						}
					}
				}
			});
			_subSave = EventAggregator.Instance().Subscribe<Save<ClientiItem, Cliente>>((a) =>
		   {
			   Save(null);
		   });
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~ControllerClienti()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		public new void Dispose()
		{
			base.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public override void RefreshList(UpdateList<Cliente> obj)
		{
			try
			{
				var list = new List<ClientiItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.ClientiRepository.Find(a => 
					a.RagioneSociale.Contains(TestoRicerca)
						||
					a.PIVA.Contains(TestoRicerca)
						||
						a.Telefono.Contains(TestoRicerca)
						||
						a.Citta.Contains(TestoRicerca)
						||
						a.Cellulare.Contains(TestoRicerca)
						||
						a.Via.Contains(TestoRicerca)
						||
						TestoRicerca==""
					).ToList().Select(a => new ClientiItem(a)
					{
						ID = a.ID.ToString(), 
						Entity = a,
						 
					}).OrderBy(a => a.RagioneSociale).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<ClientiItem>(list);
			}
			catch (Exception ex)
			{
				new Action(() =>
				{ ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
			}
		}
		public string TestoRicerca { get; set; } = "";
										   // The bulk of the clean-up code is implemented in Dispose(bool)
		protected new void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				EventAggregator.Instance().UnSbscribe(_subSave);
				EventAggregator.Instance().UnSbscribe(_selectSub);
				EventAggregator.Instance().UnSbscribe(_subRemove);
				 
			}
			// free native resources if there are any.
		}

		 
		private void RiselezionaSelezionato()
		{
			var item = (Cliente)SelectedItem;
			EventAggregator.Instance().Publish<UpdateList<Cliente>>(new UpdateList<Cliente>());
			EventAggregator.Instance().Publish<ItemSelected<ClientiItem, Cliente>>(
				new ItemSelected<ClientiItem, Cliente>(new ClientiItem()
				{
					ID = item.ID.ToString(),
					Entity = item
				}));
		}

		private void Save(Save<ClientiItem, Cliente> obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				var uof = saveManager.UnitOfWork;
				if ((((Cliente)SelectedItem).ID > 0))
				{
					uof.ClientiRepository.Update((Cliente)SelectedItem);
				}
				else
				{
					uof.ClientiRepository.Add((Cliente)SelectedItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					RiselezionaSelezionato();
				}
			}
		}
	}
}