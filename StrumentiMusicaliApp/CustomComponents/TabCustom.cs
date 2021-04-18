using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StrumentiMusicali.App.CustomComponents
{
    public partial class TabCustom : UserControl
    {
        public TabCustom()
        {
            InitializeComponent();
            this.tabControl1.TabPages[this.tabControl1.TabCount - 1].Text = "";

            this.tabControl1.Padding = new Point(12, 4);
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;

            this.tabControl1.DrawItem += tabControl1_DrawItem;
            this.tabControl1.MouseDown += tabControl1_MouseDown;
            this.tabControl1.Selecting += tabControl1_Selecting;
            this.tabControl1.HandleCreated += tabControl1_HandleCreated;

            this.tabControl1.TabPages.Clear();
        }

        public TabPage AddTab(string text, string key)
        {
            tabControl1.TabPages.Add(key, text);

            var tab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
            tabControl1.SelectedTab = tab;
            return tab;
        }

        public void RemoveTab(TabPage control)
        {
            tabControl1.TabPages.Remove(control);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private bool _allowAdd = true;

        private void tabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tabControl1.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (AllowAdd && e.TabPageIndex == this.tabControl1.TabCount - 1)
                e.Cancel = true;
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            var lastIndex = this.tabControl1.TabCount - 1;
            if (AllowAdd && this.tabControl1.GetTabRect(lastIndex).Contains(e.Location))
            {
                this.tabControl1.TabPages.Insert(lastIndex, "New Tab");
                this.tabControl1.SelectedIndex = lastIndex;
                this.tabControl1.TabPages[lastIndex].UseVisualStyleBackColor = true;
            }
            else
            {
                for (var i = 0; i < this.tabControl1.TabPages.Count; i++)
                {
                    var tabRect = this.tabControl1.GetTabRect(i);
                    tabRect.Inflate(-2, -2);
                    var closeImage = StrumentiMusicali.Core.Properties.ImageIcons.Close_48;
                    var imageRect = new Rectangle(
                        (tabRect.Right - closeImage.Width),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                        closeImage.Width,
                        closeImage.Height);
                    if (imageRect.Contains(e.Location))
                    {
                        this.tabControl1.TabPages.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = this.tabControl1.TabPages[e.Index];
            var tabRect = this.tabControl1.GetTabRect(e.Index);
            tabRect.Inflate(-2, -2);
            if (AllowAdd && e.Index == this.tabControl1.TabCount - 1)
            {
                var addImage = StrumentiMusicali.Core.Properties.ImageIcons.Add_16;
                e.Graphics.DrawImage(addImage,
                    tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                    tabRect.Top + (tabRect.Height - addImage.Height) / 2);
            }
            else
            {
                var closeImage = Properties.Resources.Close_16;
                e.Graphics.DrawImage(closeImage,
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
                TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                    tabRect, tabPage.ForeColor, TextFormatFlags.Left);
            }
        }

        public bool AllowAdd {
            get => _allowAdd;
            set {
                _allowAdd = value;
                RefreshTab();
            }
        }

        private void RefreshTab()
        {
            if (!AllowAdd)
            {
                tabControl1.TabPages.Clear();
                //tabControl1.TabPages.RemoveAt(tabControl1.TabCount - 1);
            }
            else
            {
                tabControl1.TabPages.Add("");
            }
            tabControl1.Invalidate();
        }
    }
}