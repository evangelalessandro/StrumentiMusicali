using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.Exports
{

	public class ExportMagazzino : IDisposable
	{
		private ClosedXML.Excel.XLWorkbook _excel;
		public ExportMagazzino()
		{

			_excel = new ClosedXML.Excel.XLWorkbook();
		}
		public void Stampa()
		{


			using (var uof = new UnitOfWork())
			{
				var listArt = uof.ArticoliRepository.Find(a => true).Select(a=>new { a.Libro, a.Categoria, articolo = a }).ToList().
					Select(a=>a.articolo).ToList();

				var dt = ToDataTable(listArt.Select(a =>
				new
				{
					a.ID,
					Categoria = a.Categoria.Nome,
					a.Categoria.Reparto,
					a.Titolo,
					Condizione = a.Condizione.ToString(),
					a.CodiceABarre,
					Prezzo = a.Prezzo.ToString("C2"),
					a.Colore,
					a.Marca,
					a.Note1,
					a.Note2,
					a.Note3,
					a.Rivenditore,
					a.Testo,
					a.Libro.Autore,
					a.Libro.Edizione,
					a.Libro.Edizione2,
					a.Libro.Genere,
					a.Libro.Ordine,
					a.Libro.Settore
				}

				).ToList());

				var qta =uof.MagazzinoRepository.Find(a => true)
					.Select(a => new { a.ArticoloID, a.Deposito, a.Qta })
					.GroupBy(a => new { a.ArticoloID, a.Deposito })
					.Select(a => new { a.Key, sumQta = a.Sum(b => b.Qta) })
					.ToList();

				foreach (var item in uof.DepositoRepository.Find(a => true).ToList())
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
				_excel.AddWorksheet(dt, "Generale");


			}

			var newfile = Path.Combine(System.IO.Path.GetTempPath(), DateTime.Now.Ticks.ToString() + "_Magazzino.xlsx");
			_excel.SaveAs(newfile);
			Process.Start(newfile);
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

		//private void ImpostaCampiTestata()
		//{
		//	_excel.Range("Note1").Value = fattura.Note1;
		//	_excel.Range("Note2").Value = fattura.Note2;
		//	_excel.Range("TrasportoACura").Value = fattura.TrasportoACura;
		//	_excel.Range("Vettore").Value = fattura.Vettore;

		//	_excel.Range("ClienteRagioneSociale").Value = fattura.Cliente.RagioneSociale;
		//	_excel.Range("ClienteIndirizzo").Value = fattura.Cliente.Indirizzo.IndirizzoConCivico;
		//	_excel.Range("ClienteCap").Value = fattura.Cliente.Indirizzo.Cap;
		//	_excel.Range("ClienteCitta").Value = fattura.Cliente.Indirizzo.Citta;
		//	_excel.Range("ClientePIVACF").Value = "CF - PIVA:" + fattura.Cliente.PIVA;
		//	_excel.Range("CodiceCliente").Value = fattura.Cliente.ID;

		//}

		public void Dispose()
		{
			if (_excel != null)
				_excel.Dispose();
			_excel = null;
		}
	}
}
