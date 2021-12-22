using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.MenuRibbon;
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
    public class ControllerMovimentiMagazzino : BaseControllerGeneric<Magazzino, MovimentoItemView>//, INotifyPropertyChanged
    {
      

        //private Subscription<Remove<Categoria>> _subRemove;
         


        public ControllerMovimentiMagazzino()
            : base(enAmbiente.MovimentiMagazzino, enAmbiente.MovimentiMagazzino)
        {
            AmbienteMenu = enAmbiente.MovimentiMagazzino;
            SelectedItem = new Magazzino();


             
            //_subRemove = EventAggregator.Instance().Subscribe<Remove<Categoria>>((a) =>
            //{
            //    if (!MessageManager.QuestionMessage("Sei sicuro di volere eliminare la riga selezionata?"))
            //        return;
            //    using (var saveManager = new SaveEntityManager())
            //    {
            //        var uof = saveManager.UnitOfWork;
            //        var curItem = (Categoria)SelectedItem;
            //        if (curItem.ID > 0)
            //        {
            //            try
            //            {
            //                var item = uof.CategorieRepository.Find(b => b.ID == curItem.ID).First();
            //                uof.CategorieRepository.Delete(item);

            //                if (saveManager.SaveEntity(enSaveOperation.OpDelete))
            //                {
            //                    EventAggregator.Instance().Publish<UpdateList<Utente>>(new UpdateList<Utente>(this));
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                ExceptionManager.ManageError(ex);
            //            }
            //        }
            //    }
            //});
            
        }


        public override MenuTab GetMenu()
        {
            
            base.GetMenu().ItemByTag(MenuTab.TagAdd).ForEach(a => a.Visible = false);
            base.GetMenu().ItemByTag(MenuTab.TagEdit).ForEach(a => a.Visible = false);
            
            base.GetMenu().ItemByTag(MenuTab.TagRemove).ForEach(a => a.Visible = false);


            return base.GetMenu();
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~ControllerMovimentiMagazzino()
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

        public override void RefreshList(UpdateList<Magazzino> obj)
        {
            try
            {
                if (string.IsNullOrEmpty(TestoRicerca))
                {
                    TestoRicerca = "";
                }
                TestoRicerca = TestoRicerca.Trim();
                var list = new List<MovimentoItemView>();

                using (var uof = new UnitOfWork())
                {
                    list = uof.MagazzinoRepository.Find(a =>
                    (TestoRicerca.Length>0 && ( 
                    a.Articolo.Titolo.Contains(TestoRicerca)
                        ||
                    a.Articolo.CodiceABarre.Equals(TestoRicerca )
                        ||
                        a.Note.Contains(TestoRicerca)))
                        ||
                        TestoRicerca == ""
                    ).OrderByDescending(a => a.ID).Take(ViewAllItem ? 100000 : 1000).
                    Select(a=> new { Item=a, a.Articolo, a.Deposito}).ToList().Select(a => 
                    new MovimentoItemView(a.Item)
                    {
                        ID = a.Item.ID,
                        Entity = a.Item,

                    }).ToList();
                }

                DataSource = new View.Utility.MySortableBindingList<MovimentoItemView>(list);


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
                //EventAggregator.Instance().UnSbscribe(_subSave);
                //EventAggregator.Instance().UnSbscribe(_selectSub);
                //EventAggregator.Instance().UnSbscribe(_subRemove);

            }
            // free native resources if there are any.
        }



        //private void Save(Save<Categoria> obj)
        //{
        //    using (var saveManager = new SaveEntityManager())
        //    {

        //        EditItem.GruppoCodiceRegCassa = null;

        //        var uof = saveManager.UnitOfWork;
        //        if (((EditItem).ID > 0))
        //        {
        //            uof.CategorieRepository.Update(EditItem);
        //        }
        //        else
        //        {
        //            uof.CategorieRepository.Add(EditItem);
        //        }

        //        if (saveManager.SaveEntity(enSaveOperation.OpSave))
        //        {
        //            RiselezionaSelezionato();
        //        }
        //    }
        //}
    }
}