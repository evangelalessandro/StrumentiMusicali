using StrumentiMusicali.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;

namespace StrumentiMusicali.App.Core.Imports
{
    public class ImportDaExportMagazzino : BaseImportExcel
    {
        bool _libri = false;
        public ImportDaExportMagazzino(bool libri)
            : base()
        {
            _libri = libri;
        }

        protected override void Import()
        {
            try
            {
                ProgressManager.Instance().Visible = true;

                var dati = base.ReadDatatable("Generale");

                if (_libri)
                {
                    ImportLibri(dati);
                }
                else
                {
                    ImportStrumenti(dati);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
            finally
            {
                ProgressManager.Instance().Visible = false;
            }

        }

        private void ImportStrumenti(System.Data.DataTable dati)
        {
            var list = dati.AsEnumerable().Select(a => new
            {
                // assuming column 0's type is Nullable<long>
                ID = a.Field<int>(0),
                Categoria = a.Field<string>(1),
                Titolo = a.Field<string>(3),
                Condizione = a.Field<string>(4),

                CodiceABarre= a.Field<string>("CodiceABarre"),
                Prezzo= a.Field<decimal>("Prezzo"),
                PrezzoAcquisto = a.Field<decimal>("PrezzoAcquisto"),
                Marca = a.Field<string>("Marca"),
                CodiceOrdine = a.Field<string>("CodiceOrdine"),
                Colore = a.Field<string>("Colore"),
                Rivenditore = a.Field<string>("Rivenditore"),
                Modello = a.Field<string>("Modello"),
                Nome= a.Field<string>("Nome"),
                Note1= a.Field<string>("Note1"),
                Note2 = a.Field<string>("Note2"),
                Note3 = a.Field<string>("Note3"),
                NonImponibile = a.Field<bool>("NonImponibile"),
                Testo = a.Field<string>("Testo"),
                DescrizioneBreveHtml = a.Field<string>("DescrizioneBreveHtml"),
                DescrizioneHtml = a.Field<string>("DescrizioneHtml"),
                Iva= a.Field<int>("Iva"),
                PrezzoWeb = a.Field<decimal>("PrezzoWeb"),
                CodiceArticoloEcommerce = a.Field<string>("CodiceArticoloEcommerce"),
                DataUltimaModifica = a.Field<DateTime>("DataUltimaModifica"),
                 
            }).Where(a => a.Categoria != null && a.Categoria.Length > 0 && a.Marca != null && a.Marca.Length > 0).ToList();

        }

        private void ImportLibri(System.Data.DataTable dati)
        {
            throw new NotImplementedException();
        }
    }
}
