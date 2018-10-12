using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tulpep.NotificationWindow;

namespace StrumentiMusicaliApp.Core
{
	public class MessageManager
	{
		public static void NotificaInfo(string info)
		{
			PopupNotifier popup = new PopupNotifier();
			popup.Image = StrumentiMusicaliApp.Properties.Resources.Info_64;
			popup.TitleText = "Info";
			popup.ContentText = info;
			popup.Popup();
		}
		public static void NotificaError(string message)
		{
			PopupNotifier popup = new PopupNotifier();
			popup.Image = StrumentiMusicaliApp.Properties.Resources.Error_64;
			popup.TitleText = "Error";
			popup.ContentText = message;
			popup.Popup();
		}
	}
}
