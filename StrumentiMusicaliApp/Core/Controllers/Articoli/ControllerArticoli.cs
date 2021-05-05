using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Scontrino;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Forms;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Settings;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerArticoli : BaseControllerGeneric<Articolo,
        ArticoloItem>, IDisposable
    {
        private Subscription<Add<Articolo>> sub1;
        private Subscription<Edit<Articolo>> sub2;
        private Subscription<Remove<Articolo>> sub3;
        private Subscription<ArticoloDuplicate> sub4;
        private Subscription<ImageArticoloAdd> sub5;
        private Subscription<Save<Articolo>> sub8;
        private Subscription<ArticoloSconta> sub6;
        internal enModalitaArticolo ModalitaController { get; private set; }
        internal ControllerImmagini _controllerImmagini = new ControllerImmagini();
        private Subscription<ImageSelected<FotoArticolo>> sub10;
        private Subscription<ArticoloMerge> sub11;

        public enum enModalitaArticolo
        {
            Tutto = 0,
            Ricerca = 1,
            SoloStrumenti = 2,
            SoloLibri = 3,
            SelezioneSingola = 4
        }

        public ControllerArticoli(enModalitaArticolo modalitaController)
                    : base(enAmbiente.StrumentiList, enAmbiente.StrumentiDetail)
        {
            ModalitaController = modalitaController;
            sub1 = EventAggregator.Instance().Subscribe<Add<Articolo>>(AggiungiArticolo);
            sub2 = EventAggregator.Instance().Subscribe<Edit<Articolo>>(EditArt);

            sub3 = EventAggregator.Instance().Subscribe<Remove<Articolo>>(DeleteArticolo);
            sub4 = EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);
            sub5 = EventAggregator.Instance().Subscribe<ImageArticoloAdd>(AggiungiImmagine);
            sub8 = EventAggregator.Instance().Subscribe<Save<Articolo>>(SaveArticolo);
            sub6 = EventAggregator.Instance().Subscribe<ArticoloSconta>(Sconta);

            sub10 = EventAggregator.Instance().Subscribe<ImageSelected<FotoArticolo>>(ImmagineSelezionata);

            sub11 = EventAggregator.Instance().Subscribe<ArticoloMerge>(MergeArticolo);

            AggiungiComandiMenu();

            SetKeyImageListUI();
        }

        private void MergeArticolo(ArticoloMerge obj)
        {
            MessageManager.NotificaInfo("Seleziona l'articolo da cui copiare i dati e chiudi la form.");
            var art = SelezionaSingoloArticolo();
            if (art != null && art.ID > 0)
            {
                if (this.SelectedItem.ID != art.ID)
                {
                    if (MessageManager.QuestionMessage(
                        "Sei sicuro di unire l'articolo codice "
                        + this.SelectedItem.ID.ToString() + " con l'articolo codice " + art.ID.ToString()))
                    {
                        MergeArticoli(art);
                    }
                }
            }
        }
        public Articolo SelezionaSingoloArticolo(string ricercaText = "")
        {
            using (var artController = new ControllerArticoli(enModalitaArticolo.SelezioneSingola))
            {
                artController.TestoRicerca = ricercaText;
                var viewRicercaArt = new View.Articoli.ArticoliListView(artController);
                

                this.ShowView(viewRicercaArt, artController.Ambiente, artController, true, true);

                return artController.ArticoloSelezionatoSingolo;

            }
        }
        /// <summary>
        /// Unisce gli articoli in quello selezionato nella prima form
        /// </summary>
        /// <param name="artToMerge"></param>
        private void MergeArticoli(Articolo artToMerge)
        {
            using (var save = new SaveEntityManager(true))
            {
                var elem = save.UnitOfWork.ArticoliRepository.Find(a => a.ID == this.SelectedItem.ID
                      || a.ID == artToMerge.ID).ToList();

                var baseItem = elem.Where(a => a.ID == this.SelectedItem.ID).First();
                elem.Remove(baseItem);
                var secondItem = elem.First();


                var list = UtilityProp.GetProperties(baseItem);


                /*escludo le proprieta del oggetto entita base
                 */
                MergeObjects(baseItem, secondItem);
                if (baseItem.Condizione == enCondizioneArticolo.NonSpecificato)
                {
                    baseItem.Condizione = secondItem.Condizione;
                }
                save.UnitOfWork.ArticoliRepository.Update(baseItem);

                var fotoArt2 = save.UnitOfWork.FotoArticoloRepository.Find(a => a.ArticoloID == secondItem.ID).ToList();
                if (fotoArt2.Count() > 0)
                {
                    MessageManager.NotificaInfo("Sposto le eventuali foto da un articolo all'altro");
                    foreach (var item in fotoArt2)
                    {
                        /*sostituisco l'articolo*/
                        item.ArticoloID = baseItem.ID;
                        save.UnitOfWork.FotoArticoloRepository.Update(item);
                    }

                }
                else
                {
                    MessageManager.NotificaInfo("Non ci sono foto");
                }
                var aggList = save.UnitOfWork.AggiornamentoWebArticoloRepository.Find(a => a.ArticoloID == baseItem.ID ||
                  a.ArticoloID == secondItem.ID).ToList();

                var baseAgg = aggList.Where(a => a.ArticoloID == baseItem.ID).First();
                var secondAgg = aggList.Where(a => a.ArticoloID != baseItem.ID).First();

                if (string.IsNullOrEmpty(baseAgg.CodiceArticoloEcommerce))
                {
                    /*scambio l'eventuale codice dell'articolo nell'ecommerce*/
                    baseAgg.CodiceArticoloEcommerce = secondAgg.CodiceArticoloEcommerce;
                    secondAgg.CodiceArticoloEcommerce = "";

                    save.UnitOfWork.AggiornamentoWebArticoloRepository.Update(baseAgg);
                    save.UnitOfWork.AggiornamentoWebArticoloRepository.Update(secondAgg);
                }

                save.SaveEntity(enSaveOperation.Unione);
                MessageManager.NotificaInfo("Le eventuali giacenze vanno spostate manualmente");
            }
        }

        private static void MergeObjects(object obj1Dest, object obj2)
        {
            var listPropBase = UtilityProp.GetProperties(new StrumentiMusicali.Library.Entity.Base.BaseEntity());

            var objToEnumProp = obj1Dest;
            if (objToEnumProp == null)
            {
                objToEnumProp = obj2;
            }
            var allProperties = UtilityProp.GetProperties(objToEnumProp).ToList().Where(x =>
            !listPropBase.Contains(x) &&
            x.CanRead && x.CanWrite && !x.Name.StartsWith("Show")
            && !x.Name.StartsWith("Categ")
            );
            foreach (var pi in allProperties)
            {
                object defaultValue;

                if (pi.PropertyType == typeof(string))
                {
                    defaultValue = pi.GetValue(obj1Dest, null);
                }
                else if (pi.PropertyType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(pi.PropertyType);
                }
                else
                {
                    defaultValue = null;
                    if (pi.PropertyType.FullName.StartsWith("Strument"))
                    {
                        MergeObjects(pi.GetValue(obj1Dest, null), pi.GetValue(obj2, null));
                        continue;
                    }
                    else
                    {

                    }

                }

                var value = pi.GetValue(obj1Dest, null);

                if (value != defaultValue)
                {
                    // pi.SetValue(objResult, value, null);
                }
                else
                {
                    value = pi.GetValue(obj2, null);

                    if (value != defaultValue)
                    {
                        pi.SetValue(obj1Dest, value, null);
                    }
                }
            }
        }
        private FotoArticolo _fileFotoSelezionato = null;

        private void ImmagineSelezionata(ImageSelected<FotoArticolo> obj)
        {
            if (obj.File != null)
                _fileFotoSelezionato = obj.File.Entity;
            else
                _fileFotoSelezionato = null;
        }

        private Subscription<ImageAddFiles> _subAddImage;
        private Subscription<ImageOrderSet<FotoArticolo>> _orderImage;
        private Subscription<ImageRemove<FotoArticolo>> _imageRemove;

        private void SetKeyImageListUI()
        {
            _subAddImage = EventAggregator.Instance()
                .Subscribe<ImageAddFiles>
                (AddImageFiles);

            _orderImage = EventAggregator.Instance()
                .Subscribe<ImageOrderSet<FotoArticolo>>(
                ImageSetOrder);

            _imageRemove = EventAggregator.Instance()
                .Subscribe<ImageRemove<FotoArticolo>>(
                ImageRemoveSet
                );
        }

        private void ImageRemoveSet(ImageRemove<FotoArticolo> obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {
                EventAggregator.Instance().Publish<ImageArticoloRemove>(
                    new ImageArticoloRemove(
                    FotoSelezionata(), this._controllerImmagini));
            }
        }

        private void ImageSetOrder(ImageOrderSet<FotoArticolo> obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {
                EventAggregator.Instance().Publish<ImageArticoloOrderSet>(
                    new ImageArticoloOrderSet(obj.TipoOperazione,
                    FotoSelezionata(), this._controllerImmagini));
            }
        }

        private List<ImmaginiFile<FotoArticolo>> _listFotoArticolo
            = new List<ImmaginiFile<FotoArticolo>>();

        public FotoArticolo FotoSelezionata()
        {
            return FotoSelezionata(_fileFotoSelezionato);
        }

        private FotoArticolo FotoSelezionata(FotoArticolo file)
        {
            if (file == null)
                return null;
            return _listFotoArticolo.Where(a => a.Entity == file).Select(a => a.Entity).DefaultIfEmpty(null).FirstOrDefault();
        }

        internal List<ImmaginiFile<FotoArticolo>> RefreshImageListData()
        {
            using (var uof = new UnitOfWork())
            {
                var settingSito = SettingSitoValidator.ReadSetting();

                var imageList = uof.FotoArticoloRepository.Find(a => a.Articolo.ID == this.EditItem.ID)
                    .OrderBy(a => a.Ordine).ToList();

                _listFotoArticolo = imageList.Select(a => new ImmaginiFile<FotoArticolo>(Path.Combine(
                    settingSito.CartellaLocaleImmagini, a.UrlFoto)
                                 , a.UrlFoto, a)).ToList();

                return _listFotoArticolo.ToList();
            }
        }

        private void AddImageFiles(ImageAddFiles obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {
                EventAggregator.Instance().Publish<ImageArticoloAddFiles>(
                  new ImageArticoloAddFiles(this.EditItem,
                  obj.Files, this._controllerImmagini));
            }
        }

        private void Sconta(ArticoloSconta obj)
        {
            if (!MessageManager.QuestionMessage(string.Format(@"Sei sicuro di volere applicare questo sconto su tutti i {0} articoli visualizzati nella lista?", DataSource.Count())))
                return;
            if (DataSource.Count() > 100)
            {
                if (!MessageManager.QuestionMessage(string.Format(@"Sei veramente sicuro di volere applicare questo sconto su tutti i {0} articoli  visualizzati nella lista?", DataSource.Count())))
                    return;
            }
            _logger.Info("Applicato sconto su marca {0} e filtro ricerca {1} di {2} %.  ", FiltroMarca, this.TestoRicerca, obj.Percentuale);
            using (var saveEnt = new SaveEntityManager())
            {
                var list = DataSource.Select(a => a.ID).ToList();
                foreach (var item in saveEnt.UnitOfWork.ArticoliRepository.Find(a => list.Contains(a.ID)).ToList())
                {
                    item.Prezzo += item.Prezzo * obj.Percentuale / (decimal)100;
                    _logger.Info("Applicato sconto su articolo codice {0}, nuovo prezzo ", item.ID, item.Prezzo.ToString("C2"));
                    saveEnt.UnitOfWork.ArticoliRepository.Update(item);
                }
                if (saveEnt.SaveEntity("Variazione applicata con successo"))
                {
                    EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            EventAggregator.Instance().UnSbscribe(sub11);
            EventAggregator.Instance().UnSbscribe(sub10);
            EventAggregator.Instance().UnSbscribe(sub1);
            EventAggregator.Instance().UnSbscribe(sub2);
            EventAggregator.Instance().UnSbscribe(sub3);
            EventAggregator.Instance().UnSbscribe(sub4);
            EventAggregator.Instance().UnSbscribe(sub5);
            EventAggregator.Instance().UnSbscribe(sub6);
            EventAggregator.Instance().UnSbscribe(sub8);

            EventAggregator.Instance().UnSbscribe(_orderImage);
            EventAggregator.Instance().UnSbscribe(_subAddImage);

            _controllerImmagini.Dispose();
            _controllerImmagini = null;
        }

        private void AggiungiComandiMenu()
        {
            var tabFirst = GetMenu().Tabs[0];
            var pnl = tabFirst.Pannelli.First();

            
            if (ModalitaController == enModalitaArticolo.Ricerca
                || ModalitaController == enModalitaArticolo.SelezioneSingola)
            {
                pnl.Pulsanti.RemoveAll(a => a.Tag == MenuTab.TagAdd
                || a.Tag == MenuTab.TagRemove || a.Tag == MenuTab.TagCerca
                || a.Tag == MenuTab.TagCercaClear);


                tabFirst.Pannelli.RemoveAt(2);
                tabFirst.Pannelli.RemoveAt(1);
            }
            if (ModalitaController == enModalitaArticolo.SelezioneSingola)
            {
                pnl.Pulsanti.RemoveAll(a => a.Tag == MenuTab.TagEdit);
            }
            if (ModalitaController != enModalitaArticolo.Ricerca
                && ModalitaController != enModalitaArticolo.SelezioneSingola)
            {
                var rib1 = pnl.Add("Duplica", StrumentiMusicali.Core.Properties.ImageIcons.Duplicate, true);
                rib1.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ArticoloDuplicate>(new ArticoloDuplicate(this));
                };
                var pnl2 = GetMenu().Tabs[0].Add("Prezzi");
                var rib2 = pnl2.Add("Varia prezzi", StrumentiMusicali.Core.Properties.ImageIcons.Sconta_Articoli);
                rib2.Click += (a, e) =>
                {
                    ScontaArticoliShowView();
                };
                var rib3 = pnl.Add("Unisci", StrumentiMusicali.Core.Properties.ImageIcons.Merge_64, true);
                rib3.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ArticoloMerge>(new ArticoloMerge(this));
                };
            }
            if (ModalitaController == enModalitaArticolo.Ricerca
                || ModalitaController == enModalitaArticolo.SoloLibri
                || ModalitaController == enModalitaArticolo.SoloStrumenti
                || ModalitaController == enModalitaArticolo.Tutto)
            {
                var pnlS = GetMenu().Tabs[0].Add("Scontrino");
                pnlS.Add("Aggiungi a scontrino", StrumentiMusicali.Core.Properties.ImageIcons.Add, true).Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ScontrinoAddEvents>(new ScontrinoAddEvents()
                    {
                        Articolo = this.SelectedItem
                    }) ;
                };
                pnlS.Add("Aggiungi generico", StrumentiMusicali.Core.Properties.ImageIcons.Add, true).Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ScontrinoAddEvents>(new ScontrinoAddEvents()
                    {
                        
                    });
                };
                //pnlS.Add("Aggiungi sconto", StrumentiMusicali.Core.Properties.ImageIcons.Sconto_64, true).Click += (a, e) =>
                //{
                //    EventAggregator.Instance().Publish<ScontrinoAddScontoEvents>(new ScontrinoAddScontoEvents()
                //    {

                //    });
                //};
                pnlS.Add("Rimuovi riga", StrumentiMusicali.Core.Properties.ImageIcons.CancellaRiga_scontrino, true).Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ScontrinoRemoveLineEvents>(new ScontrinoRemoveLineEvents()
                    {

                    });
                };
                pnlS.Add("Stampa", StrumentiMusicali.Core.Properties.ImageIcons.PrintScontrino_48, true).Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ScontrinoStampa>(new ScontrinoStampa());
                };
                //pnlS.Add("Elimina tutto", StrumentiMusicali.Core.Properties.ImageIcons.Cancella_scontrino_64, true).Click += (a, e) =>
                //{
                //    EventAggregator.Instance().Publish<ScontrinoClear>(new ScontrinoClear());
                //};

            }
            if (LoginData.utenteLogin.Magazzino && ModalitaController != enModalitaArticolo.SelezioneSingola)
            {
                var pnl3 = GetMenu().Tabs[0].Add("Magazzino");

                if (ModalitaController != enModalitaArticolo.Ricerca)
                {
                    var rib3 = pnl3.Add("Qta Magazzino", StrumentiMusicali.Core.Properties.ImageIcons.UnloadWareHouse, true);
                    rib3.Click += (a, e) =>
                    {
                        using (var depo = new Core.Controllers.ControllerMagazzino(SelectedItem))
                        {
                            using (var view = new View.ScaricoMagazzinoView(depo))
                            {
                                ShowView(view, enAmbiente.Magazzino, depo, true, true);

                                RiselezionaSelezionato();
                            }
                        }
                    };
                }
                var rib4 = pnl3.Add("1 pezzo venduto", StrumentiMusicali.Core.Properties.ImageIcons.Remove, true);
                rib4.Click += (a, e) =>
                {
                    using (var depo = new Core.Controllers.ControllerMagazzino(SelectedItem))
                    {
                        ScaricaQtaMagazzino scarica = new ScaricaQtaMagazzino();

                        scarica.Qta = 1;

                        using (var uof = new UnitOfWork())
                        {
                            var principale = uof.DepositoRepository.Find(b => b.Principale).FirstOrDefault();
                            if (principale == null)
                            {
                                MessageManager.NotificaWarnig("Occorre impostare un deposito principale, vai in depositi e imposta il deposito principale.");
                                return;
                            }
                            scarica.Deposito = principale.ID;
                            scarica.ArticoloID = SelectedItem.ID;
                            EventAggregator.Instance().Publish<ScaricaQtaMagazzino>(scarica);
                            RiselezionaSelezionato();
                        }
                    }
                };
                var rib5 = pnl3.Add("1 pezzo aggiunto", StrumentiMusicali.Core.Properties.ImageIcons.Add, true);
                rib5.Click += (a, e) =>
                {
                    using (var depo = new Core.Controllers.ControllerMagazzino(SelectedItem))
                    {
                        CaricaQtaMagazzino carica = new CaricaQtaMagazzino();

                        carica.Qta = 1;

                        using (var uof = new UnitOfWork())
                        {
                            var principale = uof.DepositoRepository.Find(b => b.Principale).FirstOrDefault();
                            if (principale == null)
                            {
                                MessageManager.NotificaWarnig("Occorre impostare un deposito principale, vai in depositi e imposta il deposito principale.");
                                return;
                            }
                            carica.Deposito = principale.ID;
                            carica.ArticoloID = SelectedItem.ID;
                            EventAggregator.Instance().Publish<CaricaQtaMagazzino>(carica);
                            RiselezionaSelezionato();
                        }
                    }
                };
            }


            if (ModalitaController == enModalitaArticolo.SelezioneSingola)
            {
                var pnl2 = GetMenu().Tabs[0].Add("Conferma");
                var rib2 = pnl2.Add("Conferma", StrumentiMusicali.Core.Properties.ImageIcons.Check_OK_48);
                rib2.Click += (a, e) =>
                {
                    if (SelectedItem != null && SelectedItem.ID > 0)
                    {
                        ArticoloSelezionatoSingolo = SelectedItem;
                        RaiseClose();
                    }
                    else
                    {
                        MessageManager.NotificaInfo("Selezionare un articolo");
                    }
                };
            }
            //	EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ScaricoMagazzino));
            //	EventAggregator.Instance().Publish(new MagazzinoSelezionaArticolo() { Articolo=SelectedItem});

            //};
        }

        public Articolo ArticoloSelezionatoSingolo { get; set; }

        private void ScontaArticoliShowView()
        {
            using (var view = new View.Articoli.ScontaArticoliView(this))
            {
                ShowView(view, enAmbiente.ArticoloSconto);
            }
        }

        public void InvioArticoli()
        {
            using (var export = new Exports.ExportArticoliCsv())
            {
                export.InvioArticoli();
            }
        }

        ~ControllerArticoli()
        {
            var dato = this.ReadSetting(enAmbiente.StrumentiList);
            if (SelectedItem != null)
            {
                dato.LastItemSelected = SelectedItem.ID;
                this.SaveSetting(enAmbiente.StrumentiList, dato);
            }
        }

        private void EditArt(Edit<Articolo> obj)
        {
            EditItem = SelectedItem;
            ShowViewDettaglio();
        }

        private void AggiungiArticolo(Add<Articolo> obj)
        {
            _logger.Info("Apertura Aggiungi Articolo");

            EditItem = new Articolo();
            bool libro = false;
            if (ModalitaController == enModalitaArticolo.Tutto)
            {
                if (MessageManager.QuestionMessage("Vuoi aggiungere un libro?"))
                {
                    libro = true;
                }
            }
            else
            {
                if (ModalitaController == enModalitaArticolo.SoloLibri)
                {
                    libro = true;
                }
                else if (ModalitaController == enModalitaArticolo.SoloStrumenti)
                {
                    libro = false;
                    EditItem.ShowStrumento = true;
                }
                else
                    return;
            }
            EditItem.ShowLibro = libro;
            if (libro)
            {
                using (var uof = new UnitOfWork())
                {
                    var categoria = uof.CategorieRepository.Find(a => a.Codice == 133).FirstOrDefault();
                    EditItem.CategoriaID = categoria.ID;
                }
            }

            ShowViewDettaglio();
        }

        private void ShowViewDettaglio()
        {
            if (!SettingSitoValidator.CheckFolderImmagini())
            {
                return;
            }

            using (var view = new DettaglioArticoloView(this))
            {
                //view.BindProp(EditItem,"");
                view.OnSave += (a, b) =>
                {
                    view.Validate();
                    EventAggregator.Instance().Publish<Save<Articolo>>
                    (new Save<Articolo>(this));
                };

                ShowView(view, enAmbiente.StrumentiDetail);
            }
        }

        private void DuplicaArticolo(ArticoloDuplicate obj)
        {
            try
            {
                using (var saveEntity = new SaveEntityManager())
                {
                    var uof = saveEntity.UnitOfWork;
                    var itemCurrent = (SelectedItem);
                    var art =
                    (new Articolo()
                    {
                        CategoriaID = itemCurrent.Categoria.ID,
                        Condizione = itemCurrent.Condizione,
                        
                        Prezzo = itemCurrent.Prezzo,
                        Testo = itemCurrent.Testo,
                        Titolo = "* " + itemCurrent.Titolo,
                        DataUltimaModifica = DateTime.Now,
                        DataCreazione = DateTime.Now
                    });
                    art.Strumento = itemCurrent.Strumento;
                    art.Libro = itemCurrent.Libro;

                    uof.ArticoliRepository.Add(art);
                    if (saveEntity.SaveEntity(enSaveOperation.Duplicazione))
                    {
                        _logger.Info("Duplica articolo");

                        SelectedItem = art;
                        EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        public string FiltroLibri { get; set; } = "";
        public string FiltroMarca { get; set; } = "";

        //public void ImportaCsvArticoli()
        //{
        //    try
        //    {
        //        using (OpenFileDialog res = new OpenFileDialog())
        //        {
        //            res.Title = "Seleziona file da importare";
        //            //Filter
        //            res.Filter = "File csv|*.csv;";

        //            //When the user select the file
        //            if (res.ShowDialog() == DialogResult.OK)
        //            {
        //                //Get the file's path
        //                var fileName = res.FileName;

        //                using (var curs = new CursorManager())
        //                {
        //                    using (StreamReader sr = new StreamReader(fileName, Encoding.Default, true))
        //                    {
        //                        String line;
        //                        bool firstLine = true;
        //                        int progress = 1;
        //                        // Read and display lines from the file until the end of
        //                        // the file is reached.
        //                        using (var uof = new UnitOfWork())
        //                        {
        //                            while ((line = sr.ReadLine()) != null)
        //                            {
        //                                if (!firstLine)
        //                                {
        //                                    progress = ImportLine(line, progress, uof);
        //                                }
        //                                firstLine = false;
        //                            }
        //                            uof.Commit();
        //                        }
        //                    }
        //                }
        //                EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
        //                MessageManager.NotificaInfo("Terminata importazione articoli");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.ManageError(ex);
        //    }
        //}

        //private int ImportLine(string line, int progress, UnitOfWork uof)
        //{
        //    var dat = line.Split('§');
        //    var cond = enCondizioneArticolo.Nuovo;
        //    if (dat[2] == "N")
        //    {
        //        cond = enCondizioneArticolo.Nuovo;
        //    }
        //    else if (dat[2] == "U")
        //    {
        //        cond = enCondizioneArticolo.UsatoGarantito;
        //    }
        //    else if (dat[2] == "E")
        //    {
        //        cond = enCondizioneArticolo.ExDemo;
        //    }
        //    else
        //    {
        //        throw new Exception("Tipo dato non gestito o mancante nella condizione articolo.");
        //    }
        //    decimal prezzo = 0;
        //    decimal prezzoBarrato = 0;
        //    bool prezzoARichiesta = false;
        //    var strPrezzo = dat[6];
        //    if (strPrezzo == "NC")
        //    {
        //        prezzoARichiesta = true;
        //    }
        //    else if (strPrezzo.Contains(";"))
        //    {
        //        prezzo = decimal.Parse(strPrezzo.Split(';')[0]);
                 
        //    }
        //    else
        //    {
        //        if (strPrezzo.Trim().Length > 0)
        //        {
        //            prezzo = decimal.Parse(strPrezzo);
        //        }
        //    }
        //    var artNew = (new Articolo()
        //    {
        //        CategoriaID = int.Parse(dat[1]),
        //        Condizione = cond,

        //        Titolo = dat[4],
        //        Testo = dat[5].Replace("<br>", Environment.NewLine),
        //        Prezzo = prezzo,
                
                 
        //    });
        //    artNew.Strumento.Marca = dat[3];
        //    uof.ArticoliRepository.Add(artNew);
        //    var foto = dat[7];
        //    if (foto.Length > 0)
        //    {
        //        int ordine = 0;
        //        foreach (var item in foto.Split(';'))
        //        {
        //            var artFoto = new FotoArticolo()
        //            {
        //                Articolo = artNew,
        //                UrlFoto = item,
        //                Ordine = ordine
        //            };
        //            ordine++;
        //            uof.FotoArticoloRepository.Add(artFoto);
        //        }
        //    }

        //    return progress;
        //}

        private void DeleteArticolo(object obj)
        {
            try
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare l'articolo selezionato?"))
                    return;
                using (var save = new SaveEntityManager())
                {
                    using (var immaginiController = new ControllerImmagini())
                    {
                        var item = save.UnitOfWork.ArticoliRepository.Find(a => a.ID ==
                        this.SelectedItem.ID).FirstOrDefault();
                        _logger.Info(string.Format("Cancellazione articolo /r/n{0} /r/n{1}", item.Titolo, item.ID));

                        if (!immaginiController.CheckFolderImmagini())
                            return;

                        var folderFoto = SettingSitoValidator.ReadSetting().CartellaLocaleImmagini;
                        var listFile = new List<string>();
                        foreach (var itemFoto in save.UnitOfWork.FotoArticoloRepository.Find(a => a.ArticoloID == item.ID))
                        {
                            immaginiController.RimuoviItemDaRepo(
                                folderFoto, listFile, save.UnitOfWork, itemFoto);
                        }
                        immaginiController.DeleteFile(listFile);
                        var mag = save.UnitOfWork.MagazzinoRepository.Find(a => a.ArticoloID == item.ID);
                        foreach (Magazzino itemMg in mag)
                        {
                            save.UnitOfWork.MagazzinoRepository.Delete(itemMg);
                        }
                        foreach (var itemAgg in save.UnitOfWork.AggiornamentoWebArticoloRepository.Find(a => a.ArticoloID == item.ID))
                        {
                            save.UnitOfWork.AggiornamentoWebArticoloRepository.Delete(itemAgg);
                        }

                        save.UnitOfWork.ArticoliRepository.Delete(item);

                        if (save.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        private void SaveArticolo(object arg)
        {
            try
            {
                using (var save = new SaveEntityManager())
                {
                    var uof = save.UnitOfWork;
                    if (EditItem.ID <= 0
                        || uof.ArticoliRepository.Find(a => a.ID == EditItem.ID).Count() == 0)
                    {
                        uof.ArticoliRepository.Add(EditItem);
                    }
                    else
                    {
                        EditItem.Categoria = null;
                        uof.ArticoliRepository.Update(EditItem);
                    }
                    if (
                    save.SaveEntity(enSaveOperation.OpSave))
                    {
                        EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
                    }
                }
            }
            catch (MessageException ex)
            {
                ExceptionManager.ManageError(ex);
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        private void AggiungiImmagine(ImageArticoloAdd eventArg)
        {
            try
            {
                using (OpenFileDialog res = new OpenFileDialog())
                {
                    res.Title = "Seleziona file da importare";
                    //Filter
                    res.Filter = "File jpg e jpeg|*.jpg;*.jepg|Tutti i file|*.*";

                    res.Multiselect = true;
                    //When the user select the file
                    if (res.ShowDialog() == DialogResult.OK)
                    {
                        EventAggregator.Instance().Publish<ImageArticoloAddFiles>(
                            new ImageArticoloAddFiles(eventArg.Articolo,
                            res.FileNames.ToList(), this._controllerImmagini));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        public override void RefreshList(UpdateList<Articolo> obj)
        {
            try
            {
                using (CursorManager cursorManager = new CursorManager())
                {
                    var datoRicerca = TestoRicerca.Split(' ').ToList();

                    List<ArticoloItem> list = new List<ArticoloItem>();

                    FiltroLibri = FiltroLibri.Trim();
                    FiltroMarca = FiltroMarca.Trim();

                    using (var uof = new UnitOfWork())
                    {
                        var datList = uof.ArticoliRepository.Find(a => datoRicerca.Count == 0 || ((
                            a.Libro.Autore.Contains(FiltroLibri)
                            || a.Libro.Edizione.Contains(FiltroLibri)
                            || a.Libro.Edizione2.Contains(FiltroLibri)
                            || a.Libro.Genere.Contains(FiltroLibri)
                            || a.Libro.Ordine.Contains(FiltroLibri)
                            || a.Libro.Settore.Contains(FiltroLibri)
                            || a.Libro.TitoloDelLibro.Contains(FiltroLibri)
                            && FiltroLibri.Length > 0)
                            || FiltroLibri == "")

                            && (a.Strumento.Marca.Contains(FiltroMarca) && FiltroMarca.Length > 0
                            || FiltroMarca == "")
                            );//.Select(a=>new { a, a.Categoria }).ToList().Select(a=>a.a).ToList();

                        var listRicerche = datoRicerca.Where(a => a.Length > 0).ToList();

                        foreach (var ricerca in listRicerche)
                        {
                            datList = datList.Where(a =>

                               a.Titolo.Contains(ricerca)
                              || a.Testo.Contains(ricerca)
                              || a.CodiceInterno.Contains(ricerca)
                              || a.ArticoloWeb.DescrizioneBreveHtml.Contains(ricerca)
                              || a.ArticoloWeb.DescrizioneHtml.Contains(ricerca)
                              || a.TagImport.Contains(ricerca)
                              || a.Categoria.Nome.Contains(ricerca)
                              || a.Categoria.Reparto.Contains(ricerca)
                              || a.Categoria.CategoriaCondivisaCon.Contains(ricerca)

                              || (((a.CodiceABarre.Equals(ricerca) && a.CodiceABarre.Length > 0)))
                            );
                        }

                        var preList = datList.OrderByDescending(a => a.ID)
                            .Select(a => new
                            {
                                a.Categoria,
                                Articolo = a,
                                IsBook = !string.IsNullOrEmpty(a.Libro.Autore)
                    || !string.IsNullOrEmpty(a.Libro.Edizione)
                    || !string.IsNullOrEmpty(a.Libro.Settore)
                            });


                        var listArt2 = preList
                        .Where(a => (
                        a.IsBook == true
                        && ModalitaController == enModalitaArticolo.SoloLibri)
                        ||
                        (a.IsBook == false
                        && ModalitaController == enModalitaArticolo.SoloStrumenti)
                        ||
                        ModalitaController == enModalitaArticolo.Tutto
                        ||
                        ModalitaController == enModalitaArticolo.SelezioneSingola
                        )
                            .Take(ViewAllItem ? 100000 : 300)

                        .ToList();
                        list = listArt2.Select(a =>
                        new ArticoloItem(a.Articolo)
                        ).ToList();

                        var listArt = list.Select(b => b.ID);
                        var giacenza = uof.MagazzinoRepository.Find(a => listArt.Contains(a.ArticoloID))
                           .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                           .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();


                        GC.SuppressFinalize(preList);
                        GC.SuppressFinalize(listArt);

                        foreach (var item in list)
                        {
                            var val = giacenza.Where(a => a.Articolo.ArticoloID == item.ID).ToList();
                            //.Select(a => a.Sum).FirstOrDefault();

                            item.QuantitaNegozio = val.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                                    .DefaultIfEmpty(0).FirstOrDefault();

                            item.QuantitaTotale = val.Sum(a => a.Sum);
                        }
                        GC.SuppressFinalize(giacenza);

                    }
                    DataSource = new View.Utility.MySortableBindingList<ArticoloItem>(list);
                    if (ClearRicerca)
                    {
                        TestoRicerca = "";
                        FiltroMarca = "";
                        FiltroLibri = "";
                    }
                    GC.SuppressFinalize(list);

                }
                base.RefreshList(obj);
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }
    }
}