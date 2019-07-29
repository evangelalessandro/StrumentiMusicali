using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;

namespace StrumentiMusicali.ftpBackup.Ftp
{
    public class FtpManager
    {
        public void Upload(string fileLocal)
        {
            FtpClient client = Connetti();

            client.UploadFile(fileLocal, new System.IO.FileInfo(fileLocal).Name
                , FtpExists.Overwrite, false, FtpVerify.Retry);

            // disconnect! good bye!
            client.Disconnect();
        }
        public void Delete(string remoteFileName)
        {
            FtpClient client = Connetti();

            client.DeleteFile(remoteFileName);

            // disconnect! good bye!
            client.Disconnect();
        }
        public List<FileDirectoryFtp> FileList()
        {
            var retList = new List<FileDirectoryFtp>();
            FtpClient client = Connetti();

            var dato = client.GetListing().ToList();

            retList.AddRange(ConvertiListe(dato));

            foreach (var item in retList.Where(a => a.IsDirectory).ToList())
            {
                var listFile = client.GetListing(item.FullPath).ToList();
                retList.AddRange(ConvertiListe(listFile));
            }
            // disconnect! good bye!
            client.Disconnect();
            return retList;
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

        private FtpClient Connetti()
        {
            var setting = Library.Core.Settings.SettingBackupFtpValidator.ReadSetting();
            // create an FTP client
            FtpClient client = new FtpClient(setting.UrlHost);

            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential(setting.UserName, setting.Password);

            // begin connecting to the server
            client.Connect();

            // upload a file and retry 3 times before giving up
            client.ConnectTimeout = 100;
            if (!string.IsNullOrEmpty(setting.BaseFolder))
                client.SetWorkingDirectory(setting.BaseFolder);
            return client;
        }

    }
}
