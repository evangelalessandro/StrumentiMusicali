using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
				var list = uof.MagazzinoRepository.Find(a => true).GroupBy(a =>
				new { Articolo = a.Articolo, Depo = a.Deposito.NomeDeposito })

				.Select(a => new
				{
					Quantita = a.Sum(b => b.Qta),
					a.Key.Articolo.Categoria,
					a.Key.Articolo,
					a.Key.Depo
				});
				foreach (var item in list.GroupBy(a => a.Depo).ToList())
				{
					var dt = ToDataTable(item.Select(a => new
					{
						Categoria = a.Articolo.Categoria.Nome,
						a.Articolo.Categoria.Reparto,
						a.Articolo.Titolo,
						a.Articolo.Condizione,
						a.Articolo.CodiceABarre,
						a.Articolo.Prezzo,
						a.Quantita
					}).ToList());
					_excel.AddWorksheet(dt, item.Key);

				}
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
