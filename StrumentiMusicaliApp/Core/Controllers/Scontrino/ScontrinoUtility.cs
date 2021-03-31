using StrumentiMusicali.App.Core.Events;
using StrumentiMusicali.App.Core.Events.Scontrino;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Item;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using StrumentiMusicali.Core.Settings;
using System.Text;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Repo;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraEditors.Repository;

namespace StrumentiMusicali.App.Core.Controllers.Scontrino
{
    public class ScontrinoUtility : System.IDisposable
    {
        private DevExpress.XtraGrid.Views.Grid.GridView _dgvScontrino;
        private DevExpress.XtraGrid.GridControl _gcScontrino;
        public ScontrinoUtility()
        {
            EventAggregator.Instance().Subscribe<ScontrinoAddEvents>(AggiungiArticolo);

            EventAggregator.Instance().Subscribe<ScontrinoRemoveLineEvents>(ScontrinoRemoveLine);
            EventAggregator.Instance().Subscribe<ScontrinoClear>(RipulisciScontrino);
            EventAggregator.Instance().Subscribe<ScontrinoStampa>(StampaScontrino);
        }

        private void ScontrinoRemoveLine(ScontrinoRemoveLineEvents obj)
        {
            try
            {

                var row = (ScontrinoLineItem)_dgvScontrino.GetRow(_dgvScontrino.FocusedRowHandle);
                if (row.TipoRigaScontrino == TipoRigaScontrino.Totale
                    || row.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato)
                {
                    MessageManager.NotificaInfo("Non si elimina il totale o lo sconto");
                    return;
                }


                Datasource.Remove(row);



                this.Reffreshlist();
            }
            catch (Exception ex)
            {
                MessageManager.NotificaError("Errore nella rimozione", ex);

            }
        }

