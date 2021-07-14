using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UpdaterApplication
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

        }
        Timer _tmr = new Timer();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            _tmr.Interval = 100;
            _tmr.Tick += Tmr_Tick;
            _tmr.Start();
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            _tmr.Stop();
            var checkup = new CheckUpdateFn();

            checkup.UpdateText += (o, i) =>
            {
                this.Text = i;
            };
            checkup.CloseForm += (o, i) =>
            {


                MessageBox.Show(this, "Attendere l'apertura dell'applicazione STRUMENTI MUSICALI!", 
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


            };

            MessageBox.Show(this,"Controllo aggiornamenti, premere ok e attendere!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkup.StartUpdate();
           
        }
    }
}