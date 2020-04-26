using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.CustomComponents;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Articoli;

namespace StrumentiMusicali.App.View.Articoli
{
    public partial class ArticoliListView :
        BaseGridViewGeneric<ArticoloItem, ControllerArticoli, Articolo>
    { 

        public ArticoliListView(ControllerArticoli controller)
            : base(controller)
        {

            onEditItemShowView += (a, b) =>
            { b.Cancel = true; };
            if (controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloLibri)
                AggiungiFiltroLibro(controller);
            if (controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti)
                AggiungiFiltroMarca(controller);

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
                    controller.FiltroLibri = ((AutoCompleteTextBox)control.ControlToBind).Text;

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

            dgvRighe.BestFitColumns(true);
            dgvRighe.Columns["Marca"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti;
            dgvRighe.Columns["Categoria"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti;
            dgvRighe.Columns["Reparto"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti;
            dgvRighe.Columns["QuantitaTotale"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti;
            dgvRighe.Columns["PrezzoAcquisto"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloStrumenti;

            dgvRighe.Columns["Settore"].Visible = Controller.ModalitaController == ControllerArticoli.enModalitaArticolo.SoloLibri;
        }


    }
}