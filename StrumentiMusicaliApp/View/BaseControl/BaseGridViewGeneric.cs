using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.BaseControl
{
	public abstract partial class BaseGridViewGeneric<TBaseItem, TController, TEntity> : UserControl,IDisposable
		where TEntity : BaseEntity
		where TBaseItem : BaseItem<TEntity>
		where TController : BaseControllerGeneric<TEntity, TBaseItem>
	{
		private TController _controller;
		public BaseGridViewGeneric(TController controller)
		{
			InitializeComponent();
			this.dgvRighe.DoubleClick += DgvRighe_DoubleClick;
			_controller = controller;
			this.dgvRighe.DataSource = _controller.DataSource;
			this.Load += Control_Load;
			this.dgvRighe.SelectionChanged += DgvMaster_SelectionChanged;

			_selectSub = EventAggregator.Instance().Subscribe<ItemSelected<TBaseItem,TEntity>>((a)=>
				{
					{ dgvRighe.SelezionaRiga(
						a.ItemSelected.ID.ToString()); };
				}
			);

			
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
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
		 
		Subscription<ItemSelected<TBaseItem, TEntity>> _selectSub;
		private void Control_Load(object sender, EventArgs e)
		{
			FormatGrid();
		}

		public abstract void FormatGrid();


		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			var item = dgvRighe.GetCurrentItemSelected<TBaseItem>();
			if (item != null && item.Entity!=null)
			{
				EventAggregator.Instance().Publish(new ItemSelected<TBaseItem, TEntity>(item));

				_controller.SelectedItem = item.Entity;
			}
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
				else if (hti.Type == DataGridViewHitTestType.RowHeader)
				{
					var rowIndex = hti.RowIndex;
					//You handled a double click on row header
					EventAggregator.Instance().Publish<Edit<TBaseItem, TEntity>>(new Edit<TBaseItem, TEntity>());
				}
			}

		}
	}
}
