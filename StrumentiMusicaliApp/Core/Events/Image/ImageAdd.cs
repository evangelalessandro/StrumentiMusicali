﻿using StrumentiMusicaliSql.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core
{
	public class ImageAdd
	{
		public ImageAdd(Articolo articolo)
		{
			Articolo = articolo;
		}
		public Articolo Articolo { get; private set; }
	}
}
