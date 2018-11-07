﻿using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Enums;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Controllers
{
	//[AddINotifyPropertyChangedInterface]
	public class ControllerLog : BaseControllerGeneric<EventLog, LogItem>, IMenu
	{
		Subscription<Remove<EventLog>> sub1;
		public ControllerLog()
			:base(enAmbiente.LogViewList,enAmbiente.LogViewList)
		{
			sub1 =	EventAggregator.Instance().Subscribe<Remove<EventLog>>(
				(b)=> {
					using (var saveEntity=new SaveEntityManager())
					{
						var uof=saveEntity.UnitOfWork;
						var item = ((EventLog)SelectedItem);
						var sel =uof.EventLogRepository.Find(a => a.ID == item.ID).FirstOrDefault();
						if (sel==null)
						{
							EventAggregator.Instance().Publish<UpdateList<EventLog>>(new UpdateList<EventLog>());
							return;
						}
						uof.EventLogRepository.Delete(sel);
						if (saveEntity.SaveEntity(operation: enSaveOperation.OpDelete))
						{
							EventAggregator.Instance().Publish<UpdateList<EventLog>>(new UpdateList<EventLog>());

						}

					}
				}
			);

		}
		public override MenuTab GetMenu()
		{
			base.GetMenu().ItemByTag(MenuTab.TagAdd).ForEach(a => a.Visible = false);
			base.GetMenu().ItemByTag(MenuTab.TagEdit).ForEach(a => a.Visible = false);
			return base.GetMenu();
		}

		protected override void Dispose(bool disposing)
		{
		 
			if (disposing)
			{
				EventAggregator.Instance().UnSbscribe(sub1);

				//
			}

			// Free any unmanaged objects here.
			//

			// Call base class implementation.
			base.Dispose(disposing);
		}
		public override void RefreshList(UpdateList<EventLog> obj)
		{
			try
			{
				var datoRicerca = TestoRicerca;
				var list = new List<LogItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.EventLogRepository.Find(a => datoRicerca == ""
					   || a.Errore.Contains(datoRicerca)
						|| a.Evento.Contains(datoRicerca)
						|| a.StackTrace.Contains(datoRicerca)
						|| a.InnerException.Contains(datoRicerca)

					).OrderByDescending(a => a.DataCreazione).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new LogItem(a)
					{
						
						Entity = a,

					}).ToList();
				}

				DataSource= new View.Utility.MySortableBindingList<LogItem> (list);
			}
			catch (Exception ex)
			{
				new Action(() =>
				{ ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
			}
		}
	}
}
