﻿using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraGrid.Localization;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Controllers.Base;
 
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Events.Articoli;
 
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
            GridLocalizer.Active = new ItalianGridLocalizer();
            //Localizer.Active = new GermanEditorsLocalizer();
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                bool createdNew = true;
                using (Mutex mutex = new Mutex(true, BaseController.MainName, out createdNew))
                {
                    if (createdNew)
                    {
                        StrumentiMusicali.Core.Settings.SettingProgrammaValidator.ReadSetting();


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