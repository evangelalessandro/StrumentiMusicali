using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.App.Core.Exports
{

    public class ExportMagazzino : IDisposable
    {
        private ClosedXML.Excel.XLWorkbook _excel;
        public TipoExport TipoExp { get; set; } = TipoExport.TuttoLibri;


        public enum TipoExport
        {
            TuttoLibri,
            TuttoStrumenti,
            SoloLibriMancanti,
            PerMarca
        }
        public ExportMagazzino()
        {
        }
        public void Stampa()
        {
            string marcaFiltro = "";
            if (TipoExp == TipoExport.PerMarca)
            {
                using (var uof = new UnitOfWork())
                {

                    var list = uof.ArticoliRepository.Find(a => a.Strumento.Marca.Length > 0)
                    .Select(a => a.Strumento.Marca.ToUpper()).Distinct().OrderBy(a => a).ToList();

                    using (var frmMarche = new ListViewCustom(list,"Marca"))
                    {
                        frmMarche.ShowDialog();
                        marcaFiltro = frmMarche.SelectedItem;

                    }
                }
            }

            _excel = new ClosedXML.Excel.XLWorkbook();


            using (var uof = new UnitOfWork())
            {
                var listArt = uof.AggiornamentoWebArticoloRepository.Find(a => true).Select(a => new
                {
                    ID = a.ArticoloID,
                    a.Articolo.Libro,
                    a.Articolo.Categoria,
                    articolo = a.Articolo,
                    a.CodiceArticoloEcommerce,
                    a.Articolo.Strumento
                }).ToList();

                var qta = uof.MagazzinoRepository.Find(a => true)
                    .Select(a => new { a.ArticoloID, a.Deposito, a.Qta })
                    .GroupBy(a => new { a.ArticoloID, a.Deposito })
                    .Select(a => new { a.Key, sumQta = a.Sum(b => b.Qta) })
                    .ToList();


                if (TipoExp == TipoExport.SoloLibriMancanti)
                {
                    qta = qta.Where(a => a.sumQta == 0 && a.Key.Deposito.Principale).ToList();
                    listArt = listArt.Where(a => qta.Select(b => b.Key.ArticoloID).Contains(a.ID)).ToList();

                    listArt = listArt.Where(a => !string.IsNullOrEmpty(a.articolo.Libro.Autore)
                                  || !string.IsNullOrEmpty(a.Libro.Edizione)
                                  || !string.IsNullOrEmpty(a.Libro.Genere)
                                  || !string.IsNullOrEmpty(a.Libro.Settore)
                                  || a.Categoria.Nome.Contains("libr")

                                ).ToList();
                }
                if (TipoExp == TipoExport.PerMarca)
                {
                    /*filtro per marca*/
                    listArt = listArt.Where(a => a.Strumento.Marca != null && a.Strumento.Marca.Trim() == marcaFiltro).ToList();

                    var qtaAZero = qta.Select(a => new { a.Key.ArticoloID, a.sumQta })
                        .GroupBy(a => a.ArticoloID).Select(a =>
                         new { Somma = a.Sum(b => b.sumQta), ArticoloId = a.Key })
                         .Where(a => a.Somma == 0).Select(a => a.ArticoloId).ToList();
                    /*filtro per le qta a zero*/
                    listArt = listArt.Where(a => qtaAZero.Contains(a.ID)).ToList();
                }
                var listToExport = listArt.Select(a =>
                  new
                  {
                      a.ID,
                      Categoria = a.Categoria.Nome,
                      a.Categoria.Reparto,
                      a.articolo.Titolo,
                      Condizione = a.articolo.Condizione.ToString(),
                      a.articolo.CodiceABarre,
                      Prezzo = a.articolo.Prezzo.ToString("C2"),
                      PrezzoAcquisto = a.articolo.PrezzoAcquisto.ToString("C2"),

                      a.Strumento.Marca,
                      a.Strumento.CodiceOrdine,
                      a.Strumento.Colore,
                      a.Strumento.Rivenditore,
                      a.Strumento.Modello,
                      a.Strumento.Nome,
                      a.articolo.Note1,
                      a.articolo.Note2,
                      a.articolo.Note3,
                      a.articolo.NonImponibile,

                      a.articolo.Testo,
                      a.Libro.Autore,
                      a.Libro.TitoloDelLibro,
                      a.Libro.Edizione,
                      a.Libro.Edizione2,
                      a.Libro.Genere,
                      a.Libro.Ordine,
                      a.Libro.Settore,
                      a.articolo.ArticoloWeb.DescrizioneBreveHtml,
                      a.articolo.ArticoloWeb.DescrizioneHtml,
                      a.articolo.ArticoloWeb.Iva,
                      a.articolo.ArticoloWeb.PrezzoWeb,

                      a.CodiceArticoloEcommerce
                      ,
                      a.articolo.DataUltimaModifica
                  }

            ).ToList();

                DataTable dt = null;
                if (TipoExp == TipoExport.SoloLibriMancanti)
                {
                    dt = ToDataTable(listToExport.Select(a => new
                    {
                        a.ID,
                        a.Titolo,
                        a.TitoloDelLibro,
                        a.Autore,
                        a.Genere,
                        a.Edizione,
                        a.Ordine,
                        a.Prezzo
                    }).ToList());
                }
                else if (TipoExp == TipoExport.PerMarca)
                {
                    dt = ToDataTable(listToExport.Select(a => new
                    {
                        a.ID,
                        a.Titolo,
                        a.Prezzo,
                        a.PrezzoAcquisto,
                        a.Marca,
                        a.CodiceOrdine,
                        a.Note1,
                        a.Note2,
                        a.Note3
                    }).ToList());
                }

                else if (TipoExp == TipoExport.TuttoLibri)
                {

                    dt = ToDataTable(listToExport.ToList());

                    dt.Columns.Remove("Nome");
                    dt.Columns.Remove("Marca");
                    dt.Columns.Remove("Modello");
                    dt.Columns.Remove("Rivenditore");
                    dt.Columns.Remove("Colore");
                    dt.Columns.Remove("CodiceOrdine");
                }
                else if (TipoExp == TipoExport.TuttoStrumenti)
                {
                    dt = ToDataTable(listToExport.ToList());


                    dt.Columns.Remove("Autore");
                    dt.Columns.Remove("TitoloDelLibro");
                    dt.Columns.Remove("Edizione");
                    dt.Columns.Remove("Edizione2");
                    dt.Columns.Remove("Genere");
                    dt.Columns.Remove("Ordine");
                    dt.Columns.Remove("Settore");

                }
                if (TipoExp == TipoExport.TuttoLibri
                    || TipoExp == TipoExport.TuttoStrumenti)
                {

                    foreach (var item in uof.DepositoRepository.Find(a => (a.Principale && TipoExp == TipoExport.SoloLibriMancanti) || TipoExp != TipoExport.SoloLibriMancanti).ToList())
                    {
                        dt.Columns.Add("Qta_" + item.NomeDeposito);

                    }
                    foreach (DataRow itemRow in dt.Rows)
                    {
                        var qtaArt = qta.FindAll(a => a.Key.ArticoloID.ToString() == itemRow["ID"].ToString()).ToList();
                        foreach (var itemQta in qtaArt)
                        {
                            itemRow["Qta_" + itemQta.Key.Deposito.NomeDeposito] = itemQta.sumQta;
                            qta.Remove(itemQta);
                        }
                    }
                }
                if (TipoExp != TipoExport.TuttoLibri && TipoExp != TipoExport.TuttoStrumenti)
                    dt.Columns.Remove("ID");

                _excel.AddWorksheet(dt, "Generale");
            }

            var newfile = Path.Combine(System.IO.Path.GetTempPath(), TipoExp.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_Magazzino.xlsx");
            _excel.SaveAs(newfile);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(newfile)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        protected void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (_excel != null)
                    _excel.Dispose();
                _excel = null;
            }

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
