using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerStrumenti : ControllerArticoli
    {
        public ControllerStrumenti(bool modalitaRicerca) : base(modalitaRicerca)
        {
        }

        public override void RefreshList(UpdateList<Articolo> obj)
        {
            base.RefreshList(obj);
        }
    }
}
