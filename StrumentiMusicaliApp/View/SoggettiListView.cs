using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.App.View
{
    public class SoggettiListView : BaseGridViewGeneric<ClientiItem, ControllerClienti, Soggetto>, IMenu, IDisposable
    {
        public SoggettiListView(ControllerClienti controller)
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

            dgvRighe.Columns["RagioneSociale"].Caption = @"Ragione sociale\Nome Cognome";
            dgvRighe.Columns["PIVA"].Caption = @"PIVA\Codice fiscale";
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}