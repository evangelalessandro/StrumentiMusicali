﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Utility
{
	public class OrdinaTab : IDisposable
	{
		int _tabindex = 0;

		public void Dispose()
		{
			 
		}

		public void OrderTab(Control parent)
		{
			foreach (var item in UtilityView.FindControlByType<Control>(parent, true).Where(a => a.TabStop == true).GroupBy(a => a.Parent).Select(a => new { a.Key, a }).ToList())
			{
				foreach (var b in item.a.OrderBy(a => a.Top).ThenBy(a => a.Left))
				{
					b.TabIndex = _tabindex;
					_tabindex++;
				}
			}

		}
	}
}