        private List<ScontrinoLineItem> Datasource { get; set; } = new List<ScontrinoLineItem>();

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
            _dgvScontrino.ShowingEditor += _dgvScontrino_ShowingEditor;
            _dgvScontrino.RowStyle += _dgvScontrino_RowStyle;
            _dgvScontrino.Name = "gridView1";
            _dgvScontrino.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 20, System.Drawing.FontStyle.Bold);
            control.Controls.Add(_gcScontrino);

            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).EndInit();
            UtilityView.InitGridDev(_dgvScontrino);
            _dgvScontrino.OptionsBehavior.Editable = true;
            _dgvScontrino.OptionsView.ShowAutoFilterRow = false;




            Reffreshlist();
            _dgvScontrino.OptionsCustomization.AllowSort = false;

            var rp = new RepositoryItemTextEdit();
            rp.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;


            rp.Mask.EditMask = "P0";
            _gcScontrino.RepositoryItems.Add(rp);
            var col = _dgvScontrino.Columns["ScontoPerc"];

            _dgvScontrino.Columns["ScontoPerc"].ColumnEdit = rp;
            _dgvScontrino.Columns["ScontoPerc"].OptionsColumn.AllowEdit = true;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            col.DisplayFormat.FormatString = "{0:n0} %";


        }
        private void _dgvScontrino_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                var line = (ScontrinoLineItem)view.GetRow(e.RowHandle);
                if ((line.TipoRigaScontrino == TipoRigaScontrino.Totale))
                {
                    e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, 12, FontStyle.Bold);// = Color.FromArgb(150, Color.LightCoral);
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                }
                if ((line.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato))
                {
                    e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, e.Appearance.Font.Size, FontStyle.Italic);// = Color.FromArgb(150, Color.LightCoral);
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Aqua);

                }
                //string priority = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Priority"]);
                //if (priority == "High")
                //{
                //e.Appearance.BackColor = Color.FromArgb(150, Color.LightCoral);
                //e.Appearance.BackColor2 = Color.White;
                //}
            }
        }
        private void _dgvScontrino_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridView view = sender as GridView;
            var focusCol = view.FocusedColumn.FieldName;
            if (focusCol == "Ordine" || focusCol == "TipoRigaScontrino")
                e.Cancel = true;

            var line = (ScontrinoLineItem)view.GetRow(view.FocusedRowHandle);
            if ((line.TipoRigaScontrino == TipoRigaScontrino.Totale))
                e.Cancel = true;
            else
            {
                if ((line.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato))
                {
                    if (focusCol == "ScontoPerc" || focusCol == "IvaPerc")
                        e.Cancel = true;
                }
            }

        }

        private void AggiungiArticolo(ScontrinoAddEvents obj)
        {
            var newitem = (new ScontrinoLineItem()
            {

                Descrizione = obj.Articolo.Titolo,
                PrezzoIvato = obj.Articolo.Prezzo,
                TipoRigaScontrino = TipoRigaScontrino.Vendita,


            });
            if (obj.Articolo.NonImponibile)
            {
                newitem.IvaPerc = 0;
            }
            else
            {
                newitem.IvaPerc = 22;
            }
            Datasource.Insert(0, newitem);
            if (Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita).Count() > 0)
            {
                if (Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato).Count() == 0)
                {
                    Datasource.Add(new ScontrinoLineItem()
                    {
                        TipoRigaScontrino = TipoRigaScontrino.ScontoIncondizionato,

                        Descrizione = "Abbuono",
                        PrezzoIvato = 0
                    });
                }
                if (Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).Count() == 0)
                {
                    Datasource.Add(new ScontrinoLineItem()
                    {
                        TipoRigaScontrino = TipoRigaScontrino.Totale,

                        Descrizione = "Totale",
                        PrezzoIvato = 0
                    });
                }

            }
            Reffreshlist();
        }



        private void Newitem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RefreshTotale();
        }

        private void Reffreshlist()
        {
            Datasource.OrderByDescending(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).ThenByDescending(a => a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato).ToList();
            foreach (var item in Datasource)
            {

                ((INotifyPropertyChanged)item).PropertyChanged -= Newitem_PropertyChanged;
                ((INotifyPropertyChanged)item).PropertyChanged += Newitem_PropertyChanged;
            }

            _gcScontrino.DataSource = Datasource;
            _gcScontrino.RefreshDataSource();




            RefreshTotale();
        }

        private void RefreshTotale()
        {
            var datoTotale = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).FirstOrDefault();
            decimal tot = 0;
            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita))
            {
                if (item.ScontoPerc != 0)
                {
                    tot = tot + (item.PrezzoIvato / ((decimal)100.0) * (((decimal)100) - Math.Abs(item.ScontoPerc)));
                }
                else
                {
                    tot = tot + item.PrezzoIvato;
                }

            }
            var scontoFinale = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato).FirstOrDefault();
            if (scontoFinale != null)
            {
                tot = tot - Math.Abs(scontoFinale.PrezzoIvato);
            }
            if (datoTotale != null)
                datoTotale.PrezzoIvato = Math.Round(tot, 2);

        }

        private void RipulisciScontrino(ScontrinoClear obj)
        {
            Datasource.Clear();
            Reffreshlist();
        }

        private void ScriviFile()
        {



            var listRighe = new List<ScontrinoLine>();
            for (int i = 0; i < Datasource.Count(); i++)
            {
                var a = Datasource[i];
                if (a.TipoRigaScontrino != TipoRigaScontrino.ScontoIncondizionato)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = a.Descrizione, IvaPerc = a.IvaPerc, Qta = 1, PrezzoIvato = a.PrezzoIvato, TipoRigaScontrino = a.TipoRigaScontrino });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.Vendita && a.ScontoPerc > 0)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = "Sconto " + a.ScontoPerc.ToString() + "%", IvaPerc = 0, Qta = 1, PrezzoIvato = -a.PrezzoIvato * (a.ScontoPerc) / 100, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato && a.PrezzoIvato > 0)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = a.Descrizione, IvaPerc = 0, Qta = 1, PrezzoIvato = -a.PrezzoIvato, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
            }




            using (var uof = new UnitOfWork())
            {
                var list = uof.TipiPagamentoScontrinoRepository.Find(a => a.Enable == true).ToList().Select(a => a.Codice.ToString() + " " + a.Descrizione).ToList();
                using (var tipiPagamento = new ListViewCustom(list, "Tipo pagamento"))
                {
                    tipiPagamento.ShowDialog();
                    var pagamento = tipiPagamento.SelectedItem;

                    if (string.IsNullOrEmpty(pagamento))
                    {
                        MessageManager.NotificaWarnig("Occorre selezionare il tipo di pagamento");

                        return;
                    }
                    var tot = listRighe.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).First();

                    tot.Pagamento = pagamento;
                    tot.TotaleComplessivo = tot.TotaleRiga;

                    var content = listRighe.Select(a => a.ToString()).ToList().ToArray();

                    var validator = SettingScontrinoValidator.ReadSetting();

                    System.IO.File.WriteAllLines(
                        System.IO.Path.Combine(validator.FolderDestinazione, DateTime.Now.Ticks.ToString() + ".txt"), content);

                    MessageManager.NotificaInfo("Scontrino pubblicato al servizio correttamente!");


                    RipulisciScontrino(new ScontrinoClear());

                }
            }
        }

        private void StampaScontrino(ScontrinoStampa obj)
        {

            if (!SettingScontrinoValidator.Check())
                return;
            if (Datasource.Count == 0)
            {
                MessageManager.NotificaWarnig("Non ci sono articoli da stampare");
                return;
            }

            /*
             Il file deve contenere le righe di vendita cosi strutturate  :

 

                Descrizione : per il tipo di riga V deve sempre esserci , per il resto è opzionale
                Aliquota Iva : per il tipo di riga V deve sempre esserci , per il resto è opzionale
                Quantità : per il tipo di riga V deve sempre esserci , per il resto è opzionale
                Totale : è sempre obbligatoria
                Tipo Riga (V,T) : è sempre obbligatoria
                Extra : è sempre opzionale
                
                    
                -V sta per Vendita
                -T sta per Totale
                -S sconto e importo
                Vino Lambrusco ; 22 ;1;0,75;2,50;V;

 

                Es riga Totale :

 

                Totale; ; ; ; 2,50;T;1 Riga con pagamento in contanti

                Totale; ; ; ; 2,50;T;45 Riga con pagamento bancomat

                Totale; ; ; ; 2,50;T;5 Riga con pagamento non riscosso
            */
            ScriviFile();
        }
    }
}