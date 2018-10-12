using StrumentiMusicaliSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
