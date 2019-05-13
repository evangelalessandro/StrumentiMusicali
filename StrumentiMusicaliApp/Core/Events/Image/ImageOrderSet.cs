using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.Core.Events.Image
{
	public class ImageOrderSet : FilterControllerEvent
    {
		public ImageOrderSet(enOperationOrder operationOrder, FotoArticolo fotoArticolo, BaseController controller)
            : base(controller)
        {
			TipoOperazione = operationOrder;
			FotoArticolo = fotoArticolo;
		}

		public enOperationOrder TipoOperazione { get; private set; }
		public FotoArticolo FotoArticolo { get; private set; }
	}

	public enum enOperationOrder
	{
		ImpostaPrincipale,
		AumentaPriorita,
		DiminuisciPriorita
	}
}