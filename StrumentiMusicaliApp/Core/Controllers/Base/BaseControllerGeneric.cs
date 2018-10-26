using Autofac;
using Newtonsoft.Json;
using NLog;
using PropertyChanged;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
	
	public abstract class BaseControllerGeneric<TEntity, TBaseItem> : BaseController  
		where TEntity: BaseEntity
		where TBaseItem : BaseItem<TEntity>
	{
		 

		public BaseControllerGeneric()
		{
			Init();
		}
		 

		public abstract void RefreshList(UpdateList<TEntity> obj);
		 
		public void Init()
		{
			EventAggregator.Instance().Subscribe<UpdateList<TEntity>>(RefreshList);

			EventAggregator.Instance().Subscribe<ItemSelected<TBaseItem, TEntity>>(
				(a)=> { SelectedItem = a.ItemSelected.Entity; }
				);

		}


		public BaseEntity SelectedItem { get; set; }
		public List<TBaseItem> DataSource { get; set; } = new List<TBaseItem>();

		internal void UpdateDataSource()
		{
			RefreshList(null);	
		}
	}
}
