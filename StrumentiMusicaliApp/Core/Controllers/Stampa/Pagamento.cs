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
        private ClosedXML.Excel.XLWorkbook _excel;
        public StampaPagamento()
        {

            _excel = new ClosedXML.Excel.XLWorkbook(Path.Combine(Application.StartupPath
                , @"TemplateExcel\ProtoPagamento.xlsx"));
        }
        public void Stampa(Pagamento pagamentoSel)
        {


            using (var uof = new UnitOfWork())
            {
                var pagamento = uof.PagamentoRepository.Find(a => a.ID == pagamentoSel.ID)
                    .First();
                var righePag = uof.PagamentoRepository.Find(a => a.IDPagamenti == pagamentoSel.IDPagamenti
                    && a.ID != pagamentoSel.ID).OrderBy(a => a.DataRata).ToList();

                ImpostaCampiTestata(pagamento);


                ImpostaDettaglio(righePag, pagamento);
            }

            var newfile = Path.Combine(System.IO.Path.GetTempPath(),
                DateTime.Now.Ticks.ToString() + "_Pag.xlsx");
            _excel.SaveAs(newfile);
            Process.Start(newfile);
        }


        private void ImpostaCampiTestata(Pagamento pagamento)
        {
            _excel.Range("Nome").Value = pagamento.Nome + " " + pagamento.Cognome;
            
            _excel.Range("Articolo").Value = pagamento.ArticoloAcq;
            _excel.Range("ImportoRata").Value = pagamento.ImportoRata;
            _excel.Range("ImportoResiduo").Value = pagamento.ImportoResiduo;
            _excel.Range("ImportoTotale").Value = pagamento.ImportoTotale;

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
            _excel.Range("Righe").Range(riga, colonna, riga, colonna).Value = valore;
        }
 

        public void Dispose()
        {
            if (_excel != null)
                _excel.Dispose();
            _excel = null;
        }
    }
}
