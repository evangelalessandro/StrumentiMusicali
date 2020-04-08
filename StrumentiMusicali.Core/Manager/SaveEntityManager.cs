using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;

namespace StrumentiMusicali.Core.Manager
{
    public class SaveEntityManager : IDisposable
    {
        public SaveEntityManager():
            this(true)
        {

        }

        private bool _ShowMessage;
        public SaveEntityManager(bool showMessage)
        {
            _ShowMessage = showMessage;
        }

        private UnitOfWork uof = new UnitOfWork();
        protected virtual void Dispose(bool dispose)
        {
            if (uof != null)
                uof.Dispose();

            uof = null;
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public UnitOfWork UnitOfWork { get { return uof; } }

        public bool SaveEntity(string messaggio)
        {
            try
            {
                using (var cursor = new CursorManager())
                {
                    uof.Commit();
                    if (!string.IsNullOrEmpty(messaggio))
                        ShowMessage(messaggio);
                    return true;
                }
            }
            catch (MessageException ex)
            {
                MessageManager.NotificaWarnig(ex.Messages);
            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
            return false;
        }

        public bool SaveEntity(enSaveOperation operation)
        {
            return SaveEntity(MessageManager.GetMessage(operation));
        }



        private void ShowMessage(string messaggio)
        {
            MessageManager.NotificaInfo(messaggio);
        }
    }

    public enum enSaveOperation
    {
        OpSave,
        OpDelete,
        Duplicazione
    }
}