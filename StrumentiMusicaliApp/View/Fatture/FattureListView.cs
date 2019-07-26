using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.View.Enums;

namespace StrumentiMusicali.App.View
{
    public partial class FattureListView :
        BaseGridViewGeneric<FatturaItem, ControllerFatturazione, Fattura>, IMenu
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public new MenuTab GetMenu()
        {
            return Controller.GetMenu();
        }
        public FattureListView(ControllerFatturazione controller, enAmbiente ambienteLista, enAmbiente ambienteDettaglio)
            : base(controller)
        {

            this.onEditItemShowView += FattureListView_onEditItemShowView;
        }

        private void FattureListView_onEditItemShowView(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

        }

        public override void FormatGrid()
        {
            //dgvRighe.Columns["Entity"].Visible = false;
            dgvRighe.BestFitColumns(true);
            if (dgvRighe.Columns.Count > 0)
            {
                dgvRighe.Columns["ID"].VisibleIndex = 0;
                dgvRighe.Columns["Codice"].VisibleIndex = 1;
                dgvRighe.Columns["RagioneSociale"].VisibleIndex = 2;
            }
        }


    }
}