using NLog;
using StrumentiMusicali.Library.Core;
using System;

namespace StrumentiMusicali.App.Core.Manager
{
	public class ExceptionManager
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public static void ManageError(Exception ex, bool noShowNotification = false)
		{
			if (!noShowNotification)
			{
				if (ex is MessageException)
				{
					MessageManager.NotificaError(((MessageException)ex).Messages, ex);
				}
				else
				{
					MessageManager.NotificaError("Si è verificato un errore nell'ultima operazione", ex);
				}
			}
			_logger.Error(ex, "Errore", new object[0]);
		}
	}
}