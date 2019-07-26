using System;

namespace StrumentiMusicali.App.View.Interfaces
{
    public interface ICloseSave
    {
        event EventHandler<EventArgs> OnSave;
        event EventHandler<EventArgs> OnClose;

        void RaiseSave();
        void RaiseClose();
    }
}
