using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.CustomComponents;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.View
{
	public partial class ArticoliListView :
		BaseGridViewGeneric<ArticoloItem, ControllerArticoli, Articolo>
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

	 
		public ArticoliListView(ControllerArticoli controller)
			: base(controller)
		{

			onEditItemShowView += ((a, b) =>
			{ b.Cancel = true; });
			AggiungiFiltroMarca(controller);
			AggiungiFiltroLibro(controller);
		}

		private void AggiungiFiltroLibro(ControllerArticoli controller)
		{
			BaseControl.ElementiDettaglio.EDTesto control = new BaseControl.ElementiDettaglio.EDTesto();
			control.Titolo = "Libri";
			control.Width = 200;
			pnlCerca.Controls.Add(control);
			control.BindProprieta(null, "FiltroLibri", controller);
			((AutoCompleteTextBox)control.ControlToBind).KeyUp += (a, b) =>
			{
				if (b.KeyCode == System.Windows.Forms.Keys.Return)
				{
					controller.FiltroLibri= ((AutoCompleteTextBox)control.ControlToBind).Text;

					RicercaRefresh();
				}
			};
		}

		private void AggiungiFiltroMarca(ControllerArticoli controller)
		{
			BaseControl.ElementiDettaglio.EDTesto control = new BaseControl.ElementiDettaglio.EDTesto();
			control.Titolo = "Marca";
			control.Width = 200;
			pnlCerca.Controls.Add(control);
			control.BindProprieta(null, "FiltroMarca", controller);
			((AutoCompleteTextBox)control.ControlToBind).KeyUp += (a, b) =>
			{
				if (b.KeyCode == System.Windows.Forms.Keys.Return)
				{
					controller.FiltroMarca = ((AutoCompleteTextBox)control.ControlToBind).Text;

					RicercaRefresh();
				}
			};
		}


		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.Columns["ID"].DisplayIndex = 0;
		}


	}
}