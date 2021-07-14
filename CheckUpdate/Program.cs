using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UpdaterApplication
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

            using (var checkup = new CheckUpdateFn())
            {
                if (checkup.ToUpdate())
                    Application.Run(new Main());
                else
                    checkup.StartDestinationApplication();

            }
        }
    }
}
