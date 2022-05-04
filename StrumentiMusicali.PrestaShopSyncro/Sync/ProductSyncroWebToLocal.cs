using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.EcommerceBaseSyncro;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.PrestaShopSyncro.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro.Sync
{
	public class ProductSyncroWebToLocal : BaseClass.SyncroBasePresta
	{

		StockProducts _stockProd;

		List<tax_rule_group> _taxRulgr;

		public ProductSyncroWebToLocal()
			: base()
		{
			_stockProd = new StockProducts(this);

			_taxRulgr = _taxRuleGroupFact.GetAll();

		}

		/// <summary>
		/// Salva quelli che non sono presenti in locale
		/// </summary>
		/// <returns>ritorna quanti sono stati inseriti</returns>
		public int SaveLocalFromSite()
		{
			int count = 0;
			using (var uof = new UnitOfWork())
			{
				using (var categ = new CategorySync())
				{
					var listCategories = categ.LeggiCategorieDalWeb();

					//Dictionary<string, string> filter = new Dictionary<string, string>();
					//filter.Add("Name", "EKO RANGER");
					var listProd = new List<product>();



					var codiciGiaPresenti = uof.AggiornamentoWebArticoloRepository.Find(a => a.Articolo.ArticoloWeb.CodiceArticoloWeb > 0).Select(a => a.Articolo.ArticoloWeb.CodiceArticoloWeb).ToList();
					var filterRef = new Dictionary<string, string>();
					//filterRef.Add("reference", "[MB 511]");
					filterRef = null;
					//filterRef.Add("id_tax_rules_group", "[2,100]");
					//--id_tax_rules_group
					var display = new List<string>();
					display.Add("reference");
					display.Add("id");
					display.Add("id_tax_rules_group");
					display.Add("name");

					//var taxList = _productFactory.GetByFilter(filterRef, null, "50", display).ToList();

					var listIdProdWebPrel = _productFactory.GetByFilter(filterRef, null, "10000", display).ToList();

					var listIdProdWeb = listIdProdWebPrel.Where(a => a.reference.Length > 0).Select(a => a.id.Value).ToList();
					//prendo quelli che non sono già salvati
					foreach (var item in listIdProdWeb.Where(a => !codiciGiaPresenti.Contains(a)))
					{
						var prod = _productFactory.Get((item));
						listProd.Add(prod);
						Console.WriteLine("Prodotto :" + prod.name.First().Value + " Riferimento: " + prod.reference);
					}
					foreach (var item in listProd)
					{
						var codice = item.id.Value;

						var name = item.name.First().Value;
						var articolo = uof.AggiornamentoWebArticoloRepository.Find(a => a.Articolo.ArticoloWeb.CodiceArticoloWeb == codice).Select(a => a.Articolo).FirstOrDefault();
						if (articolo == null)
						{
							articolo = uof.ArticoliRepository.Find(a => a.Strumento.CodiceOrdine == item.reference).Select(a => a).FirstOrDefault();
							if (articolo == null)
							{
								articolo = uof.ArticoliRepository.Find(a => a.Libro.Ordine == item.reference).Select(a => a).FirstOrDefault();
							}
							//se esiste l'articolo per codice ordine allora esiste in tutti e due i sistemi e devo riportare in gestionale i dati che mancano 
							if (articolo != null)
							{
								if (articolo.ArticoloWeb.CodiceArticoloWeb > 0)
									continue;
								Console.WriteLine("avvio salvataggio per codice articolo web:" + codice);
								SaveProduct(uof, item, name, listCategories, articolo);
								Console.WriteLine("Salvato nuovo articolo dal web:" + name);
								continue;
							}
							else
							{
								continue;
							}
						}
						//if (articolo == null)
						//{
						//	try
						//	{

						//		Console.WriteLine("avvio salvataggio per codice articolo web:" + codice);
						//		SaveProduct(uof, item, name, listCategories);
						//		Console.WriteLine("Salvato nuovo articolo dal web:" + name);
						//		count++;
						//	}
						//	catch (Exception ex)
						//	{
						//		Console.WriteLine("Errore:" + ex.Message);

						//		Console.WriteLine("Vuoi continuare ? Y/N");
						//		if (Console.ReadLine().ToUpperInvariant() != "Y")
						//		{
						//			return count;
						//		}
						//	}
						//}
						//else
						//{
						//	Console.WriteLine("Articolo già presente per codice:" + name);
						//}
					}
				}
			}
			return count;
		}

		#region Immagini

		private string byteArrayToImage(byte[] bytesArr)
		{
			using (MemoryStream memstr = new MemoryStream(bytesArr))
			{
				var img = new ImageMagick.MagickImage(bytesArr);
				var atempFile = System.IO.Path.GetTempFileName() + ".jpg";
				img.Write(atempFile);
				return atempFile;
			}
		}

		private List<string> ReadImmage(product item)
		{
			List<string> immagini = new List<string>();
			long idDefaultImage = -1;
			if (item.id_default_image.HasValue)
			{
				idDefaultImage = item.id_default_image.Value;
				var image = byteArrayToImage(_imageFactory.GetProductImage(item.id.Value, idDefaultImage)
					);
				immagini.Add(image);
				//fotoArticolo.UrlFoto

				//image.Save()
			}
			foreach (var itemImage in item.associations.images.Where(a => a.id != idDefaultImage))
			{
				var image = byteArrayToImage(_imageFactory.GetProductImage(item.id.Value, itemImage.id)
					);
				immagini.Add(image);
			}
			return immagini;
		}

		#endregion Immagini

		private void SaveProduct(UnitOfWork uof, product item, string name, List<category> listCategories, Articolo articoloToJoin = null)
		{
			Articolo articolo = articoloToJoin;

			var tax = _taxRulgr.Where(a => a.id == item.id_tax_rules_group).First().name;

			var iva = 0;
			if (tax.Contains("4"))
			{
				iva = 4;
			}
			else if (tax.Contains("22"))
			{
				iva = 22;
			}
			else if (tax.Contains("10"))
			{
				iva = 10;
			}
			var prezzo = Math.Round(item.price * (100 + iva) / 100);
			if (articolo.ArticoloWeb.PrezzoWeb != prezzo)
			{
				Console.WriteLine("Aggiornato prezzo web in gestionale: da " + articolo.ArticoloWeb.PrezzoWeb.ToString() + " a " + prezzo);
				articolo.ArticoloWeb.PrezzoWeb = prezzo;
			}

			//	/*categoria non specificato*/
			//	articolo.CategoriaID = 239;

			var categEcommerceList = listCategories.Where(a => item.associations.categories.Select(b => b.id).Contains(a.id.Value)).ToList()
				.Select(a=>a.name.First().Value).ToList();
			var findCat = false;
			foreach (var categEcommerce in categEcommerceList)
			{
				var categName = categEcommerce;
				var categDb = uof.CategorieRepository.Find(a => a.Nome
				== categName).FirstOrDefault();

				if (categDb != null)
				{
					findCat = true;
					articolo.CategoriaID = categDb.ID;
				}
			}
			if (!findCat)
			{

			}

			var immagini = ReadImmage(item);

			articolo.Titolo = item.name.FirstOrDefault().Value;

			articolo.ArticoloWeb.DescrizioneHtml = item.description.FirstOrDefault().Value;
			articolo.ArticoloWeb.DescrizioneBreveHtml = item.description_short.FirstOrDefault().Value;


			articolo.ArticoloWeb.CodiceArticoloWeb = item.id.Value;
			uof.ArticoliRepository.Update(articolo);



			uof.Commit();

			var agg = SalvaDatiDiImport(uof, item, articolo);

			_stockProd.UpdateStockArt(item, new ArticoloBase()
			{
				ArticoloID = agg.ArticoloID,
				Aggiornamento = agg,
				CodiceArticoloEcommerce = agg.Articolo.ArticoloWeb.CodiceArticoloWeb,
				ArticoloDb = agg.Articolo
			}, uof, true);

			//if (articoloToJoin == null)
			//{
			//	var depoPrinc = uof.DepositoRepository.Find(a => a.Principale == true).First();

			//	uof.MagazzinoRepository.Add(new Library.Entity.Magazzino()
			//	{ ArticoloID = articolo.ID, DepositoID = depoPrinc.ID, Qta = 1, PrezzoAcquisto = 0 });

			//	uof.Commit();
			//}

			if (immagini.Count() > 0 && !ImageArticoloSave.AddImageFiles(
				new ImageArticoloAddFiles(articolo, immagini, new ControllerFake())))
				throw new MessageException("Non si sono salvati correttamente le immagini degli articoli");



			//try
			//{
			//	item.reference = articolo.ID.ToString();
			//	_productFactory.Update(item);

			//}
			//catch
			//{
			//	try
			//	{
			//		item.position_in_category = 0;
			//		_productFactory.Update(item);

			//	}
			//	catch
			//	{

			//	}

			//}

		}

		private Library.Entity.Ecomm.AggiornamentoWebArticolo SalvaDatiDiImport(UnitOfWork uof, product item, Articolo articolo)
		{
			/*salva dati di import */
			var artImp = new ArticoloImportato() { CodiceArticoloEcommerce = item.id.Value.ToString(), XmlDatiProdotto = item.ToString() };

			foreach (var itemImage in item.associations.images)
			{
				var image = _imageFactory.GetProductImage(item.id.Value, itemImage.id);

				if (artImp.Immagine1 == null)
				{
					artImp.Immagine1 = image;
				}
				else if (artImp.Immagine2 == null)
				{
					artImp.Immagine2 = image;
				}
				else if (artImp.Immagine3 == null)
				{
					artImp.Immagine3 = image;
				}
			}
			artImp.XmlDatiProdotto = StrumentiMusicali.Core.Manager.ManagerLog.SerializeXmlObject(item);
			uof.ArticoloImportatoWebRepository.Add(artImp);

			var agg = uof.AggiornamentoWebArticoloRepository.Find(a => articolo.ID == a.ArticoloID).First();

			agg.Link = item.link_rewrite.First().Value.ToString();

			uof.AggiornamentoWebArticoloRepository.Update(agg);
			uof.Commit();
			return agg;
		}

		private class ControllerFake : IKeyController
		{
			public Guid INSTANCE_KEY => Guid.NewGuid();
		}
	}
}