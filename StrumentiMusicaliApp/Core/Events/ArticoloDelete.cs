﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliApp.Core.Events
{
	public class ArticoloDelete : ArticoloCurrent
	{
		public ArticoloDelete(ArticoloItem itemSelected)
			: base(itemSelected)
		{
		}
	}
}
