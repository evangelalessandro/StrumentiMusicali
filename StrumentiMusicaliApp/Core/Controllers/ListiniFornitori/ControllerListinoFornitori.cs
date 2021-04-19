using PropertyChanged;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.ListiniFornitori
{
    [AddINotifyPropertyChangedInterface]
    public class ControllerListinoFornitori : BaseControllerGeneric<ListinoPrezziFornitori, ListinoPrezziFornitoriItem>, IDisposable
    {
        private ControllerArticoli _controllerMaster;
        private Subscription<Add<ListinoPrezziFornitori>> _selectSub;

        private Subscription<Remove<ListinoPrezziFornitori>> _subRemove;

        private Subscription<Save<ListinoPrezziFornitori>> _subSave;

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
                    var curItem = SelectedItem;
                    if (curItem != null && curItem.ID > 0)
                    {
                        if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                            return;

                        var item = uof.ListinoPrezziFornitoriRepository.Find(b => b.ID == curItem.ID).First();
                        uof.ListinoPrezziFornitoriRepository.Delete(item);

                        if (saveManager.SaveEntity(enSaveOperation.OpDelete))
                        {
                            EventAggregator.Instance().Publish(new UpdateList<ListinoPrezziFornitori>(this));
                        }
                    }
                }
            });
            _subSave = EventAggregator.Instance().Subscribe<Save<ListinoPrezziFornitori>>((a) =>
           {
               EventAggregator.Instance().Publish(new ValidateViewEvent<ListinoPrezziFornitori>());
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
            var save = new RibbonMenuButton() { Testo = "Salva", Immagine = StrumentiMusicali.Core.Properties.ImageIcons.Save };
            presenti.Pulsanti.Add(save);
            save.Click += Save_Click;
            return menu;
        }

        public override void RefreshList(UpdateList<ListinoPrezziFornitori> obj)
        {
            try
            {

                using (var uof = new UnitOfWork())
                {
                    if (_controllerMaster != null)
                    {
                        var listCurrent = uof.ListinoPrezziFornitoriRepository.Find(
                         a => a.ArticoloID == _controllerMaster.EditItem.ID
                             ).Where(a => a.Fornitore.Nome.Contains(TestoRicerca) ||
                         a.Fornitore.RagioneSociale.Contains(TestoRicerca) ||
                         a.Fornitore.Cognome.Contains(TestoRicerca) ||
                         a.Fornitore.PIVA.Contains(TestoRicerca) ||
                         TestoRicerca == "").OrderBy(a => a.FornitoreID).ThenBy(a => a.ArticoloID)
                         .ToList();

                        DataSourceInRow = listCurrent;
                        return;

                    }
                    else
                    {
                        var listArtQta = uof.ArticoliRepository.Find(
                                        a => a.SottoScorta > 0).Select(a => new { Articolo = a.ID, a.SottoScorta }).ToList();

                        var listArt = listArtQta.Select(a => a.Articolo).ToList();
                        var giacenza = uof.MagazzinoRepository.Find(a =>
                               listArt.Contains(a.ArticoloID))
                          .Select(a => new { a.ArticoloID, a.Qta })
                          .GroupBy(a => new { a.ArticoloID })
                          .Select(a => new { Sum = a.Sum(b => b.Qta), Articolo = a.Key.ArticoloID }).ToList();


                        var sottoGiac = from a in listArtQta
                                        join b in giacenza
                                        on a.Articolo equals b.Articolo
                                        into c 
                                        from d in c.DefaultIfEmpty()
                                        select new { Art = a.Articolo, SottoScorta = a.SottoScorta, Giacenza = d.Sum == null ?0: d.Sum};

                        sottoGiac = sottoGiac.Where(a => a.SottoScorta < a.Giacenza).ToList();

                        var dati = uof.ListinoPrezziFornitoriRepository.Find(a =>
                                sottoGiac.Select(b => b.Art).Contains(a.ArticoloID)).Select(a =>
                                new { a.FornitoreID, a.ArticoloID, a.MinimoOrdinabile, a.Prezzo }).ToList();

                        var prezzoMinArticolo = dati.GroupBy(a => new { a.ArticoloID }).Select(a =>
                          new { a.Key.ArticoloID, PrezzoMinimo = a.Min(b => b.Prezzo) }).ToList();


                        var listF = uof.ListinoPrezziFornitoriRepository
                            .Find(a => prezzoMinArticolo.Select(b => b.ArticoloID).Contains(a.ArticoloID)).ToList();

                        var listF2 = from a in listF
                                     join b in prezzoMinArticolo
                                     on new { a.ArticoloID, a.Prezzo } equals new { b.ArticoloID, Prezzo = b.PrezzoMinimo }
                                     select a;


                        var datiListF = listF2.Select(a => new ListinoPrezziFornitoriItem
                        {
                            ArticoloID = a.ArticoloID,
                            FornitoreID = a.FornitoreID,
                            ID = a.ID,
                            CodiceArticoloFornitore = a.CodiceArticoloFornitore,
                            QtaMinimaOrdine = a.MinimoOrdinabile,
                            Prezzo = a.Prezzo,
                            QtaDaOrdinare = a.MinimoOrdinabile,
                            QtaGiacenza = 0,
                            Entity = a
                        }).ToList();

                        datiListF.ForEach(a =>
                        {

                            a.QtaGiacenza = giacenza.Where(b => b.Articolo == a.ArticoloID).Select(b => b.Sum).DefaultIfEmpty(0).FirstOrDefault(); 
                        });
                        //Join(prezzoMinArticolo,a=>new { a.ArticoloID, a.Prezzo },b=>b,(a,b)=>a).tolist

                        //    ).Where(a => a.Fornitore.Nome.Contains(TestoRicerca) ||
                        //a.Fornitore.RagioneSociale.Contains(TestoRicerca) ||
                        //a.Fornitore.Cognome.Contains(TestoRicerca) ||
                        //a.Fornitore.PIVA.Contains(TestoRicerca) ||
                        //TestoRicerca == "").OrderBy(a => a.FornitoreID).ThenBy(a => a.ArticoloID)
                        //.ToList();

                        DataSource = new View.Utility.MySortableBindingList<ListinoPrezziFornitoriItem>(datiListF);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                new Action(() =>
                { ExceptionManager.ManageError(ex); }).BeginInvoke(null, null);
            }
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

        private void Save_Click(object sender, EventArgs e)
        {
            EventAggregator.Instance().Publish(new Save<ListinoPrezziFornitori>(this));
        }
    }
}