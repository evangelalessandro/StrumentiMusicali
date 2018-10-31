using FatturaElettronica;
using FatturaElettronica.FatturaElettronicaBody;
using FatturaElettronica.Impostazioni;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	internal class FattElettronica
	{
		public DatiMittente DatiMittente { get; set; }
		public DatiDestinatario DatiDestinatario { get; set; }
		public List<FatturaHeader> FattureList { get; set; }

		public void ScriviFattura(int idFattura)
		{
			var fattura = Fattura.CreateInstance(DatiMittente.VersoPA ? Instance.PubblicaAmministrazione : Instance.Privati);

			DatiTrasmissione(fattura);
			DatiMittenteFattura(fattura);
			DatiCommittenteFattura(fattura);
			IscrizioneRegistroImpresa(fattura);

			foreach (var item in FattureList)
			{
				var body = new FatturaElettronica.FatturaElettronicaBody.Body();
				fattura.Body.Add(body);
				ImpostaDatiSingolaFattura(body, item);
			}
		}

		private void ImpostaDatiSingolaFattura(Body body, FatturaHeader itemFattura)
		{
			body.DatiGenerali.DatiGeneraliDocumento.TipoDocumento = itemFattura.TipoDocumento.ToString();
			body.DatiGenerali.DatiGeneraliDocumento.Divisa = "EUR";
			body.DatiGenerali.DatiGeneraliDocumento.Numero = itemFattura.Numero;
			if (DatiMittente.SoggettoARitenuta)
			{
				if (DatiMittente.PersonaGiuridica)
				{
					body.DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.TipoRitenuta = "RT02";
				}
				else
				{
					body.DatiGenerali.DatiGeneraliDocumento.DatiRitenuta.TipoRitenuta = "RT01";
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

			var datiPg = new FatturaElettronica.FatturaElettronicaBody.DatiPagamento.DatiPagamento();
			datiPg.CondizioniPagamento = "TP02";
			var dettPg = new FatturaElettronica.FatturaElettronicaBody.DatiPagamento.DettaglioPagamento();

			switch (itemFattura.ModalitaPagamento)
			{
				case enTipoPagamento.Contanti:
					dettPg.ModalitaPagamento = "MP01";
					break;

				case enTipoPagamento.Bonifico:
					dettPg.ModalitaPagamento = "MP05";
					if (string.IsNullOrEmpty(DatiMittente.IBAN))
					{
						throw new Exception("Codice iban mancante del mandante!");
					}
					dettPg.IBAN = DatiMittente.IBAN;
					break;

				case enTipoPagamento.Contrassegno:
					throw new Exception("Manca il tipo pagamento");

				default:
					throw new Exception("Manca il tipo pagamento");
			}

			dettPg.ImportoPagamento = body.DatiBeniServizi.DatiRiepilogo.Select(a => a.ImponibileImporto + a.Imposta).Sum();

			datiPg.DettaglioPagamento.Add(dettPg);
			body.DatiPagamento.Add(datiPg);
		}

		private void AggiungiRiepilogoIva(Body body, FatturaHeader itemFattura)
		{
			foreach (var itemIva in itemFattura.Righe.ToList().GroupBy(a => a.AliquotaIVA).ToList())
			{
				FatturaElettronica.FatturaElettronicaBody.DatiBeniServizi.DatiRiepilogo riepilogoLinea = new FatturaElettronica.FatturaElettronicaBody.DatiBeniServizi.DatiRiepilogo();
				riepilogoLinea.AliquotaIVA = itemIva.Key;
				riepilogoLinea.ImponibileImporto = itemIva.Sum(a => a.PrezzoTotale);
				riepilogoLinea.Imposta = riepilogoLinea.ImponibileImporto * (decimal)itemIva.Key / (decimal)100;

				body.DatiBeniServizi.DatiRiepilogo.Add(riepilogoLinea);
			}
		}

		private void AggiungiRighe(Body body, FatturaHeader item)
		{
			foreach (var itemLine in item.Righe)
			{
				var linea = new FatturaElettronica.FatturaElettronicaBody.DatiBeniServizi.DettaglioLinee();

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

		private void DatiCommittenteFattura(Fattura fattura)
		{
			var rel = fattura.Header.CessionarioCommittente.DatiAnagrafici;
			if (DatiDestinatario.PersonaGiuridica)
			{
				rel.IdFiscaleIVA.IdPaese = "IT";
				rel.IdFiscaleIVA.IdCodice = DatiDestinatario.PIVA;
			}
			/* CodiceFiscale=  il campo, se valorizzato, deve contenere il
			codice fiscale del cedente/prestatore che sarà composto di 11 caratteri
			numerici, se trattasi di persona giuridica, oppure di 16 caratteri
			alfanumerici, se trattasi di persona fisica.			*/

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
			var sede = fattura.Header.CessionarioCommittente.Sede;
			sede.Indirizzo = DatiDestinatario.IndirizzoConCivico;
			sede.CAP = DatiDestinatario.Cap;
			sede.Comune = DatiDestinatario.Comune;
			sede.Provincia = DatiDestinatario.ProvinciaSigla;
			sede.Nazione = "IT";
		}

		private void DatiMittenteFattura(Fattura fattura)
		{
			fattura.Header.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdPaese = "IT";
			fattura.Header.CedentePrestatore.DatiAnagrafici.IdFiscaleIVA.IdCodice = DatiMittente.PIVA;

			/* CodiceFiscale=  il campo, se valorizzato, deve contenere il
			codice fiscale del cedente/prestatore che sarà composto di 11 caratteri
			numerici, se trattasi di persona giuridica, oppure di 16 caratteri
			alfanumerici, se trattasi di persona fisica.			*/

			fattura.Header.CedentePrestatore.DatiAnagrafici.CodiceFiscale = DatiMittente.PersonaGiuridica ? DatiMittente.PIVA : DatiMittente.CodiceFiscale;
			if (DatiMittente.PersonaGiuridica)
				fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Denominazione = DatiMittente.RagioneSociale;
			else
			{
				fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Nome = DatiMittente.Nome;
				fattura.Header.CedentePrestatore.DatiAnagrafici.Anagrafica.Cognome = DatiMittente.Cognome;
			}
			fattura.Header.CedentePrestatore.DatiAnagrafici.RegimeFiscale = DatiMittente.RegimeFiscale;
			fattura.Header.CedentePrestatore.Sede.Indirizzo = DatiMittente.IndirizzoConCivico;
			fattura.Header.CedentePrestatore.Sede.CAP = DatiMittente.Cap;
			fattura.Header.CedentePrestatore.Sede.Comune = DatiMittente.Comune;
			fattura.Header.CedentePrestatore.Sede.Provincia = DatiMittente.ProvinciaSigla;
			fattura.Header.CedentePrestatore.Sede.Nazione = "IT";
		}

		private void IscrizioneRegistroImpresa(Fattura fattura)
		{
			if (DatiMittente.IscrittoRegistroImprese)
			{
				fattura.Header.CedentePrestatore.IscrizioneREA.Ufficio = DatiMittente.UfficioRegistroImp.SiglaProv;
				fattura.Header.CedentePrestatore.IscrizioneREA.NumeroREA = DatiMittente.UfficioRegistroImp.NumeroRea;
				fattura.Header.CedentePrestatore.IscrizioneREA.CapitaleSociale = DatiMittente.UfficioRegistroImp.CapitaleSociale;
				if (DatiMittente.UfficioRegistroImp.SocioUnico)
				{
					fattura.Header.CedentePrestatore.IscrizioneREA.SocioUnico = "SU";
				}
				if (DatiMittente.UfficioRegistroImp.SocioMultiplo)
				{
					fattura.Header.CedentePrestatore.IscrizioneREA.SocioUnico = "SM";
				}
				fattura.Header.CedentePrestatore.IscrizioneREA.StatoLiquidazione = "LN";
			}
		}

		private void DatiTrasmissione(Fattura fattura)
		{
			fattura.Header.DatiTrasmissione.IdTrasmittente.IdPaese = "IT";
			fattura.Header.DatiTrasmissione.IdTrasmittente.IdCodice = DatiMittente.CodiceFiscale;
			fattura.Header.DatiTrasmissione.ProgressivoInvio = DatiMittente.ProgressivoInvio;
			/*assume valore fisso pari a “FPA12”, se la fattura è
				destinata ad una pubblica amministrazione, oppure “FPR12”, se la fattura è
				destinata ad un soggetto privato.			*/
			fattura.Header.DatiTrasmissione.FormatoTrasmissione = DatiMittente.VersoPA ? "FPA12" : "FPR12";

			if (!DatiMittente.VersoPA)
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
				(1.1.6).				*/
				if (DatiDestinatario.RicezioneConCodicePec && !string.IsNullOrEmpty(DatiDestinatario.CodicePEC))
				{
					fattura.Header.DatiTrasmissione.CodiceDestinatario = DatiDestinatario.CodicePEC;
				}
				else
				{
					fattura.Header.DatiTrasmissione.CodiceDestinatario = "0000000";
				}
			}
			else
			{
				fattura.Header.DatiTrasmissione.CodiceDestinatario = "999999";
			}
			fattura.Header.DatiTrasmissione.ContattiTrasmittente.Email = DatiMittente.Email;
			fattura.Header.DatiTrasmissione.ContattiTrasmittente.Telefono = DatiMittente.Telefono;
			if (!DatiMittente.VersoPA && !DatiDestinatario.RicezioneConCodicePec)
			{
				fattura.Header.DatiTrasmissione.PECDestinatario = DatiDestinatario.EmailPEC;
			}
		}
	}
}