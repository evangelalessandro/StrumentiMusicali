using DevExpress.XtraEditors;
using StrumentiMusicali.App.View.BaseControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFormApp
{
    public partial class Form3 : Form
    {

        MultiComboItems a = new MultiComboItems();
        public Form3()
        {
            InitializeComponent();

            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            a.InstanceItem();
            a.Init();
            a.InitDati();
            a.Combo.Dock = DockStyle.Fill;

            this.Controls.Add(a.Combo);

            ((PopupContainerEdit)a.Combo).Properties.EditValueChanged += Properties_EditValueChanged;
        }

        private void Properties_EditValueChanged(object sender, EventArgs e)
        {
            this.Text = ((PopupContainerEdit)sender).EditValue.ToString();
        }
    }
}
