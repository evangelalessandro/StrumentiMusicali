
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System.Collections.Generic;
using System.Linq;
using WooCommerceNET.WooCommerce.v3;
using System.Net;

namespace StrumentiMusicali.WooCommerceSyncro.Sync
{
    internal class CategorySync : BaseClass.SyncroBaseWooCommerce
    {
        static bool Allineati = false;
        public void AllineaCategorieReparti()
        {
            if (Allineati)
                return;
            ManagerLog.Logger.Info("Avviato allineamento categorie reparti");
            var listCateg = AllineaReparti();
            AllineaCategorie(listCateg);
            ManagerLog.Logger.Info("Terminato allineamento categorie reparti");
            Allineati = true;
        }

        private void AllineaCategorie(List<ProductCategory> listCategories)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                long rootCateg = 0;// listCategories.Where(a => a.parent == 1).First().id.Value;

                //legge da db le categorie e i reparti
                foreach (var categDb in uof.CategorieRepository.Find(a => a.Codice > 0)
                    .Select(a => new { a.Reparto, NomeCateg = a.Nome, a.ID })
                    .OrderBy(a => a.Reparto).OrderBy(a => a.NomeCateg).ToList())
                {

                    var reparto = WebUtility.HtmlEncode(categDb.Reparto);
                    /*verifico che esista il reparto tra le categorie web */
                    var repartoWeb = listCategories.Where(a => a.name == reparto && a.parent == rootCateg).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb != null)
                    {
                        var categRepartoWebId = uof.RepartoWebRepository.Find(a => 1 == 1).Where(a => a.Reparto
                            == reparto).FirstOrDefault().CodiceWeb;
                        /*controllo che non esista già la categoria dentro al reparto nel ecomm*/
                        var existsCateg = listCategories.Where(a => a.parent == repartoWeb.id)
                            .Where(a => a.name == categDb.NomeCateg).FirstOrDefault();
                        if (existsCateg == null)
                        {
                            var newCateg = AggiungiCategoriaWeb((int)categRepartoWebId, categDb.NomeCateg);

                            uof.CategorieWebRepository.Add(new Library.Entity.Ecomm.CategoriaWeb()
                            { CategoriaID = categDb.ID, CodiceWeb = newCateg.id.Value });
                            uof.Commit();

                            ManagerLog.Logger.Info("Aggiunta categoria web: " + categDb.NomeCateg);
                        }
                        else
                        {
                            /*controlla che il codice web sia lo stesso*/
                            var categWebDb = uof.CategorieWebRepository.Find(a => a.CategoriaID == categDb.ID).FirstOrDefault();
                            if (categWebDb == null)
                            {
                                uof.CategorieWebRepository.Add(new Library.Entity.Ecomm.CategoriaWeb()
                                { CategoriaID = categDb.ID, CodiceWeb = existsCateg.id.Value });
                                uof.Commit();
                            }
                            else
                            {
                                if (categWebDb.CodiceWeb != existsCateg.id.Value)
                                {
                                    categWebDb.CodiceWeb = existsCateg.id.Value;
                                    uof.CategorieWebRepository.Update(categWebDb);
                                    uof.Commit();
                                }
                            }
                        }
                    }
                    else
                    {
                        var ex = new MessageException("Manca il reparto " + categDb.Reparto + "  nel ecommerce");
                        ManagerLog.Logger.Error(ex.Message, ex);
                    }
                }
            }
        }
        public const string Usato = "USATO";
        public const string ExDemo = "EX-DEMO";

        /// <summary>
        /// Aggiorna le categorie mettendo i reparti al livello superiore
        /// </summary>
        /// <returns>ritorna le categorie lette dal web</returns>
        private List<ProductCategory> AllineaReparti()
        {
            bool retVal = false;
            var listCategories = LeggiCategorieDalWeb();
            /*prima metto i reparti sotto alla root poi le categorie sotto i reparti*/
            long rootCateg = 0;// listCategories.Where(a => a.id == 1).First().id.Value;

            using (UnitOfWork uof = new UnitOfWork())
            {
                var reparti = uof.CategorieRepository.Find(a => a.Codice > 0).Select(a => a.Reparto)
                    .Distinct().OrderBy(a => a).ToList();
                reparti.Add(Usato);
                reparti.Add(ExDemo);

                foreach (var reparto in reparti.Select(a => WebUtility.HtmlEncode(a)).ToList())
                {

                    /*verifico che il reparto sia presente sotto la root*/
                    var repartoWeb = listCategories.Where(a => a.name == reparto).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb == null)
                    {
                        retVal = true;
                        //se non si trova va aggiunto
                        AggiungiRepartoWeb((int)rootCateg, reparto);
                    }
                    else
                    {
                        //aggiorno o salvo l'info a db
                        var repartoDbWeb = uof.RepartoWebRepository.Find(a => a.Reparto == reparto).FirstOrDefault();
                        if (repartoDbWeb == null)
                        {
                            repartoDbWeb = new Library.Entity.Ecomm.RepartoWeb() { Reparto = reparto, CodiceWeb = repartoWeb.id.Value };
                            uof.RepartoWebRepository.Add(repartoDbWeb);
                            uof.Commit();
                        }
                        else if (repartoDbWeb.CodiceWeb != repartoWeb.id.Value)
                        {
                            repartoDbWeb.CodiceWeb = repartoWeb.id.Value;
                            uof.RepartoWebRepository.Update(repartoDbWeb);
                            uof.Commit();
                        }
                        else
                        {
                            ManagerLog.Logger.Info("Reparto presente e corretto : " + reparto);
                        }
                    }
                };
            }
            if (retVal)
            {
                listCategories = LeggiCategorieDalWeb();
            }
            return listCategories;
        }

        public List<ProductCategory> LeggiCategorieDalWeb()
        {
            ManagerLog.Logger.Info("Avvio lettura categorie dal web");

            List<ProductCategory> listCat = new List<ProductCategory>();



            foreach (var item in Enumerable.Range(1,5))
            {
                var parms = new Dictionary<string, string>();
                parms.Add("per_page", "100");
                parms.Add("page", item.ToString());
                var cat = _wc.Category.GetAll(parms);
                cat.Wait();
                listCat.AddRange( cat.Result);


            }
            return listCat;


        }

        private void AggiungiRepartoWeb(int rootCateg, string reparto)
        {
            var newCateg = AggiungiCategoriaWeb(rootCateg, reparto);

            using (var uof = new UnitOfWork())
            {
                uof.RepartoWebRepository.Add(new Library.Entity.Ecomm.RepartoWeb() { Reparto = reparto, CodiceWeb = newCateg.id.Value });
                uof.Commit();
            }
        }

        private ProductCategory AggiungiCategoriaWeb(int rootCateg, string nomeCateg)
        {
            var newCateg = (new ProductCategory() { description = nomeCateg, name = nomeCateg, parent = rootCateg });
            //newCateg.image.src

            return _wc.Category.Add(newCateg).Result;



        }
    }
}