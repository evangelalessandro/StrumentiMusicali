using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace StrumentiMusicali.App.View.BaseControl
{
    public class SettingBaseViewVgrid : BaseDataControl, IMenu, ICloseSave
    {

        public SettingBaseViewVgrid()
           : base()
        {
            InitializeComponent();
            vGrid.CellValueChanged += VGrid_CellValueChanged;
        }
        
        public void ComplexBestFit()
        {
            int maxRowHeaderWidth = -1;
            int maxRecordWidth = -1;

            vGrid.BestFit();
            if (vGrid.RowHeaderWidth > maxRowHeaderWidth)
                maxRowHeaderWidth = vGrid.RowHeaderWidth;
            int recordWidth = CalcBestRecordWidth(vGrid);
            if (recordWidth > maxRecordWidth)
                maxRecordWidth = recordWidth;



            vGrid.RowHeaderWidth = maxRowHeaderWidth;
            //vGrid.RecordWidth = maxRecordWidth;

        }

        public int CalcBestRecordWidth(VGridControl vertGrid)
        {
            int minRecordWidth = 10;
            int recordCount = vertGrid.RecordCount;
            Graphics measureGraphics = vertGrid.CreateGraphics();
            foreach (BaseRow row in vertGrid.Rows)
                if (row.Visible)
                {
                    Font rowFont = row.Appearance.Font;
                    for (int currCell = 0; currCell < recordCount; currCell++)
                    {
                        string text = vertGrid.GetCellDisplayText(row, currCell);
                        int desiredRecordWidth = (int)measureGraphics.MeasureString(text, rowFont).Width;
                        if (desiredRecordWidth > minRecordWidth)
                            minRecordWidth = desiredRecordWidth;
                    }
                }
            return minRecordWidth;
        }
       

       
        
        private void VGrid_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            var tag = (CopyProp)e.Row.Tag;

            tag.SetValue(e.Value);

            //RefreshAllProp();
        }
     

        private void InitializeComponent()
        {
            this.vGrid = new DevExpress.XtraVerticalGrid.VGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // vGrid
            // 
            this.vGrid.Appearance.BandBorder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.BandBorder.Options.UseFont = true;
            this.vGrid.Appearance.BandBorder.Options.UseTextOptions = true;
            this.vGrid.Appearance.BandBorder.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.Category.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.Category.Options.UseFont = true;
            this.vGrid.Appearance.Category.Options.UseTextOptions = true;
            this.vGrid.Appearance.Category.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.CategoryExpandButton.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.CategoryExpandButton.Options.UseFont = true;
            this.vGrid.Appearance.CategoryExpandButton.Options.UseTextOptions = true;
            this.vGrid.Appearance.CategoryExpandButton.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.DisabledRecordValue.BackColor = System.Drawing.Color.Gainsboro;
            this.vGrid.Appearance.DisabledRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.DisabledRecordValue.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseBackColor = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseForeColor = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseTextOptions = true;
            this.vGrid.Appearance.DisabledRecordValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.vGrid.Appearance.DisabledRecordValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.DisabledRow.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.DisabledRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.DisabledRow.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.DisabledRow.Options.UseFont = true;
            this.vGrid.Appearance.DisabledRow.Options.UseForeColor = true;
            this.vGrid.Appearance.DisabledRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.DisabledRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.DisabledRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.Empty.Options.UseFont = true;
            this.vGrid.Appearance.Empty.Options.UseTextOptions = true;
            this.vGrid.Appearance.Empty.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ExpandButton.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ExpandButton.Options.UseFont = true;
            this.vGrid.Appearance.ExpandButton.Options.UseTextOptions = true;
            this.vGrid.Appearance.ExpandButton.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedCell.Options.UseFont = true;
            this.vGrid.Appearance.FocusedCell.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedRecord.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedRecord.Options.UseFont = true;
            this.vGrid.Appearance.FocusedRecord.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedRecord.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedRow.Options.UseFont = true;
            this.vGrid.Appearance.FocusedRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.FocusedRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.HideSelectionRow.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.vGrid.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.HideSelectionRow.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.vGrid.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseFont = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.HideSelectionRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.HideSelectionRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.HorzLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.HorzLine.Options.UseFont = true;
            this.vGrid.Appearance.HorzLine.Options.UseTextOptions = true;
            this.vGrid.Appearance.HorzLine.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ModifiedRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ModifiedRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.ModifiedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ModifiedRow.Options.UseFont = true;
            this.vGrid.Appearance.PressedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.PressedRow.Options.UseFont = true;
            this.vGrid.Appearance.PressedRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.PressedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.PressedRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ReadOnlyRecordValue.BackColor = System.Drawing.Color.Gainsboro;
            this.vGrid.Appearance.ReadOnlyRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ReadOnlyRecordValue.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseBackColor = true;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseForeColor = true;
            this.vGrid.Appearance.ReadOnlyRow.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.ReadOnlyRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ReadOnlyRow.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseBackColor = true;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseFont = true;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseForeColor = true;
            this.vGrid.Appearance.RecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.RecordValue.Options.UseFont = true;
            this.vGrid.Appearance.RecordValue.Options.UseTextOptions = true;
            this.vGrid.Appearance.RecordValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.vGrid.Appearance.RecordValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.RowHeaderPanel.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.RowHeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.RowHeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseBackColor = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseFont = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseForeColor = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseTextOptions = true;
            this.vGrid.Appearance.RowHeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.RowHeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.VertLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.VertLine.Options.UseFont = true;
            this.vGrid.Appearance.VertLine.Options.UseTextOptions = true;
            this.vGrid.Appearance.VertLine.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGrid.LayoutStyle = DevExpress.XtraVerticalGrid.LayoutViewStyle.SingleRecordView;
            this.vGrid.Location = new System.Drawing.Point(0, 0);
            this.vGrid.Name = "vGrid";
            this.vGrid.OptionsBehavior.UseEnterAsTab = true;
            this.vGrid.OptionsFilter.AllowFilter = false;
            this.vGrid.OptionsView.AutoScaleBands = true;
            this.vGrid.OptionsView.FixRowHeaderPanelWidth = true;
            this.vGrid.OptionsView.ShowButtons = false;
            this.vGrid.RecordWidth = 167;
            this.vGrid.RowHeaderWidth = 33;
            this.vGrid.ShowButtonMode = DevExpress.XtraVerticalGrid.ShowButtonModeEnum.ShowOnlyInEditor;
            this.vGrid.Size = new System.Drawing.Size(895, 560);
            this.vGrid.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 560);
            this.Controls.Add(this.vGrid);
            this.Name = "SettingBaseViewVgrid";
            this.Text = "SettingBaseViewVgrid";
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).EndInit();
            this.ResumeLayout(false);

        }


        internal DevExpress.XtraVerticalGrid.VGridControl vGrid;
        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~SettingBaseViewVgrid()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                AggiungiComandi();
            }
            return _menuTab;
        }

        private MenuTab _menuTab = null;


        private void AggiungiComandi()
        {
            var tabArticoli = _menuTab.Add("Principale");
            var pnl = tabArticoli.Add("Comandi");
            UtilityView.AddButtonSaveAndClose(pnl, this);
        }
        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public void RaiseSave()
        {
            if (OnSave != null)
            {
                OnSave(this, new EventArgs());
            }
        }

        public void RaiseClose()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }
    }
}
