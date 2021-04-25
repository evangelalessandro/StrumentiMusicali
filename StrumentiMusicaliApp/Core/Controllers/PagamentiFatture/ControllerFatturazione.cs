using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Controllers.Stampa;
using StrumentiMusicali.App.Core.Fatture;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Fatture;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerFatturazione : BaseControllerGeneric<Fattura, FatturaItem>, IDisposable
    {
        public ControllerFatturazione() :
            base(enAmbiente.FattureList, enAmbiente.Fattura)
        {
            _sub2 = EventAggregator.Instance().Subscribe<Add<Fattura>>(AddFattura);
            _sub3 = EventAggregator.Instance().Subscribe<Edit<Fattura>>(FatturaEdit);
            _sub4 = EventAggregator.Instance().Subscribe<Save<Fattura>>(Save);
            _sub5 = EventAggregator.Instance().Subscribe<Remove<Fattura>>(DelFattura);

            ///comando di stampa
            AggiungiComandi();
        }

        private Subscription<Remove<Fattura>> _sub5;
        private Subscription<Save<Fattura>> _sub4;
        private Subscription<Edit<Fattura>> _sub3;
        private Subscription<Add<Fattura>> _sub2;

        public override void Dispose()
        {
            EventAggregator.Instance().UnSbscribe(_sub2);
            EventAggregator.Instance().UnSbscribe(_sub3);
            EventAggregator.Instance().UnSbscribe(_sub4);
            EventAggregator.Instance().UnSbscribe(_sub5);

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Save(Save<Fattura> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                EditItem.Data = EditItem.Data.Date;



                if (EditItem.ID > 0)
                {
                    if (saveManager.UnitOfWork.FatturaRepository.Find(a => a.ID == EditItem.ID).Select(a=>a.ChiusaSpedita).First())
                    {
                        MessageManager.NotificaWarnig("Documento già chiuso, non è possibile modificare altro!");
                        return;
                    }

                    EditItem = CalcolaTotali(EditItem);
                    uof.FatturaRepository.Update(EditItem);
                }
                else
                {
                    uof.FatturaRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    EventAggregator.Instance().Publish<UpdateList<Fattura>>(
                            new UpdateList<Fattura>(this));
                }
            }
        }

        ~ControllerFatturazione()
        {
        }
        public string CalcolaCodice()
        {
            return CalcolaCodice(EditItem);
        }
        public static string CalcolaCodice(Fattura fattura)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {

                //switch (fattura.TipoDocumento)
                //{
                //    case EnTipoDocumento.NonSpecificato:
                //        return "";

                //    case EnTipoDocumento.FatturaDiCortesia:
                //    case EnTipoDocumento.RicevutaFiscale:
                //        prefix = "F";
                //        break;

                //    case EnTipoDocumento.NotaDiCredito:
                //        prefix = "NC";
                //        break;

                //    case EnTipoDocumento.DDT:
                //        prefix = "D";
                //        break;
                //    case EnTipoDocumento.OrdineAlFornitore:
                //        prefix = "ODQ";
                //        break;
                //    case EnTipoDocumento.OrdineDiCarico:
                //        prefix = "ODC";
                //        break;
                //    default:
                //        break;
                //}
                var enumVal = (int)fattura.TipoDocumento;
                var prefix = uof.TipiDocumentoFiscaleRepository.Find(a => a.IDEnum == enumVal).Select(a => a.Codice).FirstOrDefault();

                var list = new List<EnTipoDocumento>();
                list.Add(EnTipoDocumento.FatturaDiCortesia);
                list.Add(EnTipoDocumento.RicevutaFiscale);

                var codiceList = uof.FatturaRepository.Find(a => a.Data.Year == fattura.Data.Year).ToList();

                codiceList = codiceList.Where(a => a.TipoDocumento == fattura.TipoDocumento ||
                  ((list.Contains(fattura.TipoDocumento) &&
                  (list.Contains(a.TipoDocumento)
                  ))))
                    .ToList();

                var codice = codiceList.OrderByDescending(a => a.Codice).Select(a => a.Codice).DefaultIfEmpty("").FirstOrDefault();
                var valore = 1;
                if (codice != "")
                {
                    codice = codice.Replace(prefix, "");
                    if (codice.Contains("/"))
                    {
                        codice = codice.Split('/')[0];

                        if (!int.TryParse(codice, out valore))
                        {
                            return "";
                        }
                        valore++;
                    }
                }

                return prefix + valore.ToString("000") + "/" + fattura.Data.ToString("yy");
            }
        }

        private void DelFattura(Remove<Fattura> obj)
        {
            try
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare il documento selezionato?"))
                    return;
                using (var saveEntity = new SaveEntityManager())
                {
                    var uof = saveEntity.UnitOfWork;
                    int val = ((Fattura)SelectedItem).ID;
                    var item = uof.FatturaRepository.Find(a => a.ID == val).FirstOrDefault();
                    _logger.Info(string.Format("Cancellazione documento/r/n codice {0} /r/n ID {1}",
                        item.Codice, item.ID));

                    foreach (var itemRiga in uof.FattureRigheRepository.Find(a => a.FatturaID == val))
                    {
                        uof.FattureRigheRepository.Delete(itemRiga);
                    }

                    uof.FatturaRepository.Delete(item);

                    if (saveEntity.SaveEntity(enSaveOperation.OpDelete))
                    {
                        EventAggregator.Instance().Publish<UpdateList<Fattura>>(
                            new UpdateList<Fattura>(this));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        private void FatturaEdit(Edit<Fattura> edit)
        {
            EditItem = (Fattura)SelectedItem;
            ShowDettaglio();
        }

        private void ShowDettaglio()
        {
            using (var view = new DettaglioFatturaView(this))
            {
                ShowView(view, enAmbiente.Fattura);
            }
        }

        private void AddFattura(Add<Fattura> obj)
        {
            EditItem = new Fattura() { TipoDocumento = EnTipoDocumento.FatturaDiCortesia };
            EditItem.Codice = CalcolaCodice();
            EditItem.Data = DateTime.Now.Date;
            ShowDettaglio();
        }

        public static void ImportaFatture()
        {
            try
            {
                using (OpenFileDialog res = new OpenFileDialog())
                {
                    res.Title = "Seleziona file da importare";
                    //Filter
                    res.Filter = "File access|*.mdb|Tutti i file|*.*";

                    res.Multiselect = false;
                    //When the user select the file
                    if (res.ShowDialog() == DialogResult.OK)
                    {
                        using (var dat = new CursorManager())
                        {
                            using (var importa = new ImportFatture())
                            {
                                importa.ImportAccessDB(res.FileName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        internal void StampaFattura(Fattura fattura)
        {
            _logger.Info("Avvio stampa");
            try
            {
                using (var cursorManger = new CursorManager())
                {
                    using (var stampaFattura = new StampaFattura())
                    {
                        var setting = DatiIntestazioneStampaFatturaValidator.ReadSetting();

                        if (DatiIntestazioneStampaFatturaValidator.Validate())
                        {
                            stampaFattura.Stampa(
                                        setting,
                                        fattura);
                            _logger.Info("Stampa completata correttamente.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        public static Fattura CalcolaTotali(Fattura fattura)
        {
            using (var uof = new UnitOfWork())
            {
                var fattItem = uof.FatturaRepository.Find(a => a.ID == fattura.ID).
                            Select(a => new { a.RigheFattura, a }).First();

                List<FatturaRiga> righeFatt = fattItem.RigheFattura.ToList();
                decimal importoIvaTot = 0;
                decimal imponibileIVaTot = 0;

                foreach (var item in righeFatt.Where(a => a.Importo > 0 && a.IvaApplicata.Length > 0).GroupBy(a => a.IvaApplicata).OrderBy(a => a.Key))
                {
                    var imponibileIva = item.Select(a => a.Importo).DefaultIfEmpty(0).Sum();
                    decimal importoIva = 0;
                    decimal val;
                    if (decimal.TryParse(item.Key, out val))
                    {
                        importoIva = imponibileIva * ((decimal)val) / ((decimal)100);
                    }
                    importoIvaTot += importoIva;
                    imponibileIVaTot += imponibileIva;
                }
                fattura.TotaleIva = (importoIvaTot);
                fattura.ImponibileIva = (imponibileIVaTot);
                fattura.TotaleFattura = (imponibileIVaTot + importoIvaTot);
                return fattura;
            }
        }

        private void ImpostaQuadroIVa()
        {
        }

        private void GeneraFatturaXml(Fattura selectedItem)
        {
            _logger.Info("Avvio stampa");
            try
            {
                using (var cursorManger = new CursorManager())
                {
                    using (var stampa = new FattElettronica())
                    {
                        using (var uof = new UnitOfWork())
                        {
                            uof.DatiMittenteRepository.Find(a => 1 == 1).FirstOrDefault();

                            var fatt = uof.FatturaRepository.Find(a => a.ID == selectedItem.ID)
                                .Select(a => new { Fattura = a, a.ClienteFornitore, a.RigheFattura }).First();
                            stampa.DatiDestinatario = fatt.ClienteFornitore;

                            FatturaHeader header = new FatturaHeader();
                            header.Righe =
                                fatt.RigheFattura.ToList().Select(a =>

                                new FatturaElett.FatturaRighe()
                                {
                                    Descrizione = a.Descrizione,
                                    QTA = a.Qta,
                                    PrezzoUnitario = a.PrezzoUnitario,
                                    PrezzoTotale = a.Importo,
                                    AliquotaIVA = Iva(a.IvaApplicata)
                                }
                            ).ToList();
                            header.Numero = fatt.Fattura.Codice;
                            if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Bonifico".ToUpperInvariant()))
                                header.ModalitaPagamento = enTipoPagamento.Bonifico;
                            else if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Contrassegno".ToUpperInvariant()))
                                header.ModalitaPagamento = enTipoPagamento.Contrassegno;
                            else if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Rimessa".ToUpperInvariant()))
                                header.ModalitaPagamento = enTipoPagamento.Contanti;
                            else
                            {
                                throw new MessageException("Tipo pagamento non gestito");
                            }
                            header.Data = fatt.Fattura.Data;

                            header.ImportoTotaleDocumento = CalcolaTotali(fatt.Fattura).TotaleFattura;

                            stampa.FattureList.Add(header);
                            stampa.ScriviFattura(fatt.Fattura.ID);
                        }
                        //stampa.DatiDestinatario.
                        ////stampa.ScriviFattura()

                        _logger.Info("Stampa completata correttamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        private int Iva(string val)
        {
            var valiva = 0;
            if (int.TryParse(val.Trim(), out valiva))
                return valiva;
            return 0;
        }

        private void AggiungiComandi()
        {
            AggiungiComandi(GetMenu().Tabs[0], false);
        }

        public static readonly string PulsanteCambioTipoDoc = "Cambio tipo documento";

        public void AggiungiComandi(MenuRibbon.RibbonMenuTab tab, bool editItem)
        {
            if (editItem)
            {
                var pnlTipoDoc = tab.Add("Tipo documento");

                var fattCortesia = pnlTipoDoc.Add(PulsanteCambioTipoDoc, StrumentiMusicali.Core.Properties.ImageIcons.Edit, false);
                fattCortesia.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<FatturaCambiaTipoDoc>(new FatturaCambiaTipoDoc());
                };
            }
            var pnlStampa = tab.Add("Stampa");

            var ribStampa = pnlStampa.Add("Avvia stampa", StrumentiMusicali.Core.Properties.ImageIcons.Print_48, true);
            ribStampa.Click += (a, e) =>
            {
                if (editItem)
                    StampaFattura(EditItem);
                else
                    StampaFattura(SelectedItem);
            };

            var ribStampaXml = pnlStampa.Add("Genera fattura xml", StrumentiMusicali.Core.Properties.ImageIcons.Fattura_xml_48, true);
            ribStampaXml.Click += (a, e) =>
            {
                if (editItem)
                    GeneraFatturaXml(EditItem);
                else
                    GeneraFatturaXml(SelectedItem);
            };
            if (!editItem)
            {
                var ribGeneraOrdineCarico = pnlStampa.Add("Genera Ordine Carico", StrumentiMusicali.Core.Properties.ImageIcons.OrdineDiCarico, true);
                ribGeneraOrdineCarico.Click += (a, e) =>
                {

                    GeneraOrdineCarico();
                };
            }
            if (!editItem)
            {
                var ribGeneraOrdineCarico = pnlStampa.Add("Genera Giacenze da Ordine di carico", StrumentiMusicali.Core.Properties.ImageIcons.OrdineDiCarico, true);
                ribGeneraOrdineCarico.Click += (a, e) =>
                {

                    GeneraMovimentiDaOrdineDiCarico();
                };
            }
            var pnlCliente = tab.Add("Anagrafica cliente");
            var ribCust = pnlCliente.Add("Visualizza cliente", StrumentiMusicali.Core.Properties.ImageIcons.Customer_48, true);
            ribCust.Click += (x, e) =>
            {
                using (var controllerCl = new ControllerClienti())
                {
                    using (var uof = new UnitOfWork())
                    {
                        var idFatt = 0;
                        if (editItem)
                            idFatt = (EditItem).ID;
                        else
                            idFatt = (SelectedItem).ID;

                        var cliente = uof.FatturaRepository.Find(a => a.ID == idFatt).Select(a => a.ClienteFornitore).First();
                        ///impostato per la save.
                        controllerCl.EditItem = cliente;

                        //var frm = ViewFactory.GetView(enAmbienti.Cliente);
                        //if (frm == null)
                        {
                            var view = new GenericSettingView(cliente);
                            view.OnSave += (d, b) =>
                            {
                                view.Validate();
                                EventAggregator.Instance().Publish<Save<Soggetto>>
                                (new Save<Soggetto>(controllerCl));
                            };
                            ShowView(view, enAmbiente.Cliente, null, false);
                            ViewFactory.AddView(enAmbiente.Cliente, view);
                        }

                    }
                }
            };
        }

        private void GeneraMovimentiDaOrdineDiCarico()
        {
            var fatt = SelectedItem;



            //controllo se ci sono già altri ordini di carico
            //per le righe di questo ordine

            if (fatt.ChiusaSpedita)
            {
                MessageManager.NotificaWarnig("Documento già chiuso, non è possibile generare altre giacenze!");
                return;
            }
            if (fatt.TipoDocumento != EnTipoDocumento.OrdineDiCarico)
            {
                MessageManager.NotificaWarnig("Documento deve essere del tipo ordine di carico!");
                return;
            }
            if (!MessageManager.QuestionMessage("Generando i movimenti, il documento non sarà più modificabile. Vuoi proseguire?"))
            {
                return;
            }
            using (var saveEnt = new SaveEntityManager())
            {
                var depositi = saveEnt.UnitOfWork.DepositoRepository.Find(a => a.ID == a.ID).Select(a => new { a.ID, a.NomeDeposito }).ToList();

                using (var selezionaDeposito = new ListViewCustom(depositi.Select(a => a.NomeDeposito).ToList(), "Imposta deposito e nome ddt fornitore", "Numero Ddt Fornitore", false))
                {
                    var diag = selezionaDeposito.ShowDialog();
                    if (diag != DialogResult.OK)
                        return;
                    if (selezionaDeposito.SelectedItem == null)
                    {
                        MessageManager.NotificaWarnig("Occorre impostare deposito su cui versare la merce!");
                        return;
                    }
                    if (selezionaDeposito.SelectedTextProp.Length == 0)
                    {
                        if (!MessageManager.QuestionMessage("Sei sicuro di non volere impostare il codice ddt o un riferimento al ddt fornitore. Vuoi proseguire?"))
                        {
                            return;
                        }

                    }
                    var deposito = depositi.Where(a => a.NomeDeposito == selezionaDeposito.SelectedItem).FirstOrDefault();

                    var list = saveEnt.UnitOfWork.FattureRigheRepository.Find(a => a.FatturaID == fatt.ID && a.Ricevuti > 0).ToList();

                    if (list.Count() == 0)
                    {
                        MessageManager.NotificaWarnig("Non c'è niente tra le qta ricevute nelle righe del documento!");
                        return;
                    }
                    ControllerMagazzino controllerMagazzino = new ControllerMagazzino();

                    foreach (var item in list)
                    {
                        if (!controllerMagazzino.NuovoMovimento(new Library.Core.Events.Magazzino.MovimentoMagazzino()
                        {
                            ArticoloID = item.ArticoloID.Value,
                            Deposito = deposito.ID,
                            Causale = selezionaDeposito.SelectedTextProp,
                            Qta = item.Ricevuti,
                            Prezzo = item.PrezzoUnitario
                        }, saveEnt.UnitOfWork))
                        {
                            MessageManager.NotificaWarnig("Sei sicuro un errore nell'inserimento del movimento");
                            return;
                        }
                        /*salvo i dati di evasione nella riga parent dell'ordine di acquisto*/
                        var evasi = saveEnt.UnitOfWork.FattureRigheRepository.Find(a => a.IdRigaCollegata != null
                            && a.IdRigaCollegata == item.IdRigaCollegata)
                        .Select(a => new { a.IdRigaCollegata, a.Qta })
                        .GroupBy(a => a.IdRigaCollegata).Select(a => new { TOT = a.Sum(b => b.Qta), IdRigaParent = a.Key.Value }).FirstOrDefault();

                        var rigaParent = saveEnt.UnitOfWork.FattureRigheRepository.Find(a => a.ID == evasi.IdRigaParent).FirstOrDefault();

                        if (rigaParent.Evasi != evasi.TOT)
                        {
                            rigaParent.Evasi = evasi.TOT;
                            saveEnt.UnitOfWork.FattureRigheRepository.Update(rigaParent);
                        }
                    }
                    var dato = saveEnt.UnitOfWork.FatturaRepository.Find(a => a.ID == fatt.ID).FirstOrDefault();
                    dato.ChiusaSpedita = true;

                    saveEnt.UnitOfWork.FatturaRepository.Update(dato);



                    saveEnt.SaveEntity("Creati movimenti ingresso a magazzino e chiuso documento");

                }
            }
        }

        public static bool GeneraOrdAcq(List<ListinoPrezziFornitoriItem> items)
        {
            using (var saveEnt = new SaveEntityManager())
            {
                bool save = false;

                var listFatt = new List<Library.Entity.Fattura>();

                foreach (var item in items.Where(a => a.QtaDaOrdinare - a.QtaInOrdine > 0)
                    .Select(a => new { Qta = a.QtaDaOrdinare - a.QtaInOrdine, a.CodiceArticoloFornitore, a.ID, a.Entity, a.Prezzo }).OrderBy(a => a.Entity.FornitoreID).ToList())
                {

                    Library.Entity.Fattura fattExt = saveEnt.UnitOfWork.FatturaRepository.Find(a => a.ChiusaSpedita == false && a.ClienteFornitoreID == item.Entity.FornitoreID &&
                    a.TipoDocumento == Library.Entity.EnTipoDocumento.OrdineAlFornitore).FirstOrDefault();
                    var riga = new Library.Entity.FatturaRiga();

                    Library.Entity.Articoli.Articolo art = saveEnt.UnitOfWork.ArticoliRepository.Find(a => a.ID == item.Entity.ArticoloID).FirstOrDefault();



                    if (fattExt == null)
                        fattExt = listFatt.Where(a => a.ClienteFornitoreID == item.Entity.FornitoreID).FirstOrDefault();

                    if (fattExt == null)
                    {
                        fattExt = new Library.Entity.Fattura();
                        fattExt.ClienteFornitoreID = item.Entity.FornitoreID;
                        fattExt.TipoDocumento = Library.Entity.EnTipoDocumento.OrdineAlFornitore;
                        fattExt.Data = DateTime.Now;
                        fattExt.Codice = ControllerFatturazione.CalcolaCodice(fattExt);

                        var fornitore = saveEnt.UnitOfWork.SoggettiRepository.Find(a => a.ID == fattExt.ClienteFornitoreID).Select(a => new { a.RagioneSociale, a.CodiceFiscale, a.PIVA }).FirstOrDefault();
                        fattExt.RagioneSociale = fornitore.RagioneSociale;
                        fattExt.PIVA = fornitore.PIVA;

                        saveEnt.UnitOfWork.FatturaRepository.Add(fattExt);

                        saveEnt.SaveEntity("");
                    }
                    listFatt.Add(fattExt);

                    riga = (new Library.Entity.FatturaRiga()
                    {
                        CodiceFornitore = item.CodiceArticoloFornitore,
                        ArticoloID = item.Entity.ArticoloID,
                        Descrizione = art.Titolo,
                        Qta = item.Qta,
                        Fattura = fattExt,
                        PrezzoUnitario = item.Prezzo,
                        IvaApplicata = art.ArticoloWeb.Iva.ToString(),
                    });
                    saveEnt.UnitOfWork.FattureRigheRepository.Add(riga);
                    save = true;
                }


                if (save)
                    saveEnt.SaveEntity("");
                else
                {
                    MessageManager.NotificaInfo("Non ci sono articoli da ordinare!");
                    return false;
                }

                foreach (var item in listFatt.Distinct())
                {
                    ControllerFatturazione.CalcolaTotali(item);
                    saveEnt.UnitOfWork.FatturaRepository.Update(item);

                }
                if (save)
                    saveEnt.SaveEntity("Generati gli ordini di acquisto!");

                return true;
            }
        }
        private void GeneraOrdineCarico()
        {
            var fatt = SelectedItem;



            //controllo se ci sono già altri ordini di carico
            //per le righe di questo ordine

            if (fatt.ChiusaSpedita)
            {
                MessageManager.NotificaWarnig("Documento già chiuso, non è possibile fare altri documenti di carico!");
                return;
            }
            if (fatt.TipoDocumento != EnTipoDocumento.OrdineAlFornitore)
            {
                MessageManager.NotificaWarnig("Documento deve essere del tipo ordine al fornitore!");
                return;
            }


            using (var saveEnt = new SaveEntityManager())
            {
                bool save = false;

                var listFatt = new List<Library.Entity.Fattura>();

                var daGenerare = new List<Tuple<FatturaRiga, int>>();
                foreach (var item in saveEnt.UnitOfWork.FattureRigheRepository.Find(a => a.FatturaID == fatt.ID).ToList())
                {

                    /*deve cercare gli ordini di carico già fatti, e collegati, e detrarre la qta per vedere se farne di nuovi*/
                    var evaso = saveEnt.UnitOfWork.FattureRigheRepository.Find(a => a.IdRigaCollegata == item.ID).Select(a => new 
                    { Qta = a.Fattura.ChiusaSpedita == true ? a.Ricevuti : a.Qta }).Select(a=>a.Qta).DefaultIfEmpty(0).Sum();

                    if (evaso == item.Qta)
                        continue;
                    daGenerare.Add(new Tuple<FatturaRiga, int>(item, item.Qta - evaso));

                }

                if (daGenerare.Count() == 0)
                {
                    MessageManager.NotificaInfo("Non ci altre righe da fare ingressare, chiudi ordine fornitore !");
                    return;
                }


                Library.Entity.Fattura fattExt = null;

                foreach (var item in daGenerare)
                {

                    if (fattExt == null)
                    {
                        fattExt = new Library.Entity.Fattura();
                        fattExt.ClienteFornitoreID = fatt.ClienteFornitoreID;
                        fattExt.TipoDocumento = Library.Entity.EnTipoDocumento.OrdineDiCarico;
                        fattExt.Data = DateTime.Now;
                        fattExt.Codice = ControllerFatturazione.CalcolaCodice(fattExt);

                        var fornitore = saveEnt.UnitOfWork.SoggettiRepository.Find(a => a.ID == fattExt.ClienteFornitoreID).ToList().Select(a => new
                        {
                            RagioneSociale = !string.IsNullOrEmpty(a.RagioneSociale) ? a.RagioneSociale : a.Cognome + " " + a.Nome,
                            PIVA = !string.IsNullOrEmpty(a.PIVA) ? a.PIVA : a.CodiceFiscale
                        }).FirstOrDefault();
                        fattExt.RagioneSociale = fornitore.RagioneSociale;
                        fattExt.PIVA = fornitore.PIVA;

                        saveEnt.UnitOfWork.FatturaRepository.Add(fattExt);

                        //saveEnt.SaveEntity("");

                        listFatt.Add(fattExt);

                    }

                    var rigaMaster = item.Item1;
                    var riga = (new Library.Entity.FatturaRiga()
                    {
                        CodiceFornitore = rigaMaster.CodiceFornitore,
                        ArticoloID = rigaMaster.ArticoloID,
                        Descrizione = rigaMaster.Descrizione,
                        Qta = item.Item2,
                        Fattura = fattExt,
                        PrezzoUnitario = rigaMaster.PrezzoUnitario,
                        IvaApplicata = rigaMaster.IvaApplicata,
                        IdRigaCollegata = rigaMaster.ID
                    });
                    saveEnt.UnitOfWork.FattureRigheRepository.Add(riga);
                    save = true;
                }

                if (save)
                    saveEnt.SaveEntity("");
                else
                {
                    MessageManager.NotificaInfo("Non ci sono articoli da ordinare!");
                    return;
                }

                foreach (var item in listFatt.Distinct())
                {
                    ControllerFatturazione.CalcolaTotali(item);
                    saveEnt.UnitOfWork.FatturaRepository.Update(item);

                }
                if (save)
                    saveEnt.SaveEntity("Generato ordine di carico!");

                RefreshList(null);

            }


        }

        public override void RefreshList(UpdateList<Fattura> obj)
        {
            try
            {
                var datoRicerca = TestoRicerca;
                var list = new List<FatturaItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.FatturaRepository.Find(a => datoRicerca == ""
                       || a.RagioneSociale.Contains(datoRicerca)
                        || a.PIVA.Contains(datoRicerca)
                        || a.Codice.Contains(datoRicerca)

                    ).OrderByDescending(a => a.Data).Take(ViewAllItem ? 100000 : 100).ToList().Select(a => new FatturaItem
                    {
                        ID = a.ID,
                        Data = a.Data,
                        Entity = a,
                        PIVA = a.PIVA,
                        Codice = a.Codice,
                        RagioneSociale = a.RagioneSociale,
                        TipoDocumento = Enum.GetName(typeof(EnTipoDocumento), a.TipoDocumento)
                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<FatturaItem>(list);

                base.RefreshList(obj);
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }
    }
}