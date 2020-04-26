using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using System;

namespace StrumentiMusicali.Core.Manager
{
    public class ExceptionManager
    {
         
        public static void ManageError(Exception ex, bool DontShowNotification = false)
        {
            if (!DontShowNotification)
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
            ManagerLog.Logger.Error(ex, "Errore", new object[0]);
        }
    }
}