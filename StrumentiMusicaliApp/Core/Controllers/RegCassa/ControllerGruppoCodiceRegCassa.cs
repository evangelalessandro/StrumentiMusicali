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
    public class ControllerGruppoCodiceRegCassa : BaseControllerGeneric<GruppoCodiceRegCassa, GruppoCodiceRegCassaItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<GruppoCodiceRegCassa>> _selectSub;

        private Subscription<Remove<GruppoCodiceRegCassa>> _subRemove;

        private Subscription<Save<GruppoCodiceRegCassa>> _subSave;


        public ControllerGruppoCodiceRegCassa()
            : base(enAmbiente.GruppoCodiceRegCassaArticoli, enAmbiente.GruppoCodiceRegCassaArticoliDettaglio)
        {

            SelectedItem = new GruppoCodiceRegCassa();


            _selectSub = EventAggregator.Instance().Subscribe<Add<GruppoCodiceRegCassa>>((a) =>
            {
                EditItem = new GruppoCodiceRegCassa() { GruppoRaggruppamento = "Nome " };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<GruppoCodiceRegCassa>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (GruppoCodiceRegCassa)SelectedItem;
                    if (curItem.ID > 0)
                    {
                        try
                        {
                            var item = uof.GruppoCodiceRegCassaRepository.Find(b => b.ID == curItem.ID).First();
                            uof.GruppoCodiceRegCassaRepository.Delete(item);

                            if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                            {
                                EventAggregator.Instance().Publish<UpdateList<GruppoCodiceRegCassa>>(new UpdateList<GruppoCodiceRegCassa>(this));
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.ManageError(ex);
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<GruppoCodiceRegCassa>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerGruppoCodiceRegCassa()
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

        public override void RefreshList(UpdateList<GruppoCodiceRegCassa> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<GruppoCodiceRegCassaItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.GruppoCodiceRegCassaRepository.Find(a =>
                    a.GruppoRaggruppamento.Contains(TestoRicerca)
                        ||
                        TestoRicerca == ""
                    ).OrderBy(a => a.GruppoRaggruppamento).Take(ViewAllItem ? 100000 : 300).
                    ToList().Select(a => 
                    new GruppoCodiceRegCassaItem(a)
                    {
                        ID = a.ID,
                        Entity = a,

                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<GruppoCodiceRegCassaItem>(list);


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



        private void Save(Save<GruppoCodiceRegCassa> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.GruppoCodiceRegCassaRepository.Update(EditItem);
                }
                else
                {
                    uof.GruppoCodiceRegCassaRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}