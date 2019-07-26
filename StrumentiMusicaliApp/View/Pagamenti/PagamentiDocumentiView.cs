using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Generics;
using StrumentiMusicali.Library.Core.Events.Image;
using StrumentiMusicali.Library.Entity;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Forms
{
    public partial class PagamentiDocumentiView : UserControl, ICloseSave
    {

        private EditorListImmagini<PagamentoDocumenti> _EditorListImmagini;
        private Subscription<ImageSelected<PagamentoDocumenti>> _sub12;
        private Subscription<ImageListUpdate> _sub1;
        private ControllerPagamentiDocumenti _controller;
        public PagamentiDocumentiView(ControllerPagamentiDocumenti controller)
            : base()
        {
            _controller = controller;
            InitializeComponent();
            if (DesignMode)
                return;
            _sub12 = EventAggregator.Instance().Subscribe<ImageSelected<PagamentoDocumenti>>(ImmagineSelezionata);


            _EditorListImmagini = new EditorListImmagini<PagamentoDocumenti>(_controller);

            _sub1 = EventAggregator.Instance().Subscribe<ImageListUpdate>(RefreshImageList);

            _EditorListImmagini.Dock = DockStyle.Fill;
            this.Controls.Add(_EditorListImmagini);

            RefreshImageListData();
        }

        private void ImmagineSelezionata(ImageSelected<PagamentoDocumenti> obj)
        {
            UpdateButtonState();
        }



        private void frmArticolo_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;



            UpdateButtonState();

        }


        private void RefreshImageList(ImageListUpdate obj)
        {
            RefreshImageListData();
        }

        private void RefreshImageListData()
        {
            _EditorListImmagini.RefreshImageListData(_controller.RefreshImageListData());
        }





        private void UpdateButtonState()
        {

            _controller.UpdateButton();

            _EditorListImmagini.UpdateButtonState();
        }



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            EventAggregator.Instance().UnSbscribe(_sub12);

            EventAggregator.Instance().UnSbscribe(_sub1);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public void RaiseSave()
        {
            this.Validate();
            EventAggregator.Instance().Publish<Save<Articolo>>(
                new Save<Articolo>(_controller));
            UpdateButtonState();
        }

        public void RaiseClose()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }
    }
}