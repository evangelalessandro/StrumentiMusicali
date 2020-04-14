using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Altro;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerScheduler : BaseControllerGeneric<SchedulerJob, SchedulerItem>, IMenu
    {
        public ControllerScheduler()
            : base(enAmbiente.Scheduler, enAmbiente.Scheduler)
        {
        }

        public override MenuTab GetMenu()
        {
            base.GetMenu().ItemByTag(MenuTab.TagAdd).ForEach(a => a.Visible = false);
            base.GetMenu().ItemByTag(MenuTab.TagEdit).ForEach(a => a.Visible = false);
            return base.GetMenu();
        }

        public override void RefreshList(UpdateList<SchedulerJob> obj)
        {
            try
            {
                var datoRicerca = TestoRicerca;
                using (var uof = new UnitOfWork())
                {
                    var list = uof.SchedulerJobRepository.Find(a => a.Nome.Contains(TestoRicerca)).ToList()
                        .Select(a => new SchedulerItem(a)).ToList();

                    DataSource = new View.Utility.MySortableBindingList<SchedulerItem>(list);

                    base.RefreshList(obj);
                }
            }
            catch (Exception ex)
            {
                new Action(() =>
                { ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
            }
        }
    }
}