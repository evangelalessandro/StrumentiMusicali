using ClosedXML.Excel;
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
    public class StampaPagamento : IDisposable
    {
        private ClosedXML.Excel.XLWorkbook _wb;
        private ClosedXML.Excel.IXLWorksheet _ws;
        public StampaPagamento()
        {

            _wb = new ClosedXML.Excel.XLWorkbook(Path.Combine(Application.StartupPath
                , @"TemplateExcel\ProtoPagamento.xlsx"));
            _ws = _wb.Worksheet(1);

        }
        public void Stampa(Pagamento pagamentoSel)
        {


            using (var uof = new UnitOfWork())
            {
                var pagamento = uof.PagamentoRepository.Find(a => a.ID == pagamentoSel.ID)
                    .First();
                var righePag = uof.PagamentoRepository.Find(a => a.IDPagamentoMaster == pagamentoSel.IDPagamentoMaster
                    && a.ID != pagamentoSel.ID).OrderBy(a => a.DataRata).ToList();

                ImpostaCampiTestata(pagamento);


                ImpostaDettaglio(righePag, pagamento);
            }

            var newfile = Path.Combine(System.IO.Path.GetTempPath(),
                DateTime.Now.Ticks.ToString() + "_Pag.xlsx");
            _wb.SaveAs(newfile);
          
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(newfile)
            {
                UseShellExecute = true
            };
            p.Start();
        }


        private void ImpostaCampiTestata(Pagamento pagamento)
        {
            _wb.Range("Nome").Value = pagamento.Nome + " " + pagamento.Cognome;

            _wb.Range("Articolo").Value = pagamento.ArticoloAcq;
            _wb.Range("ImportoRata").Value = pagamento.ImportoRata;
            _wb.Range("ImportoResiduo").Value = pagamento.ImportoResiduo;
            _wb.Range("ImportoTotale").Value = pagamento.ImportoTotale;

        }


        private void ImpostaDettaglio(List<Pagamento> pagamentiRimanenti,
            Pagamento pagamento)
        {
            int colData = 1;
            int colImportoRata = 2;
            int colImportoResiduo = 3;

            int rigaIniziale = 2;



            foreach (var item in pagamentiRimanenti)
            {
                ImpostaValoreRiga(rigaIniziale, colData, item.DataRata.Date.ToShortDateString());
                ImpostaValoreRiga(rigaIniziale, colImportoRata, item.ImportoRata.ToString("C2"));
                ImpostaValoreRiga(rigaIniziale, colImportoResiduo, item.ImportoResiduo.ToString("C2"));

                rigaIniziale++;

            }
        }

        private void ImpostaValoreRiga(
            int riga, int colonna, object valore)
        {
            if (_wb.Range("Righe").LastRow().RowNumber() - _wb.Range("Righe").FirstRow().RowNumber() == riga)
            {
                IXLRow row1 = _ws.Row(riga + _wb.Range("Righe").FirstRow().RowNumber());
                row1.InsertRowsAbove(1);
            }
            _wb.Range("Righe").Range(riga, colonna, riga, colonna).Value = valore;

        }


        public void Dispose()
        {
            if (_wb != null)
                _wb.Dispose();
            _wb = null;
        }
    }
}
