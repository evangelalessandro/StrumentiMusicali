using System;
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
            this.Activated += Form1_Activated;
            TipoFile = CheckUpdate.Properties.Settings.Default.tipoFile;
            FileDaAprireAlTermine = CheckUpdate.Properties.Settings.Default.ApplicazioneDaAprire;

        }
        public string TipoFile { get; set; }

        private void Form1_Activated(object sender, EventArgs e)
        {

            foreach (var estensione in TipoFile.Split(';').Where(a=>a.Length>0))
            {
                foreach (var item in Directory.GetFiles(
                   ServerRemoto, estensione))
                {
                    CheckFile(item);
                }
            } 

            Process.Start(Path.Combine(CartellaLocale
                , FileDaAprireAlTermine));
            this.Activated -= Form1_Activated;
            this.Hide();
        }

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
    }
}
