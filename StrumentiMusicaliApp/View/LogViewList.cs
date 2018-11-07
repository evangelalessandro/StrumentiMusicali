using NLog;

using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Entity;
using System.Windows.Forms;
using StrumentiMusicali.App.View.Interfaces;

namespace StrumentiMusicali.App.View
{
	public partial class LogViewList : BaseGridViewGeneric<LogItem, ControllerLog, EventLog>, IMenu
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		 
 

		public LogViewList(ControllerLog baseController)
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
		 



	}
}