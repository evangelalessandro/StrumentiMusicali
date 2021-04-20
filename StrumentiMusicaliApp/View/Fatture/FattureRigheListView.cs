using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;

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
            gcControl.DataSource = _controllerRigheFatture.DataSource;

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
                
               
               ItemEditorManager manager = new ItemEditorManager(gcControl, dgvRighe);
               manager.BindProp(new FatturaRigaItem(), "");

               dgvRighe.BestFitColumns(true);
               dgvRighe.Columns["Entity"].Visible = false;
               dgvRighe.Columns["ID"].Visible = false;
               //dgvRighe.Columns["RigaDescrizione"].VisibleIndex = 1;
               //dgvRighe.Columns["RigaQta"].VisibleIndex = 2;
               //dgvRighe.Columns["PrezzoUnitario"].VisibleIndex = 3;
               //dgvRighe.Columns["RigaImporto"].VisibleIndex = 4;
               //dgvRighe.Columns["Iva"].VisibleIndex = 5;
           });
        }
    }
}