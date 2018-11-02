using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl
{
	public abstract partial class BaseGridViewGeneric<TBaseItem, TController, TEntity> : UserControl, IDisposable
		where TEntity : BaseEntity, new()
		where TBaseItem : BaseItem<TEntity>
		where TController : BaseControllerGeneric<TEntity, TBaseItem>
	{
		private System.Windows.Forms.Panel pnlArticoli;
		private System.Windows.Forms.Panel pnlCerca;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCerca;
		private Subscription<ViewRicerca<TEntity>> viewRicerca;
		public TController Controller { get; private set; }

		public BaseGridViewGeneric(TController controllerItem)
		{
			InitializeComponent();
			Init();
			txtCerca.Text = controllerItem.TestoRicerca;
			this.dgvRighe.DoubleClick += DgvRighe_DoubleClick;
			Controller = controllerItem;
			controllerItem.RefreshList(null);
			this.dgvRighe.DataSource = controllerItem.DataSource;
			this.Load += Control_Load;
			this.dgvRighe.SelectionChanged += DgvMaster_SelectionChanged;

			_selectSub = EventAggregator.Instance().
				Subscribe<ItemSelected<TBaseItem, TEntity>>(
				(a) =>
				 {
					 {
						 var T = new Task(() =>
						 {
							 dgvRighe.SelezionaRiga(a.ItemSelected.ID.ToString());
						 });
						 Task.WhenAll(new Task[] { T });
					 };
				 }
			);
			_subEdit = EventAggregator.Instance().Subscribe<Edit<TEntity>>((a) =>
			{
				EditItem();
			});
			viewRicerca = EventAggregator.Instance().Subscribe<ViewRicerca<TEntity>>((a) =>
			{
				bool visibleCerca = pnlCerca.Visible;
				pnlCerca.Visible = !visibleCerca;
				splitter1.Visible = !visibleCerca;
				if (!visibleCerca)
				{
					pnlArticoli.Dock = DockStyle.Fill;
				}
				else
				{
					pnlCerca.Dock = DockStyle.Top;
				}
				UpdateButtonState();
			});
			txtCerca.KeyUp += TxtCerca_KeyUp;
		}

		private Subscription<Edit<TEntity>> _subEdit;

		private void Init()
		{
			this.pnlArticoli = new System.Windows.Forms.Panel();
			this.pnlCerca = new System.Windows.Forms.Panel();
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
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Testo";
			// 
			// txtCerca
			// 
			this.txtCerca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCerca.Location = new System.Drawing.Point(76, 23);
			this.txtCerca.Name = "txtCerca";
			this.txtCerca.Size = new System.Drawing.Size(763, 22);
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
				Controller.RefreshList(null);

				dgvRighe.DataSource = Controller.DataSource;

				dgvRighe.Refresh();

				FormatGrid();

				UpdateButtonState();

			}
		}
		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				// free managed resources
				EventAggregator.Instance().UnSbscribe(_selectSub);

				components.Dispose();
			}
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
			EventAggregator.Instance().UnSbscribe(viewRicerca);
			EventAggregator.Instance().UnSbscribe(_subEdit);
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		private Subscription<ItemSelected<TBaseItem, TEntity>> _selectSub;

		private void Control_Load(object sender, EventArgs e)
		{
			EventAggregator.Instance().Subscribe<UpdateList<TEntity>>(RefreshList);

			FormatGrid();
			UpdateButtonState();
		}

		public abstract void FormatGrid();

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			var item = dgvRighe.GetCurrentItemSelected<TBaseItem>();
			if (item != null && item.Entity != null)
			{
				EventAggregator.Instance().Publish(new ItemSelected<TBaseItem, TEntity>(item));

				Controller.SelectedItem = item.Entity;

				if (SelectionChanged != null)
				{
					SelectionChanged(this, new EventArgs());
				}
			}
		}
		public event EventHandler SelectionChanged;
		public MenuTab GetMenu()
		{
			return Controller.GetMenu();
		}
		/// <summary>
		/// se si risponde con cancel a Si allora non mostra la view generica
		/// </summary>
		public event EventHandler<CancelEventArgs> onEditItemShowView;
		private async void EditItem()
		{
			var item = Controller.ReadSetting().settingSito;
			if (!item.CheckFolderImmagini())
			{
				return;
			}
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
				using (var view = new GenericSettingView(itemSelected.Entity))
				{
					view.OnSave += (a, b) =>
					{
						view.Validate();
						EventAggregator.Instance().Publish<Save<TEntity>>
						(new Save<TEntity>());
					};
					Controller.ShowView(view, Controller.AmbienteDettaglio);
				}
			}
			Controller.RefreshList(null);
			dgvRighe.DataSource = Controller.DataSource;
			dgvRighe.Update();
			FormatGrid();
			await dgvRighe.SelezionaRiga(itemSelected.ID);
		}
		private void UpdateButtonState()
		{
			if (GetMenu() != null)
			{
				var menu = GetMenu();
				menu.Enabled = !(dgvRighe.DataSource == null);

				menu.ApplyValidation(dgvRighe.SelectedRows.Count > 0);
				foreach (var item in menu.ItemByTag(BaseController.TagCerca))
				{
					item.Checked = pnlCerca.Visible;
				}

			}
		}
		private async void RefreshList(UpdateList<TEntity> obj)
		{
			await Task.Run(() =>

			this.InvokeIfRequired((b) =>
			{

				{
					this.Invalidate();

					dgvRighe.DataSource = Controller.DataSource;

					dgvRighe.Refresh();
					dgvRighe.Update();
					FormatGrid();
				}
			}
			));
		}

		private void DgvRighe_DoubleClick(object sender, EventArgs e)
		{
			var g = sender as DataGridView;
			if (g != null)
			{
				var p = g.PointToClient(MousePosition);
				var hti = g.HitTest(p.X, p.Y);
				if (hti.Type == DataGridViewHitTestType.ColumnHeader)
				{
					var columnIndex = hti.ColumnIndex;
					//You handled a double click on column header
					//Do what you need
				}
				else if (hti.Type == DataGridViewHitTestType.RowHeader || hti.Type == DataGridViewHitTestType.Cell)
				{
					var rowIndex = hti.RowIndex;
					//You handled a double click on row header
					EventAggregator.Instance().Publish<Edit<TEntity>>(new Edit<TEntity>());
				}
			}
		}
	}
}