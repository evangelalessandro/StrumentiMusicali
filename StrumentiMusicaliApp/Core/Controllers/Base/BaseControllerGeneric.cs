﻿using PropertyChanged;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Ambienti;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Library.Core;

using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StrumentiMusicali.App.Core.Controllers.Base
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseControllerGeneric<TEntity, TBaseItem> : BaseController, IMenu, IDisposable, ICloseSave //, INotifyPropertyChanged
        where TEntity : BaseEntity, new()
        where TBaseItem : BaseItem<TEntity>, new()
    {



        public BaseControllerGeneric(enAmbiente ambiente, enAmbiente ambienteDettaglio, bool gestioneInline = false) :
            base(gestioneInline)
        {
            AmbienteDettaglio = ambienteDettaglio;

            Ambiente = ambiente;

            Init();

            //TestoRicerca = ReadSetting(ambiente).LastStringaRicerca;

        }
        internal void UpdateButtonState()
        {
            if (GetMenu() != null)
            {
                var menu = GetMenu();
                menu.Enabled = !(DataSource == null);

                menu.ApplyValidation(SelectedItem != null && SelectedItem.ID > 0);
                foreach (var item in menu.ItemByTag(MenuTab.TagCerca))
                {
                    item.Checked = PannelloRicercaVisibile;
                }

                foreach (var item in menu.ItemByTag(MenuTab.TagCercaClear))
                {
                    item.Checked = !ClearRicerca;
                }


            }
        }
        public bool PannelloRicercaVisibile { get; set; } = true;

        ~BaseControllerGeneric()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public enAmbiente Ambiente { get; private set; }
        public enAmbiente AmbienteDettaglio { get; private set; }

        public string TestoRicerca { get; set; } = "";
        public virtual void RefreshList(UpdateList<TEntity> itemobj)
        {
            if (ClearRicerca)
            {
                this.TestoRicerca = "";
            }
        }

        public TEntity EditItem
        {
            get;
            set;
        } = new TEntity();


        private Subscription<UpdateList<TEntity>> _updateList = null;
        private Subscription<ItemSelected<TBaseItem, TEntity>> _selectItemSub;


        public void Init()
        {
            _updateList = EventAggregator.Instance().Subscribe<UpdateList<TEntity>>(RefreshList);

            _selectItemSub = EventAggregator.Instance().Subscribe<ItemSelected<TBaseItem, TEntity>>(
                (a) =>
                {
                    if (a.ItemSelected != null)
                        SelectedItem = a.ItemSelected.Entity;
                }
                );
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // The bulk of the clean-up code is implemented in Dispose(bool)
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var dato = ReadSetting(Ambiente);
                //dato.LastStringaRicerca = TestoRicerca;
                //if (string.IsNullOrEmpty(dato.LastStringaRicerca))
                //{
                //    dato.LastStringaRicerca = "";
                //}
                SaveSetting(Ambiente, dato);

                // free managed resources
                EventAggregator.Instance().UnSbscribe(_updateList);
                EventAggregator.Instance().UnSbscribe(_selectItemSub);
                if (DataSource != null)
                    DataSource.Clear();
                DataSource = null;
            }
            // free native resources if there are any.
        }

        internal void RiselezionaSelezionato()
        {
            var item = (TEntity)SelectedItem;
            EventAggregator.Instance().Publish<UpdateList<TEntity>>(new UpdateList<TEntity>(this));
            if (item != null)
                EventAggregator.Instance().Publish<ItemSelected<TBaseItem, TEntity>>(
                    new ItemSelected<TBaseItem, TEntity>(new TBaseItem()
                    {
                        ID = item.ID,
                        Entity = item
                    }, this));
        }

        public List<TBaseItem> SelectedItems { get; set; }
        public TEntity SelectedItem { get; set; }
        public void ShowEditView()
        {

            using (var view = new GenericSettingView(EditItem))
            {
                view.OnSave += (a, b) =>
                {
                    view.Validate();
                    EventAggregator.Instance().Publish<Save<TEntity>>
                    (new Save<TEntity>(this));
                };
                ShowView(view, AmbienteDettaglio);
            }
        }
        public MySortableBindingList<TBaseItem> DataSource { get; set; } = new MySortableBindingList<TBaseItem>();


        public List<TEntity> DataSourceInRow { get; set; } = new List<TEntity>();
        internal void UpdateDataSource()
        {
            RefreshList(null);
        }
        private MenuTab _menuTab = null;



        public virtual MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                AggiungiComandi();

            }
            return _menuTab;
        }
        //private RibbonMenuButton ribCerca;

        //private RibbonMenuButton ribDelete;


        //private RibbonMenuButton ribEdit;

        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public enAmbiente AmbienteMenu { get; set; }

        private void AggiungiComandi()
        {

            var tab1 = _menuTab.Add(UtilityAmbienti.TestoAmbiente(AmbienteMenu));
            var panelComandi = tab1.Add("Comandi");
            UtilityView.AddButtonSaveAndClose(panelComandi, this, false);

            var ribCrea = panelComandi.Add("Crea", StrumentiMusicali.Core.Properties.ImageIcons.Add);
            ribCrea.Tag = MenuTab.TagAdd;

            var ribEdit = panelComandi.Add(@"Vedi\Modifica", StrumentiMusicali.Core.Properties.ImageIcons.Edit, true);
            ribEdit.Tag = MenuTab.TagEdit;

            var ribDelete = panelComandi.Add("Cancella", StrumentiMusicali.Core.Properties.ImageIcons.Delete, true);
            ribDelete.Tag = MenuTab.TagRemove;

            var panel2 = tab1.Add("Cerca");
            var ribCerca = panel2.Add("Cerca", StrumentiMusicali.Core.Properties.ImageIcons.Find);
            ribCerca.Tag = MenuTab.TagCerca;

            ribCrea.Click += (a, e) =>
            {
                EventAggregator.Instance().Publish(new Add<TEntity>(this));
            };

            var ribClearFilter = panel2.Add("Mantieni filtro", StrumentiMusicali.Core.Properties.ImageIcons.MantieniFiltro);
            ribClearFilter.Tag = MenuTab.TagCercaClear;
            ribClearFilter.Checked = false;
            ribClearFilter.Click += (a, e) =>
            {
                ClearRicerca = !ClearRicerca;
                ribClearFilter.Checked = !ClearRicerca;

            };


            ribDelete.Click += (a, e) =>
              {
                  EventAggregator.Instance().Publish(new Remove<TEntity>(this));
              };
            ribEdit.Click += (a, e) =>
          {
              EventAggregator.Instance().Publish(new Edit<TEntity>(this));
          };
            ribCerca.Click += (a, e) =>
            {

                EventAggregator.Instance().Publish(new ViewRicerca<TEntity>());
            };

            var panel3 = tab1.Add("Visualizza");
            var rib1 = panel3.Add("Visualizza tutti", StrumentiMusicali.Core.Properties.ImageIcons.View_all_48);
            rib1.Click += (b, c) =>
              {
                  rib1.Checked = !rib1.Checked;
                  if (rib1.Checked)
                  {
                      ViewAllItem = true;
                  }
                  else
                  {
                      ViewAllItem = false;
                  }
                  EventAggregator.Instance().Publish(new UpdateList<TEntity>(this));
              };

        }
        public bool ViewAllItem { get; set; }
        public bool ClearRicerca { get; set; } = false;
        public void RaiseSave()
        {
            if (OnSave != null)
                OnSave(this, new EventArgs());
        }

        public void RaiseClose()
        {
            if (OnClose != null)
                OnClose(this, new EventArgs());
        }
    }
}