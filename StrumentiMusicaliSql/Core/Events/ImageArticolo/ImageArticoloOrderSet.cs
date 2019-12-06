using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageArticoloOrderSet : FilterControllerEvent
    {
        public ImageArticoloOrderSet(enOperationOrder operationOrder, FotoArticolo fotoArticolo, interfaces.IKeyController controller)
            : base(controller)
        {
            TipoOperazione = operationOrder;
            FotoArticolo = fotoArticolo;
        }

        public enOperationOrder TipoOperazione { get; private set; }
        public FotoArticolo FotoArticolo { get; private set; }
    }

}