using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CheckUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ServerRemoto = CheckUpdate.Properties.Settings.Default.CartellaServer;
            CartellaLocale = CheckUpdate.Properties.Settings.Default.CartellaLocale;
            TipoFile = CheckUpdate.Properties.Settings.Default.tipoFile;
            FileDaAprireAlTermine = CheckUpdate.Properties.Settings.Default.ApplicazioneDaAprire;
            CartelleDaIncludere = CheckUpdate.Properties.Settings.Default.SottoCartelle;

        }
        public string TipoFile { get; set; }
        public string CartelleDaIncludere { get; set; }
        public string ServerRemoto { get; set; }
        public string CartellaLocale { get; set; }
        public string FileDaAprireAlTermine { get; set; }


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
        BackgroundWorker _background;
        private void Form1_Load(object sender, EventArgs e)
        {
            _background = new BackgroundWorker();
            _background.DoWork += Background_DoWork;
            _background.WorkerReportsProgress = true;
            _background.ProgressChanged += Background_ProgressChanged;
            _background.RunWorkerCompleted += _background_RunWorkerCompleted;
            _background.RunWorkerAsync();
        }

        private void _background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            var start = Process.Start(Path.Combine(CartellaLocale
                , FileDaAprireAlTermine));
            MessageBox.Show("Attendere l'apertura dell'applicazione STRUMENTI MUSICALI!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.Hide();
        }

        private void Background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Text = "Aggiornamento... " + e.UserState.ToString();
            Application.DoEvents();
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
