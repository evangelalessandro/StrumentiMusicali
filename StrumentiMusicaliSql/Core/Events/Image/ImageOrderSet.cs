using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImageOrderSet<T> : FilterEvent
         where T : Entity.Base.BaseEntity
    {
		public ImageOrderSet(enOperationOrder operationOrder, 
            ImmaginiFile<T> file,
              Guid key)
            : base(key)
        {
			TipoOperazione = operationOrder;
            File = file;
		}

		public enOperationOrder TipoOperazione { get; private set; }
		public ImmaginiFile<T> File { get; private set; }
	}

	public enum enOperationOrder
	{
		ImpostaPrincipale,
		AumentaPriorita,
		DiminuisciPriorita
	}
}