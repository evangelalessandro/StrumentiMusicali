using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
	[AddINotifyPropertyChangedInterface]
	public class ControllerRigheFatture :    BaseControllerGeneric<FatturaRiga, FatturaRigaItem>
	{
		ControllerFatturazione _controllerFatturazione;
		public ControllerRigheFatture(ControllerFatturazione controllerFatturazione)
		{
			_controllerFatturazione = controllerFatturazione;

			SelectedItem = new FatturaRiga();
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

				DataSource = list;
			}
			catch (Exception ex)
			{
				new Action(() =>
				{ ExceptionManager.ManageError(ex); }).BeginInvoke(null,null);
			}


		}
	}
}
