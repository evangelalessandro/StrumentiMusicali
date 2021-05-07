//using NLog;
//using StrumentiMusicali.App.Core.Controllers;
//using StrumentiMusicali.App.Core.Controllers.Base;
//using StrumentiMusicali.Core.Manager;
//using StrumentiMusicali.Core.Settings;
//using StrumentiMusicali.Library.Core;
//using StrumentiMusicali.Library.Entity.Articoli;
//using StrumentiMusicali.Library.Entity.Enums;
//using StrumentiMusicali.Library.Entity.Setting;
//using StrumentiMusicali.Library.Repo;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Windows.Forms;

//namespace StrumentiMusicali.App.Core.Exports
//{
//    internal class ExportArticoliCsv : IDisposable
//    {
//        private const String Separatore = "§";
//        private List<FotoArticolo> _fotoToUpload = new List<FotoArticolo>();
//        private SettingSito _settingSito;

//        private WebClient _webClient = new WebClient();

//        public void Dispose()
//        {
//            if (_webClient != null)
//            {
//                _webClient.Dispose();
//            }
//            _webClient = null;
//            _fotoToUpload.Clear();
//            _fotoToUpload = null;
//        }

//        internal readonly ILogger _logger = ManagerLog.Logger;

//        //public void InvioArticoli()
//        //{
//        //    _logger.Info("Inizio controlli per invio CSV");
//        //    using (var controller = new ControllerImmagini())
//        //    {
//        //        if (!controller.CheckFolderImmagini())
//        //            return;
//        //    }
//        //    try
//        //    {
//        //        var controller = new BaseController();
//        //        _settingSito = SettingSitoValidator.ReadSetting();
//        //        if (string.IsNullOrEmpty(_settingSito.UrlCompletaImmagini))
//        //        {
//        //            MessageManager.NotificaWarnig("Occorre impostare l'url per uploadare le immagini.");
//        //            return;
//        //        }
//        //        if (string.IsNullOrEmpty(_settingSito.UrlSito))
//        //        {
//        //            MessageManager.NotificaWarnig("Occorre impostare l'url del sito proprio.");
//        //            return;
//        //        }
//        //        //if (string.IsNullOrEmpty(_settingSito.UrlCompletoFileMercatino))
//        //        //{
//        //        //    MessageManager.NotificaWarnig("Occorre impostare l'url per il file del mercatino.");
//        //        //    return;
//        //        //}
//        //        //if (string.IsNullOrEmpty(_settingSito.UrlCompletoFileEcommerce))
//        //        //{
//        //        //    MessageManager.NotificaWarnig("Occorre impostare l'url per il file del e-commerce.");
//        //        //    return;
//        //        //}
//        //        using (var curs = new CursorManager())
//        //        {
//        //            using (var uof = new UnitOfWork())
//        //            {
//        //                _logger.Info("Inizio procedura invio CSV");

//        //                List<FotoArticolo> fotoList = uof.FotoArticoloRepository.Find(a => true).ToList();
//        //                List<GiacenzaArt> magazzinoGiac = uof.MagazzinoRepository.Find(a => true).GroupBy(a => a.ArticoloID)
//        //                    .Select(a => new GiacenzaArt { ArticoloId = a.Key, Giacenza = a.Sum(b => b.Qta) }).ToList();

//        //                //var fileEcommerce = FileEcommerce(uof, fotoList, magazzinoGiac);


//        //                UploadNewFoto();
//        //                MessageManager.NotificaInfo("Terminato upload nuove foto");

//        //                var artToUpdate = _fotoToUpload.Select(a => a.ArticoloID).Distinct().ToList();

//        //                //UploadEndRetray(fileEcommerce, _settingSito.UrlCompletoFileEcommerce);

//        //                uof.Commit();
//        //            }
//        //        }
//        //        _logger.Info(@"Invio CSV\Foto Terminato Correttamente");

//        //        MessageManager.NotificaInfo("Terminato Invio Articoli");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.Error(ex);
//        //        ExceptionManager.ManageError(ex);
//        //    }
//        //}

