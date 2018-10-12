using StrumentiMusicaliSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core
{
	public class ImageRemove
	{
		public ImageRemove(FotoArticolo fotoArticolo)
		{
			FotoArticolo = fotoArticolo;
		}
		public FotoArticolo FotoArticolo { get; private set; }
	}
}
