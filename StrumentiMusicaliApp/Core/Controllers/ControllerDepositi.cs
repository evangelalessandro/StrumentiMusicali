using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
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
	public class ControllerDepositi : BaseControllerGeneric<Deposito, DepositoItem>, 
		IDisposable//, INotifyPropertyChanged
	{ 
		private Subscription<Add<Deposito>> _selectSub;

		private Subscription<Remove<Deposito>> _subRemove;

		private Subscription<Save<Deposito>> _subSave;

		 
		public ControllerDepositi()
			:base(enAmbienti.DepositoList,enAmbienti.Deposito)
		{
			 
			SelectedItem = new Deposito();

		 
			_selectSub = EventAggregator.Instance().Subscribe<Add<Deposito>>((a) =>
			{
				EditItem = new Deposito() { NomeDeposito="Nuovo Deposito"};
				ShowEditView();
			});
			_subRemove = EventAggregator.Instance().Subscribe<Remove<Deposito>>((a) =>
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare il deposito selezionato?"))
					return;
				using (var saveManager = new SaveEntityManager())
				{
					var uof = saveManager.UnitOfWork;
					var curItem = SelectedItem;
					if (curItem.ID > 0)
					{
						var item = uof.DepositoRepository.Find(b => b.ID == curItem.ID).First();
						uof.DepositoRepository.Delete(item);

						if (saveManager.SaveEntity(enSaveOperation.OpDelete))
						{
							EventAggregator.Instance().Publish<UpdateList<Deposito>>(new UpdateList<Deposito>());
						}
					}
				}
			});
			_subSave = EventAggregator.Instance().Subscribe<Save<Deposito>>((a) =>
		   {
			   Save(null);
		   });
		}
		 

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~ControllerDepositi()
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

		public override void RefreshList(UpdateList<Deposito> obj)
		{
			try
			{
				if (string.IsNullOrEmpty(TestoRicerca))
				{
					TestoRicerca = "";
				}
				var list = new List<DepositoItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.DepositoRepository.Find(a => 
					a.NomeDeposito.Contains(TestoRicerca) ||
						TestoRicerca=="" 
					).ToList().Select(a => new DepositoItem(a)
					{
						 
						 
					}).OrderBy(a => a.NomeDeposito).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<DepositoItem>(list);
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

		 

		private void Save(Save<Deposito> obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				var uof = saveManager.UnitOfWork;
				if (((EditItem).ID > 0))
				{
					uof.DepositoRepository.Update(EditItem);
				}
				else
				{
					uof.DepositoRepository.Add(EditItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					RiselezionaSelezionato();
				}
			}
		}
	}
}