using StrumentiMusicali.App.Core.Controllers;
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
	public abstract partial  class BaseGridViewGeneric<TBaseItem, TController, TEntity> : UserControl
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
		}

		private void Control_Load(object sender, EventArgs e)
		{
			FormatGrid();
		}

		public abstract void FormatGrid();
		

		private void DgvMaster_SelectionChanged(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ItemSelected<TBaseItem,TEntity>(dgvRighe.GetCurrentItemSelected<TBaseItem>()));
		}
		private void DgvRighe_DoubleClick(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish<Edit< TBaseItem,TEntity >> (new Edit<TBaseItem, TEntity > ());
		}
	}
}
