using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Stampa
{
	public class StampaFattura : IDisposable
	{
		private ClosedXML.Excel.XLWorkbook _excel;
		public StampaFattura()
		{

			_excel = new ClosedXML.Excel.XLWorkbook(Path.Combine(Application.StartupPath
				, @"TemplateExcel\ProtoFatt.xlsx"));
		}
		public void Stampa(DatiIntestazioneStampaFattura intestStampa,
			Fattura fatturaSel)
		{
			DatiIntestazione(intestStampa);


			using (var uof = new UnitOfWork())
			{
				var fattura = uof.FatturaRepository.Find(a => a.ID == fatturaSel.ID).
					Select(a => new { a.Cliente, fat = a }).First().fat;
				var righeFatt = uof.FattureRigheRepository.Find(a => a.FatturaID == fatturaSel.ID).ToList();

				ImpostaCampiTestata(fattura);

				/*calcolo iva*/
				ImpostaQuadroIVa(righeFatt);

				ImpostaDettaglio(righeFatt);
			}

			var newfile = Path.Combine(System.IO.Path.GetTempPath(), DateTime.Now.Ticks.ToString() + "_Fatt.xlsx");
			_excel.SaveAs(newfile);
			Process.Start(newfile);
		}

		private void ImpostaQuadroIVa(List<FatturaRiga> righeFatt)
		{
			decimal importoIvaTot = 0;
			decimal imponibileIVaTot = 0;
			var rigaIniziale = 1;

			foreach (var item in righeFatt.Where(a=>a.Importo>0 && a.IvaApplicata.Length>0).GroupBy(a => a.IvaApplicata).OrderBy(a => a.Key))
			{
				var imponibileIVa = item.Select(a => a.Importo).DefaultIfEmpty(0).Sum();
				ImpostaRigaIva(rigaIniziale, 1, item.Key);
				ImpostaRigaIva(rigaIniziale, 2, imponibileIVa.ToString("C2"));
				decimal importoIva = 0;
				decimal val;
				if (decimal.TryParse(item.Key, out val))
				{
					importoIva = imponibileIVa * ((decimal)val) / ((decimal)100);
				}
				ImpostaRigaIva(rigaIniziale, 3, importoIva.ToString("C2"));
				importoIvaTot += importoIva;
				imponibileIVaTot += imponibileIVa;
				rigaIniziale++;


			}
			_excel.Range("ImportoIVa").Value = (importoIvaTot).ToString("C2");
			_excel.Range("ImportoTotale").Value = (imponibileIVaTot).ToString("C2");

			_excel.Range("Totale").Value = "Totale Fattura:  " +
					(imponibileIVaTot + importoIvaTot).ToString("C2");
		}

		private void ImpostaCampiTestata(Fattura fattura)
		{
			_excel.Range("Note1").Value = fattura.Note1;
			_excel.Range("Note2").Value = fattura.Note2;
			_excel.Range("TrasportoACura").Value = fattura.TrasportoACura;
			_excel.Range("Vettore").Value = fattura.Vettore;
			_excel.Range("AspettoBeni").Value = fattura.AspettoEsterno;
			_excel.Range("CausaleTrasporto").Value = fattura.CausaleTrasporto;
			_excel.Range("DataOraTrasporto").Value = fattura.DataTrasporto;
			_excel.Range("NColli").Value = fattura.NumeroColli;
			_excel.Range("PesoKg").Value = fattura.PesoKg;
			_excel.Range("Porto").Value = fattura.Porto;
			_excel.Range("Codice").Value = fattura.Codice;
			_excel.Range("Data").Value = fattura.Data;
			_excel.Range("Pagamento").Value = fattura.Pagamento;
			_excel.Range("TipoDocumento").Value = fattura.TipoDocumento == 2 ? "Fattura" : "DDT";

			_excel.Range("ClienteRagioneSociale").Value = fattura.Cliente.RagioneSociale;
			_excel.Range("ClienteIndirizzo").Value = fattura.Cliente.Via;
			_excel.Range("ClienteCap").Value = fattura.Cliente.Cap;
			_excel.Range("ClienteCitta").Value = fattura.Cliente.Citta;
			_excel.Range("ClientePIVACF").Value = "CF - PIVA:" + fattura.Cliente.PIVA;
			_excel.Range("CodiceCliente").Value = fattura.Cliente.ID;

		}

		private void DatiIntestazione(DatiIntestazioneStampaFattura intestStampa)
		{
			_excel.Range("NegozioRagSoc").Value = intestStampa.NegozioRagSoc;
			_excel.Range("NegozioTelefonoFax").Value = intestStampa.NegozioTelefonoFax;
			_excel.Range("NomeBanca").Value = intestStampa.NomeBanca;
			_excel.Range("NegozioPIVA").Value = intestStampa.NegozioPIVA;
			_excel.Range("NegozioIndirizzo").Value = intestStampa.NegozioIndirizzo;
			_excel.Range("NegozioEmail").Value = intestStampa.NegozioEmail;
			_excel.Range("NegozioCF").Value = intestStampa.NegozioCF;
			_excel.Range("IBAN").Value = intestStampa.IBAN;
		}

		private void ImpostaDettaglio(List<FatturaRiga> righeFatt)
		{
			int rigaIniziale = 2;
			int colArt = 1;
			int colDes = 2;
			int colQta = 4;
			int colPrezzo = 5;
			int colImporto = 6;
			int colIva = 7;

			foreach (var item in righeFatt)
			{
				ImpostaValoreRiga(rigaIniziale, colArt, item.CodiceArticoloOld);
				ImpostaValoreRiga(rigaIniziale, colDes, item.Descrizione);
				ImpostaValoreRiga(rigaIniziale, colQta, item.Qta);
				ImpostaValoreRiga(rigaIniziale, colPrezzo,item.PrezzoUnitario.ToString("C2"));
				ImpostaValoreRiga(rigaIniziale, colImporto, item.Importo.ToString("C2"));
				ImpostaValoreRiga(rigaIniziale, colIva, item.IvaApplicata);

				_excel.Worksheet(1).Row(rigaIniziale).AdjustToContents();
				//if (_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes+1).IsMerged())
				//{
				//	_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes+1).Unmerge();
				//}
				_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes).
					Style.Alignment.WrapText = true;

				var address = _excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes).RangeAddress;
				_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes + 1).Merge();
				if (item.Descrizione == null)
					item.Descrizione = "";
				var lengh = item.Descrizione.Length;
				if (lengh > 15)
				{
					lengh = (lengh - 15) / 15;

					_excel.Worksheet(1).Row(address.FirstAddress.RowNumber).Height = 15+ lengh*15;
				}
				rigaIniziale++;
			}
		}

		private void ImpostaValoreRiga(
			int riga, int colonna, object valore)
		{
			_excel.Range("Righe").Range(riga, colonna, riga, colonna).Value = valore;
		}
		private void ImpostaRigaIva(int riga, int colonna, object valore)
		{
			_excel.Range("IVA").Range(riga, colonna, riga, colonna).Value = valore;
		}

		public void Dispose()
		{
			if (_excel != null)
				_excel.Dispose();
			_excel = null;
		}
	}
}
