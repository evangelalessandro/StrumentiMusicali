using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdaterApplication
{
    public class CheckUpdateFn : IDisposable
    {

        private BackgroundWorker _background;
        public CheckUpdateFn()
        {
            ServerRemoto = UpdaterApplication.Properties.Settings.Default.CartellaServer;
            CartellaLocale = UpdaterApplication.Properties.Settings.Default.CartellaLocale;
            TipoFile = UpdaterApplication.Properties.Settings.Default.tipoFile;
            FileDaAprireAlTermine = UpdaterApplication.Properties.Settings.Default.ApplicazioneDaAprire;
            CartelleDaIncludere = UpdaterApplication.Properties.Settings.Default.SottoCartelle;

            _background = new BackgroundWorker();
            _background.DoWork += Background_DoWork;
            _background.WorkerReportsProgress = true;
            _background.ProgressChanged += Background_ProgressChanged;
            _background.RunWorkerCompleted += _background_RunWorkerCompleted;
            _background.RunWorkerAsync();
        }
        public event EventHandler<string> UpdateText;
        public string CartellaLocale { get; set; }
        public string CartelleDaIncludere { get; set; }
        public string FileDaAprireAlTermine { get; set; }
        public string ServerRemoto { get; set; }
        public string TipoFile { get; set; }
        public void Dispose()
        {
            if (_background != null)
                _background.Dispose();
            _background = null;
        }

        public void StartApplication()
        {
            Process.Start(Path.Combine(CartellaLocale
              , FileDaAprireAlTermine));

            Application.Exit();
        }

        private void _background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Attendere l'apertura dell'applicazione STRUMENTI MUSICALI!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            StartApplication();

        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            GetFiles(ServerRemoto);
            foreach (var item in Directory.GetDirectories(
               ServerRemoto + @"\", this.CartelleDaIncludere, SearchOption.AllDirectories))
            {
                GetFiles(item);
            }
        }

        private void Background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (UpdateText != null)
                UpdateText(this, "Aggiornamento... " + e.UserState.ToString());

            Application.DoEvents();
        }

        private void CheckFile(string file1)
        {
            Application.DoEvents();

            var file = file1.Replace(
                ServerRemoto
                , "");
            if (file.StartsWith(@"\"))
                file = file.Remove(0, 1);
            var path2 = Path.Combine(CartellaLocale
                , file);
            _background.ReportProgress(1, file);

            if (!File.Exists(path2))
            {
                File.Copy(file1, path2);
                return;
            }
            bool filesAreEqual = File.ReadAllBytes(file1).SequenceEqual(File.ReadAllBytes(path2));
            if (!filesAreEqual)
            {
                File.Copy(file1, path2, true);
                return;
            }
            else
            {
            }
        }
        private void GetFiles(string directory)
        {
            foreach (var estensione in TipoFile.Split(';').Where(a => a.Length > 0))
            {
                foreach (var item in Directory.GetFiles(
                   directory, estensione))
                {
                    CheckFile(item);
                }
            }
        }
    }
}
