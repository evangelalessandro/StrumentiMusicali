using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
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
	public class ImportCavalloPazzoExcel : BaseImportExcel, IDisposable
	{
		public ImportCavalloPazzoExcel()
			:base()
		{
			
		}
		
		protected override void Import()
		{
			
			
			using (var uof = new UnitOfWork())
			{
				try
				{
					ProgressManager.Instance().Visible = true;


					 
					var listCategorie = uof.CategorieRepository.Find(a => true).ToList();
					var deposito = uof.DepositoRepository.Find(a => a.NomeDeposito == "Negozio").ToList().DefaultIfEmpty(
						new Deposito() { NomeDeposito = "Negozio" }).FirstOrDefault();

					ImportArticoli(uof, deposito, listCategorie);

					ImportStrumenti(uof, deposito, listCategorie);

					ImportLibri(uof, deposito, listCategorie);
					ProgressManager.Instance().Messaggio = "Salvataggio..";
					uof.Commit();

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

		}
		private void ImportLibri(UnitOfWork uof, Deposito deposito, System.Collections.Generic.List<Categoria> listCategorie)
		{
			DataTable dt = ReadDatatable(enNomeTabellaExcel.LIBRI.ToString());

			var list = dt.AsEnumerable().Select(a => new
			{
				// assuming column 0's type is Nullable<long>
				CodiceABarre = a.Field<string>(0),
				Titolo = a.Field<string>(1),
				Autore = a.Field<string>(2),
				Genere = a.Field<string>(3),
				Edizione = a.Field<string>(4),
				Quantita = int.Parse(a.Field<string>(5)),
				Edizione2 = a.Field<string>(6),
				Ordine = a.Field<string>(7),
				Varie1 = a.Field<string>(8),
				Settore = a.Field<string>(9),
				PrezzoVendita = decimal.Parse(a.Field<string>(10)),
				Categoria = a.Field<string>(12),
			}).ToList();



			ProgressManager.Instance().Messaggio = "Libri";
			ProgressManager.Instance().Value = 0;
			ProgressManager.Instance().Max = list.Count();

			foreach (var item in list.ToList())
			{
				var magItem = new Magazzino();
				var articolo = new Articolo();

				articolo.Prezzo = (item.PrezzoVendita);
				articolo.CodiceAbarre = item.CodiceABarre;


				var categoriaSel = listCategorie.Where(a => a.Nome.ToUpper() == item.Categoria.ToUpper()).FirstOrDefault();
				if (categoriaSel == null)
				{
					throw new MessageException(string.Format("Negli strumenti non è specificata una categoria corretta per l'articolo {0}", item.CodiceABarre));

				}
				articolo.CategoriaID = categoriaSel.ID;

				articolo.Titolo = (item.Autore + " " + item.Titolo + " " + item.Genere + " " + item.Edizione + " " + item.Edizione2).Trim().Replace("  ", " ");
				articolo.Libro = new Libro
				{
					Autore = item.Autore,
					Edizione = item.Edizione,
					Edizione2 = item.Edizione2,
					Genere = item.Genere,
					Ordine = item.Ordine,
					Settore = item.Settore
				};
				articolo.Note1 = item.Varie1;
				articolo.TagImport = "MulinoLibri";


				magItem.Qta = item.Quantita;
				magItem.Articolo = articolo;
				magItem.Deposito = deposito;
				magItem.PrezzoAcquisto = 0;

				uof.MagazzinoRepository.Add(magItem);
				ProgressManager.Instance().Value++;
			}

		}

		private void ImportStrumenti(UnitOfWork uof, Deposito deposito, System.Collections.Generic.List<Categoria> listCategorie)
		{
			DataTable dt = ReadDatatable(enNomeTabellaExcel.strum.ToString());
			try
			{


				var list = dt.AsEnumerable().Select(a => new
				{
					// assuming column 0's type is Nullable<long>
					CodiceABarre = a.Field<string>(0),
					Marca = a.Field<string>(1),
					Articolo = a.Field<string>(2),
					Modello = a.Field<string>(3),
					Varie1 = a.Field<string>(4),
					PrezzoAcq = a.Field<string>(5),
					PrezzoVendita = a.Field<string>(6),
					Rivenditore = a.Field<string>(7),
					Quantita = a.Field<string>(8),
					Categoria = a.Field<string>(10),
				}).ToList();




				ProgressManager.Instance().Messaggio = "Strumenti";
				ProgressManager.Instance().Value = 0;
				ProgressManager.Instance().Max = list.Count();


				foreach (var item in list.ToList())
				{
					var magItem = new Magazzino();
					var articolo = new Articolo();
					articolo.Marca = item.Marca;
					articolo.Prezzo = decimal.Parse(item.PrezzoVendita);
					articolo.CodiceAbarre = item.CodiceABarre;
					var categoriaSel = listCategorie.Where(a => a.Nome.ToUpper() == item.Categoria.ToUpper()).FirstOrDefault();
					if (categoriaSel == null)
					{
						throw new MessageException(string.Format("Negli strumenti non è specificata una categoria corretta per l'articolo {0}", item.CodiceABarre));

					}
					articolo.CategoriaID = categoriaSel.ID;

					articolo.Titolo = item.Marca + " " + item.Articolo + " " + item.Modello;

					articolo.Rivenditore = item.Rivenditore;
					articolo.Note1 = item.Varie1;
					articolo.TagImport = "MulinoStrumenti";
					magItem.Qta = int.Parse(item.Quantita);
					magItem.Articolo = articolo;
					magItem.Deposito = deposito;
					magItem.PrezzoAcquisto = decimal.Parse(item.PrezzoAcq);

					uof.MagazzinoRepository.Add(magItem);
					ProgressManager.Instance().Value++;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		private enum enNomeTabellaExcel
		{
			artic,
			LIBRI,
			strum
		}
		
		private void ImportArticoli(UnitOfWork uof, Deposito deposito, System.Collections.Generic.List<Categoria> listCategorie)
		{
			DataTable dt = ReadDatatable(enNomeTabellaExcel.artic.ToString());
			try
			{


				var list = dt.AsEnumerable().Select(a => new
				{
					// assuming column 0's type is Nullable<long>
					CodiceABarre = a.Field<string>(0),
					Marca = a.Field<string>(1),
					Articolo = a.Field<string>(2),
					Varie1 = a.Field<string>(3),
					Varie2 = a.Field<string>(4),
					Varie3 = a.Field<string>(5),
					PrezzoVendita = a.Field<string>(6),
					PrezzoAcq = a.Field<string>(7),
					Rivenditore = a.Field<string>(8),
					Quantita = a.Field<string>(9),
					Categoria = a.Field<string>(10),
				}).ToList();


				 

				ProgressManager.Instance().Messaggio = "Articoli";
				ProgressManager.Instance().Value = 0;
				ProgressManager.Instance().Max = list.Count();

				foreach (var item in list.ToList())
				{
					var magItem = new Magazzino();
					var articolo = new Articolo();

					articolo.Marca = item.Marca;

					articolo.Prezzo = decimal.Parse(item.PrezzoVendita);
					articolo.CodiceAbarre = item.CodiceABarre;
					var categoriaSel = listCategorie.Where(a => a.Nome.ToUpper() == item.Categoria.ToUpper()).FirstOrDefault();
					if (categoriaSel == null)
					{
						throw new MessageException(string.Format("Nei libri non è specificata una categoria corretta per l'articolo {0}", item.CodiceABarre));
					}
					articolo.CategoriaID = categoriaSel.ID;

					articolo.Titolo = item.Marca + " " + item.Articolo;

					articolo.Rivenditore = item.Rivenditore;
					articolo.Note1 = item.Varie1;
					articolo.Note2 = item.Varie2;
					articolo.Note3 = item.Varie3;

					articolo.TagImport = "MulinoArticoli";
					magItem.Qta = int.Parse(item.Quantita);
					magItem.Articolo = articolo;
					magItem.Deposito = deposito;
					magItem.PrezzoAcquisto = decimal.Parse(item.PrezzoAcq);

					//uof.ArticoliRepository.Find(a=>a.)
					uof.MagazzinoRepository.Add(magItem);
					ProgressManager.Instance().Value++;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}

		
	}
}
