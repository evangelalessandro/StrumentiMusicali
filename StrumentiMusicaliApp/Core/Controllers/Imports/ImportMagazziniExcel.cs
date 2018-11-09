using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Data;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.Imports
{
	public class ImportMagazziniExcel : BaseImportExcel
	{
		protected override void Import()
		{
			try
			{
				ProgressManager.Instance().Visible = true;

				LeggiMagazzino("Mag GARAGE");
				LeggiMagazzino("Mag NEGOZIO2");

				LeggiMagazzino("Mag SANMAURO");
				LeggiMagazzino("Mag.NEGOZIO1");


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

		private void LeggiMagazzino(string nomeDeposito)
		{
			DataTable dt = ReadDatatable(nomeDeposito);
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
				Categoria = a.Field<string>(1),
				Marca = a.Field<string>(2),
				Modello = a.Field<string>(3),
				Note = a.Field<string>(4),
				DescrBreve = a.Field<string>(5),
				Colore = a.Field<string>(6),
				Qta = (a.Field<string>(7)),
				PrezzoVendita = (a.Field<string>(8)),
				PrezzoAcq = (a.Field<string>(9)),
				Condizione = a.Field<string>(10),
			}).Where(a => a.Categoria != null && a.Categoria.Length > 0 && a.Marca != null && a.Marca.Length > 0).ToList();

			ProgressManager.Instance().Value = 0;
			ProgressManager.Instance().Max = list.Count;
			ProgressManager.Instance().Messaggio = nomeDeposito;

			using (var uof = new UnitOfWork())
			{
				var deposito = uof.DepositoRepository.Find(a => a.NomeDeposito == nomeDeposito).ToList().DefaultIfEmpty(
					new Deposito() { NomeDeposito = nomeDeposito }).FirstOrDefault();

				var listCategorie = uof.CategorieRepository.Find(a => true).ToList();
				foreach (var item in list.ToList())
				{
					try
					{


						var magItem = new Magazzino();
						var articolo = new Articolo();
						articolo.Marca = item.Marca;
						articolo.Prezzo = decimal.Parse(item.PrezzoVendita.Replace("€", "").Trim());
						articolo.Testo = (item.DescrBreve);
						articolo.Note1 = (item.Note);
						articolo.Colore = (item.Colore);
						articolo.Titolo = item.Marca + " " + item.Modello + " " + item.DescrBreve + " " + item.Colore;
						articolo.Titolo = articolo.Titolo.Trim();
						articolo.Testo = articolo.Titolo;
						articolo.TagImport = "ExcelMagazzini " + nomeDeposito;

						try
						{
							articolo.Condizione = (enCondizioneArticolo)Enum.Parse(typeof(enCondizioneArticolo), item.Condizione, true);
						}
						catch
						{
							if (item.Condizione.ToUpper().Contains("USATO"))
							{
								articolo.Condizione = enCondizioneArticolo.UsatoGarantito;
							}
							else if (item.Condizione.ToUpper().Contains("DEMO"))
							{
								articolo.Condizione = enCondizioneArticolo.ExDemo;
							}
							else
							{
								continue;
								throw new MessageException(
								string.Format(@"Non è specificata la condizione (Nuovo\usato\demo) per l'articolo {0}"
								, articolo.Titolo));
							}
						}
						var categoriaName = item.Categoria;

						if (categoriaName.Equals( "Panca",StringComparison.InvariantCultureIgnoreCase))
						{
							categoriaName = "Sgabelli e Panche";
						}
						if (categoriaName.StartsWith("sax", StringComparison.InvariantCultureIgnoreCase))
						{
							categoriaName = "Sax";
						}
						categoriaName = categoriaName.Replace(" ", "").ToUpper();


						var categoriaSel = listCategorie.Where(a => a.Nome.ToUpper().Replace(" ", "")
						== categoriaName).FirstOrDefault();

						if (categoriaSel == null)
						{
							categoriaName = categoriaName.Remove(categoriaName.Length - 1);

							categoriaSel = listCategorie.Where(a => a.Nome.ToUpper().Replace(" ", "").StartsWith(categoriaName))
								.FirstOrDefault();
						}
						if (categoriaSel == null)
						{
							articolo.Note2 = item.Categoria;
							categoriaSel = listCategorie.Where(a => a.Codice == -1000).First();
						}
						articolo.CategoriaID = categoriaSel.ID;


						magItem.Qta = int.Parse(item.Qta);
						magItem.Articolo = articolo;
						magItem.Deposito = deposito;
						if (!string.IsNullOrEmpty(item.PrezzoAcq) && item.PrezzoAcq.Trim().Length > 0)
							magItem.PrezzoAcquisto = decimal.Parse(item.PrezzoAcq);
						else
						{
							magItem.PrezzoAcquisto = 0;
						}
						magItem.Articolo = articolo;
						//uof.ArticoliRepository.Find(a=>a.)
						uof.MagazzinoRepository.Add(magItem);

					}
					catch (Exception ex)
					{

						throw;
					}
					ProgressManager.Instance().Value++;
				}
				uof.Commit();
			}
		}

	}
}