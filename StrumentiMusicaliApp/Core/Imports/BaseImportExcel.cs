using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Imports
{
    public abstract class BaseImportExcel : IDisposable
    {
        public BaseImportExcel()
        {

        }
        protected string NomeFile { get; set; }

        public void ImportFile(string file = "")
        {
            if (string.IsNullOrEmpty(file))
            {
                using (OpenFileDialog res = new OpenFileDialog())
                {
                    res.Title = "Seleziona file excel da importare";
                    //Filter
                    res.Filter = "File excel|*.xls;*.xlsx|Tutti i file|*.*";

                    res.Multiselect = false;
                    //When the user select the file
                    if (res.ShowDialog() == DialogResult.OK)
                    {
                        NomeFile = res.FileName;
                        Import();
                    }
                }
            }
            else
            {
                NomeFile = file;
                Import();

            }
        }

        protected abstract void Import();


        private string ConnectionString(string Header)
        {
            OleDbConnectionStringBuilder Builder = new OleDbConnectionStringBuilder();
            if (Path.GetExtension(NomeFile).ToUpper() == ".XLS")
            {
                Builder.Provider = "Microsoft.Jet.OLEDB.4.0";
                Builder.Add("Extended Properties", string.Format("Excel 8.0;IMEX=1;HDR={0};", Header));
            }
            else
            {
                Builder.Provider = "Microsoft.ACE.OLEDB.12.0";
                Builder.Add("Extended Properties", string.Format("Excel 12.0;IMEX=1;HDR={0};", Header));
            }

            Builder.DataSource = NomeFile;

            return Builder.ConnectionString;
        }

        protected DataTable ReadDatatable(string nameTabella)
        {
            var dt = new DataTable();

            var query = string.Format("SELECT * FROM [{0}$]", nameTabella);
            using (OleDbConnection cn = new OleDbConnection { ConnectionString = ConnectionString("No") })
            {
                using (OleDbCommand cmd = new OleDbCommand { CommandText = query, Connection = cn })
                {
                    cn.Open();

                    OleDbDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            if (dt.Rows.Count > 1)
            {
                if (dt.Rows[0][0].ToString().ToUpper() == "Codice".ToUpper())
                {
                    dt.Rows[0].Delete();
                }
            }
            //if (dt.Rows.Count > 1)
            //{
            //	if (dt.Rows[0][2].ToString() == ""
            //		&& dt.Rows[0][3].ToString() == ""
            //		&& dt.Rows[0][4].ToString() == ""
            //		)
            //	{
            //		// remove header
            //		dt.Rows[0].Delete();
            //		dt.Rows[1].Delete();
            //	}
            //}
            dt.AcceptChanges();
            return dt;
        }

        public void Dispose()
        {

        }
    }
}
