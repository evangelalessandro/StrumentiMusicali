using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using NLog;

namespace StrumentiMusicali.ftpBackup.Ftp
{
    public class FtpManager : IDisposable
    {
        internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public bool Upload(string fileLocal)
        {
            FtpClient client = Connetti();
            if (client != null)
            {
                var setting = Library.Core.Settings.SettingBackupFtpValidator.ReadSetting();

                var dest = System.IO.Path.Combine(setting.BaseFolder, new System.IO.FileInfo(fileLocal).Name);

                var operazione = client.UploadFile(fileLocal, dest
                        , FtpExists.Overwrite, false, FtpVerify.Retry);

                if (client.FileExists(dest))
                {
                    _logger.Info("Upload riuscito del file : [" + fileLocal + "]  in [" + dest + "]");
                    operazione = true;
                }
                else
                {
                    _logger.Error("Upload non riuscito del file : [" + fileLocal + "]  in [" + dest + "]");
                    operazione = false;
                }
                 
                return operazione;
            }
            return false;
        }
        private FtpClient _client =null;
        public bool Delete(string remoteFileName)
        {
            FtpClient client = Connetti();
            if (client != null)
            {
                client.DeleteFile(remoteFileName);

                 
                return true;
            }
            return false;
        }
        public List<FileDirectoryFtp> FileList()
        {
            var retList = new List<FileDirectoryFtp>();
            FtpClient client = Connetti();
            if (client != null)
            {
                var dato = client.GetListing().ToList();

                retList.AddRange(ConvertiListe(dato));

                foreach (var item in retList.Where(a => a.IsDirectory).ToList())
                {
                    var listFile = client.GetListing(item.FullPath).ToList();
                    retList.AddRange(ConvertiListe(listFile));
                }
                // disconnect! good bye!
                 
                return retList;
            }
            return null;
        }

        private List<FileDirectoryFtp> ConvertiListe(List<FtpListItem> dato)
        {
            List<FileDirectoryFtp> retList = new List<FileDirectoryFtp>();
            foreach (var item in dato)
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    retList.Add(new FileDirectoryFtp()
                    {
                        FullPath = item.FullName,
                        IsDirectory = false,
                        Name = item.Name,
                        Size = item.Size,
                        CreatedData = item.Modified
                    });
                }
                else
                {
                    retList.Add(new FileDirectoryFtp()
                    {
                        FullPath = item.FullName,
                        IsDirectory = true,
                        Name = item.Name,
                        CreatedData = item.Modified
                    });
                }
            }
            return retList;
        }
        public bool ConnessioneOk()
        {
            FtpClient client = Connetti();
            if (client != null)
            { 
                return true;
            }
            return false;
        }
        private FtpClient Connetti()
        {
            try
            {
                if (_client != null && _client.IsConnected)
                    return _client;

                var setting = Library.Core.Settings.SettingBackupFtpValidator.ReadSetting();
                // create an FTP client
                FtpClient client = new FtpClient(setting.UrlHost);

                // if you don't specify login credentials, we use the "anonymous" user account
                client.Credentials = new NetworkCredential(setting.UserName, setting.Password);
                client.Port = setting.Porta;
                // begin connecting to the server
                client.Connect();

                // upload a file and retry 3 times before giving up
                client.ConnectTimeout = 100;
                if (!string.IsNullOrEmpty(setting.BaseFolder))
                    client.SetWorkingDirectory(setting.BaseFolder);

                _client = client;
                return client;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Errore nella connessione ftp");
                return null;
            }
        }

        public void Dispose()
        {
            if (_client!=null)
            { 
                if (_client.IsConnected)
                    _client.Disconnect();
                _client.Dispose();

                GC.SuppressFinalize(_client);
            }
        }
    }
}
