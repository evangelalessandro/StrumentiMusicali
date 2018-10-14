using StrumentiMusicaliSql.Entity;

namespace StrumentiMusicaliApp.Core.Events.Image
{
	public  class ImageOrderSet
	{
		public	ImageOrderSet(enOperationOrder operationOrder,FotoArticolo fotoArticolo)
		{
			TipoOperazione = operationOrder;
			FotoArticolo = fotoArticolo;
		}
		public enOperationOrder TipoOperazione { get; private set; }
		public FotoArticolo FotoArticolo{ get; private set; }
	}
	public enum enOperationOrder
	{
		ImpostaPrincipale,
		AumentaPriorita,
		DiminuisciPriorita
	}
}
