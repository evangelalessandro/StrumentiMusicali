using NLog;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.View.Utility;
using System;
using System.Drawing;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace StrumentiMusicali.App.Core
{
    public class MessageManager
    {
        internal static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void NotificaInfo(string info)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = StrumentiMusicali.App.Properties.Resources.Info_64;
            popup.TitleText = "Info";
            popup.OptionsMenu = getContextMenu(info);
            popup.ShowOptionsButton = true;
            popup.ContentText = info;
            popup.Popup();
            {
                ShowMessagio(popup);
            }
        }
        public static string GetMessage(enSaveOperation operation)
        {
            switch (operation)
            {
                case enSaveOperation.OpSave:
                    return ("Salvataggio avvenuto con successo");

                case enSaveOperation.OpDelete:
                    return ("Cancellazione avvenuta correttamente!");

                case enSaveOperation.Duplicazione:
                    return ("Duplicazione avvenuta con successo");

                default:
                    break;
            }
            return "";
        }
        public static bool QuestionMessage(string textQuestion)
        {
            return MessageBox.Show(textQuestion, "Domanda", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
        private static ContextMenuStrip getContextMenu(string text)
        {
            ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

            ToolStripMenuItem aboutToolStripMenuItem = new ToolStripMenuItem("Copia");
            aboutToolStripMenuItem.Click += (b, c) => { Clipboard.SetText(text); };
            contextMenuStrip1.Items.Add(aboutToolStripMenuItem);
            return contextMenuStrip1;
        }


        public static void NotificaWarnig(string info, Action action = null)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = StrumentiMusicali.App.Properties.Resources.warning_64;
            popup.TitleText = "Attenzione";
            popup.ContentText = info;
            popup.OptionsMenu = getContextMenu(info);
            popup.ShowOptionsButton = true;
            popup.Popup();

            if (action != null)
            {
                popup.Click += (a, b) => { action.Invoke(); };
            }
            else
            {
                ShowMessagio(popup);
            }
            _logger.Warn(info);
        }
        private static void ShowMessagio(PopupNotifier popup)
        {
            popup.Click += (b, c) =>
            {
                var frm = new Form();
                {
                    try
                    {
                        frm.Icon = UtilityView.GetIco(popup.Image as Bitmap);
                    }
                    catch
                    {

                    }
                    frm.Text = popup.TitleText;
                    frm.Size = new System.Drawing.Size(600, 600);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowInTaskbar = true;
                    frm.Controls.Add(new TextBox() { Multiline = true, Dock = DockStyle.Fill, Font = new System.Drawing.Font(Form.DefaultFont.Name, 14), Text = popup.ContentText });
                    frm.Show();
                }
            };
        }

        public static void NotificaError(string message, System.Exception ex)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = StrumentiMusicali.App.Properties.Resources.Error_64;
            popup.TitleText = "Error";
            popup.ContentText = message;
            popup.OptionsMenu = getContextMenu(message);
            popup.ShowOptionsButton = true;
            popup.Popup();
            if (ex != null)
            {
                popup.Click += delegate (object sender, System.EventArgs e)
                {
                    NotificaError(ex.Message, null);
                };
            }
            else
            {
                ShowMessagio(popup);
            }
        }
    }
}