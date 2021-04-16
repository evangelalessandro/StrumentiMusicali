using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Forms;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;
using System.Windows.Forms;

namespace TestFormApp
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var uof = new UnitOfWork())
            {
                var utente = uof.UtentiRepository.Find(a => a.NomeUtente == "Admin").FirstOrDefault();

                if (utente != null)
                {
                    LoginData.SetUtente(utente);
                }
            }

            using (var controllerArt = new ControllerArticoli(ControllerArticoli.enModalitaArticolo.Tutto))
            {
                controllerArt.RefreshList(null);
                var item = controllerArt.DataSource.First();
                controllerArt.EditItem = item.Entity;
                using (var view = new DettaglioArticoloView(controllerArt))
                {
                    using (var form=new Form())
                    {
                        form.WindowState = FormWindowState.Maximized;
                        form.Controls.Add(view);
                        view.Dock = DockStyle.Fill;
                        form.ShowDialog();
                    }
                    
                }
            }
            Application.Run(new Form2());
        }
    }
}
