using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl
{
    public class UserControlGridViewInlineEdit<TEntity, TBaseItem> : IDisposable
        where TEntity : BaseEntity, new()
        where TBaseItem : BaseItem<TEntity>, new()

    {
        private GridView _dgv;
        private DevExpress.XtraGrid.GridControl _gc;
        private RepositoryItemButtonEdit emptyEditor;
        private BaseControllerGeneric<TEntity, TBaseItem> _baseController;

        ////BaseControllerGeneric<TEntity, TBaseItem> : BaseController, IMenu, IDisposable, ICloseSave //, INotifyPropertyChanged
        //where TEntity : BaseEntity, new()
        //where TBaseItem : BaseItem<TEntity>,  new()
        public UserControlGridViewInlineEdit(BaseControllerGeneric<TEntity, TBaseItem> baseController)
        {
            ControlContainer = new DevExpress.XtraEditors.XtraUserControl();
            ControlContainer.Dock = DockStyle.Fill;
            Init(ControlContainer);
            //_dgvScontrino.InitNewRow += _dgvScontrino_InitNewRow;
            _baseController = baseController;
            //_dgvScontrino.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            //_dgvScontrino.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;

            _subSave = EventAggregator.Instance().Subscribe<ValidateViewEvent<TEntity>>((a) =>
              {
                  CloseEditor();
              });
        }

        private void _dgvScontrino_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns["RecordDate"], DateTime.Today); // Set the new row cell value
            //view.SetRowCellValue(e.RowHandle, view.Columns["Name"], "CustomName");
            //int newRowID = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "ID")); // Obtain the new row cell value 
            //view.SetRowCellValue(e.RowHandle, view.Columns["Notes"], string.Format("Row ID: {0}", newRowID));

        }

        private Subscription<ValidateViewEvent<TEntity>> _subSave;


        private void CloseEditor()
        {
            _dgv.CloseEditor();
            _dgv.UpdateCurrentRow();

        }
        public Control ControlContainer { get; private set; }


        private void Init(Control control)
        {
            _gc = new DevExpress.XtraGrid.GridControl();
            _dgv = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(_gc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_dgv)).BeginInit();

            //
            // _gridControlScontrino
            //
            _gc.Location = new System.Drawing.Point(8, 0);
            _gc.MainView = _dgv;
            _gc.Name = "_gridControlScontrino";
            _gc.Size = new System.Drawing.Size(411, 244);
            _gc.TabIndex = 0;
            _gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            _dgv});
            _gc.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // gridView1
            //
            _dgv.GridControl = _gc;
            //_dgvScontrino.ShowingEditor += _dgvScontrino_ShowingEditor;
            //_dgvScontrino.RowStyle += _dgvScontrino_RowStyle;
            _dgv.Name = "gridView1";
            _dgv.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 20, System.Drawing.FontStyle.Bold);
            control.Controls.Add(_gc);

            ((System.ComponentModel.ISupportInitialize)(_gc)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(_dgv)).EndInit();
            UtilityView.InitGridDev(_dgv);
            _dgv.OptionsBehavior.Editable = true;
            _dgv.OptionsView.ShowAutoFilterRow = true;


            emptyEditor = new RepositoryItemButtonEdit();
            emptyEditor.Buttons.Clear();
            emptyEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            _gc.RepositoryItems.Add(emptyEditor);


            _dgv.OptionsCustomization.AllowSort = true;


            _dgv.FocusedRowChanged += DgvMaster_SelectionChanged;


        }
        List<string> _colListToHide = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomeCol">Nome colonna da nascondere</param>
        internal void ForceHideCol(string nomeCol)
        {
            if (_colListToHide == null)
                _colListToHide = new List<string>();
            _colListToHide.Add(nomeCol);
        }

        private void DgvMaster_SelectionChanged(object sender, EventArgs e)
        {
            var current = _dgv.GetRow(_dgv.FocusedRowHandle);

            var item = (TEntity)null;


            if (current != null)
            {
                item = (TEntity)current;
            }

            _baseController.SelectedItem = item;


            EventAggregator.Instance().Publish(new ItemSelected<TBaseItem, TEntity>(new TBaseItem()
            {
                Entity = item,
                ID = item == null ? -1 : item.ID
            }, _baseController));

            _baseController.UpdateButtonState();
        }
        public void Refreshlist(object datasource)
        {

            _gc.DataSource = datasource;
            _gc.RefreshDataSource();

            ItemEditorManager managerEditor = new ItemEditorManager(_gc, _dgv);
            managerEditor.BindProp(new TEntity(), "");
            /*hide columns*/
            foreach (var item in _dgv.Columns.Where(a => _colListToHide.Contains(a.FieldName)))
            {
                item.Visible = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventAggregator.Instance().UnSbscribe(_subSave);

                _subSave.Dispose();
                _dgv.Dispose();
                _gc.Dispose();
            }
        }
    }
}
