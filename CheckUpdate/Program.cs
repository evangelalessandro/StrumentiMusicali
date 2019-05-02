using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckUpdate
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
            MessageBox.Show("Controllo aggiornamenti, premere ok e attendere!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Application.Run(new Form1());
        }
    }
}
