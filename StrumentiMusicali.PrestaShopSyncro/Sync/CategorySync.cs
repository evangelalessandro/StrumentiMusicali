using Bukimedia.PrestaSharp.Entities;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.PrestaShopSyncro
{
    public class CategorySync : BaseClass.SyncroBase
    {
        static bool Allineati = false;
        public void AllineaCategorieReparti()
        {
            if (Allineati)
                return;
            ManagerLog.Logger.Info("Avviato allineamento categorie reparti");
            List<category> listCateg = AllineaReparti();
            AllineaCategorie(listCateg);
            ManagerLog.Logger.Info("Terminato allineamento categorie reparti");
            Allineati = true;
        }

        private void AllineaCategorie(List<category> listCategories)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                long rootCateg = listCategories.Where(a => a.is_root_category == 1).First().id.Value;

                //legge da db le categorie e i reparti
                foreach (var categDb in uof.CategorieRepository.Find(a => a.Codice > 0)
                    .Select(a => new { a.Reparto, NomeCateg = a.Nome, a.ID })
                    .OrderBy(a => a.Reparto).OrderBy(a => a.NomeCateg).ToList())
                {
                    var categRepartoWebId = uof.RepartoWebRepository.Find(a => 1 == 1).Where(a => a.Reparto
                    == categDb.Reparto).FirstOrDefault().CodiceWeb;

                    /*verifico che esista il reparto tra le categorie web */
                    var repartoWeb = listCategories.Where(a => a.name.First().Value == categDb.Reparto && a.id_parent == rootCateg).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb != null)
                    {
                        /*controllo che non esista già la categoria dentro al reparto nel ecomm*/
                        var existsCateg = listCategories.Where(a => a.id_parent == repartoWeb.id)
                            .Where(a => a.name.First().Value == categDb.NomeCateg).FirstOrDefault();
                        if (existsCateg == null)
                        {
                            var newCateg = AggiungiCategoriaWeb(categRepartoWebId, categDb.NomeCateg);

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
                                categWebDb.CodiceWeb = existsCateg.id.Value;
                                uof.CategorieWebRepository.Update(categWebDb);
                                uof.Commit();
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
        private List<category> AllineaReparti()
        {
            bool retVal = false;
            List<category> listCategories = LeggiCategorieDalWeb();
            /*prima metto i reparti sotto alla root poi le categorie sotto i reparti*/
            long rootCateg = listCategories.Where(a => a.is_root_category == 1).First().id.Value;

            using (UnitOfWork uof = new UnitOfWork())
            {
                var reparti = uof.CategorieRepository.Find(a => a.Codice > 0).Select(a => a.Reparto)
                    .Distinct().OrderBy(a => a).ToList();
                reparti.Add(Usato);
                reparti.Add(ExDemo);

                foreach (var reparto in reparti)
                {
                    /*verifico che il reparto sia presente sotto la root*/
                    var repartoWeb = listCategories.Where(a => a.name.First().Value == reparto).DefaultIfEmpty(null).FirstOrDefault();

                    if (repartoWeb == null)
                    {
                        retVal = true;
                        //se non si trova va aggiunto
                        AggiungiRepartoWeb(rootCateg, reparto);
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

        public List<category> LeggiCategorieDalWeb()
        {
            ManagerLog.Logger.Info("Avvio lettura categorie dal web");
            var listCategories = new List<category>();

            foreach (var item in _categoriesFact.GetAll())
            {
                listCategories.Add((item));
                //Core.Manager.ManagerLog.AddLogObject(item, "categoria:");
            }
            //Core.Manager.ManagerLog.AddLogObject(listCategories.Select(a =>
            //new { a.id, a.id_parent, a.is_root_category, Nome = a.name.First().Value }).ToList(), "categorie:");
            //Core.Manager.ManagerLog.AddLogMessage("Termine lettura categorie dal web");

            return listCategories;
        }

        private void AggiungiRepartoWeb(long rootCateg, string reparto)
        {
            category newCateg = AggiungiCategoriaWeb(rootCateg, reparto);

            using (var uof = new UnitOfWork())
            {
                uof.RepartoWebRepository.Add(new Library.Entity.Ecomm.RepartoWeb() { Reparto = reparto, CodiceWeb = newCateg.id.Value });
                uof.Commit();
            }
        }

        private category AggiungiCategoriaWeb(long rootCateg, string nomeCateg)
        {
            category newCateg = (new category() { active = 1, id_parent = rootCateg });
            newCateg.AddName(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, nomeCateg));
            newCateg.AddLinkRewrite(new Bukimedia.PrestaSharp.Entities.AuxEntities.language(1, nomeCateg));

            newCateg = _categoriesFact.Add(newCateg);

            //Dictionary<string, string> dtn = new Dictionary<string, string>();
            //dtn.Add("name", nomeCateg);
            //var listFind = _categoriesFact.GetByFilter(dtn, null, null);
            //listFind = listFind.Where(a => a.id_parent == rootCateg).ToList();
            //if (listFind.Count() == 1)
            //{
            //    newCateg = listFind.First();
            //}
            //else
            //{
            //    ManagerLog.AddLogMessage("C'è un problema con la categoria'" + nomeCateg + "' nel web");
            //}

            return newCateg;
        }
    }
}