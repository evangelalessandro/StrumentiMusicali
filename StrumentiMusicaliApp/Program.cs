using DevExpress.XtraGrid.Localization;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace StrumentiMusicali.App
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {

            GridLocalizer.Active = new ItalianGridLocalizer();
            //Localizer.Active = new GermanEditorsLocalizer();
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                bool createdNew = true;
                using (Mutex mutex = new Mutex(true, BaseController.MainName, out createdNew))
                {
                    if (createdNew)
                    {
                        using (var uof=new UnitOfWork())
                        {
                            if (uof.ServerName()==Environment.MachineName)
                            {
                                AttivaSchedulatore();
                            }
                        }
                        using (var controller = new ControllerMaster())
                        {
                            controller.ShowMainView();
                        }
                    }

                    else
                    {
                        ProcessUtils.SetFocusToPreviousInstance(BaseController.MainName);
                    }
                }
            }
            else
            {
                /*invio articoli csv*/
                if (args[0] == "INVIO")
                {
                    using (var controller = new ControllerMaster())
                    {
                        EventAggregator.Instance().Publish<InvioArticoliCSV>(new InvioArticoliCSV());
                    }
                }
            }
        }

        private static void AttivaSchedulatore()
        {
            StrumentiMusicali.Core.Scheduler.SchedulerTask scheduler = new StrumentiMusicali.Core.Scheduler.SchedulerTask();
            scheduler.Init();
        }
    }
    public static class ProcessUtils
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindowEnabled(IntPtr hWnd);


        public static void SetFocusToPreviousInstance(string windowCaption)
        {

            IntPtr hWnd = FindWindow(null, windowCaption);


            if (hWnd != null)
            {

                IntPtr hPopupWnd = GetLastActivePopup(hWnd);



                if (hPopupWnd != null && IsWindowEnabled(hPopupWnd))
                {
                    hWnd = hPopupWnd;
                }

                SetForegroundWindow(hWnd);


                if (IsIconic(hWnd))
                {
                    ShowWindow(hWnd, SW_RESTORE);
                }
            }
        }
    }

}