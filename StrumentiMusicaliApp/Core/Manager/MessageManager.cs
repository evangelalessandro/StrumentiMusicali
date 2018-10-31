using StrumentiMusicali.App.Core.Controllers;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace StrumentiMusicali.App.Core
{
	public class MessageManager
	{
		public static void NotificaInfo(string info)
		{
			PopupNotifier popup = new PopupNotifier();
			popup.Image = StrumentiMusicali.App.Properties.Resources.Info_64;
			popup.TitleText = "Info";
			popup.ContentText = info;
			popup.Popup();
		}
		public static string GetMessage(enSaveOperation operation)
		{
			switch (operation)
			{
				case enSaveOperation.OpSave:
					return ("Salvataggio avvenuto con successo");

				case enSaveOperation.OpDelete:
					return ("Cancellazione avvenuta correttamente!");

				case enSaveOperation.Duplicazione:
					return ("Duplicazione avvenuta con successo");

				default:
					break;
			}
			return "";
		}
		public static bool QuestionMessage(string textQuestion)
		{
			return MessageBox.Show(textQuestion, "Domanda", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		public static void NotificaWarnig(string info)
		{
			PopupNotifier popup = new PopupNotifier();
			popup.Image = StrumentiMusicali.App.Properties.Resources.warning_64;
			popup.TitleText = "Attenzione";
			popup.ContentText = info;
			popup.Popup();
		}

		public static void NotificaError(string message, System.Exception ex)
		{
			PopupNotifier popup = new PopupNotifier();
			popup.Image = StrumentiMusicali.App.Properties.Resources.Error_64;
			popup.TitleText = "Error";
			popup.ContentText = message;

			popup.Popup();
			if (ex != null)
			{
				popup.Click += delegate (object sender, System.EventArgs e)
				{
					NotificaError(ex.Message, null);
				};
			}
		}
	}
}