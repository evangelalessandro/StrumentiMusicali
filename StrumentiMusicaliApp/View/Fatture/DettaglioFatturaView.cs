using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Fatture;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
    public partial class DettaglioFatturaView : UserControl, IMenu, ICloseSave
    {
        private ControllerFatturazione _controllerFatturazione;

        private ControllerRigheFatture _controllerRighe;
        private Subscription<RebindItemUpdated<Fattura>> _bindSub;
        private Subscription<FatturaCambiaTipoDoc> _cambioTipo;

        public DettaglioFatturaView(ControllerFatturazione controllerFatturazione)
            : base()
        {
            
            _controllerFatturazione = controllerFatturazione;
            _cambioTipo = EventAggregator.Instance().Subscribe<FatturaCambiaTipoDoc>(AbilitaCambioTipoFatt);

            EventAggregator.Instance().Subscribe<ItemSelected<FatturaRigaItem, FatturaRiga>>(
                a =>
                {
                    this.UpdateButtonState();
                }
                );

            InitializeComponent();
            this.cboClienteFornitoreID.Tag = "ClienteFornitoreID";

            UpdateViewByTipoDocumento();
            _bindSub = EventAggregator.Instance().Subscribe<RebindItemUpdated<Fattura>>((a) => { 
                RebindEditItem(); 
            });
        }

        private void AbilitaCambioTipoFatt(FatturaCambiaTipoDoc obj)
        {
            cboTipoDocumento.Enabled = true;
        }

        private void UpdateViewByTipoDocumento()
        {
            pnl3Basso.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);
            lblPagamento.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);
            cboPagamento.Visible = (_controllerFatturazione.EditItem.TipoDocumento != EnTipoDocumento.DDT);
            if (_controllerFatturazione.EditItem.TipoDocumento == EnTipoDocumento.OrdineAlFornitore)
                lblCliente.Text = "Fornitore";
            else
                lblCliente.Text = "Cliente";

            using (UnitOfWork uof = new UnitOfWork())
            {

                FillSoggetto(uof);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (_controllerRighe != null)
            {
                _controllerRighe.Dispose();
            }

            EventAggregator.Instance().UnSbscribe(_bindSub);

            EventAggregator.Instance().UnSbscribe(_cambioTipo);

            _controllerRighe = null;
            foreach (Control item in tabPage2.Controls)
            {
                item.Dispose();
                GC.SuppressFinalize(item);
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillCombo()
        {
            FillPagamenti();

            FillTipoDocumenti();

            using (var uof = new UnitOfWork())
            {
                txtAspettoBeni.Values = uof.FatturaRepository.Find(a => true).Select(a => a.AspettoEsterno).Distinct().ToList().ToArray();
                txtCausaleTrasporto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.CausaleTrasporto).Distinct().ToList().ToArray();
                txtPorto.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Porto).Distinct().ToList().ToArray();
                txtTrasportoACura.Values = uof.FatturaRepository.Find(a => true).Select(a => a.TrasportoACura).Distinct().ToList().ToArray();
                txtVettore.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Vettore).Distinct().ToList().ToArray();
                txtNote1.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note1).Distinct().ToList().ToArray();
                txtNote2.Values = uof.FatturaRepository.Find(a => true).Select(a => a.Note2).Distinct().ToList().ToArray();

                FillSoggetto(uof);
            }
        }
        private EnTipoDocumento UltimoTipodocSoggCaricato=EnTipoDocumento.NonSpecificato;
        private void FillSoggetto(UnitOfWork uof)
        {

            if (UltimoTipodocSoggCaricato== _controllerFatturazione.EditItem.TipoDocumento
                && cboClienteFornitoreID.Properties.DataSource!=null)
            {
                return;
            }
            UltimoTipodocSoggCaricato = _controllerFatturazione.EditItem.TipoDocumento;

            var clienti = uof.SoggettiRepository.Find(a=> a.TipiSoggetto!=null).Select(a => new
            {
                a.ID,

                RagioneSociale = a.RagioneSociale,
                a.PIVA,
                a.CodiceFiscale,
                a.Nome,
                a.Cognome,
                a.TipiSoggetto
            })
                .Distinct().ToList();
             
            if (_controllerFatturazione.EditItem.ID == 0)
                clienti = clienti.Where(a => (_controllerFatturazione.EditItem.TipoDocumento
                                        == EnTipoDocumento.OrdineAlFornitore
                                        && a.TipiSoggetto.Contains("Fornitore")
                                        )
                                        ||
                                    (_controllerFatturazione.EditItem.TipoDocumento
                                        != EnTipoDocumento.OrdineAlFornitore
                                        && a.TipiSoggetto.Contains("Cliente")
                                        )).ToList();
                                        

            var clientiDati = clienti.Select(a => new
            {
                a.ID,
                RagioneSociale =
                (!string.IsNullOrEmpty(a.RagioneSociale) && a.RagioneSociale.Length > 0
                        ? a.RagioneSociale + "  " :
                            a.Cognome + " " + a.Nome + "  ")
                 + (!string.IsNullOrEmpty(a.PIVA)
                 && a.PIVA.Length > 0
                        ? a.PIVA : a.CodiceFiscale)
            }).ToList();

            cboClienteFornitoreID.Properties.DataSource = clientiDati;

            cboClienteFornitoreID.Properties.ValueMember = "ID";
            cboClienteFornitoreID.Properties.DisplayMember = "RagioneSociale";
            cboClienteFornitoreID.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            cboClienteFornitoreID.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            cboClienteFornitoreID.Properties.BestFit();
        }

        private void FillTipoDocumenti()
        {
            var listItem = new List<EnTipoDocumento>();

            listItem.Add((EnTipoDocumento.NonSpecificato));
            listItem.Add(EnTipoDocumento.DDT);
            listItem.Add(EnTipoDocumento.FatturaDiCortesia);
            listItem.Add(EnTipoDocumento.RicevutaFiscale);

            listItem.Add(EnTipoDocumento.NotaDiCredito);
            listItem.Add(EnTipoDocumento.OrdineAlFornitore);

            cboTipoDocumento.DataSource = listItem.Select(a =>
                    new
                    {
                        ID = a,
                        Descrizione = UtilityView.GetTextSplitted(a.ToString())
                    }
                    ).ToList();
            cboTipoDocumento.DisplayMember = "Descrizione";
            cboTipoDocumento.ValueMember = "ID";
        }

        private void FillPagamenti()
        {
            var listPagamenti = new List<Tuple<string, string>>();
            listPagamenti.Add(new Tuple<string, string>("", "Seleziona pagamento"));
            listPagamenti.Add(new Tuple<string, string>("Bonifico Bancario", "Bonifico Bancario"));
            listPagamenti.Add(new Tuple<string, string>("Rimessa Diretta", "Rimessa Diretta"));
            listPagamenti.Add(new Tuple<string, string>("CONTRASSEGNO CONTANTI", "CONTRASSEGNO CONTANTI"));

            cboPagamento.DataSource = listPagamenti.Select(a =>
                    new
                    {
                        ID = a.Item1,
                        Descrizione = a.Item2
                    }
                    ).ToList();
            cboPagamento.DisplayMember = "Descrizione";
            cboPagamento.ValueMember = "ID";
        }

        private void TxtRagioneSociale_TextChanged(object sender, EventArgs e)
        {
            //using (var uof = new UnitOfWork())
            //{
            //	var finded = uof.ClientiRepository.Find(a => a.RagioneSociale == txtRagioneSociale.Text).Select(a => a.PIVA).FirstOrDefault();
            //	if (finded != null && finded.Length > 0)
            //	{
            //		txtPIVA.Text = finded;
            //	}
            //}
        }

        private async void frm_Load(object sender, EventArgs e)
        {
            await AddElemet();

            FillCombo();

            UpdateButtonState();

            UtilityView.SetDataBind(this, null, _controllerFatturazione.EditItem);

            cboClienteFornitoreID.EditValueChanged += CboClienteID_EditValueChanged;

            txtRagioneSociale.TextChanged += TxtRagioneSociale_TextChanged;

            ////calcolo del codice fattura
            if (_controllerFatturazione.EditItem.ID == 0)
            {
                cboTipoDocumento.SelectedValueChanged += (b, c) =>
                {
                    try
                    {
                        _controllerFatturazione.EditItem.TipoDocumento = (EnTipoDocumento)cboTipoDocumento.SelectedValue;
                    }
                    catch (Exception)
                    {
                    }

                    this.Validate();
                    this.UpdateViewByTipoDocumento();
                    //_controllerFatturazione.EditItem.TipoDocumento=
                    if (txtCodice.Text == "")
                    {
                        var codice = _controllerFatturazione.CalcolaCodice();
                        _controllerFatturazione.EditItem.Codice = codice;

                        txtCodice.Text = codice;
                    }
                };
            }
            else
            {
                cboTipoDocumento.Enabled = false;
            }
            cboClienteFornitoreID.EditValue = _controllerFatturazione.EditItem.ClienteFornitoreID;

            using (var ord = new OrdinaTab())
            {
                ord.OrderTab(pnl1Alto);
                ord.OrderTab(pnl2Testo);
                ord.OrderTab(pnl3Basso);
            }
        }

        private int prevCliente = -1;

        private void CboClienteID_EditValueChanged(object sender, EventArgs e)
        {
            var valCli = (int)cboClienteFornitoreID.EditValue;
            if (valCli == 0)
                return;
            if (prevCliente == valCli)
                return;
            try
            {
                cboClienteFornitoreID.EditValueChanged -= CboClienteID_EditValueChanged;

                prevCliente = valCli;

                using (var uof = new UnitOfWork())
                {
                    var cliente = uof.SoggettiRepository.Find(a => a.ID == valCli).FirstOrDefault();

                    var item = (_controllerFatturazione.EditItem);

                    item.RagioneSociale = cliente.RagioneSociale;

                    if (string.IsNullOrEmpty(cliente.RagioneSociale))
                    {
                        item.RagioneSociale = cliente.Cognome + " " + cliente.Nome;
                    }
                    item.PIVA = cliente.PIVA;
                    if (string.IsNullOrEmpty(cliente.PIVA))
                    {
                        item.PIVA = cliente.CodiceFiscale;
                    }
                    Debug.WriteLine(item.RagioneSociale);
                    this.Validate();
                    cboClienteFornitoreID.EditValue = valCli;
                }
            }
            catch (Exception ex)
            {
                _controllerFatturazione._logger.Error(ex, "Combo cliente fattura");
            }
            finally
            {
                cboClienteFornitoreID.EditValueChanged += CboClienteID_EditValueChanged;
            }
        }

        private async Task AddElemet()
        {
            await Task.Run(() =>
            {
                _controllerRighe = new ControllerRigheFatture(_controllerFatturazione);
                var controlFattRigheList = new FattureRigheListView(_controllerRighe);

                controlFattRigheList.Height = 200;
                controlFattRigheList.Dock = DockStyle.Fill;
                tabPage2.Controls.Add(controlFattRigheList);
            });
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                if (((Fattura)_controllerFatturazione.EditItem).ID == 0)
                {
                    tabControl1.SelectedTab = tabPage1;
                    return;
                }
                RefreshRighe();
            }
            UpdateButtonState();
        }

        private void RefreshRighe()
        {
            _controllerRighe.UpdateDataSource();
        }

        private void UpdateButtonState()
        {
            if (_menuTab != null)
            {
                ribPannelRighe.Enabled = tabControl1.SelectedTab == tabPage2 &&
                    _controllerFatturazione.SelectedItem.ID > 0;
                GetMenu().ApplyValidation(_controllerFatturazione.EditItem.ID > 0);
                var button = GetMenu().Tabs.First().Pannelli.SelectMany(a => a.Pulsanti).Where(a => a.Testo
                         == ControllerFatturazione.PulsanteCambioTipoDoc).First();
                button.Enabled = true;

                var enableRowSpecific = _controllerRighe != null && _controllerRighe.SelectedItem != null && _controllerRighe.SelectedItem.ID > 0;
                var list = GetMenu().ItemByTag(ControllerRigheFatture.KEYEXISTsRiga);
                foreach (var item in list)
                {
                    item.Enabled =
                    enableRowSpecific;
                }
            }
            this.UpdateViewByTipoDocumento();
        }

        private MenuTab _menuTab = null;
        private RibbonMenuPanel ribPannelRighe = null;

        public event EventHandler<EventArgs> OnSave;

        public event EventHandler<EventArgs> OnClose;

        public void RaiseSave()
        {
            this.Validate();
            EventAggregator.Instance().Publish<Save<Fattura>>(
                new Save<Fattura>(_controllerFatturazione));

            txtID.Text = _controllerFatturazione.EditItem.ID.ToString();

            UpdateButtonState();
        }

        public void RaiseClose()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }

        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                var tab = _menuTab.Add("Principale");
                var ribPannel = tab.Add("Principale");
                UtilityView.AddButtonSaveAndClose(ribPannel, this);
                ribPannelRighe = tab.Add("Righe");
                ribPannelRighe.Add("Aggiungi", StrumentiMusicali.Core.Properties.ImageIcons.Add).Click += (a, b) =>
                {
                    EventAggregator.Instance().Publish<Add<FatturaRiga>>(new Add<FatturaRiga>(_controllerRighe));
                };
                var rimuovi = ribPannelRighe.Add("Rimuovi", StrumentiMusicali.Core.Properties.ImageIcons.Remove, true);
                rimuovi.Click
                    += (a, b) =>
                    {
                        EventAggregator.Instance().Publish<Remove<FatturaRiga>>(new Remove<FatturaRiga>(_controllerRighe));
                    };
                rimuovi.Tag = ControllerRigheFatture.KEYEXISTsRiga;

                var menoPrio = ribPannelRighe.Add("Meno prioritario", StrumentiMusicali.Core.Properties.ImageIcons.Up, true);
                menoPrio.Click += (a, b) =>
                {
                    EventAggregator.Instance().Publish<AddPriority<FatturaRiga>>(
                        new AddPriority<FatturaRiga>());
                };
                menoPrio.Tag = ControllerRigheFatture.KEYEXISTsRiga;
                var addPrio = ribPannelRighe.Add("Più prioritario", StrumentiMusicali.Core.Properties.ImageIcons.Down, true);
                addPrio.Click
                += (a, b) =>
                {
                    EventAggregator.Instance().Publish<RemovePriority<FatturaRiga>>(
                        new RemovePriority<FatturaRiga>());
                };
                addPrio.Tag = ControllerRigheFatture.KEYEXISTsRiga;

                _controllerFatturazione.AggiungiComandiStampa(tab, true);
                //var pnlStampa = tab.Add("Stampa");
                //var ribStampa = pnlStampa.Add("Avvia stampa", StrumentiMusicali.Core.Properties.ImageIcons.Print_48,true);
                //ribStampa.Click += (a, e) =>
                //{
                //	_controllerFatturazione.StampaFattura(_controllerFatturazione.EditItem);
                //};

                var pnl2 = tab.Add("Totali");
                var rib01 = pnl2.Add("Aggiorna totali", StrumentiMusicali.Core.Properties.ImageIcons.Totali_Aggiorna_48, true);
                rib01.Click += (a, e) =>
                {
                    RebindEditItem();
                };
            }
            return _menuTab;
        }

        private void RebindEditItem()
        {
            Validate();
            _controllerFatturazione.EditItem = ControllerFatturazione.CalcolaTotali(_controllerFatturazione.EditItem);

            UtilityView.SetDataBind(this, null, _controllerFatturazione.EditItem);
        }
    }
}