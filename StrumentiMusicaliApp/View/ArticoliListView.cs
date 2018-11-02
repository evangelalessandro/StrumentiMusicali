using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.View
{
	public partial class ArticoliListView :
		BaseGridViewGeneric<ArticoloItem, ControllerArticoli, Articolo>, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public new MenuTab GetMenu()
		{
			return Controller.GetMenu();
		}
		public ArticoliListView(ControllerArticoli controller)
			: base(controller)
		{
			onEditItemShowView += ((a, b) =>
			{ b.Cancel = true; });
		}

		 

		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.Columns["ID"].DisplayIndex = 0;
		}


	}
}