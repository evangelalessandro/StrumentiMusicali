using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerUtenti : BaseControllerGeneric<Utente, UtentiItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<Utente>> _selectSub;

        private Subscription<Remove<Utente>> _subRemove;

        private Subscription<Save<Utente>> _subSave;


        public ControllerUtenti()
            : base(enAmbiente.UtentiList, enAmbiente.Utente)
        {

            SelectedItem = new Utente();


            _selectSub = EventAggregator.Instance().Subscribe<Add<Utente>>((a) =>
            {
                EditItem = new Utente() { NomeUtente = "Nome utente" };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<Utente>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (Utente)SelectedItem;
                    if (curItem.ID > 0)
                    {
                        try
                        {
                            var item = uof.UtentiRepository.Find(b => b.ID == curItem.ID).First();
                            uof.UtentiRepository.Delete(item);

                            if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                            {
                                EventAggregator.Instance().Publish<UpdateList<Utente>>(new UpdateList<Utente>(this));
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.ManageError(ex);
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<Utente>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerUtenti()
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

        public override void RefreshList(UpdateList<Utente> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<UtentiItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.UtentiRepository.Find(a =>
                    a.NomeUtente.Contains(TestoRicerca)
                        ||
                        TestoRicerca == ""
                    ).OrderBy(a => a.NomeUtente).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new UtentiItem(a)
                    {
                        ID = a.ID,
                        Entity = a,

                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<UtentiItem>(list);


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



        private void Save(Save<Utente> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.UtentiRepository.Update(EditItem);
                }
                else
                {
                    uof.UtentiRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}