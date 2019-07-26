using DevExpress.XtraEditors.Repository;
using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Entity;

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
            dgvRighe.BestFitColumns(true);
            dgvRighe.Columns["ID"].VisibleIndex = 0;
            for (int i = 0; i < dgvRighe.Columns.Count; i++)
            {
                //dgvRighe.Columns[i].WrapMode = DataGridViewTriState.True;
            }

            dgvRighe.Columns["DataCreazione"].VisibleIndex = 1;
            dgvRighe.OptionsView.RowAutoHeight = true;
            for (int i = 0; i < dgvRighe.Columns.Count; i++)
            {
                if (dgvRighe.Columns[i].ColumnEdit != null)
                    break;
                if (!dgvRighe.Columns[i].FieldName.Equals("ID", System.StringComparison.InvariantCultureIgnoreCase)
                    &&
                    !dgvRighe.Columns[i].FieldName.Contains("Data")
                    &&
                    !dgvRighe.Columns[i].FieldName.Contains("TipoEvento")

                    )
                {
                    dgvRighe.Columns[i].ColumnEdit = new RepositoryItemMemoEdit();
                }
                if (dgvRighe.Columns[i].FieldName.Contains("TipoEvento")
                    )
                {
                    dgvRighe.Columns[i].ColumnEdit = new RepositoryItemPictureEdit();
                }
            }
        }




    }
}