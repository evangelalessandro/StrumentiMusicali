using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public class FattureRigheListView : BaseControl.BaseGridViewGeneric<FatturaRigaItem, ControllerRigheFatture, FatturaRiga>
	{
		ControllerRigheFatture _controllerRigheFatture;

		 
		public FattureRigheListView(ControllerRigheFatture controllerRigheFatture)
			: base(controllerRigheFatture)
		{
			this.Load += FattureRigheListView_Load;
			_controllerRigheFatture = controllerRigheFatture;
		}

		private void FattureRigheListView_Load(object sender, System.EventArgs e)
		{
			_controllerRigheFatture.RefreshList(null);
			dgvRighe.DataSource = _controllerRigheFatture.DataSource ;

			 
			dgvRighe.Refresh();
			EventAggregator.Instance().Subscribe<UpdateList<FatturaRiga>>(RefreshList);

			dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void RefreshList(UpdateList<FatturaRiga> obj)
		{
			this.Invalidate();

			dgvRighe.DataSource = _controllerRigheFatture.DataSource;

			dgvRighe.Refresh();
			dgvRighe.Update();

		}

		public override void FormatGrid()
		{


			var provider = new System.Globalization.CultureInfo("it-IT");
			//var provider = new System.Globalization.CultureInfo("en");

			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.FormatProvider = provider;
			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.Format = "C2";
			dgvRighe.Columns["PrezzoUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgvRighe.Columns["RigaImporto"].DefaultCellStyle.FormatProvider = provider;
			dgvRighe.Columns["RigaImporto"].DefaultCellStyle.Format = "C2";
			dgvRighe.Columns["RigaImporto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.Columns["ID"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.Columns["CodiceArt"].DisplayIndex = 0;
			dgvRighe.Columns["RigaDescrizione"].DisplayIndex = 1;
			dgvRighe.Columns["RigaQta"].DisplayIndex = 2;
			dgvRighe.Columns["PrezzoUnitario"].DisplayIndex = 3;
			dgvRighe.Columns["RigaImporto"].DisplayIndex = 4;
			dgvRighe.Columns["Iva"].DisplayIndex = 5;

		}
	}
}
