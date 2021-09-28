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
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Scontrino;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;

namespace StrumentiMusicali.App.Core.Controllers.Scontrino
{
    public class ScontrinoUtility : System.IDisposable
    {
        private DevExpress.XtraGrid.Views.Grid.GridView _dgvScontrino;
        private DevExpress.XtraGrid.GridControl _gcScontrino;

        private BackgroundWorker _workerCheckScontrini = new BackgroundWorker();
        private Timer _timer = new Timer();
        public ScontrinoUtility()
        {
            EventAggregator.Instance().Subscribe<ScontrinoAddEvents>(AggiungiArticolo);

            EventAggregator.Instance().Subscribe<ScontrinoRemoveLineEvents>(ScontrinoRemoveLine);
            EventAggregator.Instance().Subscribe<ScontrinoClear>(RipulisciScontrino);
            EventAggregator.Instance().Subscribe<ScontrinoStampa>(StampaScontrino);

            _workerCheckScontrini.DoWork += _workerCheckScontrini_DoWork;
            _workerCheckScontrini.RunWorkerCompleted += _workerCheckScontrini_RunWorkerCompleted;


            _timer.Interval = 7000;
            _timer.Enabled = true;
            _timer.Tick += Timer_Tick;
        }

        private void _workerCheckScontrini_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            _timer.Enabled = EsistonoScontriniDaElaborare;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            _workerCheckScontrini.RunWorkerAsync();
        }

        private void _workerCheckScontrini_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckScontrini();

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


            var rpReparto = new RepositoryItemLookUpEdit();
            _gcScontrino.RepositoryItems.Add(rpReparto);

            _dgvScontrino.Columns["Reparto"].OptionsColumn.AllowEdit = true;
            _dgvScontrino.Columns["Reparto"].ColumnEdit = rpReparto;

            using (var uof=new UnitOfWork())
            {
                rpReparto.DisplayMember = "NomeReparto";
                rpReparto.ValueMember = "CodicePerRegistratoreDiCassa";
                rpReparto.DataSource = uof.RegistratoreDiCassaRepository.Find(a => 1 == 1).Select(a =>new { a.CodicePerRegistratoreDiCassa, a.NomeReparto }).ToList();
                
            }
           

            _dgvScontrino.Columns["TipoRigaScontrino"].Visible = false;

