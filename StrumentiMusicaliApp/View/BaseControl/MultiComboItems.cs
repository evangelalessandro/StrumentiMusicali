using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl
{
    public class MultiComboItems
    {

        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private DevExpress.XtraGrid.GridControl popupGridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView popupGridView2;
        private DevExpress.XtraEditors.PopupContainerEdit popupContainerEdit1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl popupGridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView popupGridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;

        public void InstanceItem()
        {
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.popupGridControl1 = new DevExpress.XtraGrid.GridControl();
            this.popupGridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.popupContainerEdit1 = new DevExpress.XtraEditors.PopupContainerEdit();
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.popupGridControl2 = new DevExpress.XtraGrid.GridControl();
            this.popupGridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            //this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridView2)).BeginInit();

            this.gridControl2.Location = new System.Drawing.Point(12, 36);
            this.gridControl2.MainView = this.gridView1;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(907, 516);
            this.gridControl2.TabIndex = 4;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl2;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsDetail.EnableMasterViewMode = false;


            // 
            // popupGridControl1
            // 
            this.popupGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.popupGridControl1.Location = new System.Drawing.Point(0, 0);
            this.popupGridControl1.MainView = this.popupGridView1;
            this.popupGridControl1.Name = "popupGridControl1";
            this.popupGridControl1.Size = new System.Drawing.Size(931, 261);
            this.popupGridControl1.TabIndex = 0;
            this.popupGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.popupGridView1,
            this.gridView2});
            // 
            // popupGridView1
            // 
            this.popupGridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.popupGridView1.GridControl = this.popupGridControl1;
            this.popupGridView1.Name = "popupGridView1";
            this.popupGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.popupGridView1.OptionsSelection.EnableAppearanceHideSelection = false;
            this.popupGridView1.OptionsSelection.MultiSelect = true;
            this.popupGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.popupGridControl1;
            this.gridView2.Name = "gridView2";
            // 
            // layoutControl1
            // 
            //this.layoutControl1.Controls.Add(this.popupContainerControl1);
            //this.layoutControl1.Controls.Add(this.popupContainerEdit1);
            //this.layoutControl1.Controls.Add(this.gridControl2);


            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Location = new System.Drawing.Point(12, 12);
            this.popupContainerEdit1.Name = "popupContainerEdit1";
            this.popupContainerEdit1.Properties.Buttons.AddRange(new EditorButton[] {
            new EditorButton(ButtonPredefines.Combo)});
            this.popupContainerEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.popupContainerEdit1.Size = new System.Drawing.Size(297, 20);
            //this.popupContainerEdit1.StyleController = this.layoutControl1;
            this.popupContainerEdit1.TabIndex = 5;

            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.popupGridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(279, 238);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(399, 229);
            this.popupContainerControl1.TabIndex = 6;

            // 
            // popupGridControl2
            // 
            this.popupGridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.popupGridControl2.Location = new System.Drawing.Point(0, 0);
            this.popupGridControl2.MainView = this.popupGridView2;
            this.popupGridControl2.Name = "popupGridControl2";
            this.popupGridControl2.Size = new System.Drawing.Size(399, 229);
            this.popupGridControl2.TabIndex = 0;
            this.popupGridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.popupGridView2});
            // 
            // popupGridView2
            // 
            this.popupGridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.popupGridView2.GridControl = this.popupGridControl2;
            this.popupGridView2.Name = "popupGridView2";
            this.popupGridView2.OptionsBehavior.Editable = false;
            this.popupGridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.popupGridView2.OptionsSelection.EnableAppearanceHideSelection = false;
            this.popupGridView2.OptionsSelection.MultiSelect = true;
            this.popupGridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.popupGridView2.OptionsView.ShowGroupPanel = false;
            // 
            // Form1
            // 

            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupGridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridView2)).EndInit();
            //this.ResumeLayout(false);

        }
        public void Init()
        {
            RepositoryItemPopupContainerEdit ri = new RepositoryItemPopupContainerEdit();
            ri.QueryResultValue += ri_QueryResultValue;
            ri.EditValueChanged += ri_EditValueChanged;
            ri.Popup += ri_Popup;
            PopupContainerControl popupControl = new PopupContainerControl();
            popupControl.Controls.Add(popupGridControl1);
            popupControl.Size = new System.Drawing.Size(500, 300);
            ri.PopupControl = popupControl;

            gridControl2.RepositoryItems.Add(ri);
            //gridView1.Columns["Name"].ColumnEdit = ri;

            popupContainerEdit1.Properties.PopupControl = popupContainerControl1;
            popupContainerEdit1.Properties.QueryResultValue += Properties_QueryResultValue;
            popupContainerEdit1.Properties.EditValueChanged += Properties_EditValueChanged;
            popupContainerEdit1.Properties.Popup += Properties_Popup;
        }
        public Control Combo { get { return popupContainerEdit1; } }
        void Properties_Popup(object sender, EventArgs e)
        {
            PopupContainerEdit edit = sender as PopupContainerEdit;
            if (!edit.IsPopupOpen)
                edit.ShowPopup();
            UpdateSelection(edit, popupGridView2);
        }

        void Properties_EditValueChanged(object sender, EventArgs e)
        {
            PopupContainerEdit edit = sender as PopupContainerEdit;
            UpdateSelection(edit, popupGridView2);
        }

        void Properties_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
        {
            int[] selectedRows = popupGridView2.GetSelectedRows();
            List<string> values = new List<string>();
            foreach (int rowHandle in selectedRows)
            {
                values.Add(popupGridView2.GetRowCellValue(rowHandle, "Name").ToString());
            }
            string csv = String.Join(", ", values);
            e.Value = csv;
        }

        void ri_Popup(object sender, EventArgs e)
        {
            PopupContainerEdit edit = sender as PopupContainerEdit;
            UpdateSelection(edit, popupGridView1);
        }

        void ri_EditValueChanged(object sender, EventArgs e)
        {
            PopupContainerEdit edit = sender as PopupContainerEdit;
            if (!edit.IsPopupOpen)
                edit.ShowPopup();
            UpdateSelection(edit, popupGridView1);
        }

        void ri_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
        {
            int[] selectedRows = popupGridView1.GetSelectedRows();
            List<string> values = new List<string>();
            foreach (int rowHandle in selectedRows)
            {
                values.Add(popupGridView1.GetRowCellValue(rowHandle, "Name").ToString());
            }
            string csv = String.Join(", ", values);
            e.Value = csv;
        }

        private void UpdateSelection(PopupContainerEdit edit, GridView view)
        {
            view.BeginSelection();
            view.ClearSelection();
            if (edit != null)
                if (edit.EditValue != null)
                {
                    edit.Focus();
                    List<int> selection = GetSelection(edit.EditValue.ToString().Split(new string[] { ", " }, StringSplitOptions.None), "Name", view);
                    foreach (int rowHandle in selection)
                    {
                        view.SelectRow(rowHandle);
                    }
                }
            view.EndSelection();
        }

        private List<int> GetSelection(string[] values, string fieldName, GridView view)
        {
            List<int> selection = new List<int>();
            foreach (string val in values)
            {
                for (int i = 0; i < view.RowCount; i++)
                {
                    if (view.GetRowCellValue(i, fieldName).ToString() == val)
                        selection.Add(i);
                }
            }
            return selection;
        }

       public void InitDati()
        {
            popupGridControl2.DataSource = CreateList(5);
        }
        public BindingList<object> CreateList(int count)
        {
            var list = new BindingList<object>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new { ID = i, Name = "Name" + i, Info = "Info" + i });
            }
            return list;
        }
    }
}
