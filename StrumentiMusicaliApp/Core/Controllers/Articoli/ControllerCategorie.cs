using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers
{
    //[AddINotifyPropertyChangedInterface]
    public class ControllerCategorie : BaseControllerGeneric<Categoria, CategoriaItem>,
        IDisposable//, INotifyPropertyChanged
    {
        private Subscription<Add<Categoria>> _selectSub;

        private Subscription<Remove<Categoria>> _subRemove;

        private Subscription<Save<Categoria>> _subSave;


        public ControllerCategorie()
            : base(enAmbiente.CategoriaArticoli, enAmbiente.CategoriaArticoliDettaglio)
        {

            SelectedItem = new Categoria();


            _selectSub = EventAggregator.Instance().Subscribe<Add<Categoria>>((a) =>
            {
                EditItem = new Categoria() { Nome = "Nome cat" };
                ShowEditView();
            });
            _subRemove = EventAggregator.Instance().Subscribe<Remove<Categoria>>((a) =>
            {
                if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
                    return;
                using (var saveManager = new SaveEntityManager())
                {
                    var uof = saveManager.UnitOfWork;
                    var curItem = (Categoria)SelectedItem;
                    if (curItem.ID > 0)
                    {
                        try
                        {
                            var item = uof.CategorieRepository.Find(b => b.ID == curItem.ID).First();
                            uof.CategorieRepository.Delete(item);

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
            _subSave = EventAggregator.Instance().Subscribe<Save<Categoria>>((a) =>
           {
               Save(null);
           });
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerCategorie()
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

        public override void RefreshList(UpdateList<Categoria> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                var list = new List<CategoriaItem>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.CategorieRepository.Find(a =>
                    a.Nome.Contains(TestoRicerca)
                        ||
                        a.Reparto.Contains(TestoRicerca)
                        ||
                        a.GruppoCodiceRegCassa.GruppoRaggruppamento.Contains(TestoRicerca)
                        ||
                        TestoRicerca == ""
                    ).OrderBy(a => a.Reparto).ThenBy(b=>b.Nome).Take(ViewAllItem ? 100000 : 300).
                    Select(a=> new { Item=a, a.GruppoCodiceRegCassa}).ToList().Select(a => 
                    new CategoriaItem(a.Item)
                    {
                        ID = a.Item.ID,
                        Entity = a.Item,

                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<CategoriaItem>(list);


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



        private void Save(Save<Categoria> obj)
        {
            using (var saveManager = new SaveEntityManager())
            {

                EditItem.GruppoCodiceRegCassa = null;

                var uof = saveManager.UnitOfWork;
                if (((EditItem).ID > 0))
                {
                    uof.CategorieRepository.Update(EditItem);
                }
                else
                {
                    uof.CategorieRepository.Add(EditItem);
                }

                if (saveManager.SaveEntity(enSaveOperation.OpSave))
                {
                    RiselezionaSelezionato();
                }
            }
        }
    }
}