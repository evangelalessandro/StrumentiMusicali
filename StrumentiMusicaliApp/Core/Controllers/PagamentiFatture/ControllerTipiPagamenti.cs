using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Fatture;
using StrumentiMusicali.Library.Repo; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerTipiPagamentiFatture : BaseControllerGeneric<TipiPagamentoDocumenti, TipiPagamentoDocumentiItem>
    {
        private Subscription<Add<TipiPagamentoDocumenti>> _selectSub;

        private Subscription<Remove<TipiPagamentoDocumenti>> _subRemove;

        private Subscription<Save<TipiPagamentoDocumenti>> _subSave;

        public ControllerTipiPagamentiFatture()
            : base(enAmbiente.TipiPagamentiList, enAmbiente.TipiPagamenti)
        {
            SelectedItem = new TipiPagamentoDocumenti();

            _selectSub = EventAggregator.Instance().Subscribe<Add<TipiPagamentoDocumenti>>((a) =>
            {
                EditItem = new TipiPagamentoDocumenti() { Descrizione = "Nuovo tipo pagamento" };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<TipiPagamentoDocumenti>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare il tipo pagamento selezionato?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = SelectedItem;
                    if (curItem.ID > 0)
                    {
                        var item = uof.TipiPagamentoDocumentiRepository.Find(b => b.ID == curItem.ID).First();
                        uof.TipiPagamentoDocumentiRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish<UpdateList<TipiPagamentoDocumenti>>(new UpdateList<TipiPagamentoDocumenti>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<TipiPagamentoDocumenti>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerTipiPagamentiFatture()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
         

        public override void RefreshList(UpdateList<TipiPagamentoDocumenti> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<TipiPagamentoDocumentiItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.TipiPagamentoDocumentiRepository.Find(a =>
                    a.Descrizione.Contains(TestoRicerca) ||
                        TestoRicerca == ""
                    ).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new TipiPagamentoDocumentiItem(a)
                    {
                    }).OrderBy(a => a.Descrizione).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<TipiPagamentoDocumentiItem>(list);

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
                    uof.TipiPagamentoDocumentiRepository.Update(EditItem);
                }
                else
                {
                    uof.TipiPagamentoDocumentiRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}