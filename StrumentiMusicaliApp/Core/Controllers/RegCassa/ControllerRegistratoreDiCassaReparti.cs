using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.RegistratoreDiCassa;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerRegistratoreDiCassaReparti : BaseControllerGeneric<RegistratoreDiCassaReparti, RegistratoreDiCassaRepartiItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<RegistratoreDiCassaReparti>> _selectSub;

        private Subscription<Remove<RegistratoreDiCassaReparti>> _subRemove;

        private Subscription<Save<RegistratoreDiCassaReparti>> _subSave;


        public ControllerRegistratoreDiCassaReparti()
            : base(enAmbiente.GruppoCodiceRegCassaArticoli, enAmbiente.GruppoCodiceRegCassaArticoliDettaglio)
        {

            SelectedItem = new RegistratoreDiCassaReparti();


            _selectSub = EventAggregator.Instance().Subscribe<Add<RegistratoreDiCassaReparti>>((a) =>
            {
                EditItem = new RegistratoreDiCassaReparti() { NomeReparto = "Nome " };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<RegistratoreDiCassaReparti>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (RegistratoreDiCassaReparti)SelectedItem;
                    if (curItem.ID > 0)
                    {
                        try
                        {
                            var item = uof.GruppoCodiceRegCassaRepository.Find(b => b.ID == curItem.ID).First();
                            uof.GruppoCodiceRegCassaRepository.Delete(item);

                            if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                            {
                                EventAggregator.Instance().Publish<UpdateList<RegistratoreDiCassaReparti>>(new UpdateList<RegistratoreDiCassaReparti>(this));
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.ManageError(ex);
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<RegistratoreDiCassaReparti>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerRegistratoreDiCassaReparti()
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

        public override void RefreshList(UpdateList<RegistratoreDiCassaReparti> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<RegistratoreDiCassaRepartiItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.RegistratoreDiCassaRepository.Find(a =>
                    a.NomeReparto.Contains(TestoRicerca)
                        ||
                        TestoRicerca == ""
                    ).OrderBy(a => a.NomeReparto).Take(ViewAllItem ? 100000 : 300).
                    ToList().Select(a => 
                    new RegistratoreDiCassaRepartiItem(a)
                    {
                        ID = a.ID,
                        Entity = a,

                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<RegistratoreDiCassaRepartiItem>(list);


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



        private void Save(Save<RegistratoreDiCassaReparti> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.RegistratoreDiCassaRepository.Update(EditItem);
                }
                else
                {
                    uof.RegistratoreDiCassaRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}