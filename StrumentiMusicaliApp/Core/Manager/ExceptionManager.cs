using NLog;
using StrumentiMusicaliSql.Core;
using System;

namespace StrumentiMusicaliApp.Core
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
					MessageManager.NotificaError(((MessageException)ex).Messages);
				}
				else
				{
					MessageManager.NotificaError("Si è verificato un errore nell'ultima operazione");
				}
			}
			_logger.Error("Errore", ex);
		}
	}
}
