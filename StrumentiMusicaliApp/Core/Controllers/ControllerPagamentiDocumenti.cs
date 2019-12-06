using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerPagamentiDocumenti : BaseControllerGeneric<Pagamento, PagamentoItem>,
        IDisposable
    {
        Guid _IDPagamentoMaster;
        public ControllerPagamentiDocumenti(enAmbiente ambiente,
            enAmbiente ambienteDettaglio, Guid IDPagamentoMaster)
            : base(ambiente, ambienteDettaglio)
        {
            SetKeyImageListUI();

            _IDPagamentoMaster = IDPagamentoMaster;
            AggiungiComandiMenu();

        }
        public void UpdateButton()
        {
            _ribRemove.Enabled = true;
            _ribRemove.Enabled = FotoSelezionata() != null;

        }
        RibbonMenuButton _ribRemove;
        private void AggiungiComandiMenu()
        {
            var tabFirst = GetMenu().Tabs[0];
            var pnl = tabFirst.Pannelli.First();


            pnl.Pulsanti.RemoveAll(a => a.Tag == MenuTab.TagAdd
            || a.Tag == MenuTab.TagRemove || a.Tag == MenuTab.TagCerca
            || a.Tag == MenuTab.TagCercaClear
            || a.Tag == MenuTab.TagEdit
            );

            tabFirst.Pannelli.RemoveAt(2);
            tabFirst.Pannelli.RemoveAt(1);

            var _ribPannelImmagini = tabFirst.Add("Immagini");

            var ribAdd = _ribPannelImmagini.Add("Aggiungi",
                Properties.Resources.Add);

            ribAdd.Click += (a, e) =>
            {
                try
                {
                    using (OpenFileDialog res = new OpenFileDialog())
                    {
                        res.Title = "Seleziona file da importare";
                        //Filter
                        res.Filter = "File pdf|*.pdf;|Tutti i file|*.*";

                        res.Multiselect = true;
                        //When the user select the file
                        if (res.ShowDialog() == DialogResult.OK)
                        {
                            var arr = res.FileNames;

                            EventAggregator.Instance().Publish<ImageAddFiles>(
                                new ImageAddFiles(arr.ToList()
                                , this._INSTANCE_KEY));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.ManageError(ex);

                }
            };

            _ribRemove = _ribPannelImmagini.Add(
                "Rimuovi",
                Properties.Resources.Remove
            );
            _ribRemove.Click += (b, e) =>
            {
                var foto = this.FotoSelezionata();

                EventAggregator.Instance().Publish<ImageRemove<PagamentoDocumenti>>(
                    new ImageRemove<PagamentoDocumenti>(
                        new ImmaginiFile<PagamentoDocumenti>(
                              foto.PathFile,
                    foto.FileName, foto),
                    this._INSTANCE_KEY));
            };

        }

        private void SetKeyImageListUI()
        {
            _subAddImage = EventAggregator.Instance()
                .Subscribe<ImageAddFiles>
                (AddImageFiles);

            _orderImage = EventAggregator.Instance()
                .Subscribe<ImageOrderSet<PagamentoDocumenti>>(
                ImageSetOrder);

            _imageRemove = EventAggregator.Instance()
                .Subscribe<ImageRemove<PagamentoDocumenti>>(
                ImageRemoveSet
                );

            EventAggregator.Instance()
                .Subscribe<ImageSelected<PagamentoDocumenti>>(ImmagineSelezionata);

        }
        internal List<ImmaginiFile<PagamentoDocumenti>> RefreshImageListData()
        {
            if (!SettingDocumentiPagamentiValidator.CheckFolderPdfPagamenti())
            {
                return _listFotoArticolo = new List<ImmaginiFile<PagamentoDocumenti>>();
            }
            using (var uof = new UnitOfWork())
            {
                var setting = SettingDocumentiPagamentiValidator.ReadSetting();


                var imageList = uof.PagamentoDocumentiRepository.Find(a => a.IDPagamentoMaster == _IDPagamentoMaster)
                    .OrderBy(a => a.Ordine).ToList();

                _listFotoArticolo = imageList.Select(a =>
                new ImmaginiFile<PagamentoDocumenti>(Path.Combine(
                    setting.CartellaReteDocumentiPagamenti, a.PathFile)
                                 , a.FileName, a)).ToList();

                return (_listFotoArticolo.ToList());


            }
        }

        private void ImageRemoveSet(ImageRemove<PagamentoDocumenti> obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {

                if (!SettingDocumentiPagamentiValidator.CheckFolderPdfPagamenti())
                    return;

                var folderPagamenti = SettingDocumentiPagamentiValidator.ReadSetting().CartellaReteDocumentiPagamenti;


                using (var saveManager = new SaveEntityManager())
                {
                    var repo = saveManager.UnitOfWork.PagamentoDocumentiRepository;

                    var filePdf = obj.File.Entity;

                    var nomeFile = Path.Combine(folderPagamenti, filePdf.PathFile);

                    File.Delete(nomeFile);

                    var item = repo.Find(a => a.ID == filePdf.ID).First();
                    repo.Delete(item);

                    saveManager.SaveEntity("Rimossa immagine");
                }

                EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
            }
        }

        private void ImageSetOrder(ImageOrderSet<PagamentoDocumenti> obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {

            }

        }
        private void AddImageFiles(ImageAddFiles obj)
        {
            if (obj.GuidKey == this._INSTANCE_KEY)
            {
                if (!SettingDocumentiPagamentiValidator.CheckFolderPdfPagamenti())
                    return;

                var folderPagamenti = SettingDocumentiPagamentiValidator.ReadSetting().CartellaReteDocumentiPagamenti;


                using (var saveManager = new SaveEntityManager())
                {
                    var repo = saveManager.UnitOfWork.PagamentoDocumentiRepository;
                    foreach (var item in obj.Files)
                    {

                        var filePdf = item;

                        var info = new FileInfo(filePdf);
                        var nomeFile = info.Name;
                        var dir = Path.Combine(folderPagamenti, this._IDPagamentoMaster.ToString());
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        File.Copy(filePdf, Path.Combine(dir, nomeFile));

                        repo.Add(new PagamentoDocumenti()
                        {
                            IDPagamentoMaster = this._IDPagamentoMaster,
                            FileName = nomeFile,
                            Ordine = 0,
                            PathFile = Path.Combine(this._IDPagamentoMaster.ToString(), nomeFile),
                        });

                    }


                    saveManager.SaveEntity("Immagini aggiunte correttamente");

                    EventAggregator.Instance().Publish<ImageListUpdate>(new ImageListUpdate());
                }
            }
        }
        Subscription<ImageAddFiles> _subAddImage;
        Subscription<ImageOrderSet<PagamentoDocumenti>> _orderImage;
        Subscription<ImageRemove<PagamentoDocumenti>> _imageRemove;

        PagamentoDocumenti _fileFotoSelezionato = null;
        private void ImmagineSelezionata(ImageSelected<PagamentoDocumenti> obj)
        {
            if (obj.File != null)
                _fileFotoSelezionato = obj.File.Entity;
            else
            {
                _fileFotoSelezionato = null;
            }
        }
        List<ImmaginiFile<PagamentoDocumenti>> _listFotoArticolo
            = new List<ImmaginiFile<PagamentoDocumenti>>();
        public PagamentoDocumenti FotoSelezionata()
        {
            return FotoSelezionata(_fileFotoSelezionato);
        }
        private PagamentoDocumenti FotoSelezionata(PagamentoDocumenti file)
        {
            if (file == null)
                return null;
            return _listFotoArticolo.Where(a => a.Entity == file).Select(a => a.Entity).DefaultIfEmpty(null).FirstOrDefault();
        }
    }
}
