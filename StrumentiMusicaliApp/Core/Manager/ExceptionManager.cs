using NLog;
using StrumentiMusicali.Library.Core;
using System;

namespace StrumentiMusicali.App.Core
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
					MessageManager.NotificaError(((MessageException)ex).Messages,ex);
				}
				else
				{
					MessageManager.NotificaError("Si è verificato un errore nell'ultima operazione",ex);
				}
			}
#pragma warning disable CS0618 // 'ILogger.Error(string, Exception)' è obsoleto: 'Use Error(Exception exception, string message, params object[] args) method instead.'
			_logger.Error("Errore", ex);
#pragma warning restore CS0618 // 'ILogger.Error(string, Exception)' è obsoleto: 'Use Error(Exception exception, string message, params object[] args) method instead.'
		}
	}
}
