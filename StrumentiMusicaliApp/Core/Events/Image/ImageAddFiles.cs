using StrumentiMusicaliSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core
{
	public class ImageAddFiles
	{
		public ImageAddFiles(Articolo articolo,List<string> files)
		{
			Articolo = articolo;
			Files = files;
		}
		public Articolo Articolo { get; private set; }
		public List<string> Files { get; private set; }
	}
}
