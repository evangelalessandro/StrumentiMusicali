using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Forms;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.Events.Magazzino;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
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
        public enum enModalitaArticolo
        {
            Tutto = 0,
            Ricerca = 1,
            SoloStrumenti = 2,
            SoloLibri = 3,

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

            AggiungiComandiMenu();

            SetKeyImageListUI();
        }
        FotoArticolo _fileFotoSelezionato = null;
        private void ImmagineSelezionata(ImageSelected<FotoArticolo> obj)
        {
            if (obj.File != null)
                _fileFotoSelezionato = obj.File.Entity;
            else
                _fileFotoSelezionato = null;
        }

        Subscription<ImageAddFiles> _subAddImage;
        Subscription<ImageOrderSet<FotoArticolo>> _orderImage;
        Subscription<ImageRemove<FotoArticolo>> _imageRemove;


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
        List<ImmaginiFile<FotoArticolo>> _listFotoArticolo
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

            if (ModalitaController == enModalitaArticolo.Ricerca)
            {
                pnl.Pulsanti.RemoveAll(a => a.Tag == MenuTab.TagAdd
                || a.Tag == MenuTab.TagRemove || a.Tag == MenuTab.TagCerca
                || a.Tag == MenuTab.TagCercaClear);

                tabFirst.Pannelli.RemoveAt(2);
                tabFirst.Pannelli.RemoveAt(1);

            }
            if (ModalitaController != enModalitaArticolo.Ricerca)
            {
                var rib1 = pnl.Add("Duplica", Properties.Resources.Duplicate, true);
                rib1.Click += (a, e) =>
                {
                    EventAggregator.Instance().Publish<ArticoloDuplicate>(new ArticoloDuplicate(this));

                };
                var pnl2 = GetMenu().Tabs[0].Add("Prezzi");
                var rib2 = pnl2.Add("Varia prezzi", Properties.Resources.Sconta_Articoli);
                rib2.Click += (a, e) =>
                {
                    ScontaArticoliShowView();
                };
            }
            if (LoginData.utenteLogin.Magazzino)
            {
                var pnl3 = GetMenu().Tabs[0].Add("Magazzino");

                if (ModalitaController != enModalitaArticolo.Ricerca)
                {
                    var rib3 = pnl3.Add("Qta Magazzino", Properties.Resources.UnloadWareHouse, true);
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
                var rib4 = pnl3.Add("1 pezzo venduto", Properties.Resources.Remove, true);
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
                var rib5 = pnl3.Add("1 pezzo aggiunto", Properties.Resources.Add, true);
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
            //	EventAggregator.Instance().Publish(new ApriAmbiente(enAmbiente.ScaricoMagazzino));
            //	EventAggregator.Instance().Publish(new MagazzinoSelezionaArticolo() { Articolo=SelectedItem});


            //};
        }

        private void ScontaArticoliShowView()
        {
            using (var view = new View.Articoli.ScontaArticoliView(this))
            {
                ShowView(view, enAmbiente.ArticoloSconto);
            }
        }

        public void InvioArticoli(InvioArticoliCSV obj)
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
                        BoxProposte = itemCurrent.BoxProposte,
                        Prezzo = itemCurrent.Prezzo,
                        PrezzoARichiesta = itemCurrent.PrezzoARichiesta,
                        PrezzoBarrato = itemCurrent.PrezzoBarrato,
                        Testo = itemCurrent.Testo,
                        Titolo = "* " + itemCurrent.Titolo,
                        UsaAnnuncioTurbo = itemCurrent.UsaAnnuncioTurbo,
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
        public void ImportaCsvArticoli(ImportArticoliCSVMercatino obj)
        {
            try
            {
                using (OpenFileDialog res = new OpenFileDialog())
                {
                    res.Title = "Seleziona file da importare";
                    //Filter
                    res.Filter = "File csv|*.csv;";

                    //When the user select the file
                    if (res.ShowDialog() == DialogResult.OK)
                    {
                        //Get the file's path
                        var fileName = res.FileName;

                        using (var curs = new CursorManager())
                        {
                            using (StreamReader sr = new StreamReader(fileName, Encoding.Default, true))
                            {
                                String line;
                                bool firstLine = true;
                                int progress = 1;
                                // Read and display lines from the file until the end of
                                // the file is reached.
                                using (var uof = new UnitOfWork())
                                {
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        if (!firstLine)
                                        {
                                            progress = ImportLine(line, progress, uof);
                                        }
                                        firstLine = false;
                                    }
                                    uof.Commit();
                                }
                            }
                        }
                        EventAggregator.Instance().Publish<UpdateList<Articolo>>(new UpdateList<Articolo>(this));
                        MessageManager.NotificaInfo("Terminata importazione articoli");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }

        private int ImportLine(string line, int progress, UnitOfWork uof)
        {
            var dat = line.Split('§');
            var cond = enCondizioneArticolo.Nuovo;
            if (dat[2] == "N")
            {
                cond = enCondizioneArticolo.Nuovo;
            }
            else if (dat[2] == "U")
            {
                cond = enCondizioneArticolo.UsatoGarantito;
            }
            else if (dat[2] == "E")
            {
                cond = enCondizioneArticolo.ExDemo;
            }
            else
            {
                throw new Exception("Tipo dato non gestito o mancante nella condizione articolo.");
            }
            decimal prezzo = 0;
            decimal prezzoBarrato = 0;
            bool prezzoARichiesta = false;
            var strPrezzo = dat[6];
            if (strPrezzo == "NC")
            {
                prezzoARichiesta = true;
            }
            else if (strPrezzo.Contains(";"))
            {
                prezzo = decimal.Parse(strPrezzo.Split(';')[0]);
                prezzoBarrato = decimal.Parse(strPrezzo.Split(';')[1]);
            }
            else
            {
                if (strPrezzo.Trim().Length > 0)
                {
                    prezzo = decimal.Parse(strPrezzo);
                }
            }
            var artNew = (new Articolo()
            {
                CategoriaID = int.Parse(dat[1]),
                Condizione = cond,

                Titolo = dat[4],
                Testo = dat[5].Replace("<br>", Environment.NewLine),
                Prezzo = prezzo,
                PrezzoARichiesta = prezzoARichiesta,
                PrezzoBarrato = prezzoBarrato,
                BoxProposte = int.Parse(dat[9]) == 1 ? true : false
            });
            artNew.Strumento.Marca = dat[3];
            uof.ArticoliRepository.Add(artNew);
            var foto = dat[7];
            if (foto.Length > 0)
            {
                int ordine = 0;
                foreach (var item in foto.Split(';'))
                {
                    var artFoto = new FotoArticolo()
                    {
                        Articolo = artNew,
                        UrlFoto = item,
                        Ordine = ordine
                    };
                    ordine++;
                    uof.FotoArticoloRepository.Add(artFoto);
                }
            }

            return progress;
        }

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
                              || a.TagImport.Contains(ricerca)
                              || a.Categoria.Nome.Contains(ricerca)
                              || a.Categoria.Reparto.Contains(ricerca)
                              || a.Categoria.CategoriaCondivisaCon.Contains(ricerca)

                              || (((a.CodiceABarre.Equals(ricerca) && a.CodiceABarre.Length > 0)))
                            );
                        }

                        list = datList.OrderByDescending(a => a.ID)
                            .Select(a => new { a.Categoria, Articolo = a }).ToList()

                        .Select(a => a.Articolo)
                        .Where(a => (
                        a.IsLibro() == true
                        && ModalitaController == enModalitaArticolo.SoloLibri)
                        ||
                        (a.IsLibro() == false
                        && ModalitaController == enModalitaArticolo.SoloStrumenti)
                        ||
                        ModalitaController == enModalitaArticolo.Tutto
                        )
                            .Take(ViewAllItem ? 100000 : 300)

                        .Select(a => new ArticoloItem(a)
                        {

                            //Pinned = a.Pinned
                        }).ToList();

                        var listArt = list.Select(b => b.ID);
                        var giacenza = uof.MagazzinoRepository.Find(a => listArt.Contains(a.ArticoloID))
                           .Select(a => new { a.ArticoloID, a.Qta, a.Deposito }).GroupBy(a => new { a.ArticoloID, a.Deposito })
                           .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key }).ToList();


                        foreach (var item in list)
                        {
                            var val = giacenza.Where(a => a.Articolo.ArticoloID == item.ID).ToList();
                            //.Select(a => a.Sum).FirstOrDefault();

                            item.QuantitaNegozio = val.Where(a => a.Articolo.Deposito.Principale).Select(a => a.Sum)
                                    .DefaultIfEmpty(0).FirstOrDefault();

                            item.QuantitaTotale = val.Sum(a => a.Sum);
                        }
                    }




                    DataSource = new View.Utility.MySortableBindingList<ArticoloItem>(list);
                    if (ClearRicerca)
                    {
                        TestoRicerca = "";
                        FiltroMarca = "";
                        FiltroLibri = "";
                    }
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