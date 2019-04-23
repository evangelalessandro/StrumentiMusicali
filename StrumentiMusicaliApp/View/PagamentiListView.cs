using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.CustomComponents;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
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