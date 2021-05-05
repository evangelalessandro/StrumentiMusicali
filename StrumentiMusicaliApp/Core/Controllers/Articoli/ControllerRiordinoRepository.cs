using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Fatture;
using StrumentiMusicali.Library.Repo; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerRiordinoPeriodi : BaseControllerGeneric<RiordinoPeriodi, RiordinoPeriodiItem>
    {
        private Subscription<Add<RiordinoPeriodi>> _selectSub;

        private Subscription<Remove<RiordinoPeriodi>> _subRemove;

        private Subscription<Save<RiordinoPeriodi>> _subSave;

        public ControllerRiordinoPeriodi()
            : base(enAmbiente.RiordinoPeriodiList, enAmbiente.RiordinoPeriodi)
        {
            SelectedItem = new RiordinoPeriodi();

            _selectSub = EventAggregator.Instance().Subscribe<Add<RiordinoPeriodi>>((a) =>
            {
                EditItem = new RiordinoPeriodi() { Descrizione = "Nuovo periodo" };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<RiordinoPeriodi>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare il periodo selezionato?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = SelectedItem;
                    if (curItem.ID > 0)
                    {
                        var item = uof.RiordinoPeriodiRepository.Find(b => b.ID == curItem.ID).First();
                        uof.RiordinoPeriodiRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish<UpdateList<RiordinoPeriodi>>(new UpdateList<RiordinoPeriodi>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<RiordinoPeriodi>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerRiordinoPeriodi()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
         

        public override void RefreshList(UpdateList<RiordinoPeriodi> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<RiordinoPeriodiItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.RiordinoPeriodiRepository.Find(a =>
                    a.Descrizione.Contains(TestoRicerca) ||
                        TestoRicerca == ""
                    ).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new RiordinoPeriodiItem(a)
                    {
                    }).OrderBy(a => a.Descrizione).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<RiordinoPeriodiItem>(list);

                base.RefreshList(obj);
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

        private void Save(Save<Deposito> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.RiordinoPeriodiRepository.Update(EditItem);
                }
                else
                {
                    uof.RiordinoPeriodiRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}