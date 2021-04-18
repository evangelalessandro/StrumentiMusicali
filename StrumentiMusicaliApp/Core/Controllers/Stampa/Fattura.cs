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
                    Select(a => new { a.ClienteFornitore, fat = a }).First().fat;
                var righeFatt = uof.FattureRigheRepository.Find(a => a.FatturaID == fatturaSel.ID).OrderBy(a => a.OrdineVisualizzazione).ToList();

                ImpostaCampiTestata(fattura);

                /*calcolo iva*/

                ImpostaQuadroIVa(righeFatt, fattura);

                ImpostaDettaglio(righeFatt, fattura);
            }

            var newfile = Path.Combine(System.IO.Path.GetTempPath(), DateTime.Now.Ticks.ToString() + "_Fatt.xlsx");
            _excel.SaveAs(newfile);
            Process.Start(newfile);
        }

        private void ImpostaQuadroIVa(List<FatturaRiga> righeFatt, Fattura fattura)
        {
            decimal importoIvaTot = 0;
            decimal imponibileIVaTot = 0;
            var rigaIniziale = 1;
            if (fattura.TipoDocumento == EnTipoDocumento.FatturaDiCortesia ||
                fattura.TipoDocumento == EnTipoDocumento.RicevutaFiscale ||
                fattura.TipoDocumento == EnTipoDocumento.NotaDiCredito)
            {
                foreach (var item in righeFatt.Where(a => a.Importo > 0 && a.IvaApplicata.Length > 0).GroupBy(a => a.IvaApplicata).OrderBy(a => a.Key))
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
                var tot = "Totale Fattura";
                if (fattura.TipoDocumento == EnTipoDocumento.NotaDiCredito)
                {
                    tot = "Totale Nota";
                }
                _excel.Range("Totale").Value = tot + ":  " +
                    (imponibileIVaTot + importoIvaTot).ToString("C2");
            }
            else
            {
                _excel.Range("QuadroDDTDaCancellare").Clear(ClosedXML.Excel.XLClearOptions.Contents);
            }
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
            switch (fattura.TipoDocumento)
            {
                case EnTipoDocumento.FatturaDiCortesia:
                    _excel.Range("TipoDocumento").Value = "Fattura di cortesia";
                    break;

                case EnTipoDocumento.RicevutaFiscale:
                    _excel.Range("TipoDocumento").Value = "Ricevuta fiscale";
                    break;

                case EnTipoDocumento.NotaDiCredito:
                    _excel.Range("TipoDocumento").Value = "Nota di credito";
                    break;

                case EnTipoDocumento.DDT:
                    _excel.Range("TipoDocumento").Value = "DDT";
                    break;
                case EnTipoDocumento.OrdineAlFornitore:
                    _excel.Range("TipoDocumento").Value = "Ordine al fornitore";
                    break;
                default:
                    break;
            }

            _excel.Range("ClienteRagioneSociale").Value = fattura.ClienteFornitore.RagioneSociale;
            _excel.Range("ClienteIndirizzo").Value = fattura.ClienteFornitore.Indirizzo.IndirizzoConCivico;
            _excel.Range("ClienteCap").Value = fattura.ClienteFornitore.Indirizzo.Cap;
            _excel.Range("ClienteCitta").Value = fattura.ClienteFornitore.Indirizzo.Citta;
            _excel.Range("ClientePIVACF").Value = "CF - PIVA:" + fattura.ClienteFornitore.PIVA;
            _excel.Range("CodiceCliente").Value = fattura.ClienteFornitore.ID;
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
            _excel.Range("NegozioPEC").Value = intestStampa.NegozioEmailPEC;
        }

        private void ImpostaDettaglio(List<FatturaRiga> righeFatt, Fattura fattura)
        {
            int rigaIniziale = 2;
            int colArt = 1;
            int colDes = 2;
            int colQta = 4;
            int colPrezzo = 5;
            int colImporto = 6;
            int colIva = 7;

            if (fattura.TipoDocumento == EnTipoDocumento.DDT)
            {
                colQta = colIva;
            }

            if (fattura.TipoDocumento == EnTipoDocumento.DDT)
            {
                //riscrivo intestazioni
                _excel.Range("Righe").Range(rigaIniziale - 1, colDes, rigaIniziale - 1, colImporto).Merge();
                ImpostaValoreRiga(rigaIniziale - 1, colQta, "Qta");
            }

            foreach (var item in righeFatt)
            {
                ImpostaValoreRiga(rigaIniziale, colArt, item.CodiceArticoloOld);
                ImpostaValoreRiga(rigaIniziale, colDes, item.Descrizione);

                if (item.Qta > 0 || (fattura.TipoDocumento != EnTipoDocumento.DDT))
                    ImpostaValoreRiga(rigaIniziale, colQta, item.Qta);

                if (fattura.TipoDocumento != EnTipoDocumento.DDT)
                {
                    ImpostaValoreRiga(rigaIniziale, colPrezzo, item.PrezzoUnitario.ToString("C2"));
                    ImpostaValoreRiga(rigaIniziale, colImporto, item.Importo.ToString("C2"));
                    ImpostaValoreRiga(rigaIniziale, colIva, item.IvaApplicata);
                }

                _excel.Worksheet(1).Row(rigaIniziale).AdjustToContents();
                //if (_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes+1).IsMerged())
                //{
                //	_excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes+1).Unmerge();
                //}
                _excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes).
                    Style.Alignment.WrapText = true;

                var address = _excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes).RangeAddress;

                if (item.Descrizione == null)
                    item.Descrizione = "";
                var lengh = (double)item.Descrizione.Length;
                var caratteriPerRiga = 0;
                if (fattura.TipoDocumento == EnTipoDocumento.DDT)
                {
                    caratteriPerRiga = 68;
                    _excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colImporto).Merge();
                }
                else
                {
                    caratteriPerRiga = 15;
                    _excel.Range("Righe").Range(rigaIniziale, colDes, rigaIniziale, colDes + 1).Merge();
                }

                if (lengh > caratteriPerRiga)
                {
                    lengh = Math.Round((double)(lengh) / (double)caratteriPerRiga, 0, MidpointRounding.ToEven);

                    _excel.Worksheet(1).Row(address.FirstAddress.RowNumber).Height = (double)15 + lengh * (double)15;
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