﻿using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.View
{
    public partial class PagamentiListView :
		BaseGridViewGeneric<PagamentoItem, ControllerPagamenti, Pagamento>
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

	 
		public PagamentiListView(ControllerPagamenti controller)
			: base(controller)
		{

            onEditItemShowView += ((a, b) =>
            { b.Cancel = true; });

        }

		  

		public override void FormatGrid()
		{
			
		}


	}
}