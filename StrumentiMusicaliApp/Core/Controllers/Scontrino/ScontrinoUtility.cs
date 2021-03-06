﻿using StrumentiMusicali.App.Core.Events;
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
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Core.Manager;
using DevExpress.XtraEditors;

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
        RepositoryItemButtonEdit emptyEditor;
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
        enum enListTipoScontrino
        {
            Scontrino,
            ScontrinoDocScarico,
            DocScarico
        }

        private DevExpress.XtraEditors.LookUpEdit cboTipoDoc = new LookUpEdit();
        private DevExpress.XtraEditors.LookUpEdit cboListinoPrezzi = new LookUpEdit();
        public void Init(Control controlParent)
        {
            _gcScontrino = new DevExpress.XtraGrid.GridControl();
            _dgvScontrino = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).BeginInit();

            InitCombo(controlParent, cboTipoDoc);
            InitCombo(controlParent, cboListinoPrezzi);

            Splitter splitter = new Splitter();
            splitter.Dock = DockStyle.Top;
            controlParent.Controls.Add(splitter);
            splitter.BringToFront();



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
            controlParent.Controls.Add(_gcScontrino);
            _gcScontrino.BringToFront();
            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).EndInit();
            UtilityView.InitGridDev(_dgvScontrino);
            _dgvScontrino.OptionsBehavior.Editable = true;
            _dgvScontrino.OptionsView.ShowAutoFilterRow = false;


            emptyEditor = new RepositoryItemButtonEdit();
            emptyEditor.Buttons.Clear();
            emptyEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            _gcScontrino.RepositoryItems.Add(emptyEditor);


            Reffreshlist();
            _dgvScontrino.OptionsCustomization.AllowSort = false;

            var rp = new RepositoryItemTextEdit();
            rp.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;


            rp.Mask.EditMask = "P0";
            _gcScontrino.RepositoryItems.Add(rp);
            var col = _dgvScontrino.Columns["ScontoPerc"];


            col.ColumnEdit = rp;
            col.OptionsColumn.AllowEdit = true;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            col.DisplayFormat.FormatString = "{0:n0} %";

            col = _dgvScontrino.Columns["IvaPerc"];
            col.ColumnEdit = rp;
            col.OptionsColumn.AllowEdit = true;
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            col.DisplayFormat.FormatString = "{0:n0} %";

            _dgvScontrino.Columns["TipoRigaScontrino"].Visible = false;

            _dgvScontrino.Columns["Articolo"].Visible = false;
            _dgvScontrino.CustomDrawCell += _dgvScontrino_CustomDrawCell;

            InitTipoDoc();
            InitListinoPrezzi();
        }

        private void InitCombo(Control controlParent, LookUpEdit cbo)
        {

            cbo.Location = new System.Drawing.Point(24, 42);
            cbo.Name = "cbo";
            cbo.Properties.Appearance.Options.UseTextOptions = true;
            cbo.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            cbo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            cbo.Properties.NullText = "Nessuna voce selezionata";
            cbo.Size = new System.Drawing.Size(266, 100);

            cbo.TabIndex = 0;
            cbo.Dock = DockStyle.Top;
            controlParent.Controls.Add(cbo);

            cbo.Properties.ValueMember = "ID";
            cbo.Properties.DisplayMember = "Descrizione";
        }

        private void InitTipoDoc()
        {
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();
            list.Add(new Tuple<int, string>((int)enListTipoScontrino.Scontrino, "Scontrino"));
            list.Add(new Tuple<int, string>((int)enListTipoScontrino.ScontrinoDocScarico, "Scontrino + Doc.Scarico"));
            list.Add(new Tuple<int, string>((int)enListTipoScontrino.DocScarico, "Doc.Scarico"));
            cboTipoDoc.Properties.DataSource = list.Select(a => new { ID = a.Item1, Descrizione = a.Item2 }).ToList();
            cboTipoDoc.Properties.PopulateColumns();
            //cboTipoDoc.Properties.Columns[0].Visible = false;
            cboTipoDoc.EditValue = 0;
        }
        private void InitListinoPrezzi()
        {
            using (var uof = new UnitOfWork())
            {

                DateTime now = DateTime.Now;
                var list = uof.ListinoPrezziVenditaNomeRepository.Find(a => 1 == 1).ToList();
                list = list.Where(a => a.DataInizioValidita <= now &&
                     a.DataFineValidita >= now).ToList();

                var listSel=list.Select(a => new { ID = a.ID, Descrizione = a.Nome, a.PercentualeVariazione }).OrderBy(a=>a.Descrizione).ToList();
                cboListinoPrezzi.Properties.DataSource = listSel;
                

                cboListinoPrezzi.Properties.PopulateColumns();
                cboListinoPrezzi.EditValueChanged += CboListinoPrezzi_EditValueChanged;
                if (list.Count == 0)
                {
                    cboListinoPrezzi.Properties.NullText = "Nessun listino presente";
                }
                else
                {
                    cboListinoPrezzi.EditValue = listSel.First().ID;
                }
            }
        }


        private void CboListinoPrezzi_EditValueChanged(object sender, EventArgs e)
        {
            var intVal = (int)cboListinoPrezzi.EditValue;

            using (var uof = new UnitOfWork())
            {

                DateTime now = DateTime.Now;
                var variazione = uof.ListinoPrezziVenditaNomeRepository.Find(a => a.ID == intVal).
                    Select(a => a.PercentualeVariazione).DefaultIfEmpty(0).FirstOrDefault();
                PercentualeVariazioneListinoAttuale = -variazione;
                foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita))
                {
                    item.ScontoPerc = PercentualeVariazioneListinoAttuale;
                    
                }

                Reffreshlist();
            }
        }
        private int PercentualeVariazioneListinoAttuale { get; set; }

        private void _dgvScontrino_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            GridView view = sender as GridView;
            if (e.Column.FieldName == "Descrizione")
                return;
            if (e.Column.FieldName == "TipoRigaScontrino")
                return;
            if (e.RowHandle >= 0)
            {
                var line = (ScontrinoLineItem)view.GetRow(e.RowHandle);
                if ((line.TipoRigaScontrino == TipoRigaScontrino.Totale)
                    || (line.TipoRigaScontrino == TipoRigaScontrino.Incassato)

                    || (line.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato))
                {
                    if (e.Column.FieldName != "PrezzoIvato")
                    {
                        e.DisplayText = "";
                        e.Appearance.BackColor = Color.Transparent;
                    }
                }
            }
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
                    e.Appearance.BackColor = Color.FromArgb(150, Color.GreenYellow);
                }
                if ((line.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato))
                {
                    e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, e.Appearance.Font.Size, FontStyle.Italic);// = Color.FromArgb(150, Color.LightCoral);
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Aqua);

                }
                if ((line.TipoRigaScontrino == TipoRigaScontrino.Incassato))
                {
                    e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, 12, FontStyle.Bold);// = Color.FromArgb(150, Color.LightCoral);
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Coral);

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
            if ((line.TipoRigaScontrino == TipoRigaScontrino.Incassato))
            {
                if (focusCol != "PrezzoIvato")
                    e.Cancel = true;

            }


        }

        private void AggiungiArticolo(ScontrinoAddEvents obj)
        {
            var newitem = new ScontrinoLineItem();
            if (obj.Articolo != null)
            {
                using (var uof = new UnitOfWork())
                {
                    var giacenza = uof.MagazzinoRepository.Find(a => obj.Articolo.ID == (a.ArticoloID) && a.Deposito.Principale == true)
                              .Select(a => new { a.ArticoloID, a.Qta }).GroupBy(a => new { a.ArticoloID })
                              .Select(a => new { Sum = a.Sum(b => b.Qta) }).ToList();



                    var giacNegozio = giacenza.DefaultIfEmpty(new { Sum = 0 }).FirstOrDefault().Sum - Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita &&
                    a.Articolo == obj.Articolo.ID).Count();

                    if (giacNegozio == 0)
                    {
                        MessageManager.NotificaWarnig("Quantità in negozio non sufficiente!");
                        return;
                    }

                }
                newitem = (new ScontrinoLineItem()
                {
                    Articolo = obj.Articolo.ID,
                    Descrizione = obj.Articolo.Titolo,
                    PrezzoIvato = obj.Articolo.Prezzo,
                    TipoRigaScontrino = TipoRigaScontrino.Vendita,
                    ScontoPerc = PercentualeVariazioneListinoAttuale

                }) ;
                if (obj.Articolo.NonImponibile)
                {
                    newitem.IvaPerc = 0;
                }
                else
                {
                    newitem.IvaPerc = obj.Articolo.Iva;
                }
            }
            else
            {
                newitem = (new ScontrinoLineItem()
                {
                    Articolo = -1,
                    Descrizione = "Generico",
                    PrezzoIvato = 1,
                    TipoRigaScontrino = TipoRigaScontrino.Vendita,
                    IvaPerc = 22,
                    ScontoPerc = PercentualeVariazioneListinoAttuale
                });
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
                if (Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Incassato).Count() == 0)
                {
                    Datasource.Add(new ScontrinoLineItem()
                    {
                        TipoRigaScontrino = TipoRigaScontrino.Incassato,

                        Descrizione = "Incassato",

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
                if (item.TipoRigaScontrino != TipoRigaScontrino.Incassato)
                    ((INotifyPropertyChanged)item).PropertyChanged += Newitem_PropertyChanged;
            }

            _gcScontrino.DataSource = Datasource;
            _gcScontrino.RefreshDataSource();




            RefreshTotale();
        }

        private void RefreshTotale()
        {
            if (Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita).Count() == 0)
            {
                Datasource.Clear();

            }
            var datoTotale = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).FirstOrDefault();
            decimal tot = 0;
            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita))
            {
                if (item.ScontoPerc != 0)
                {
                    tot = tot + (item.PrezzoIvato / ((decimal)100.0) * (((decimal)100) - (item.ScontoPerc)));
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


            var incassato = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Incassato).FirstOrDefault();
            if (incassato != null
                //&& incassato.PrezzoIvato < datoTotale.PrezzoIvato
                )
            {
                incassato.PrezzoIvato = datoTotale.PrezzoIvato;
            }
        }

        private void RipulisciScontrino(ScontrinoClear obj)
        {
            Datasource.Clear();
            Reffreshlist();
        }

        /// <summary>
        ///  
        ///Il file deve contenere le righe di vendita cosi strutturate  :



        ///       Descrizione : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        ///       Aliquota Iva : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        ///       Quantità : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        ///       Totale : è sempre obbligatoria
        ///       Tipo Riga(V, T) : è sempre obbligatoria
        ///       Extra : è sempre opzionale


        ///       -V sta per Vendita
        ///       -T sta per Totale
        ///       -S sconto e importo
        ///       Vino Lambrusco; 22 ;1;0,75;2,50;V;



        ///       Es riga Totale :



        ///       Totale; ; ; ; 2,50;T;1 Riga con pagamento in contanti

        ///       Totale; ; ; ; 2,50;T;45 Riga con pagamento bancomat

        ///       Totale; ; ; ; 2,50;T;5 Riga con pagamento non riscosso
        ///
        /// </summary>
        private bool ScriviFileScontrino(List<ScontrinoLine> listRighe, bool scaricaGiacenze, SaveEntityManager saveEntity)
        {



            var uof = saveEntity.UnitOfWork;
            {
                var list = uof.TipiPagamentoScontrinoRepository.Find(a => a.Enable == true).ToList().Select(a => a.Codice.ToString() + " " + a.Descrizione).ToList();
                using (var tipiPagamento = new ListViewCustom(list, "Tipo pagamento", "Codice lotteria"))
                {
                    var diag = tipiPagamento.ShowDialog();
                    if (diag != DialogResult.OK)
                        return false;
                    var pagamento = tipiPagamento.SelectedItem;
                    var codiceLotteria = tipiPagamento.SelectedTextProp;

                    if (string.IsNullOrEmpty(pagamento))
                    {
                        MessageManager.NotificaWarnig("Occorre selezionare il tipo di pagamento");

                        return false;
                    }
                    var tot = listRighe.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).First();

                    tot.Pagamento = pagamento.Split(" ".ToCharArray()).First();
                    tot.TotaleComplessivo = tot.TotaleRiga;

                    var incassato = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Incassato).First();

                    tot.TotaleComplessivo = incassato.PrezzoIvato;
                    if (codiceLotteria.Trim().Length > 0)
                    {
                        tot.CodiceLotteria = codiceLotteria;
                    }
                    var content = listRighe.Select(a => a.ToString()).ToList().ToArray();


                    var validator = SettingScontrinoValidator.ReadSetting();
                    if (scaricaGiacenze)
                    {
                        var negozio = uof.DepositoRepository.Find(a => a.Principale == true).First();
                        using (var depo = new Core.Controllers.ControllerMagazzino())
                        {
                            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita & a.Articolo >= 0))
                            {

                                ScaricaQtaMagazzino scarica = new ScaricaQtaMagazzino();

                                scarica.Qta = 1;
                                scarica.Prezzo = item.PrezzoIvato - item.PrezzoIvato * (item.ScontoPerc) / 100;
                                scarica.Deposito = negozio.ID;
                                scarica.ArticoloID = item.Articolo;
                                EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(scarica);

                            }
                        }
                    }
                    System.IO.File.WriteAllLines(
                          System.IO.Path.Combine(validator.FolderDestinazione, DateTime.Now.Ticks.ToString() + ".txt"), content);


                    MessageManager.NotificaInfo("Scontrino pubblicato al servizio correttamente!");

                    return true;
                }
            }
        }
        private void StampaScontrino(ScontrinoStampa obj)
        {
            if (Datasource.Count == 0)
            {
                MessageManager.NotificaWarnig("Non ci sono articoli da stampare");
                return;
            }
            /*cambia riga per salvare il dato*/
            _dgvScontrino.ValidateEditor();
            _dgvScontrino.FocusedRowHandle = 0;

            _dgvScontrino.FocusedRowHandle = 1;



            var listRighe = new List<ScontrinoLine>();
            for (int i = 0; i < Datasource.Count(); i++)
            {
                var a = Datasource[i];
                if (a.TipoRigaScontrino != TipoRigaScontrino.ScontoIncondizionato
                    && a.TipoRigaScontrino != TipoRigaScontrino.Incassato)
                {
                    listRighe.Add(new ScontrinoLine { Articolo = a.Articolo, Descrizione = a.Descrizione, IvaPerc = a.IvaPerc, Qta = 1, PrezzoIvato = a.PrezzoIvato, TipoRigaScontrino = a.TipoRigaScontrino });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.Vendita && a.ScontoPerc != 0)
                {
                    var descr = "Sconto " + a.ScontoPerc.ToString() + "%";
                    if (a.ScontoPerc<0)
                    {
                        descr = "Maggiorazione " + Math.Abs( a.ScontoPerc).ToString() + "%";
                    }
                    listRighe.Add(new ScontrinoLine { Descrizione = descr, IvaPerc = 0, Qta = 1, PrezzoIvato = a.PrezzoIvato * (a.ScontoPerc) / 100, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato && a.PrezzoIvato > 0)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = a.Descrizione, IvaPerc = 0, Qta = 1, PrezzoIvato = a.PrezzoIvato, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
            }
            enListTipoScontrino tipoScontrino = (enListTipoScontrino)cboTipoDoc.EditValue;
            //enListTipoScontrino tipoScontrino = (enListTipoScontrino)Enum.Parse(typeof(enListTipoScontrino), .ToString());
            using (var saveEntity = new SaveEntityManager())
            {
                if (tipoScontrino == enListTipoScontrino.Scontrino ||
                    tipoScontrino == enListTipoScontrino.ScontrinoDocScarico)
                {
                    if (!SettingScontrinoValidator.Check())
                        return;
                    if (!ScriviFileScontrino(listRighe, tipoScontrino == enListTipoScontrino.Scontrino, saveEntity))
                        return;

                }
                if (tipoScontrino == enListTipoScontrino.DocScarico ||
                   tipoScontrino == enListTipoScontrino.ScontrinoDocScarico)
                {
                    if (!GeneraOrdineScarico(saveEntity))
                        return;
                }
            }
            RipulisciScontrino(new ScontrinoClear());
        }

        private bool GeneraOrdineScarico(SaveEntityManager saveEntity)
        {

            using (var uof = new UnitOfWork())
            {
                var list = uof.ClientiRepository.Select(a => new { a.ID, Descrizione = a.RagioneSociale.Length > 0 ? a.RagioneSociale : a.Cognome + " " + a.Nome, CfPIVA = a.PIVA != null && a.PIVA.Length > 0 ? a.PIVA : a.CodiceFiscale }).ToList();

                using (var clientiList = new ListViewCustom(
                    new ListViewCustom.settingCombo() { ValueMember = "ID", DisplayMember = "Descrizione", DataSource = list, TitoloCombo = "Cliente" }))
                {
                    var diag = clientiList.ShowDialog();
                    if (diag != DialogResult.OK)
                        return false;
                    var cliente = int.Parse(clientiList.SelectedItem);


                    var listRighe = new List<ScontrinoLine>();
                    for (int i = 0; i < Datasource.Count(); i++)
                    {
                        var a = Datasource[i];
                        if (a.TipoRigaScontrino != TipoRigaScontrino.ScontoIncondizionato
                            && a.TipoRigaScontrino != TipoRigaScontrino.Incassato
                            && a.TipoRigaScontrino != TipoRigaScontrino.Totale)
                        {
                            listRighe.Add(new ScontrinoLine
                            {
                                Articolo = a.Articolo,
                                Descrizione = a.Descrizione,
                                IvaPerc = a.IvaPerc,
                                Qta = 1,
                                PrezzoIvato = a.PrezzoIvato - a.PrezzoIvato * (a.ScontoPerc) / 100,
                                TipoRigaScontrino = a.TipoRigaScontrino
                            });
                        }


                    }
                    return ControllerFatturazione.GeneraOrdScarico(listRighe, cliente, saveEntity);


                }

            }
        }
    }
}