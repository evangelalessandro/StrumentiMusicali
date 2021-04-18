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
        private DevExpress.XtraGrid.Views.Grid.GridView _dgvScontrino;
        private DevExpress.XtraGrid.GridControl _gcScontrino;
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
            _dgvScontrino.CloseEditor();
            _dgvScontrino.UpdateCurrentRow();

        }
        public Control ControlContainer { get; private set; }


        private void Init(Control control)
        {
            _gcScontrino = new DevExpress.XtraGrid.GridControl();
            _dgvScontrino = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).BeginInit();

            //
            // _gridControlScontrino
            //
            _gcScontrino.Location = new System.Drawing.Point(8, 0);
            _gcScontrino.MainView = _dgvScontrino;
            _gcScontrino.Name = "_gridControlScontrino";
            _gcScontrino.Size = new System.Drawing.Size(411, 244);
            _gcScontrino.TabIndex = 0;
            _gcScontrino.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            _dgvScontrino});
            _gcScontrino.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // gridView1
            //
            _dgvScontrino.GridControl = _gcScontrino;
            //_dgvScontrino.ShowingEditor += _dgvScontrino_ShowingEditor;
            //_dgvScontrino.RowStyle += _dgvScontrino_RowStyle;
            _dgvScontrino.Name = "gridView1";
            _dgvScontrino.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 20, System.Drawing.FontStyle.Bold);
            control.Controls.Add(_gcScontrino);

            ((System.ComponentModel.ISupportInitialize)(_gcScontrino)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(_dgvScontrino)).EndInit();
            UtilityView.InitGridDev(_dgvScontrino);
            _dgvScontrino.OptionsBehavior.Editable = true;
            _dgvScontrino.OptionsView.ShowAutoFilterRow = true;


            emptyEditor = new RepositoryItemButtonEdit();
            emptyEditor.Buttons.Clear();
            emptyEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            _gcScontrino.RepositoryItems.Add(emptyEditor);


            _dgvScontrino.OptionsCustomization.AllowSort = true;


            _dgvScontrino.FocusedRowChanged += DgvMaster_SelectionChanged;

            
        }
        private void DgvMaster_SelectionChanged(object sender, EventArgs e)
        {
            var current = _dgvScontrino.GetRow(_dgvScontrino.FocusedRowHandle);
            
            var item = (TEntity)null;

             
            if (current != null)
            {
                item = (TEntity)current;
            }

            _baseController.SelectedItem = item;


            EventAggregator.Instance().Publish(new ItemSelected<TBaseItem, TEntity>(new TBaseItem() { 
                Entity = item, ID = item ==null ? -1 : item.ID 
            }, _baseController));

            _baseController.UpdateButtonState();
        }
        public void Refreshlist(object datasource)
        {

            _gcScontrino.DataSource = datasource;
            _gcScontrino.RefreshDataSource();

            ItemEditorManager managerEditor = new ItemEditorManager(_gcScontrino, _dgvScontrino);
            managerEditor.BindProp(new TEntity(), "");

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
                _dgvScontrino.Dispose();
                _gcScontrino.Dispose();
            }
        }
    }
}
