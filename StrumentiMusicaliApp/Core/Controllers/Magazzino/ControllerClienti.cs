using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerClienti : BaseControllerGeneric<Soggetto, ClientiItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<Soggetto>> _selectSub;

        private Subscription<Remove<Soggetto>> _subRemove;

        private Subscription<Save<Soggetto>> _subSave;

        public ControllerClienti()
            : base(enAmbiente.ClientiList, enAmbiente.Cliente)
        {
            SelectedItem = new Soggetto();

            _selectSub = EventAggregator.Instance().Subscribe<Add<Soggetto>>((a) =>
            {
                EditItem = new Soggetto() { RagioneSociale = "" };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<Soggetto>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (Soggetto)SelectedItem;
                    if (curItem.ID > 0)
                    {
                        var item = uof.ClientiRepository.Find(b => b.ID == curItem.ID).First();
                        uof.ClientiRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish<UpdateList<Soggetto>>(new UpdateList<Soggetto>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<Soggetto>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerClienti()
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

        public override void RefreshList(UpdateList<Soggetto> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<ClientiItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.ClientiRepository.Find(a =>
                    a.RagioneSociale.Contains(TestoRicerca)
                        ||
                    a.Nome.Contains(TestoRicerca)
                    ||
                    a.Cognome.Contains(TestoRicerca)
                    ||
                    a.PIVA.Contains(TestoRicerca)
                    ||
                    a.CodiceFiscale.Contains(TestoRicerca)

                    ||
                        a.Telefono.Contains(TestoRicerca)
                        ||
                        a.Indirizzo.Citta.Contains(TestoRicerca)
                        ||
                        a.Indirizzo.Comune.Contains(TestoRicerca)
                        ||
                        a.Cellulare.Contains(TestoRicerca)
                        ||
                        a.Indirizzo.IndirizzoConCivico.Contains(TestoRicerca)
                        ||
                        TestoRicerca == ""
                    ).OrderBy(a => a.RagioneSociale).Take(ViewAllItem ? 100000 : 300).ToList().Select(a => new ClientiItem(a)
                    {
                        ID = a.ID,
                        Entity = a,
                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<ClientiItem>(list);

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

        private void Save(Save<Soggetto> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.ClientiRepository.Update(EditItem);
                }
                else
                {
                    uof.ClientiRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}