﻿using PropertyChanged;
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
    [AddINotifyPropertyChangedInterface]
    public class ControllerRigheFatture : BaseControllerGeneric<FatturaRiga, FatturaRigaItem>, IDisposable
    {
        private Subscription<RemovePriority<FatturaRiga>> _addPrio;
        private ControllerFatturazione _controllerFatturazione;
        private Subscription<AddPriority<FatturaRiga>> _removePrio;
        private Subscription<Add<FatturaRiga>> _selectSub;

        private Subscription<Remove<FatturaRiga>> _subRemove;

        private Subscription<Save<FatturaRiga>> _subSave;

        public EnTipoDocumento TipoDocFattura { get; private set; }
        public ControllerRigheFatture(ControllerFatturazione controllerFatturazione)
            : base(enAmbiente.FattureRigheList, enAmbiente.FattureRigheDett)
        {
            _controllerFatturazione = controllerFatturazione;
            TipoDocFattura = _controllerFatturazione.EditItem.TipoDocumento;
            SelectedItem = new FatturaRiga();

            _removePrio = EventAggregator.Instance().Subscribe<AddPriority<FatturaRiga>>((a) => { CambiaPriorita(true); }); ;
            _addPrio = EventAggregator.Instance().Subscribe<RemovePriority<FatturaRiga>>((a) => { CambiaPriorita(false); }); ;

            EventAggregator.Instance().Subscribe<Save<FatturaRiga>>((a) =>
            {
            });
            _selectSub = EventAggregator.Instance().Subscribe<Add<FatturaRiga>>((a) =>
        {
            EditItem = new FatturaRiga() { IvaApplicata = "22", Fattura = _controllerFatturazione.EditItem };

            ShowEditView();
        });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<FatturaRiga>>((x) =>
            {
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (FatturaRiga)SelectedItem;
                    if (curItem != null && curItem.ID > 0)
                    {
                        if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                            return;

                        if (saveManager.UnitOfWork.FatturaRepository.Find(a => a.ID == curItem.FatturaID).First().ChiusaSpedita)
                        {
                            MessageManager.NotificaWarnig("Documento già chiuso, non è possibile modificare altro!");
                            return;
                        }
                        var item = uof.FattureRigheRepository.Find(b => b.ID == curItem.ID).First();
                        uof.FattureRigheRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            AggiornaTotaliFattura();

                            EventAggregator.Instance().Publish<UpdateList<FatturaRiga>>(new UpdateList<FatturaRiga>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<FatturaRiga>>((a) =>
           {
               Save(null);
           });
        }

        public const string KEYEXISTsRiga = "FatturaRIGA";

        private void AggiornaTotaliFattura()
        {
            var item = ControllerFatturazione.CalcolaTotali(
                    _controllerFatturazione.EditItem);

            _controllerFatturazione.EditItem = item;

            EventAggregator.Instance().Publish(new RebindItemUpdated<Fattura>(_controllerFatturazione));
            EventAggregator.Instance().Publish(new Save<Fattura>(_controllerFatturazione));
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerRigheFatture()
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

        public override void RefreshList(UpdateList<FatturaRiga> obj)
        {
            try
            {
                var list = new List<FatturaRigaItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.FattureRigheRepository.Find(a => a.FatturaID == _controllerFatturazione.EditItem.ID

                    ).Where(a => a.Descrizione.Contains(TestoRicerca) ||
                    TestoRicerca == "").Select(a => new { Riga = a, a.Fattura }).ToList().Select(a => a.Riga).Select(a => new FatturaRigaItem
                    {
                        ID = a.ID,
                        //CodiceArt = a.CodiceArticoloOld,
                        RigaDescrizione = a.Descrizione,
                        Entity = a,
                        RigaImporto = a.Qta * a.PrezzoUnitario,
                        PrezzoUnitario = a.PrezzoUnitario,
                        RigaQta = a.Qta,
                        Iva = a.IvaApplicata,
                        CodiceFornitore = a.CodiceFornitore,
                        Evasi = a.Evasi,
                        TipoDoc = a.Fattura.TipoDocumento,
                        Ricevuti = a.Ricevuti

                    }).OrderBy(a => a.Entity.OrdineVisualizzazione).ThenBy(a => a.ID).ToList();

                    
                }

                DataSource = new View.Utility.MySortableBindingList<FatturaRigaItem>(list);
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
                EventAggregator.Instance().UnSbscribe(_addPrio);
                EventAggregator.Instance().UnSbscribe(_removePrio);
            }
            // free native resources if there are any.
        }

        private void CambiaPriorita(bool aumenta)
        {
            if (_controllerFatturazione.EditItem.ID == 0)
                return;

            var itemSel = ((FatturaRiga)SelectedItem);
            if (itemSel.ID == 0)
            {
                return;
            }
            else
            {
                try
                {
                    using (var curs = new CursorManager())
                    {
                        using (var save = new SaveEntityManager())
                        {
                            var uof = save.UnitOfWork;
                            var list = uof.FattureRigheRepository.Find(
                                a => a.FatturaID == itemSel.FatturaID).OrderBy(a => a.OrdineVisualizzazione).ToList();
                            foreach (var item in list)
                            {
                                if (item.ID == itemSel.ID)
                                {
                                    if (aumenta)
                                    {
                                        var itemToUpdate = list.Where(a => a.OrdineVisualizzazione == item.OrdineVisualizzazione - 1).FirstOrDefault();
                                        if (itemToUpdate != null)
                                        {
                                            itemToUpdate.OrdineVisualizzazione++;
                                        }
                                        item.OrdineVisualizzazione--;
                                    }
                                    else
                                    {
                                        var itemToUpdateTwo = list.Where(a => a.OrdineVisualizzazione == item.OrdineVisualizzazione + 1).FirstOrDefault();
                                        if (itemToUpdateTwo != null)
                                        {
                                            itemToUpdateTwo.OrdineVisualizzazione--;
                                        }
                                        item.OrdineVisualizzazione++;
                                    }
                                }
                            }
                            var setOrdine = 0;
                            foreach (var item in list.OrderBy(a => a.OrdineVisualizzazione))
                            {
                                item.OrdineVisualizzazione = setOrdine;
                                setOrdine++;
                                uof.FattureRigheRepository.Update(item);
                            }

                            if (save.SaveEntity(enSaveOperation.OpSave))
                            {
                                RiselezionaSelezionato();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.ManageError(ex);
                }
            }
        }

        private void Save(Save<FatturaRiga> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                EditItem.Fattura = null;
                EditItem.FatturaID = _controllerFatturazione.EditItem.ID;
                if (_controllerFatturazione.EditItem.ID == 0)
                    return;

                if (saveManager.UnitOfWork.FatturaRepository.Find(a => a.ID == EditItem.FatturaID).Select(a=>a.ChiusaSpedita).First())
                {
                    MessageManager.NotificaWarnig("Documento già chiuso, non è possibile modificare altro!");
                    return;
                }
                if (EditItem.ID > 0)
                {
                    uof.FattureRigheRepository.Update(EditItem);
                }
                else
                {
                    uof.FattureRigheRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    AggiornaTotaliFattura();

                    RiselezionaSelezionato();
                }
            }
        }
    }
}