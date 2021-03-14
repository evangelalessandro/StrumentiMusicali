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

                Vino Lambrusco ; 22 ;1;0,75;2,50;V;

 

                Es riga Totale :

 

                Totale; ; ; ; 2,50;T;1 Riga con pagamento in contanti

                Totale; ; ; ; 2,50;T;45 Riga con pagamento bancomat

                Totale; ; ; ; 2,50;T;5 Riga con pagamento non riscosso
            */
            ScriviFile();
        }

        private void ScriviFile()
        {
            var righe = Datasource.Select(a => new ScontrinoLine { Descrizione = a.Descrizione, Aliquota = a.IvaPerc, Qta = 1, PrezzoSingoloIvato = a.PrezzoIvato, TipoTotale = false })
                .ToList();


             

            using (var uof = new UnitOfWork())
            {
                var list = uof.TipiPagamentoScontrinoRepository.Find(a => 1 == 1).ToList().Select(a => a.Codice.ToString() + " " + a.Descrizione).ToList();
                using (var frmMarche = new ListViewCustom(list, "Tipo pagamento"))
                {
                    frmMarche.ShowDialog();
                    var pagamento = frmMarche.SelectedItem;

                    if (string.IsNullOrEmpty(pagamento))
                    {
                        MessageManager.NotificaWarnig("Occorre selezionare il tipo di pagamento");

                        return;
                    }
                    righe.Add(new ScontrinoLine { TotaleComplessivo = righe.Sum(a => a.TotaleRiga), TipoTotale = true , Pagamento=pagamento});

                    var content = righe.Select(a => a.ToString()).ToList().ToArray();

                    var validator = SettingScontrinoValidator.ReadSetting();

                    System.IO.File.WriteAllLines(
                        System.IO.Path.Combine(validator.FolderDestinazione, DateTime.Now.Ticks.ToString() + ".txt"), content);

                    MessageManager.NotificaInfo("Scontrino pubblicato al servizio correttamente");

                    if (MessageManager.QuestionMessage("Ripulisco la lista che è nello scontrino?"))
                    {
                        RipulisciScontrino(new ScontrinoClear());
                    }
                }
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