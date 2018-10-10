using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core
{
	public class ExceptionManager
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		public static void ManageError(Exception ex)
		{
			MessageManager.NotificaError("Si è verificato un errore nell'ultima operazione");
			_logger.Error("Errore", ex);
		}
	}
}
