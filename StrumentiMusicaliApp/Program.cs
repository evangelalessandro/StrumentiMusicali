using DevExpress.XtraGrid.Localization;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.Core.Manager;
using StrumentiMusicali.Core.Scheduler;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
using StrumentiMusicali.Library.Repo;
using System;

using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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
             

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event.
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

           

            GridLocalizer.Active = new ItalianGridLocalizer(); 
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, BaseController.MainName, out createdNew))
            {
                if (createdNew)
                {

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
 
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageManager.NotificaError("Si è verificato un errore nell'ultima operazione !", null);

            ManagerLog.Logger.Error(e.ExceptionObject.ToString());
        }
         
    }

    public static class ProcessUtils
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

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