            _dgvScontrino.Columns["Articolo"].Visible = false;
            _dgvScontrino.CustomDrawCell += _dgvScontrino_CustomDrawCell;
        }

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
                    if (focusCol == "ScontoPerc" || focusCol == "IvaPerc" || focusCol == "Reparto")
                        e.Cancel = true;
                }
            }
            if ((line.TipoRigaScontrino == TipoRigaScontrino.Incassato))
            {
                if (focusCol != "PrezzoIvato" && focusCol != "Reparto")
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
                    Qta = 1

                });
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
                    Qta=1,
                    TipoRigaScontrino = TipoRigaScontrino.Vendita,
                    IvaPerc = 22,

                });
            }

            var reparto = CalcolaRepartoCassa(newitem.Articolo, newitem.IvaPerc);
            if (reparto!=null)
            {
                newitem.Reparto = reparto.CodicePerRegistratoreDiCassa;
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

            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita && a.Articolo > 0))
            {
                using (var uof = new UnitOfWork())
                {
                    var articolo = item.Articolo;

                    var giacenza = uof.MagazzinoRepository.Find(a => articolo == (a.ArticoloID) && a.Deposito.Principale == true)
                              .Select(a => new { a.ArticoloID, a.Qta }).GroupBy(a => new { a.ArticoloID })
                              .Select(a => new { Sum = a.Sum(b => b.Qta) }).ToList();



                    var giacNegozio = giacenza.DefaultIfEmpty(new { Sum = 0 }).FirstOrDefault().Sum;

                    giacNegozio-= Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita &&
                    a.Articolo == articolo).Sum(a=>a.Qta);

                    if (giacNegozio<0)
                    {
                        item.Qta = item.Qta-1;

                        MessageManager.NotificaWarnig("Quantità in negozio non sufficiente!");
                        return;
                    }

                }
            }
            //if (obj.Articolo != null)
            //{
            //    using (var uof = new UnitOfWork())
            //    {
            //        var giacenza = uof.MagazzinoRepository.Find(a => obj.Articolo.ID == (a.ArticoloID) && a.Deposito.Principale == true)
            //                  .Select(a => new { a.ArticoloID, a.Qta }).GroupBy(a => new { a.ArticoloID })
            //                  .Select(a => new { Sum = a.Sum(b => b.Qta) }).ToList();



            //        var giacNegozio = giacenza.DefaultIfEmpty(new { Sum = 0 }).FirstOrDefault().Sum - Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita &&
            //        a.Articolo == obj.Articolo.ID).Count();

            //        if (giacNegozio == 0)
            //        {
            //            MessageManager.NotificaWarnig("Quantità in negozio non sufficiente!");
            //            return;
            //        }

            //    }
            //}

            var datoTotale = Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).FirstOrDefault();
            decimal tot = 0;
            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita))
            {
                if (item.Qta > 0)
                    if (item.ScontoPerc != 0)
                    {
                        tot = tot + ((item.PrezzoIvato * item.Qta) / ((decimal)100.0) * (((decimal)100) - Math.Abs(item.ScontoPerc)));
                    }
                    else
                    {
                        tot = tot + (item.PrezzoIvato * item.Qta);
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

        private void ScriviFile()
        {

            if (!SettingScontrinoValidator.Check())
                return;

            var listRighe = new List<ScontrinoLine>();
            for (int i = 0; i < Datasource.Count(); i++)
            {
                var a = Datasource[i];
                if (a.TipoRigaScontrino != TipoRigaScontrino.ScontoIncondizionato
                    && a.TipoRigaScontrino != TipoRigaScontrino.Incassato)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = a.Descrizione, IvaPerc = a.IvaPerc, Qta = a.Qta, PrezzoIvato = a.PrezzoIvato, TipoRigaScontrino = a.TipoRigaScontrino });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.Vendita && a.ScontoPerc > 0)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = "Sconto " + a.ScontoPerc.ToString() + "%", IvaPerc = 0, Qta = 1, PrezzoIvato = (a.Qta * a.PrezzoIvato) * (a.ScontoPerc) / 100, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
                if (a.TipoRigaScontrino == TipoRigaScontrino.ScontoIncondizionato && a.PrezzoIvato > 0)
                {
                    listRighe.Add(new ScontrinoLine { Descrizione = a.Descrizione, IvaPerc = 0, Qta = 1, PrezzoIvato = a.PrezzoIvato, TipoRigaScontrino = TipoRigaScontrino.Sconto });
                }
            }



            using (SaveEntityManager save = new SaveEntityManager())
            {
                var uof = save.UnitOfWork;

                var list = uof.TipiPagamentoScontrinoRepository.Find(a => a.Enable == true).ToList().Select(a => a.Codice.ToString() + " " + a.Descrizione).ToList();
                using (var tipiPagamento = new ListViewCustom(list, "Tipo pagamento", true))
                {
                    var diag = tipiPagamento.ShowDialog();
                    if (diag != DialogResult.OK)
                        return;
                    var pagamento = tipiPagamento.SelectedItem;
                    var codiceLotteria = tipiPagamento.SelectedTextProp;

                    if (string.IsNullOrEmpty(pagamento))
                    {
                        MessageManager.NotificaWarnig("Occorre selezionare il tipo di pagamento");

                        return;
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
                    SalvaScontrino(save, uof, content);

                    MessageManager.NotificaInfo("Scontrino pubblicato al servizio correttamente!");


                    RipulisciScontrino(new ScontrinoClear());

                    _workerCheckScontrini.RunWorkerAsync();

                }
            }
        }
        public bool EsistonoScontriniDaElaborare { get; set; } = false;
        /// <summary>
        /// Controlla se ci sono degli scontrini che hanno cambiato stato
        /// 
        /// Elaborati
        /// Annullati
        /// InErrore
        /// </summary>
        private void CheckScontrini()
        {
            EsistonoScontriniDaElaborare = false;
            var validator = SettingScontrinoValidator.ReadSetting();

            using (SaveEntityManager save = new SaveEntityManager())
            {
                var uof = save.UnitOfWork;
                var nomePostazione = Environment.MachineName;
                var scontrini = uof.ScontrinoTestataRepository.Find(a => a.NomePostazione == nomePostazione).ToList()
                    .Where(a => a.StatoElaborazione == enStatoElaborato.DaElaborare).ToList();
                using (var depo = new ControllerMagazzino())
                {
                    foreach (var scontrino in scontrini)
                    {
                        if (!System.IO.File.Exists(scontrino.NomeFile))
                        {
                            //var file = System.IO.Path.Combine(
                            //           System.IO.Path.Combine(validator.FolderDestinazione, @"Elaborati")
                            //           ,);

                            var count=System.IO.Directory.GetFiles(System.IO.Path.Combine(validator.FolderDestinazione, @"Elaborati"), "*_" + System.IO.Path.GetFileName(scontrino.NomeFile));

                            /*se è negli elaborati decurto giacenza*/
                            if (count.Length>0)
                            {
                                ScaricaScontrino(uof, scontrino);
                            }
                            else
                            //if (System.IO.File.Exists(

                            //    System.IO.Path.Combine(
                            //           System.IO.Path.Combine(validator.FolderDestinazione, @"\Annullati\")
                            //           , System.IO.Path.GetFileName(scontrino.NomeFile)))
                            //    ||
                            //          (System.IO.File.Exists(

                            //    System.IO.Path.Combine(
                            //           System.IO.Path.Combine(validator.FolderDestinazione, @"\InErrore\")
                            //           , System.IO.Path.GetFileName(scontrino.NomeFile))))
                            //)
                            {
                                var scontrUPDATE = uof.ScontrinoTestataRepository.Find(a => a.ID == scontrino.ID).FirstOrDefault();

                                scontrUPDATE.StatoElaborazione = enStatoElaborato.InErrore;
                                scontrUPDATE.DataErrore = DateTime.Now;
                                uof.Commit();

                            }

                        }
                        else
                        {
                            EsistonoScontriniDaElaborare = true;
                        }
                    }
                }


            }

        }

        private static void ScaricaScontrino(UnitOfWork uof, ScontrinoTestata scontrino)
        {
            foreach (var rigaSContrino in uof.ScontrinoRigheRepository.Find(a =>
            a.ScontrinoTestataID == scontrino.ID
            && a.ArticoloID > 0))
            {

                ScaricaQtaMagazzino scarica = new ScaricaQtaMagazzino();

                scarica.Qta = rigaSContrino.Quantita;
                
                scarica.Deposito = scontrino.Deposito;
                scarica.ArticoloID = rigaSContrino.ArticoloID;
                EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(scarica);

            }
            var scontrUPDATE = uof.ScontrinoTestataRepository.Find(a => a.ID == scontrino.ID).FirstOrDefault();

            scontrUPDATE.StatoElaborazione = enStatoElaborato.Elaborato;
            scontrUPDATE.DataConfermaSuccesso = DateTime.Now;
            uof.Commit();

        }
        private RegistratoreDiCassaReparti  CalcolaRepartoCassa(int articoloId,decimal iva)
        {
            using (var uof = new UnitOfWork())
            {
                var gruppoCassa = uof.ArticoliRepository.Find(a => a.ID == articoloId)
                    .Select(a =>new {articoloIva= a.Iva, a.NonImponibile, a.Categoria.GruppoCodiceRegCassa.GruppoRaggruppamento})
                    .FirstOrDefault();
                if (gruppoCassa!=null)
                {
                    var list = uof.RegistratoreDiCassaRepository.Find(a => a.GruppoCodiceRegCassa.GruppoRaggruppamento 
                    == gruppoCassa.GruppoRaggruppamento).ToList();
                    if (list.Count == 1)
                        return list.First();

                    var listSpecifico = list.Where(a => gruppoCassa.articoloIva != 22 && a.Iva != 22).FirstOrDefault();
                    if (listSpecifico != null)
                    {
                        return listSpecifico;
                    }

                    if (gruppoCassa.NonImponibile==false)
                    {
                         listSpecifico = list.Where(a => a.Iva == gruppoCassa.articoloIva).FirstOrDefault();
                        if (listSpecifico!=null)
                        {
                            return listSpecifico;
                        }
                       

                    }
                   
                }


                var listItem = uof.RegistratoreDiCassaRepository.Find(a => a.Iva == iva).FirstOrDefault();
                if (listItem!=null)
                    return listItem;
                listItem = uof.RegistratoreDiCassaRepository.Find(a => a.Iva != iva).FirstOrDefault();
                if (listItem != null)
                    return listItem;
                return null;

            }
        }

        private void SalvaScontrino(SaveEntityManager save, UnitOfWork uof, string[] content)
        {
            var validator = SettingScontrinoValidator.ReadSetting();

            var negozio = uof.DepositoRepository.Find(a => a.Principale == true).First();


            var nomeFile = System.IO.Path.Combine(validator.FolderDestinazione, DateTime.Now.Ticks.ToString() + ".txt");

            System.IO.File.WriteAllLines(
                  nomeFile, content);


            var scontrino = new ScontrinoTestata();
            scontrino.NomeFile = nomeFile;
            scontrino.Deposito = negozio.ID;
            scontrino.NomePostazione = Environment.MachineName;
            scontrino.StatoElaborazione = Library.Entity.Scontrino.enStatoElaborato.DaElaborare;
            scontrino.Totale = Datasource.FindAll(a => a.TipoRigaScontrino == TipoRigaScontrino.Totale).First().PrezzoIvato;
            uof.ScontrinoTestataRepository.Add(scontrino);
            uof.Commit();

            foreach (var item in Datasource.Where(a => a.TipoRigaScontrino == TipoRigaScontrino.Vendita))
            {

                ScontrinoRighe riga = new ScontrinoRighe();
                riga.ArticoloID = item.Articolo;
                riga.ArticoloDescrizione = item.Descrizione;
                riga.PrezzoIvato = item.PrezzoIvato;
                riga.IvaPerc = item.IvaPerc;
                riga.Quantita = item.Qta;
                riga.ScontrinoTestataID = scontrino.ID;
                riga.ArticoloDescrizione = item.Descrizione;
                uof.ScontrinoRigheRepository.Add(riga);


            }


            save.SaveEntity("Scontrino salvato");
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
            _dgvScontrino.ValidateEditor();
            _dgvScontrino.FocusedRowHandle = 0;

            _dgvScontrino.FocusedRowHandle = 1;

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