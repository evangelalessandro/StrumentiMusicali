using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core
{
	public class CursorHandler
	: IDisposable
	{
		public CursorHandler(Cursor cursor = null)
		{
			_saved = Cursor.Current;
			Cursor.Current = cursor ?? Cursors.WaitCursor;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		~CursorHandler()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
				if (_saved != null)
				{
					Cursor.Current = _saved;
					_saved = null;
				}
			}
		}

		private Cursor _saved;
	}
}
