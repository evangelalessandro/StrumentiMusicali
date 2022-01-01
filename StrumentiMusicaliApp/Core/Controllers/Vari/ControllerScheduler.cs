using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Entity.Altro;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    public class ControllerScheduler : BaseControllerGeneric<SchedulerJob, SchedulerItem>
    {
        public ControllerScheduler()
            : base(enAmbiente.Scheduler, enAmbiente.SchedulerDetail)
        {
            AggiungiComandiMenu();
            _subSave = EventAggregator.Instance().Subscribe<Save<SchedulerJob>>((a) =>
            {
                Save(null);
            });
        }
        private Subscription<Save<SchedulerJob>> _subSave;

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                EventAggregator.Instance().UnSbscribe(_subSave);
            }
        }
        private void Save(Save<SchedulerJob> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.SchedulerJobRepository.Update(EditItem);
                }
                else
                {
                    uof.SchedulerJobRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
        private void AggiungiComandiMenu()
        {
            GetMenu().Tabs[0].Pannelli[1].Visible=false;
            var menu=GetMenu();
            base.GetMenu().ItemByTag(MenuTab.TagAdd).ForEach(a => a.Visible = false);
            base.GetMenu().ItemByTag(MenuTab.TagRemove).ForEach(a => a.Visible = false);
            base.GetMenu().ItemByTag(MenuTab.TagEdit).ForEach(a => a.Visible = true);
            base.GetMenu().ItemByTag(MenuTab.TagCerca).ForEach(a => a.Visible = false);

            menu.Tabs[0].Add("Backup").Add("Esegui Backup",Properties.Resources.BackupDatabase).Click += backup_Click;

            //var rib1 = pnl.Add("Unisci", StrumentiMusicali.Core.Properties.ImageIcons.Merge_64, true);
            //rib1.Click += (a, e) =>
            //{
            //    EventAggregator.Instance().Publish<ArticoloMerge>(new ArticoloMerge(this));
            //};

        }

        private void backup_Click(object sender, EventArgs e)
        {

            try
            {
                ManagerLog.Logger.Info("Backup manuale inizio");


                using (var uof = new UnitOfWork())
                {
                    uof.EseguiBackup();

                    using (var ftpManager = new ftpBackup.Backup.BackupManager())
                    {
                        ftpManager.Manage();

                        ManagerLog.Logger.Info("Backup Effettuato correttamente");
                    }
                }
            }
            catch (Exception ex)
            {
                ManagerLog.Logger.Error(ex, "Nella fase di backup automatico");
                if (Environment.UserInteractive)
                {
                    MessageManager.NotificaWarnig(ex.Message);
                }
            }
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