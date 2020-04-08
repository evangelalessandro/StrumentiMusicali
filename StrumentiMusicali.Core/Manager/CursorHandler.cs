using System;
using System.Windows.Forms;

namespace StrumentiMusicali.Core.Manager
{
    public class CursorManager
    : IDisposable
    {
        public CursorManager(Cursor cursor = null)
        {
            _saved = Cursor.Current;
            if (_saved == null)
            {
                _saved = Cursors.Default;
            }
            Cursor.Current = cursor ?? Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CursorManager()
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