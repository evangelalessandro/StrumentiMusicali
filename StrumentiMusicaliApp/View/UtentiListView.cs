using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Entity;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public class UtentiListView : BaseGridViewGeneric<UtentiItem, ControllerUtenti, Utente>, IMenu, IDisposable
	{
		 
		public UtentiListView(ControllerUtenti controller)
			: base(controller)
		{
			this.Load += control_Load;

		}
		private void control_Load(object sender, System.EventArgs e)
		{
			//dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.BestFitColumns(true);
			dgvRighe.Columns["ID"].VisibleIndex = 0;
		}
		
		
		public new void Dispose()
		{
			base.Dispose();	
		}
	}
	
}