//        /// <summary>
//        /// Foto da uploadare
//        /// </summary>
//        /// <param name="fotos"></param>
//        private void AddFotoToUpload(List<FotoArticolo> fotos)
//        {
//            _fotoToUpload.AddRange(fotos);
//        }

//        /// <summary>
//        /// ritorna il testo del file
//        /// </summary>
//        /// <param name="listArticoli"></param>
//        /// <param name="magazzinoGiac"></param>
//        /// <param name="fotoList"></param>
//        /// <returns></returns>
//        private StringBuilder ExportFile(List<Articolo> listArticoli, List<GiacenzaArt> magazzinoGiac, List<FotoArticolo> fotoList)
//        {
//            StringBuilder sb = new StringBuilder();
//            foreach (var art in listArticoli)
//            {
//                var giac = magazzinoGiac.Where(a => a.ArticoloId == art.ID).Select(a => a.Giacenza)
//                    .DefaultIfEmpty(0).FirstOrDefault();
//                if (giac <= 0)
//                    continue;

//                ExportLineBase(_settingSito, fotoList, sb, art);

//                //aggiunta giacenza rispetto al file normale
//                sb.Append(giac);
//                sb.Append(Separatore);
//                sb.AppendLine();
//            }
//            return sb;
//        }

//        private void ExportLineBase(SettingSito settingSito, List<FotoArticolo> fotoList, StringBuilder sb, Articolo art)
//        {
//            var fotoOrdinate = fotoList.Where(b => b.ArticoloID == art.ID).OrderBy(b => b.Ordine).ToList();

//            foreach (var item in fotoOrdinate)
//            {
//                var file = Path.Combine(_settingSito.CartellaLocaleImmagini, item.UrlFoto);
//                if (!File.Exists(file))
//                {
//                    throw new MessageException(string.Format("Errore, manca l'immagine {0} dell'articolo con codice {1}, nome file {2}."
//                        , item.Articolo.ID + " Titolo:" + item.Articolo.Titolo
//                        , item.Ordine
//                        , file
//                        ));
//                }
//            }
//            AddFotoToUpload(fotoOrdinate);

//            sb.Append(art.ID + Separatore);

//            sb.Append(art.Categoria.Codice + Separatore);
//            switch (art.Condizione)
//            {
//                case enCondizioneArticolo.Nuovo:
//                    sb.Append("N");
//                    break;

//                case enCondizioneArticolo.ExDemo:
//                    sb.Append("E");
//                    break;

//                case enCondizioneArticolo.UsatoGarantito:
//                    sb.Append("U");
//                    break;

//                default:
//                    break;
//            }
//            sb.Append(Separatore);
//            if (art.Strumento.Marca.Length > 100)
//                sb.Append(art.Strumento.Marca.Substring(0, 100));
//            else
//            {
//                sb.Append(art.Strumento.Marca);
//            }
//            sb.Append(Separatore);
//            if (art.Titolo.Length > 100)
//                sb.Append(art.Titolo.Substring(0, 100));
//            else
//            {
//                sb.Append(art.Titolo);
//            }
//            sb.Append(Separatore);
//            if (art.Testo.Length > 2000)
//                sb.Append(art.Testo.Substring(0, 2000).Replace(Environment.NewLine, "<br/>"));
//            else
//            {
//                sb.Append(art.Testo.Replace(Environment.NewLine, "<br/>"));
//            }

//            sb.Append(Separatore);
//            //Prezzo

//            sb.Append(art.Prezzo.ToString("0.00"));


//            sb.Append(Separatore);

//            //URLFotografia
//            sb.Append(string.Join(";", fotoOrdinate.Select(a => a.UrlFoto).ToList()));

//            sb.Append(Separatore);
//            /*URLSchedaProdotto*/
//            sb.Append(GetUrlProdotto(art, settingSito));

//            sb.Append(Separatore);



//            sb.Append(Separatore);
//        }

//        //private string FileEcommerce(UnitOfWork uof, List<FotoArticolo> fotoList, List<GiacenzaArt> magazzinoGiac)
//        //{
//        //    List<Articolo> listArticoli = uof.ArticoliRepository.Find(a => a.CaricainECommerce && a.Categoria.Codice >= 0)
//        //        .Select(a => new { articolo = a, Categoria = a.Categoria }).ToList().Select(a => a.articolo).ToList();

