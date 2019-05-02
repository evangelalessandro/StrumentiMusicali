using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Login
{
	public partial class CambioPassword : Form
	{
		public string NomeUtente { get; set; }
		public CambioPassword(string nomeUtente)
		{
			InitializeComponent();
			NomeUtente = nomeUtente;
		}

		private void LoginButton_Click(object sender, EventArgs e)
		{
			/**
			* @desc Do your login thing here, for example we sleep it here for a bit of time
			*/
			using (SaveEntityManager saveEntityManager = new SaveEntityManager())
			{
				// Login success? Open up dashboard
				var uof = saveEntityManager.UnitOfWork;

				var utente = uof.UtentiRepository.Find(a => a.NomeUtente == NomeUtente
				&& a.Password == txtVecchiaPwd.Text).FirstOrDefault();

				if (utente == null)
				{
					MessageManager.NotificaWarnig("Vecchia Password errata");
				}
				else
				{
					if (txtPassword.Text != txtConfermaPwd.Text)
					{
						MessageManager.NotificaWarnig("La nuova password non coincide con quella confermata");
						return;
					}

					utente.Password = txtPassword.Text;
					uof.UtentiRepository.Update(utente);
					saveEntityManager.SaveEntity(enSaveOperation.OpSave);

					this.DialogResult = DialogResult.OK;
					Hide();
				}
			}

		}

		protected void SetControlState(bool loading)
		{
			LoginButton.Enabled = !loading;
			txtVecchiaPwd.Enabled = !loading;
			txtPassword.Enabled = !loading;
		}

		private void btnCambioPassword_Click(object sender, EventArgs e)
		{

		}
	}
}
