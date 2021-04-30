using FatturaElettronica.Defaults;
using FatturaElettronica.Ordinaria;
using FatturaElettronica.Ordinaria.FatturaElettronicaBody;
using FatturaElettronica.Ordinaria.FatturaElettronicaBody.DatiGenerali;
using FatturaElettronica.Validators;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
    internal class FattElettronica : IDisposable
    {
        public DatiMittente DatiMittente { get; set; }
        public Soggetto DatiDestinatario { get; set; }
        public List<FatturaHeader> FattureList { get; set; } = new List<FatturaHeader>();

        public void ScriviFattura(int idFattura)
        {
            FatturaOrdinaria fattura = FatturaElettronica.Ordinaria.FatturaOrdinaria.CreateInstance(DatiDestinatario.FatturaVersoPA ? Instance.PubblicaAmministrazione : Instance.Privati);

            try
            {



                DatiTrasmissione(fattura);
                DatiMittenteFattura(fattura);
                DatiCommittenteFattura(fattura);
                IscrizioneRegistroImpresa(fattura);


                CheckValidate(new CessionarioCommittenteValidator().Validate(fattura.FatturaElettronicaHeader.CessionarioCommittente));

                CheckValidate(new DatiTrasmissioneValidator().Validate(fattura.FatturaElettronicaHeader.DatiTrasmissione));

                CheckValidate(new DatiAnagraficiCedentePrestatoreValidator().Validate(fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici));

            }
            catch (MessageException ex)
            {
                MessageManager.NotificaWarnig(ex.Messages);
                return;
            }

            foreach (var item in FattureList)
            {
                var body = new FatturaElettronicaBody();
                fattura.FatturaElettronicaBody.Add(body);
                try
                {

                    ImpostaDatiSingolaFattura(body, item);
                    var validator = new FatturaOrdinariaValidator();
                    CheckValidate(validator.Validate(fattura));
                }
                catch (MessageException ex)
                {
                    MessageManager.NotificaWarnig(ex.Messages.Replace("Header.CedentePrestatore", " [Mittente Fattura] "));
                    return;
                }


                WriteFattura(fattura, item, idFattura);

            }


        }

        private static void WriteFattura(FatturaOrdinaria fattura, FatturaHeader item, int idFattura)
        {
            var dir = Path.Combine(Application.StartupPath, "FattureXml");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            Regex illegalInFileName = new Regex(@"[\\/:*?""<>|]");


            string nomeFile = illegalInFileName.Replace(item.Numero + ".xml", "_");
            var file = Path.Combine(dir, nomeFile);

            using (var w = XmlWriter.Create(file, new XmlWriterSettings { Indent = true }))
            {
                fattura.WriteXml(w);
            }
            using (var uof = new UnitOfWork())
            {
                uof.FattureGenerateInvioRepository.Add(new FattureGenerateInvio()
                {
                    NomeFile = nomeFile,
                    ProgressivoInvio = fattura.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio,
                    FatturaID = idFattura,
                    FileInviato = false
                });
                uof.Commit();

                var p = new Process();
                p.StartInfo = new ProcessStartInfo("notepad.exe", file)
                {
                    UseShellExecute = true
                };
                p.Start(); 
            }
        }

        private void ImpostaDatiSingolaFattura(FatturaElettronicaBody body, FatturaHeader itemFattura)
        {
            body.DatiGenerali.DatiGeneraliDocumento.TipoDocumento = itemFattura.TipoDocumento.ToString();
            body.DatiGenerali.DatiGeneraliDocumento.Divisa = "EUR";
            body.DatiGenerali.DatiGeneraliDocumento.Numero = itemFattura.Numero;
            body.DatiGenerali.DatiGeneraliDocumento.Data = itemFattura.Data;

            if (DatiMittente.SoggettoARitenuta)
            {
                if (DatiMittente.PersonaGiuridica)
                {
                    body.DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.Add(new DatiRitenuta() { TipoRitenuta = "RT02" });
                }
                else
                {
                    body.DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.Add(new DatiRitenuta() { TipoRitenuta = "RT01" });
                }
                /*[2.1.1.5.3] AliquotaRitenuta TODO*/
            }
            /*[2.1.1.6] DatiBollo sulla tipologia di documento/operazione è previsto l’assolvimento
					dell’imposta di bollo*/

            body.DatiGenerali.DatiGeneraliDocumento.ImportoTotaleDocumento = itemFattura.ImportoTotaleDocumento;
            if (itemFattura.TipoDocumento == enTipoDocumento.TD04)
            {
                //body.DatiGenerali.DatiFattureCollegate.Add(new FatturaElettronica.FatturaElettronicaBody.DatiGenerali.DatiFattureCollegate() { IdDocumento})
            }
            /*DatiDDT*/
            AggiungiRighe(body, itemFattura);
            AggiungiRiepilogoIva(body, itemFattura);

            var datiPg = new FatturaElettronica.Ordinaria.FatturaElettronicaBody.DatiPagamento.DatiPagamento();
            datiPg.CondizioniPagamento = "TP02";
            var dettPg = new FatturaElettronica.Ordinaria.FatturaElettronicaBody.DatiPagamento.DettaglioPagamento();

            switch (itemFattura.ModalitaPagamento)
            {
                case enTipoPagamento.Contanti:
                    dettPg.ModalitaPagamento = "MP01";
                    break;

                case enTipoPagamento.Bonifico:
                    dettPg.ModalitaPagamento = "MP05";
                    if (string.IsNullOrEmpty(DatiMittente.IBAN))
                    {
                        throw new MessageException("Codice iban mancante del mandante!");
                    }
                    dettPg.IBAN = DatiMittente.IBAN;
                    break;

                case enTipoPagamento.Contrassegno:
                    throw new MessageException("Pagamento contrassegno ancora non gestito");

                default:
                    throw new MessageException("Manca il tipo pagamento");
            }

            dettPg.ImportoPagamento = body.DatiBeniServizi.DatiRiepilogo.Select(a => a.ImponibileImporto + a.Imposta).Sum();

            datiPg.DettaglioPagamento.Add(dettPg);
            body.DatiPagamento.Add(datiPg);
        }

        private void AggiungiRiepilogoIva(FatturaElettronicaBody body, FatturaHeader itemFattura)
        {
            foreach (var itemIva in itemFattura.Righe.ToList().GroupBy(a => a.AliquotaIVA).ToList())
            {
                var riepilogoLinea = new FatturaElettronica.Ordinaria.FatturaElettronicaBody.DatiBeniServizi.DatiRiepilogo();
                riepilogoLinea.AliquotaIVA = itemIva.Key;
                riepilogoLinea.ImponibileImporto = itemIva.Sum(a => a.PrezzoTotale);
                riepilogoLinea.Imposta = riepilogoLinea.ImponibileImporto * (decimal)itemIva.Key / (decimal)100;

                body.DatiBeniServizi.DatiRiepilogo.Add(riepilogoLinea);
            }
        }

        private void AggiungiRighe(FatturaElettronicaBody body, FatturaHeader item)
        {
            foreach (var itemLine in item.Righe)
            {
                var linea = new FatturaElettronica.Ordinaria.FatturaElettronicaBody.DatiBeniServizi.DettaglioLinee();

                linea.Descrizione = itemLine.Descrizione;
                linea.Quantita = itemLine.QTA;
                linea.UnitaMisura = "NR";
                linea.PrezzoUnitario = itemLine.PrezzoUnitario;
                linea.PrezzoTotale = itemLine.PrezzoTotale;
                linea.AliquotaIVA = itemLine.AliquotaIVA;

                linea.NumeroLinea = body.DatiBeniServizi.DettaglioLinee.Count + 1;

                body.DatiBeniServizi.DettaglioLinee.Add(linea);
            }
        }

        private void DatiCommittenteFattura(FatturaOrdinaria fattura)
        {
            var rel = fattura.FatturaElettronicaHeader.CessionarioCommittente.DatiAnagrafici;
            if (DatiDestinatario.PersonaGiuridica)
            {
                rel.IdFiscaleIVA.IdPaese = "IT";
                rel.IdFiscaleIVA.IdCodice = DatiDestinatario.PIVA;
            }

            /* CodiceFiscale=  il campo, se valorizzato, deve contenere il
			codice fiscale del cedente/prestatore che sarà composto di 11 caratteri
			numerici, se trattasi di persona giuridica, oppure di 16 caratteri
			alfanumerici, se trattasi di persona fisica.
			*/

            if (DatiDestinatario.PersonaGiuridica)
            {
                rel.Anagrafica.Denominazione = DatiDestinatario.RagioneSociale;
            }
            else
            {
                rel.Anagrafica.Nome = DatiDestinatario.Nome;
                rel.Anagrafica.Cognome = DatiDestinatario.Cognome;
                rel.CodiceFiscale = DatiDestinatario.CodiceFiscale;
            }
            var sede = fattura.FatturaElettronicaHeader.CessionarioCommittente.Sede;
            sede.Indirizzo = DatiDestinatario.Indirizzo.IndirizzoConCivico;
            sede.CAP = DatiDestinatario.Indirizzo.Cap.Trim();
            sede.Comune = DatiDestinatario.Indirizzo.Comune;
            sede.Provincia = DatiDestinatario.Indirizzo.ProvinciaSigla;
            sede.Nazione = "IT";
        }

        private void DatiMittenteFattura(FatturaOrdinaria fattura)
        {
            fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdPaese = "IT";
            fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdCodice = DatiMittente.PIVA;

            if (string.IsNullOrEmpty(DatiMittente.PIVA))
            {
                throw new MessageException("Occorre impostare la PIVA del mittente!");
            }
            /* CodiceFiscale=  il campo, se valorizzato, deve contenere il
			codice fiscale del cedente/prestatore che sarà composto di 11 caratteri
			numerici, se trattasi di persona giuridica, oppure di 16 caratteri
			alfanumerici, se trattasi di persona fisica.
			*/

            fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.CodiceFiscale = DatiMittente.PersonaGiuridica ? DatiMittente.PIVA : DatiMittente.CodiceFiscale;
            if (DatiMittente.PersonaGiuridica)
            {
                fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.Anagrafica.Denominazione = DatiMittente.RagioneSociale;
                if (string.IsNullOrEmpty(DatiMittente.RagioneSociale))
                {
                    throw new MessageException("Occorre impostare la Ragione Sociale del mittente!");
                }
            }
            else
            {
                fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.Anagrafica.Nome = DatiMittente.Nome;
                fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.Anagrafica.Cognome = DatiMittente.Cognome;


                if (string.IsNullOrEmpty(DatiMittente.Nome) || string.IsNullOrEmpty(DatiMittente.Cognome))
                {
                    throw new MessageException("Occorre impostare nome e cognome del mittente!");
                }
            }

            fattura.FatturaElettronicaHeader.CedentePrestatore.DatiAnagrafici.RegimeFiscale = DatiMittente.RegimeFiscale;
            fattura.FatturaElettronicaHeader.CedentePrestatore.Sede.Indirizzo = DatiMittente.Indirizzo.IndirizzoConCivico;
            fattura.FatturaElettronicaHeader.CedentePrestatore.Sede.CAP = DatiMittente.Indirizzo.Cap;
            fattura.FatturaElettronicaHeader.CedentePrestatore.Sede.Comune = DatiMittente.Indirizzo.Comune;
            fattura.FatturaElettronicaHeader.CedentePrestatore.Sede.Provincia = DatiMittente.Indirizzo.ProvinciaSigla;
            fattura.FatturaElettronicaHeader.CedentePrestatore.Sede.Nazione = "IT";

        }

        private static void CheckValidate(FluentValidation.Results.ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new MessageException(result.Errors.Select(a => a.PropertyName + " --> " + a.ErrorMessage + " " + a.ErrorCode).ToList());
            }
        }

        private void IscrizioneRegistroImpresa(FatturaOrdinaria fattura)
        {
            if (DatiMittente.IscrittoRegistroImprese)
            {
                fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.Ufficio = DatiMittente.UfficioRegistroImp.SiglaProv;
                fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.NumeroREA = DatiMittente.UfficioRegistroImp.NumeroRea;
                fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.CapitaleSociale = DatiMittente.UfficioRegistroImp.CapitaleSociale;
                if (DatiMittente.UfficioRegistroImp.SocioUnico)
                {
                    fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.SocioUnico = "SU";
                }
                if (DatiMittente.UfficioRegistroImp.SocioMultiplo)
                {
                    fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.SocioUnico = "SM";
                }
                fattura.FatturaElettronicaHeader.CedentePrestatore.IscrizioneREA.StatoLiquidazione = "LN";
            }
        }

        private void DatiTrasmissione(FatturaOrdinaria fattura)
        {
            fattura.FatturaElettronicaHeader.DatiTrasmissione.IdTrasmittente.IdPaese = "IT";
            fattura.FatturaElettronicaHeader.DatiTrasmissione.IdTrasmittente.IdCodice = DatiMittente.CodiceFiscale;
            if (string.IsNullOrEmpty(fattura.FatturaElettronicaHeader.DatiTrasmissione.IdTrasmittente.IdCodice))
            {
                throw new MessageException("Impostare il codice fiscale del mittente. Sistemare i dati mittente fattura");
            }


            fattura.FatturaElettronicaHeader.DatiTrasmissione.ProgressivoInvio = CalcolaProgressivo();
            /*assume valore fisso pari a “FPA12”, se la fattura è
				destinata ad una pubblica amministrazione, oppure “FPR12”, se la fattura è
				destinata ad un soggetto privato.
			*/
            fattura.FatturaElettronicaHeader.DatiTrasmissione.FormatoTrasmissione = DatiDestinatario.FatturaVersoPA ? "FPA12" : "FPR12";

            if (!DatiDestinatario.FatturaVersoPA)
            {
                /*se la fattura è destinata ad una pubblica amministrazione, il campo deve
				contenere il codice di 6 caratteri, presente su IndicePA tra le informazioni
				relative al servizio di fatturazione elettronica, associato all’ufficio che,
				all’interno dell’amministrazione destinataria, svolge la funzione di ricezione
				(ed eventualmente lavorazione) della fattura; in alternativa, è possibile
				valorizzare il campo con il codice Ufficio “centrale” o con il valore di default
				“999999”, quando ricorrono le condizioni previste dalle disposizioni della
				circolare interpretativa del Ministero dell’Economia e delle Finanze n.1 del
				31 marzo 2014;
				se la fattura è destinata ad un soggetto privato, il campo deve contenere il
				codice di 7 caratteri che il Sistema di Interscambio ha attribuito a chi, in
				qualità di titolare di un canale di trasmissione diverso dalla PEC abilitato a
				ricevere fatture elettroniche, ne abbia fatto richiesta attraverso l’apposita
				funzionalità presente sul sito www.fatturapa.gov.it; se la fattura deve
				essere recapitata ad un soggetto che intende ricevere le fatture
				elettroniche attraverso il canale PEC, il campo deve essere valorizzato con
				sette zeri (“0000000”) e deve essere valorizzato il campo PECDestinatario
				(1.1.6).
				*/
                if (DatiDestinatario.PecConfig.RicezioneConCodicePec &&
                    !string.IsNullOrEmpty(DatiDestinatario.PecConfig.CodicePec))
                {
                    fattura.FatturaElettronicaHeader.DatiTrasmissione.CodiceDestinatario = DatiDestinatario.PecConfig.CodicePec;

                    if (string.IsNullOrEmpty(DatiDestinatario.PecConfig.CodicePec))
                    {
                        throw new MessageException("Al cliente è impostata ricezione con codice PEC, ma non è impostato. Sistemare il cliente");
                    }
                }
                else
                {
                    fattura.FatturaElettronicaHeader.DatiTrasmissione.CodiceDestinatario = "0000000";
                }
            }
            else
            {
                fattura.FatturaElettronicaHeader.DatiTrasmissione.CodiceDestinatario = "999999";
            }
            fattura.FatturaElettronicaHeader.DatiTrasmissione.ContattiTrasmittente.Email = DatiMittente.Email;
            fattura.FatturaElettronicaHeader.DatiTrasmissione.ContattiTrasmittente.Telefono = DatiMittente.Telefono;
            if (!DatiDestinatario.FatturaVersoPA && !DatiDestinatario.PecConfig.RicezioneConCodicePec)
            {
                fattura.FatturaElettronicaHeader.DatiTrasmissione.PECDestinatario = DatiDestinatario.PecConfig.EmailPec;

                if (string.IsNullOrEmpty(DatiDestinatario.PecConfig.EmailPec))
                {
                    throw new MessageException("Al cliente non è impostata la Email-PEC. Sistemare il cliente");
                }
            }





        }

        private string CalcolaProgressivo()
        {
            using (var uof = new UnitOfWork())
            {
                var progress = uof.FattureGenerateInvioRepository.Find(a => true).Select(a => a.ID).DefaultIfEmpty(0).Max() + 1;

                return progress.ToString("00000");

            }
        }

        public void Dispose()
        {

        }
    }
}