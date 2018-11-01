﻿using PropertyChanged;
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
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
	[AddINotifyPropertyChangedInterface]
	public class ControllerRigheFatture : BaseControllerGeneric<FatturaRiga, FatturaRigaItem>, IDisposable
	{
		private Subscription<RemovePriority<FatturaRigaItem, FatturaRiga>> _addPrio;
		private ControllerFatturazione _controllerFatturazione;
		private Subscription<AddPriority<FatturaRigaItem, FatturaRiga>> _removePrio;
		private Subscription<Add<FatturaRigaItem, FatturaRiga>> _selectSub;

		private Subscription<Remove<FatturaRigaItem, FatturaRiga>> _subRemove;

		private Subscription<FatturaSave> _subSave;

		public ControllerRigheFatture(ControllerFatturazione controllerFatturazione)
		{
			_controllerFatturazione = controllerFatturazione;

			SelectedItem = new FatturaRiga();

			_removePrio = EventAggregator.Instance().Subscribe<AddPriority<FatturaRigaItem, FatturaRiga>>((a) => { CambiaPriorita(true); }); ;
			_addPrio = EventAggregator.Instance().Subscribe<RemovePriority<FatturaRigaItem, FatturaRiga>>((a) => { CambiaPriorita(false); }); ;

			_selectSub = EventAggregator.Instance().Subscribe<Add<FatturaRigaItem, FatturaRiga>>((a) =>
			{
				SelectedItem = new FatturaRiga() { IvaApplicata = "22" };
			});
			_subRemove = EventAggregator.Instance().Subscribe<Remove<FatturaRigaItem, FatturaRiga>>((a) =>
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
					return;
				using (var saveManager = new SaveEntityManager())
				{
					var uof = saveManager.UnitOfWork;
					var curItem = (FatturaRiga)SelectedItem;
					if (curItem.ID > 0)
					{
						var item = uof.FattureRigheRepository.Find(b => b.ID == curItem.ID).First();
						uof.FattureRigheRepository.Delete(item);

						if (saveManager.SaveEntity(enSaveOperation.OpDelete))
						{
							EventAggregator.Instance().Publish<UpdateList<FatturaRiga>>(new UpdateList<FatturaRiga>());
						}
					}
				}
			});
			_subSave = EventAggregator.Instance().Subscribe<FatturaSave>((a) =>
		   {
			   Save(null);
		   });
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~ControllerRigheFatture()
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

		public override void RefreshList(UpdateList<FatturaRiga> obj)
		{
			try
			{
				var list = new List<FatturaRigaItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.FattureRigheRepository.Find(a => a.FatturaID == _controllerFatturazione.SelectedItem.ID

					).Select(a => new FatturaRigaItem
					{
						ID = a.ID.ToString(),
						CodiceArt = a.CodiceArticoloOld,
						RigaDescrizione = a.Descrizione,
						Entity = a,
						RigaImporto = a.Qta * a.PrezzoUnitario,
						PrezzoUnitario = a.PrezzoUnitario,
						RigaQta = a.Qta,
						Iva = a.IvaApplicata
					}).OrderBy(a => a.Entity.OrdineVisualizzazione).ThenBy(a => a.ID).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<FatturaRigaItem>(list);
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
				EventAggregator.Instance().UnSbscribe(_addPrio);
				EventAggregator.Instance().UnSbscribe(_removePrio);
			}
			// free native resources if there are any.
		}

		private void CambiaPriorita(bool aumenta)
		{
			if (_controllerFatturazione.SelectedItem.ID == 0)
				return;

			var itemSel = ((FatturaRiga)SelectedItem);
			if (itemSel.ID == 0)
			{
				return;
			}
			else
			{
				try
				{
					using (var curs = new CursorManager())
					{
						using (var save = new SaveEntityManager())
						{
							var uof = save.UnitOfWork;
							var list = uof.FattureRigheRepository.Find(
								a => a.FatturaID == itemSel.FatturaID).OrderBy(a => a.OrdineVisualizzazione).ToList();
							foreach (var item in list)
							{
								if (item.ID == itemSel.ID)
								{
									if (aumenta)
									{
										var itemToUpdate = list.Where(a => a.OrdineVisualizzazione == item.OrdineVisualizzazione - 1).FirstOrDefault();
										if (itemToUpdate != null)
										{
											itemToUpdate.OrdineVisualizzazione++;
										}
										item.OrdineVisualizzazione--;
									}
									else
									{
										var itemToUpdateTwo = list.Where(a => a.OrdineVisualizzazione == item.OrdineVisualizzazione + 1).FirstOrDefault();
										if (itemToUpdateTwo != null)
										{
											itemToUpdateTwo.OrdineVisualizzazione--;
										}
										item.OrdineVisualizzazione++;
									}
								}
							}
							var setOrdine = 0;
							foreach (var item in list.OrderBy(a => a.OrdineVisualizzazione))
							{
								item.OrdineVisualizzazione = setOrdine;
								setOrdine++;
								uof.FattureRigheRepository.Update(item);
							}

							if (save.SaveEntity(enSaveOperation.OpSave))
							{
								RiselezionaSelezionato();
							}
						}
					}
				}
				catch (Exception ex)
				{
					ExceptionManager.ManageError(ex);
				}
			}
		}

		private void RiselezionaSelezionato()
		{
			var item = (FatturaRiga)SelectedItem;
			EventAggregator.Instance().Publish<UpdateList<FatturaRiga>>(new UpdateList<FatturaRiga>());
			EventAggregator.Instance().Publish<ItemSelected<FatturaRigaItem, FatturaRiga>>(
				new ItemSelected<FatturaRigaItem, FatturaRiga>(new FatturaRigaItem()
				{
					ID = item.ID.ToString(),
					Entity = item
				}));
		}

		private void Save(FatturaSave obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				((FatturaRiga)SelectedItem).FatturaID = _controllerFatturazione.SelectedItem.ID;
				if (_controllerFatturazione.SelectedItem.ID == 0)
					return;
				var uof = saveManager.UnitOfWork;
				if ((((FatturaRiga)SelectedItem).ID > 0))
				{
					uof.FattureRigheRepository.Update((FatturaRiga)SelectedItem);
				}
				else
				{
					uof.FattureRigheRepository.Add((FatturaRiga)SelectedItem);
				}

				if (saveManager.SaveEntity(""))
				{
					RiselezionaSelezionato();
				}
			}
		}
	}
}