using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Entity;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public class FattureRigheListView : BaseGridViewGeneric<FatturaRigaItem, ControllerRigheFatture, FatturaRiga>
	{
		private ControllerRigheFatture _controllerRigheFatture;

		public FattureRigheListView(ControllerRigheFatture controllerRigheFatture)
			: base(controllerRigheFatture)
		{
			//this.Load += FattureRigheListView_Load;
			_controllerRigheFatture = controllerRigheFatture;
		}

		private void FattureRigheListView_Load(object sender, System.EventArgs e)
		{
			_controllerRigheFatture.RefreshList(null);
			gridControl1.DataSource = _controllerRigheFatture.DataSource;

			dgvRighe.RefreshData();
			//EventAggregator.Instance().Subscribe<UpdateList<FatturaRiga>>(RefreshList);

			//dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		//private void RefreshList(UpdateList<FatturaRiga> obj)
		//{
		//	this.Invalidate();

		//	dgvRighe.DataSource = _controllerRigheFatture.DataSource;

		//	dgvRighe.Refresh();
		//	dgvRighe.Update();
		//}

		public override void FormatGrid()
		{
			this.InvokeIfRequired((b) =>
		   {
			   var provider = new System.Globalization.CultureInfo("it-IT");
			   //var provider = new System.Globalization.CultureInfo("en");
			   if (dgvRighe.Columns.Count == 0)
				   return;
			   //dgvRighe.Columns["PrezzoUnitario"].DisplayFormat..FormatProvider = provider;
			   dgvRighe.Columns["PrezzoUnitario"].DisplayFormat.FormatString = "C2";
			   //dgvRighe.Columns["PrezzoUnitario"]. = DataGridViewContentAlignment.MiddleRight;

			   //dgvRighe.Columns["RigaImporto"].DefaultCellStyle.FormatProvider = provider;
			   dgvRighe.Columns["RigaImporto"].DisplayFormat.FormatString = "C2";
			   //dgvRighe.Columns["RigaImporto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			   dgvRighe.Columns["Entity"].Visible = false;
			   dgvRighe.Columns["ID"].Visible = false;
			   dgvRighe.BestFitColumns(true);
			   dgvRighe.Columns["CodiceArt"].VisibleIndex = 0;
			   dgvRighe.Columns["RigaDescrizione"].VisibleIndex = 1;
			   dgvRighe.Columns["RigaQta"].VisibleIndex = 2;
			   dgvRighe.Columns["PrezzoUnitario"].VisibleIndex = 3;
			   dgvRighe.Columns["RigaImporto"].VisibleIndex = 4;
			   dgvRighe.Columns["Iva"].VisibleIndex = 5;
		   });
		}
	}
}