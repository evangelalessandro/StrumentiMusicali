using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            using (var uof = new UnitOfWork())
            {
                if (uof.UtentiRepository.Find(a => true).Count() == 0)
                {
                    uof.UtentiRepository.Add(new Library.Entity.Utente()
                    {
                        NomeUtente = "Admin",
                        Password = "Admin",
                        AdminUtenti = true,
                        Fatturazione = true,
                        Magazzino = true,
                        //ScontaArticoli = true
                    });
                    uof.Commit();
                    txtUsername.Text = "Admin";
                    txtPassword.Text = "Admin";

                }
            }
            SetControlState(false);
            this.KeyPreview = true;
            this.KeyDown += LoginForm_KeyDown1;
        }

        private void LoginForm_KeyDown1(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }



        private async void LoginButton_Click(object sender, EventArgs e)
        {
            SetControlState(true);
            /**
             * @desc Do your login thing here, for example we sleep it here for a bit of time
             */
            await Task.Run(() => Thread.Sleep(2 * 1000));
            // Login success? Open up dashboard
            using (var uof = new UnitOfWork())
            {
                var utente = uof.UtentiRepository.Find(a => a.NomeUtente == txtUsername.Text && a.Password == txtPassword.Text).FirstOrDefault();

                if (utente == null)
                {
                    if (uof.UtentiRepository.Find(a => a.NomeUtente == txtUsername.Text).Count() > 0)
                    {
                        MessageManager.NotificaWarnig("Password errata");
                    }
                    else
                    {
                        MessageManager.NotificaWarnig("Nome utente non trovato");

                    }
                    SetControlState(false);

                }
                else
                {
                    LoginData.SetUtente(utente);
                    this.DialogResult = DialogResult.OK;
                    Hide();
                }
            }

        }

        protected void SetControlState(bool loading)
        {
            LoginButton.Enabled = !loading;
            btnCambioPassword.Enabled = !loading;
            txtUsername.Enabled = !loading;
            txtPassword.Enabled = !loading;
            LoginStatusStrip.Visible = loading;
            LoginProgressBar.Style = loading ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        }

        private void btnCambioPassword_Click(object sender, EventArgs e)
        {
            using (var uof = new UnitOfWork())
            {
                var utente = uof.UtentiRepository.Find(a => a.NomeUtente == txtUsername.Text).FirstOrDefault();
                if (utente != null)
                {
                    using (var cambioPwd = new Login.CambioPassword(utente.NomeUtente))
                    {
                        cambioPwd.ShowDialog();
                    }
                }
                else
                {
                    MessageManager.NotificaWarnig("Nome utente non trovato");
                }
            }
        }
    }
}
