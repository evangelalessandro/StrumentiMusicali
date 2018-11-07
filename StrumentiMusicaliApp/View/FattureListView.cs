using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.View
{
	public partial class FattureListView : 
		BaseGridViewGeneric<FatturaItem,ControllerFatturazione,Fattura>, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		 
		public new MenuTab GetMenu()
		{
			return Controller.GetMenu();
		}
		public	FattureListView(ControllerFatturazione controller, enAmbiente ambienteLista,enAmbiente ambienteDettaglio)
			:base(controller)
		{
			
			this.onEditItemShowView += FattureListView_onEditItemShowView;
		}

		private void FattureListView_onEditItemShowView(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			
		}

		public override void FormatGrid()
		{  
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.Columns["ID"].DisplayIndex = 0;
			dgvRighe.Columns["Codice"].DisplayIndex = 1;
			dgvRighe.Columns["RagioneSociale"].DisplayIndex = 2;
		}
		 
		 
	}
}