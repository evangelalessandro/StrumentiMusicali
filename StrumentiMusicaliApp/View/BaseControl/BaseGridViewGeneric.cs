﻿using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NLog;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl
{
    public partial class BaseGridViewGeneric<TBaseItem, TController, TEntity> : UserControl, IKeyController, IDisposable, Interfaces.ICloseSave
        where TEntity : BaseEntity, new()
        where TBaseItem : BaseItem<TEntity>, new()
        where TController : BaseControllerGeneric<TEntity, TBaseItem>
    {
        private System.Windows.Forms.Panel pnlGridView;
        protected System.Windows.Forms.FlowLayoutPanel pnlCerca;
        internal readonly ILogger _logger = ManagerLog.Logger;

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCerca;
        private Subscription<ViewRicerca<TEntity>> viewRicerca;
        public TController Controller { get; private set; }
        public Guid INSTANCE_KEY
        {
            get
            {
                if (this.Controller != null)
                    return this.Controller._INSTANCE_KEY;
                return Guid.Empty;
            }

        }
        public bool InLine { get; private set; }
        public BaseGridViewGeneric(TController controllerItem, bool inLine = false, bool multiSelectRow = false)
        {
            InitializeComponent();

            _multiSelect = multiSelectRow;

            InLine = inLine;

            Init();

            txtCerca.DataBindings.Add("Text", controllerItem, "TestoRicerca");
            txtCerca.Text = controllerItem.TestoRicerca;

            this.dgvRighe.DoubleClick += DgvRighe_DoubleClick;
            Controller = controllerItem;

            ((INotifyPropertyChanged)controllerItem).PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "DataSource")
                {
                    this.gcControl.DataSource = controllerItem.DataSource;
                }
            };
            controllerItem.RefreshList(null);


            this.Load += Control_Load;
            if (File.Exists(getLayoutFile()))
            {
                dgvRighe.RestoreLayoutFromXml(getLayoutFile());
            }
            dgvRighe.ClearColumnsFilter();


            this.dgvRighe.FocusedRowChanged += DgvMaster_SelectionChanged;
            this.dgvRighe.SelectionChanged+= DgvMaster_SelectionChanged;


            _selectSub = EventAggregator.Instance().
                Subscribe<ItemSelected<TBaseItem, TEntity>>(
                (a) =>
                 {
                     {
                         if (a.ItemSelected != null)

                             dgvRighe.SelezionaRiga(a.ItemSelected.ID);

                     };
                 }
            );
            _subEdit = EventAggregator.Instance().Subscribe<Edit<TEntity>>((a) =>
            {
                Controller.EditItem = Controller.SelectedItem;
                EditItemView();
            });
            viewRicerca = EventAggregator.Instance().Subscribe<ViewRicerca<TEntity>>((a) =>
            {

                Controller.PannelloRicercaVisibile = !Controller.PannelloRicercaVisibile;
                pnlCerca.Visible = Controller.PannelloRicercaVisibile;
                splitter1.Visible = Controller.PannelloRicercaVisibile;
                if (Controller.PannelloRicercaVisibile)
                {
                    pnlGridView.Dock = DockStyle.Fill;
                }
                else
                {
                    pnlCerca.Dock = DockStyle.Top;
                }
                Controller.UpdateButtonState();
            });
            txtCerca.KeyUp += TxtCerca_KeyUp;
        }


        private Subscription<Edit<TEntity>> _subEdit;

        private void Init()
        {
            this.pnlGridView = new System.Windows.Forms.Panel();
            this.pnlCerca = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCerca = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            // 
            // pnlCerca
            // 
            this.pnlCerca.Controls.Add(this.label1);
            this.pnlCerca.Controls.Add(this.txtCerca);
            this.pnlCerca.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlCerca.Location = new System.Drawing.Point(0, 0);
            this.pnlCerca.Name = "pnlCerca";
            this.pnlCerca.Size = new System.Drawing.Size(851, 71);
            this.pnlCerca.TabIndex = 3;
            this.pnlCerca.FlowDirection = FlowDirection.LeftToRight | FlowDirection.TopDown;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Testo - Codice a barre";
            // 
            // txtCerca
            // 
            //this.txtCerca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.txtCerca.Location = new System.Drawing.Point(76, 23);
            this.txtCerca.Name = "txtCerca";
            this.txtCerca.Size = new System.Drawing.Size(400, 22);
            this.txtCerca.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.PowderBlue;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 71);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(851, 11);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlCerca);

        }

        private void TxtCerca_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Controller.TestoRicerca = txtCerca.Text;
                RicercaRefresh();


            }
        }

        public void RicercaRefresh()
        {
            Controller.RefreshList(null);

            dgvRighe.RefreshData();

            ForceRefreshSelectItem();
        }
        private string getLayoutFile()
        {
            var dir = Application.StartupPath + @"\Layout\";
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            return dir + "Layout_Grid_" + this.Name + ".xml";
        }
        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            dgvRighe.SaveLayoutToXml(getLayoutFile());
            if (disposing && (components != null))
            {
                // free managed resources

                components.Dispose();
            }
            EventAggregator.Instance().UnSbscribe(_selectSub);
            EventAggregator.Instance().UnSbscribe(viewRicerca);
            EventAggregator.Instance().UnSbscribe(_subEdit);

            base.Dispose(disposing);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~BaseGridViewGeneric()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        private Subscription<ItemSelected<TBaseItem, TEntity>> _selectSub;
        private bool _multiSelect;
        private void Control_Load(object sender, EventArgs e)
        {
            EventAggregator.Instance().Subscribe<UpdateList<TEntity>>(RefreshList);

            UtilityView.InitGridDev(dgvRighe, _multiSelect);

            dgvRighe.OptionsBehavior.Editable = InLine;

            FormatNameColumn();

            ForceRefreshSelectItem();
        }

        ItemEditorManager _manager = null;
        private void FormatNameColumn()
        {
            if (dgvRighe.Columns["ID"] != null)
            {
                dgvRighe.Columns["ID"].VisibleIndex = 0;
            }
            if (dgvRighe.Columns["Entity"] != null)
            {
                dgvRighe.Columns["Entity"].Visible = false;
            }
            for (int i = 0; i < dgvRighe.Columns.Count; i++)
            {
                dgvRighe.Columns[i].Caption =
                    UtilityView.GetTextSplitted(dgvRighe.Columns[i].Caption);

                if (dgvRighe.Columns[i].FieldName.Contains("Data"))
                {
                    dgvRighe.Columns[i].DisplayFormat.FormatString = "G";
                }
            }

            if (dgvRighe.Columns.Count()>0 &&  _manager == null)
            {
                _manager = new ItemEditorManager(gcControl, dgvRighe);
                _manager.BindProp(new TBaseItem(), "");
            }

            if (dgvRighe.Columns["ID"] != null)
            {
                dgvRighe.Columns["ID"].VisibleIndex = 0;
                dgvRighe.Columns["ID"].OptionsColumn.AllowEdit = false;
            }
        }
        public virtual void FormatGrid() { }

        private void DgvMaster_SelectionChanged(object sender, EventArgs e)
        {
            var current = dgvRighe.GetRow(dgvRighe.FocusedRowHandle);
            var item = (TBaseItem)current;

            if (item != null && item.Entity != null)
            {

                Controller.SelectedItem = item.Entity;


            }
            else
            {
                Controller.SelectedItem = null;

            }
            if (_multiSelect)
                Controller.SelectedItems = dgvRighe.GetSelectedRows<TBaseItem>();

            EventAggregator.Instance().Publish(new ItemSelected<TBaseItem, TEntity>(item, Controller));

            Controller.UpdateButtonState();


        }
        public MenuTab GetMenu()
        {
            return Controller.GetMenu();
        }
        /// <summary>
        /// se si risponde con cancel a Si allora non mostra la view generica
        /// </summary>
        public event EventHandler<CancelEventArgs> onEditItemShowView;
        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        private void EditItemView()
        {
            bool skipView = false;
            var itemSelected = UtilityView.GetCurrentItemSelected<TBaseItem>(dgvRighe);
            if (onEditItemShowView != null)
            {
                var canc = new CancelEventArgs();
                onEditItemShowView(this, canc);
                if (canc.Cancel)
                {
                    skipView = true;
                    //return;
                }
            }
            if (!skipView)
            {
                Controller.ShowEditView();
            }
            Controller.RefreshList(null);
            gcControl.DataSource = Controller.DataSource;
            dgvRighe.RefreshData();
            FormatGrid();
            if (itemSelected != null)
                dgvRighe.SelezionaRiga(itemSelected.ID);
        }




        private void RefreshList(UpdateList<TEntity> obj)
        {

            ForceUpdateGridAsync();

            ForceRefreshSelectItem();

        }

        private void ForceRefreshSelectItem()
        {
            FormatGrid();

            DgvMaster_SelectionChanged(gcControl, null);

        }

        private void ForceUpdateGridAsync()
        {
            this.Invalidate();

            gcControl.DataSource = Controller.DataSource;

            dgvRighe.RefreshData();
            //dgvRighe.Update();

        }

        private void DgvRighe_DoubleClick(object sender, EventArgs e)
        {
            var g = sender as GridView;
            if (g != null)
            {

                if (g == null) return;

                if (InLine)
                    return;
                var location = gcControl.PointToClient(Cursor.Position);
                // Get a View at the current point.
                // Retrieve information on the current View element.
                GridHitInfo gridHI = g.CalcHitInfo(location);
                if (gridHI != null)
                    Text = gridHI.HitTest.ToString();
                //var p = g.CalcHitInfo(MousePosition);
                if (gridHI.InRowCell)
                {

                    //You handled a double click on row header
                    EventAggregator.Instance().Publish<Edit<TEntity>>(new Edit<TEntity>(Controller));
                }
            }
        }

        public void RaiseSave() => OnSave(this, new EventArgs());


        public void RaiseClose() => OnClose(this, new EventArgs());

    }
}