using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using System;
using System.Windows.Forms;
using System.Linq;
using StrumentiMusicali.App.View.Interfaces;

namespace StrumentiMusicali.App.View
{
	public partial class LogView : BaseGridViewGeneric<LogItem, ControllerLog, EventLog>, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		 
 

		public LogView(ControllerLog baseController)
			: base(baseController)
		{
		 
			 
			_logger.Debug(this.Name + " init");

			onEditItemShowView += (a, b) => { b.Cancel = true; };
		 
		}

		 
		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.AutoResizeColumns();
			dgvRighe.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			for (int i = 0; i < dgvRighe.ColumnCount; i++)
			{
				dgvRighe.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			}
			dgvRighe.Columns["ID"].DisplayIndex = 0;
			dgvRighe.Columns["DataCreazione"].DisplayIndex = 1;

		}

		public new MenuTab GetMenu()
		{
			var menu = Controller.GetMenu();
			menu.ItemByTag(MenuTab.TagAdd).First().Visible = false;

			menu.ItemByTag(MenuTab.TagEdit).First().Visible = false;
			return menu;
		}



	}
}