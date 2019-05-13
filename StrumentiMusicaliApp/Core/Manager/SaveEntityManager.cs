using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Repo;
using System;

namespace StrumentiMusicali.App.Core.Manager
{
	public class SaveEntityManager : IDisposable
	{
		private UnitOfWork uof = new UnitOfWork();

		public void Dispose()
		{
			uof.Dispose();
			uof = null;
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
				MessageManager.NotificaWarnig (ex.Messages);
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