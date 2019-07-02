using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.Stampa;
using StrumentiMusicali.App.Core.Manager;
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
    public class ControllerPagamenti : BaseControllerGeneric<Pagamento, PagamentoItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<Pagamento>> _selectSub;

        private Subscription<Remove<Pagamento>> _subRemove;

        private Subscription<Save<Pagamento>> _subSave;


        public ControllerPagamenti()
            : base(enAmbiente.PagamentiList, enAmbiente.Pagamento)
        {

            SelectedItem = new Pagamento();


            _selectSub = EventAggregator.Instance().Subscribe<Add<Pagamento>>((a) =>
            {
                EditItem = new Pagamento();
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<Pagamento>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare il pagamento selezionato?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = SelectedItem;
                    if (curItem.ID > 0)
                    {
                        var item = uof.PagamentoRepository.Find(b => b.ID == curItem.ID).First();
                        uof.PagamentoRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish<UpdateList<Pagamento>>(new UpdateList<Pagamento>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<Pagamento>>((a) =>
           {
               Save(null);
           });
            ///comando di stampa
			AggiungiComandi();

        }
        private void AggiungiComandi()
        {

            AggiungiComandiStampa(GetMenu().Tabs[0], false);
        }
        public void AggiungiComandiStampa(MenuRibbon.RibbonMenuTab tab, bool editItem)
        {
             
            var pnlStampa = tab.Add("Stampa");

            var ribStampa = pnlStampa.Add("Avvia stampa", Properties.Resources.Print_48, true);
            ribStampa.Click += (a, e) =>
            {
                if (editItem)
                    Stampa(EditItem);
                else
                    Stampa(SelectedItem);

            };
            var pnlDoc = tab.Add("Documenti");

            var ribDocumenti = pnlDoc.Add("Documenti", Properties.Resources.Identity_48, true);



        }


        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerPagamenti()
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

        public override void RefreshList(UpdateList<Pagamento> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<PagamentoItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.PagamentoRepository.Find(a =>
                    a.Nome.Contains(TestoRicerca) ||
                    a.Cognome.Contains(TestoRicerca) ||
                    a.ArticoloAcq.Contains(TestoRicerca) ||
                        TestoRicerca == ""
                    ).Take(ViewAllItem ? 100000 : 300)
                    .ToList()
                    .Select(a => new PagamentoItem(a)
                    {


                    }).OrderBy(a => a.Cognome).OrderBy(a => a.DataRata).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<PagamentoItem>(list);


                base.RefreshList(obj);
                UpdateButtonState();
            }
            catch (Exception ex)
            {
                new Action(() =>
                { ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
            }
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected new void Dispose(bool disposing)
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
        internal void Stampa(Pagamento pagamento)
        {
            _logger.Info("Avvio stampa");
            try
            {
                using (var cursorManger = new CursorManager())
                {
                    using (var stampa = new StampaPagamento())
                    {

                        stampa.Stampa(pagamento);
                        _logger.Info("Stampa completata correttamente.");
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.ManageError(ex);
            }
        }



        private void Save(Save<Pagamento> obj)
        {
            Pagamento nuovoPagamento = null;
            using (var saveManager = new SaveEntityManager())
            {
                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.PagamentoRepository.Update(EditItem);
                }
                else
                {

                    if (EditItem.ID == 0)
                    {
                        Guid guid = Guid.NewGuid();
                        var importoRata = EditItem.ImportoTotale / (decimal)EditItem.NumeroRate;
                        foreach (var item in Enumerable.Range(1, EditItem.NumeroRate))
                        {
                            var pagamento = (Pagamento)EditItem.Clone();

                            {
                                pagamento.IDPagamenti = guid;
                                pagamento.ImportoRata = importoRata;
                                pagamento.DataRata = EditItem.DataInizio.AddMonths(item);
                                pagamento.ImportoResiduo =
                                    EditItem.ImportoTotale - (importoRata * item);
                            };
                            uof.PagamentoRepository.Add(pagamento);

                            if (nuovoPagamento == null)
                                nuovoPagamento = pagamento;
                        }
                    }
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                    if (nuovoPagamento != null)
                    {
                        EventAggregator.Instance().Publish<ForceCloseActiveFormView>(new ForceCloseActiveFormView());
                    }

                }
            }
        }
    }
}