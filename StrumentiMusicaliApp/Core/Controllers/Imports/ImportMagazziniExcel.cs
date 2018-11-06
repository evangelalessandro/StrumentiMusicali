using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Imports
{
	public class ImportMagazziniExcel
	{
		public void SelectFile()
		{
			using (OpenFileDialog res = new OpenFileDialog())
			{
				res.Title = "Seleziona file excel magazzino da importare";
				//Filter
				res.Filter = "File excel|*.xls;*.xlsx|Tutti i file|*.*";

				res.Multiselect = false;
				//When the user select the file
				if (res.ShowDialog() == DialogResult.OK)
				{
					Import(res.FileName);
				}
			}
		}

		private void Import(string fileName)
		{
			var dt = new DataTable();

			var query = "SELECT * FROM [Mag SANMAURO$]";
			using (OleDbConnection cn = new OleDbConnection { ConnectionString = ConnectionString(fileName, "No") })
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
				if (dt.Rows[0][2].ToString() == ""
					&& dt.Rows[0][3].ToString() == ""
					&& dt.Rows[0][4].ToString() == ""
					)
				{
					// remove header
					dt.Rows[0].Delete();
					dt.Rows[1].Delete();
				}
			}
			dt.AcceptChanges();

			var list = dt.AsEnumerable().Select(a => new
			{
				// assuming column 0's type is Nullable<long>
				Categoria = a.Field<string>(0),
				Marca = a.Field<string>(1),
				Modello = a.Field<string>(2),
				Note = a.Field<string>(3),
				DescrBreve = a.Field<string>(4),
				Colore = a.Field<string>(5),
				Scorta = a.Field<string>(6),
				PrezzoVendita = a.Field<string>(7),
				PrezzoAcq = a.Field<string>(8),
				Condizione = a.Field<string>(9),
			}).Where(a => a.Categoria != null && a.Categoria.Length > 0 && a.Marca != null && a.Marca.Length > 0).ToList();

			using (var uof = new UnitOfWork())
			{
				var listCategorie = uof.CategorieRepository.Find(a => true).ToList();
				foreach (var item in list.ToList())
				{
					var magItem = new Magazzino();
					var articolo = new Articolo();
					var categoriaSel = listCategorie.Where(a => a.Nome.ToUpper() == item.Categoria.ToUpper()).FirstOrDefault();

					articolo.Marca = item.Marca;
					articolo.CategoriaID = categoriaSel.ID;
					articolo.Prezzo = int.Parse(item.PrezzoVendita);
					articolo.Testo = (item.DescrBreve);
					articolo.Titolo = item.Marca + " " + item.Modello + " " + item.DescrBreve + " " + item.Colore;

					magItem.Articolo = articolo;

					//uof.ArticoliRepository.Find(a=>a.)
					uof.MagazzinoRepository.Add(magItem);
				}
			}
			Console.WriteLine();
		}

		public string ConnectionString(string FileName, string Header)
		{
			OleDbConnectionStringBuilder Builder = new OleDbConnectionStringBuilder();
			if (Path.GetExtension(FileName).ToUpper() == ".XLS")
			{
				Builder.Provider = "Microsoft.Jet.OLEDB.4.0";
				Builder.Add("Extended Properties", string.Format("Excel 8.0;IMEX=1;HDR={0};", Header));
			}
			else
			{
				Builder.Provider = "Microsoft.ACE.OLEDB.12.0";
				Builder.Add("Extended Properties", string.Format("Excel 12.0;IMEX=1;HDR={0};", Header));
			}

			Builder.DataSource = FileName;

			return Builder.ConnectionString;
		}
	}
}