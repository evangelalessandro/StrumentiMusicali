using StrumentiMusicali.App.Core.Events;
using StrumentiMusicali.App.Core.Events.Scontrino;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Item;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Scontrino
{
    public class ScontrinoUtility : System.IDisposable
    {
        private DevExpress.XtraGrid.Views.Grid.GridView _dgvScontrino;
        private DevExpress.XtraGrid.GridControl _gcScontrino;
        public ScontrinoUtility()
        {
            EventAggregator.Instance().Subscribe<ScontrinoAddEvents>(AggiungiArticolo);
            EventAggregator.Instance().Subscribe<ScontrinoClear>(RipulisciScontrino);
            EventAggregator.Instance().Subscribe<ScontrinoStampa>(StampaScontrino);
        }

        private void StampaScontrino(ScontrinoStampa obj)
        {
            if (Datasource.Count==0)
            {
                MessageManager.NotificaWarnig("Non ci sono articoli da stampare");
                return;
            }

        }
         
        private void RipulisciScontrino(ScontrinoClear obj)
        {
            Datasource.Clear();
            Reffreshlist();
        }

        private List<ScontrinoLineItem> Datasource { get; set; } = new List<ScontrinoLineItem>();
        public void Init(Control control)
        {
            _gcScontrino = new DevExpress.XtraGrid.GridControl();
            _dgvScontrino = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).BeginInit();

            //
            // _gridControlScontrino
            //
            _gcScontrino.Location = new System.Drawing.Point(8, 0);
            _gcScontrino.MainView = _dgvScontrino;
            _gcScontrino.Name = "_gridControlScontrino";
            _gcScontrino.Size = new System.Drawing.Size(411, 244);
            _gcScontrino.TabIndex = 0;
            _gcScontrino.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            _dgvScontrino});
            _gcScontrino.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // gridView1
            //
            _dgvScontrino.GridControl = _gcScontrino;
             
            _dgvScontrino.Name = "gridView1";
            _dgvScontrino.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 20, System.Drawing.FontStyle.Bold);
            control.Controls.Add(_gcScontrino);

            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).EndInit();
            UtilityView.InitGridDev(_dgvScontrino);
            _dgvScontrino.OptionsBehavior.Editable = true;
            _dgvScontrino.OptionsView.ShowAutoFilterRow = false;
            Reffreshlist();
        }

        private void AggiungiArticolo(ScontrinoAddEvents obj)
        {
            var newitem = (new ScontrinoLineItem()
            {
                Articolo = obj.Articolo.ID,
                Descrizione = obj.Articolo.Titolo,
                PrezzoIvato = obj.Articolo.Prezzo
            });
            if (obj.Articolo.NonImponibile)
            {
                newitem.IvaPerc = 0;
            }
            else
            {
                newitem.IvaPerc = 22;
            }
            Datasource.Add(newitem);
            Reffreshlist();
        }

        private void Reffreshlist()
        {
            _gcScontrino.DataSource = Datasource;
            _gcScontrino.RefreshDataSource();
        }

        public void Dispose(bool dispose)
        {
            if (dispose)
            {
                _dgvScontrino.Dispose();
                _gcScontrino.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}