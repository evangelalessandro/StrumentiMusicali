using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageOrderSet : FilterEvent
    {
		public ImageOrderSet(enOperationOrder operationOrder, 
            string file,
              Guid key)
            : base(key)
        {
			TipoOperazione = operationOrder;
            File = file;
		}

		public enOperationOrder TipoOperazione { get; private set; }
		public string File { get; private set; }
	}

	public enum enOperationOrder
	{
		ImpostaPrincipale,
		AumentaPriorita,
		DiminuisciPriorita
	}
}