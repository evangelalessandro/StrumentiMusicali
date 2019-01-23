using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Entity;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public class DepositiListView : BaseGridViewGeneric<DepositoItem, ControllerDepositi, Deposito>
	{
		 
		public DepositiListView(ControllerDepositi controller)
			: base(controller)
		{
			this.Load += control_Load;
		}
		private void control_Load(object sender, System.EventArgs e)
		{
		 
			
		}

		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.BestFitColumns(true);
			dgvRighe.Columns["ID"].VisibleIndex = 0;
		}
		 
	}
	
}