using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.ListiniFornitori
{
    [AddINotifyPropertyChangedInterface]
    public class ControllerListinoPrezziNomi :
        BaseControllerGeneric<ListinoPrezziVenditaNome, ListinoPrezziVenditaNomeItem>, IDisposable
    {
        private Subscription<Add<ListinoPrezziVenditaNome>> _selectSub;

        private Subscription<Remove<ListinoPrezziVenditaNome>> _subRemove;

        private Subscription<Save<ListinoPrezziVenditaNome>> _subSave;

        public ControllerListinoPrezziNomi(bool gestioneInline)
            : base(enAmbiente.ListinoPrezziFornitoreList, enAmbiente.ListinoPrezziFornitoreDett, gestioneInline)
        {

            SelectedItem = new ListinoPrezziVenditaNome();

            _selectSub = EventAggregator.Instance().Subscribe<Add<ListinoPrezziVenditaNome>>((a) =>
            {
                EditItem = new ListinoPrezziVenditaNome();

                ShowEditView();

            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<ListinoPrezziVenditaNome>>((a) =>
            {
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = SelectedItem;
                    if (curItem != null && curItem.ID > 0)
                    {
                        if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                            return;

                        var item = uof.ListinoPrezziVenditaNomeRepository.Find(b => b.ID == curItem.ID).First();
                        uof.ListinoPrezziVenditaNomeRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish(new UpdateList<ListinoPrezziVenditaNome>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<ListinoPrezziVenditaNome>>((a) =>
           {
               EventAggregator.Instance().Publish(new ValidateViewEvent<ListinoPrezziVenditaNome>());
               Save(null);
           });

            EventAggregator.Instance().Subscribe<UpdateList<ListinoPrezziVenditaNome>>((a) =>
            {
                RefreshList(a);
            });
            EventAggregator.Instance().Subscribe<ItemSelected<ListinoPrezziVenditaNomeItem, ListinoPrezziVenditaNome>>((a) =>
            {
                if (a.ItemSelected != null)
                    SelectedItem = a.ItemSelected.Entity;
                else
                {
                    SelectedItem = null;
                }
            });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerListinoPrezziNomi()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public override void Dispose()
        {
            base.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                EventAggregator.Instance().UnSbscribe(_subSave);
                EventAggregator.Instance().UnSbscribe(_selectSub);
                EventAggregator.Instance().UnSbscribe(_subRemove);
            }
            // free native resources if there are any.
        }

        public override MenuTab GetMenu()
        {
            var menu = base.GetMenu();
            //if (InLineEditor)
            //{
            //    int removed = 0;
            //    foreach (var item in menu.Tabs[0].Pannelli)
            //    {
            //        var items = item.Pulsanti.Where(a => a.Tag != null
            //        && (a.Tag.Equals(MenuTab.TagAdd)
            //        || a.Tag.Contains(MenuTab.TagEdit)
            //        || a.Tag.Contains(MenuTab.TagCercaClear)
            //        || a.Tag.Contains(MenuTab.TagCerca)
            //        || a.Tag.Contains(MenuTab.TagRemove)
            //        )

            //        ).ToList();
            //        removed += item.Pulsanti.RemoveAll(a => items.Contains(a));

            //    }
            //    menu.Tabs[0].Pannelli.RemoveAll(a => a.Pulsanti.Count == 0);
            //    if (removed != 0)
            //    {
            //    }
            //}
            return menu;
        }


        public override void RefreshList(UpdateList<ListinoPrezziVenditaNome> obj)
        {
            try
            {

                using (var uof = new UnitOfWork())
                {

                    var listCurrent = uof.ListinoPrezziVenditaNomeRepository.Find(
                     a => a.Nome.Contains(TestoRicerca) ||
                     TestoRicerca == "")
                     .ToList();

                    DataSource = new MySortableBindingList<ListinoPrezziVenditaNomeItem>(listCurrent.Select(a => new ListinoPrezziVenditaNomeItem()
                    { Nome = a.Nome, Entity = a, ID = a.ID }).ToList());
                }


            }
            catch (Exception ex)
            {
                new Action(() =>
                { ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
            }
        }

        private void Save(Save<ListinoPrezziVenditaNome> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (EditItem.ID > 0)
                {
                    uof.ListinoPrezziVenditaNomeRepository.Update(EditItem);
                }
                else
                {
                    uof.ListinoPrezziVenditaNomeRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {

                    RiselezionaSelezionato();
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish(new Save<ListinoPrezziVenditaNome>(this));
        }
    }
}