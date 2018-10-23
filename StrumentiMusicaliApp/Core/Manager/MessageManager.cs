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