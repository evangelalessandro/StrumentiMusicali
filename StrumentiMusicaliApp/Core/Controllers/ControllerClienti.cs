using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Enums;
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
		private Subscription<Add<Cliente>> _selectSub;

		private Subscription<Remove<Cliente>> _subRemove;

		private Subscription<Save<Cliente>> _subSave;

		 
		public ControllerClienti()
			:base(enAmbiente.ClientiList,enAmbiente.Cliente)
		{
			 
			SelectedItem = new Cliente();

		 
			_selectSub = EventAggregator.Instance().Subscribe<Add<Cliente>>((a) =>
			{
				EditItem = new Cliente() { RagioneSociale="Nuovo cliente"};
				ShowEditView();
			});
			_subRemove = EventAggregator.Instance().Subscribe<Remove<Cliente>>((a) =>
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
			_subSave = EventAggregator.Instance().Subscribe<Save<Cliente>>((a) =>
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
				if (string.IsNullOrEmpty(TestoRicerca))
				{
					TestoRicerca = "";
				}
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
						a.Indirizzo.Citta.Contains(TestoRicerca)
						||
						a.Indirizzo.Comune.Contains(TestoRicerca)
						||
						a.Cellulare.Contains(TestoRicerca)
						||
						a.Indirizzo.IndirizzoConCivico.Contains(TestoRicerca)
						||
						TestoRicerca=="" 
					).OrderBy(a => a.RagioneSociale).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new ClientiItem(a)
					{
						ID = a.ID, 
						Entity = a,
						 
					}).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<ClientiItem>(list);
			}
			catch (Exception ex)
			{
				new Action(() =>
				{ ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
			}
		}
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

		 

		private void Save(Save<Cliente> obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				var uof = saveManager.UnitOfWork;
				if (((EditItem).ID > 0))
				{
					uof.ClientiRepository.Update(EditItem);
				}
				else
				{
					uof.ClientiRepository.Add(EditItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					RiselezionaSelezionato();
				}
			}
		}
	}
}