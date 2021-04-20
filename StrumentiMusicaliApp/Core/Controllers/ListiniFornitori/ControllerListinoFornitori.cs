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
using System.Data;
using System.Data.SqlClient;
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
                if (a.ItemSelected != null)
                    SelectedItem = a.ItemSelected.Entity;
                else
                {
                    SelectedItem = null;
                }
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
            if (_controllerMaster != null)
            {
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
            }
            else
            {
                if (InLineEditor)
                {
                    int removed = 0;
                    foreach (var item in menu.Tabs[0].Pannelli)
                    {
                        var items = item.Pulsanti.Where(a => a.Tag != null
                        && (a.Tag.Equals(MenuTab.TagAdd)
                        || a.Tag.Contains(MenuTab.TagEdit)
                        || a.Tag.Contains(MenuTab.TagCercaClear)
                        || a.Tag.Contains(MenuTab.TagCerca)
                        || a.Tag.Contains(MenuTab.TagRemove)
                        )

                        ).ToList();
                        removed += item.Pulsanti.RemoveAll(a => items.Contains(a));

                    }
                    menu.Tabs[0].Pannelli.RemoveAll(a => a.Pulsanti.Count == 0);
                    if (removed != 0)
                    {
                        var presenti = menu.Tabs[0].Pannelli.Where(a => a.Pulsanti.Count() > 0).First();
                        var generaOdg = new RibbonMenuButton() { Testo = "Genera ordini di acquisto", Immagine = StrumentiMusicali.Core.Properties.ImageIcons.GeneraODQ_ };
                        presenti.Pulsanti.Add(generaOdg);
                        generaOdg.Click += GeneraOdq_click;

                        var savePreordine = new RibbonMenuButton() { Testo = "Salva Qta da ordinare", Immagine = StrumentiMusicali.Core.Properties.ImageIcons.Save };
                        presenti.Pulsanti.Add(savePreordine);
                        savePreordine.Click += SavePreordine_Click; ;
                    }
                }
            }
            return menu;
        }

        private void SavePreordine_Click(object sender, EventArgs e)
        {
            using (var saveEnt = new SaveEntityManager())
            {
                bool save = false;
                foreach (var item in DataSource.Where(a => a.ToSave == true))
                {
                    var listino = saveEnt.UnitOfWork.ListinoPrezziFornitoriRepository.Find(a => a.ID == item.ID).FirstOrDefault();
                    var preOrdAcq = saveEnt.UnitOfWork.PreOrdineAcquistoRepository.Find(a => a.FornitoreID == listino.FornitoreID && a.ArticoloID == listino.ArticoloID).FirstOrDefault();
                    if (preOrdAcq == null)
                    {
                        preOrdAcq = new PreOrdineAcquisto();
                        preOrdAcq.ArticoloID = listino.ArticoloID;
                        preOrdAcq.FornitoreID = listino.FornitoreID;



                    }
                    save = true;
                    preOrdAcq.QtaDaOrdinare = item.QtaDaOrdinare;
                    if (preOrdAcq.ID <= 0)
                    {
                        saveEnt.UnitOfWork.PreOrdineAcquistoRepository.Add(preOrdAcq);
                    }
                    else

                        saveEnt.UnitOfWork.PreOrdineAcquistoRepository.Update(preOrdAcq);

                }
                if (save)
                    saveEnt.SaveEntity(enSaveOperation.OpSave);

            }
        }

        private void GeneraOdq_click(object sender, EventArgs e)
        {

            //CalcolaTotali
            using (var saveEnt = new SaveEntityManager())
            {
                bool save = false;

                var listFatt = new List<Library.Entity.Fattura>();

                foreach (var item in DataSource.Where(a => a.QtaDaOrdinare - a.QtaInOrdine > 0)
                    .Select(a => new { Qta = a.QtaDaOrdinare - a.QtaInOrdine, a.CodiceArticoloFornitore, a.ID, a.Entity, a.Prezzo }).OrderBy(a => a.Entity.FornitoreID).ToList())
                {

                    Library.Entity.Fattura fattExt = saveEnt.UnitOfWork.FatturaRepository.Find(a => a.ChiusaSpedita == false && a.ClienteFornitoreID == item.Entity.FornitoreID &&
                    a.TipoDocumento == Library.Entity.EnTipoDocumento.OrdineAlFornitore).FirstOrDefault();
                    var riga = new Library.Entity.FatturaRiga();

                    Library.Entity.Articoli.Articolo art = saveEnt.UnitOfWork.ArticoliRepository.Find(a => a.ID == item.Entity.ArticoloID).FirstOrDefault();



                    if (fattExt == null)
                        fattExt = listFatt.Where(a => a.ClienteFornitoreID == item.Entity.FornitoreID).FirstOrDefault();
                    
                    if (fattExt == null)
                    {
                        fattExt = new Library.Entity.Fattura();
                        fattExt.ClienteFornitoreID = item.Entity.FornitoreID;
                        fattExt.TipoDocumento = Library.Entity.EnTipoDocumento.OrdineAlFornitore;
                        fattExt.Data = DateTime.Now;
                        fattExt.Codice = ControllerFatturazione.CalcolaCodice(fattExt);
                        fattExt.RagioneSociale = "";
                        saveEnt.UnitOfWork.FatturaRepository.Add(fattExt);

                        saveEnt.SaveEntity("");
                    }
                    listFatt.Add(fattExt);
                    
                    riga = (new Library.Entity.FatturaRiga()
                    {
                        CodiceFornitore = item.CodiceArticoloFornitore,
                        ArticoloID = item.Entity.ArticoloID,
                        Descrizione = art.Titolo,
                        Qta = item.Qta,
                        Fattura = fattExt,
                        PrezzoUnitario = item.Prezzo,
                        IvaApplicata = "22",
                    });
                    saveEnt.UnitOfWork.FattureRigheRepository.Add(riga);
                    save = true;
                }


                if (save)
                    saveEnt.SaveEntity("");
                else
                {
                    MessageManager.NotificaInfo("Non ci sono articoli da ordinare!");
                    return;
                }

                foreach (var item in listFatt.Distinct())
                {
                    ControllerFatturazione.CalcolaTotali(item);
                    saveEnt.UnitOfWork.FatturaRepository.Update(item);

                }
                if (save)
                    saveEnt.SaveEntity("Generati gli ordini di acquisto!");

                RefreshList(null);

            }
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




                        using (var connection = new SqlConnection(uof.ConnectionString))
                        {
                            SqlDataAdapter da = new SqlDataAdapter();
                            var cmd = new SqlCommand("dbo.NG_SottoScorta_SelectDati", connection);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@idUtente", LoginData.utenteLogin.ID);
                            cmd.CommandType = CommandType.StoredProcedure;

                            connection.Open();

                            da.SelectCommand = cmd;
                            var ds = new DataTable();

                            ///conn.Open();
                            da.Fill(ds);

                            //  
                            var dat = View.Utility.UtilityView.DataTableToList<ListinoPrezziFornitoriItem>(ds);
                            //);

                            dat.ForEach(a =>
                            {

                                a.Entity = uof.ListinoPrezziFornitoriRepository.Find(b => b.ID == a.ID).FirstOrDefault();
                                a.ToSave = false;
                            }
                                );

                            DataSource = new View.Utility.MySortableBindingList<ListinoPrezziFornitoriItem>(dat);
                        }

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