//        //    var fileEcommerceContent = ExportFile(listArticoli, magazzinoGiac, fotoList);
//        //    string fileEcommerce = SaveFileCsv(fileEcommerceContent, _settingSito.SoloNomeFileEcommerce);
//        //    MessageManager.NotificaInfo("Creato file E-Commerce");
//        //    _logger.Info("Creato file E-Commerce con '{0}' articoli ", listArticoli.Count.ToString());
//        //    return fileEcommerce;
//        //}

//        private string SaveFileCsv(StringBuilder fileEcommerceContent, string nomeFile)
//        {
//            var fileEcommerce = Path.Combine(GetTempFolder(), nomeFile);
//            File.WriteAllText(fileEcommerce, fileEcommerceContent.ToString(), Encoding.Unicode);
//            return fileEcommerce;
//        }



//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="filePath">file con cartella locale</param>
//        /// <param name="ftpUri">Cartella di destizione. Il nome del file viene calcolato dal paramento <paramref name="filePath"/></param>
//        /// <returns></returns>
//        private bool ftpUpFile(string filePath, string ftpUri)
//        {
//            string file = "";
//            try
//            {
//                //create full uri path
//                file = Path.Combine(ftpUri, Path.GetFileName(filePath));
//                _logger.Debug(string.Format("Upload file {0} in {1} iniziato", file, filePath));
//                //ftp the file to Uri path via the ftp STOR command
//                // (ignoring the the Byte[] array return since it is always empty in this case)
//                _webClient.UploadFile(file, filePath);
//                _logger.Debug(string.Format("Upload file {0} in {1} terminato", file, filePath));
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.Error(string.Format("Upload non riuscito del file {0} in {1} ", file, filePath));

//                ExceptionManager.ManageError(ex, false);
//                return false;
//            }
//        }

//        private string GetTempFolder()
//        {
//            var name = Path.Combine((System.IO.Path.GetDirectoryName(Application.ExecutablePath)), "TEMP");
//            if (!Directory.Exists(name))
//            {
//                Directory.CreateDirectory(name);
//            }
//            return name;
//        }

//        private string GetUrlProdotto(Articolo articolo, SettingSito settingSito)
//        {
//            return settingSito.UrlSito + "?" + articolo.ID;
//        }

//        private void UploadNewFoto()
//        {
//            //webClient.Credentials = new NetworkCredential(ArgList["-user"], ArgList["-pwd"]);
//            _logger.Info("Inizio upload di {0} foto", _fotoToUpload);

//            _webClient.Credentials = new NetworkCredential("dlpuser@dlptest.com", "e73jzTRTNqCN9PYAAjjn");

//            var baseLocalFolder = _settingSito.CartellaLocaleImmagini;
//            var folderFtpImmagini = _settingSito.UrlCompletaImmagini;
//            //loop through files in folder and upload
//            foreach (var file in _fotoToUpload)
//            {
//                var fileToUpload = Path.Combine(baseLocalFolder, file.UrlFoto);

//                UploadEndRetray(fileToUpload, folderFtpImmagini);
//            }
//            _logger.Info("Terminato upload di {0} foto", _fotoToUpload);
//        }

//        private void UploadEndRetray(string fileToUpload, string destFtpFolder)
//        {
//            int wait = 100;

//            bool uploadCompleted;
//            var retry = 0;
//            do
//            {
//                uploadCompleted = ftpUpFile(fileToUpload, destFtpFolder);
//                if (!uploadCompleted)
//                {
//                    retry++;
//                    Thread.Sleep(wait);
//                    wait += 500; //wait an extra second after each failed attempt

//                    if (retry == 5)
//                    {
//                        _logger.Error("Upload Error del file " + fileToUpload);
//                        throw new MessageException("Non si riesce a fare upload del file " + fileToUpload);
//                    }
//                }
//            } while (!uploadCompleted);
//        }

//        private class GiacenzaArt
//        {
//            public int ArticoloId { get; set; }
//            public int Giacenza { get; set; }
//        }
//    }
//}