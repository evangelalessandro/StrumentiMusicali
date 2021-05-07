using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NLog;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Events.Tab;
using StrumentiMusicali.App.Core.Exports;
using StrumentiMusicali.App.Core.Imports;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Core.Enum;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Core.Events.Generics;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App
{
    public partial class MainView : XtraUserControl
    {
        internal readonly ILogger _logger = StrumentiMusicali.Core.Manager.ManagerLog.Logger;

        private BaseController _baseController;

       
        private DevExpress.XtraTab.XtraTabControl tab;

        public MainView(BaseController baseController)
        {
            _baseController = baseController;
            InitializeComponent();

            _logger.Debug("Form main init");

            tab = new DevExpress.XtraTab.XtraTabControl
            {
                Dock = DockStyle.Fill,
                ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders,
                HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Always
            };
            tab.CloseButtonClick += Tab_CloseButtonClick;
            this.Controls.Add(tab);

            EventAggregator.Instance().Subscribe<GetNewTab>(TakeNewTab);
            EventAggregator.Instance().Subscribe<RemoveNewTab>(RemoveTab);


        }

        private void Tab_CloseButtonClick(object sender, System.EventArgs e)
        {
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            var page = arg.Page as XtraTabPage;

            tab.TabPages.Remove(page);
            page.Dispose();
            GC.SuppressFinalize(page);
        }

        private void RemoveTab(RemoveNewTab obj)
        {
            tab.TabPages.Remove(obj.Tab);
        }

        private void TakeNewTab(GetNewTab obj)
        {
            obj.Tab = tab.TabPages.Add(obj.Text);

            obj.Tab.Tag = obj.Ambiente;
        }

       
    }
}