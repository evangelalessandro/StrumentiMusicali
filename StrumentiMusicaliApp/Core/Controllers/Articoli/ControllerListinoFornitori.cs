using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    [AddINotifyPropertyChangedInterface]
    public class ControllerListinoFornitori : BaseControllerGeneric<ListinoPrezziFornitori, ListinoPrezziFornitoriItem>, IDisposable
    {

        private ControllerArticoli _controllerMaster;
        private Subscription<Add<ListinoPrezziFornitori>> _selectSub;

        private Subscription<Remove<ListinoPrezziFornitori>> _subRemove;

        private Subscription<Save<ListinoPrezziFornitori>> _subSave;

        public override MenuTab GetMenu()
        {


            var menu = base.GetMenu();

            foreach (var item in menu.Tabs[0].Pannelli)
            {
                var items = item.Pulsanti.Where(a => a.Tag != null
                && !a.Tag.Equals(MenuTab.TagAdd)
                && !a.Tag.Contains(MenuTab.TagRemove)


                ).ToList();
                item.Pulsanti.RemoveAll(a => items.Contains(a));
                item.Testo = "Listino fornitori";
            }
            var presenti = menu.Tabs[0].Pannelli.Where(a => a.Pulsanti.Count() > 0).First();
            var save = new RibbonMenuButton() { Testo = "Salva", Immagine = Properties.Resources.Save };
            presenti.Pulsanti.Add(save);
            save.Click += Save_Click;
            return menu;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish<Save<ListinoPrezziFornitori>>(new Save<ListinoPrezziFornitori>(this));
        }

        public ControllerListinoFornitori(ControllerArticoli controllerMaster, bool gestioneInline)
            : base(enAmbiente.ListinoPrezziFornitoreList, enAmbiente.ListinoPrezziFornitoreDett, gestioneInline)
        {
            _controllerMaster = controllerMaster;

            SelectedItem = new ListinoPrezziFornitori();


            _selectSub = EventAggregator.Instance().Subscribe<Add<ListinoPrezziFornitori>>((a) =>
            {
                EditItem = new ListinoPrezziFornitori() { ArticoloID = _controllerMaster.EditItem.ID };
                if (!InLineEditor)
                {
                    ShowEditView();
                }
                else
                {
                    DataSourceInRow.Add(EditItem);

                    DataSourceInRow = DataSourceInRow.ToList();


                }
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<ListinoPrezziFornitori>>((a) =>
            {
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (ListinoPrezziFornitori)SelectedItem;
                    if (curItem != null && curItem.ID > 0)
                    {
                        if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                            return;

                        var item = uof.ListinoPrezziFornitoriRepository.Find(b => b.ID == curItem.ID).First();
                        uof.ListinoPrezziFornitoriRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {

                            EventAggregator.Instance().Publish<UpdateList<ListinoPrezziFornitori>>(new UpdateList<ListinoPrezziFornitori>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<ListinoPrezziFornitori>>((a) =>
           {
               EventAggregator.Instance().Publish<ValidateViewEvent<ListinoPrezziFornitori>>(new ValidateViewEvent<ListinoPrezziFornitori>());
               Save(null);
           });

            EventAggregator.Instance().Subscribe<UpdateList<ListinoPrezziFornitori>>((a) =>
            {
                RefreshList(a);
            });
            EventAggregator.Instance().Subscribe<ItemSelected<ListinoPrezziFornitoriItem, ListinoPrezziFornitori>>((a) =>
            {
                SelectedItem = a.ItemSelected.Entity;
            });

        }




        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerListinoFornitori()
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

        public override void RefreshList(UpdateList<ListinoPrezziFornitori> obj)
        {
            try
            {
                var list = new List<ListinoPrezziFornitori>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.ListinoPrezziFornitoriRepository.Find(a => a.ArticoloID == _controllerMaster.EditItem.ID

                    ).Where(a => a.Fornitore.Nome.Contains(TestoRicerca) ||
                    a.Fornitore.RagioneSociale.Contains(TestoRicerca) ||
                    a.Fornitore.Cognome.Contains(TestoRicerca) ||
                    a.Fornitore.PIVA.Contains(TestoRicerca) ||
                    TestoRicerca == "").OrderBy(a => a.Prezzo).ToList();
                }

                DataSourceInRow = (list);
            }
            catch (Exception ex)
            {
                new Action(() =>
                { ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
            }
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
        private void Save(Save<ListinoPrezziFornitori> obj)
        {


            using (var saveManager = new SaveEntityManager())
            {
                SelectedItem.ArticoloID = _controllerMaster.EditItem.ID;
                if (_controllerMaster.EditItem.ID == 0)
                    return;
                var uof = saveManager.UnitOfWork;
                if (SelectedItem.ID > 0)
                {
                    uof.ListinoPrezziFornitoriRepository.Update(SelectedItem);
                }
                else
                {
                    if (uof.ListinoPrezziFornitoriRepository.Find(a => a.ArticoloID == SelectedItem.ArticoloID && a.FornitoreID == SelectedItem.FornitoreID)
                        .Count() > 0)
                    {
                        MessageManager.NotificaWarnig("Esiste già l''associazione articolo fornitore!");
                        return;
                    }
                    uof.ListinoPrezziFornitoriRepository.Add(SelectedItem);

                }


                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {

                    RiselezionaSelezionato();
                }
            }
        }
    }
}