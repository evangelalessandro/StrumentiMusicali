using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;

namespace StrumentiMusicali.ftpBackup
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
        public List<string> FileList()
        {
            FtpClient client = Connetti();

            var dato = client.GetNameListing().ToList();

            // disconnect! good bye!
            client.Disconnect();
            return dato;
        }
        private static FtpClient Connetti()
        {
            var setting = StrumentiMusicali.Library.Core.Settings.SettingBackupFtpValidator.ReadSetting();
            // create an FTP client
            FtpClient client = new FtpClient(setting.UrlHost);

            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential(setting.UserName, setting.Password);

            // begin connecting to the server
            client.Connect();



            //// check if a file exists
            //if (client.FileExists("/htdocs/Extra/negozio1.bak"))
            //{
            //    System.Diagnostics.Debug.Print("Prova");
            //}

            //// check if a folder exists
            //if (client.DirectoryExists("/htdocs/extras/")) { }

            // upload a file and retry 3 times before giving up
            client.ConnectTimeout = 100;
            if (!string.IsNullOrEmpty(setting.BaseFolder))
                client.SetWorkingDirectory(setting.BaseFolder);
            return client;
        }
    }
}
