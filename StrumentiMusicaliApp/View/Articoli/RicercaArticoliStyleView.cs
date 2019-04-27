using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Articoli
{
    public class RicercaArticoliStyleView : BaseControl.BaseDataControl
    {
        private TextBox txtRicerca;
        private Label label1;
        private RichTextBox rtcNote;

        public RicercaArticoliStyleView()
            : base()
        {
            InitializeComponent();
        }
        ControllerArticoli _controllerArticoli;
        public RicercaArticoliStyleView(Core.Controllers.ControllerArticoli controllerArticoli)
            : base()
        {
            _controllerArticoli = controllerArticoli;
            InitializeComponent();
            txtRicerca.AcceptsReturn = true;
            this.txtRicerca.KeyUp += TxtRicerca_KeyUp;
        }

        private void TxtRicerca_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                AvviaRicerca();
            }
        }

        private void InitializeComponent()
        {
            this.txtRicerca = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtcNote = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtRicerca
            // 
            this.txtRicerca.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRicerca.Location = new System.Drawing.Point(10, 41);
            this.txtRicerca.Name = "txtRicerca";
            this.txtRicerca.Size = new System.Drawing.Size(836, 38);
            this.txtRicerca.TabIndex = 0;
            this.txtRicerca.TextChanged += new System.EventHandler(this.TxtRicerca_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ricerca:";
            // 
            // rtcNote
            // 
            this.rtcNote.AcceptsTab = true;
            this.rtcNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtcNote.Location = new System.Drawing.Point(10, 79);
            this.rtcNote.Name = "rtcNote";
            this.rtcNote.Size = new System.Drawing.Size(836, 386);
            this.rtcNote.TabIndex = 2;
            this.rtcNote.Text = "";
            // 
            // RicercaArticoliStyleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.Controls.Add(this.rtcNote);
            this.Controls.Add(this.txtRicerca);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RicercaArticoliStyleView";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(856, 475);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TxtRicerca_TextChanged(object sender, EventArgs e)
        {

            if (txtRicerca.Text.Contains(Environment.NewLine))
            {
                AvviaRicerca();
            }
        }

        private void AvviaRicerca()
        {
            using (var uof = new UnitOfWork())
            {
                rtcNote.Clear();

                var item = uof.ArticoliRepository.Find(a => a.CodiceABarre == txtRicerca.Text).
                    FirstOrDefault();
                txtRicerca.Text = "";
                if (item == null)
                {
                    return;
                }
                _controllerArticoli.EditItem = item;
                _controllerArticoli.SelectedItem = item;

                var itemQta = uof.MagazzinoRepository.Find(a => a.ArticoloID == item.ID).
                    Where(a => a.Deposito.Principale == true).
                    Select(a => a.Qta).Sum();


                AggiungiIntestazione("Codice:" );
                AggiungiValore(item.ID.ToString());
                AggiungiIntestazione("Articolo: ");
                AggiungiValore(item.Titolo.ToString());
                AggiungiIntestazione("Prezzo: ");
                AggiungiValore(item.Prezzo.ToString("C2"));
                AggiungiIntestazione("Pezzi: ");
                AggiungiValore(itemQta.ToString(""));


            }
        }

        private void AggiungiValore(string item)
        {
            richTextBox_LOG_write_text(item + Environment.NewLine, Color.Black, Color.Transparent);
        }

        private void AggiungiIntestazione(string dato)
        {

            richTextBox_LOG_write_text( dato.PadRight(10) + "\t", Color.Blue, Color.Transparent);
        }

        private void richTextBox_LOG_write_text(string text, Color text_color, Color background_color)
        {
            try
            {
                if (rtcNote.InvokeRequired == true)
                {
                    rtcNote.Invoke(new MethodInvoker(() => { richTextBox_LOG_write_text(text, text_color, background_color); }));
                    return;
                }
                int text_size = rtcNote.Text.Length;
                
                rtcNote.AppendText(text);
                rtcNote.Select(text_size, text.Length);
                if (text_color == null)
                {
                    text_color = Color.Black;
                }
                Font currentFont = rtcNote.SelectionFont;
                FontStyle newFontStyle = (FontStyle)(currentFont.Style | FontStyle.Bold);
                rtcNote.SelectionFont = new Font(currentFont.FontFamily, 28, newFontStyle);
                
                rtcNote.SelectionColor = text_color;
                if (background_color != null)
                {
                    rtcNote.SelectionBackColor = background_color;
                }
            }
            catch { }
        }
    }
}
