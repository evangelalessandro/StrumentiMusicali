namespace SturmentiMusicaliApp
{
    partial class fmrMain
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmrMain));
			this.ribbon1 = new System.Windows.Forms.Ribbon();
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
			this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
			this.ribAddArt = new System.Windows.Forms.RibbonButton();
			this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
			this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbon1
			// 
			this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.ribbon1.Location = new System.Drawing.Point(0, 0);
			this.ribbon1.Margin = new System.Windows.Forms.Padding(2);
			this.ribbon1.Minimized = false;
			this.ribbon1.Name = "ribbon1";
			// 
			// 
			// 
			this.ribbon1.OrbDropDown.BorderRoundness = 8;
			this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
			this.ribbon1.OrbDropDown.Name = "";
			this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 447);
			this.ribbon1.OrbDropDown.TabIndex = 0;
			this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			this.ribbon1.Size = new System.Drawing.Size(836, 124);
			this.ribbon1.TabIndex = 0;
			this.ribbon1.Tabs.Add(this.ribbonTab1);
			this.ribbon1.Tabs.Add(this.ribbonTab2);
			this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
			this.ribbon1.Text = "ribbon1";
			// 
			// ribbonTab1
			// 
			this.ribbonTab1.Name = "ribbonTab1";
			this.ribbonTab1.Panels.Add(this.ribbonPanel1);
			this.ribbonTab1.Panels.Add(this.ribbonPanel2);
			this.ribbonTab1.Text = "ribbonTab1";
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.Items.Add(this.ribbonButton1);
			this.ribbonPanel1.Items.Add(this.ribAddArt);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "Comandi";
			// 
			// ribbonButton1
			// 
			this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
			this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
			this.ribbonButton1.Name = "ribbonButton1";
			this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
			this.ribbonButton1.Text = "Salva";
			// 
			// ribAddArt
			// 
			this.ribAddArt.Image = ((System.Drawing.Image)(resources.GetObject("ribAddArt.Image")));
			this.ribAddArt.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribAddArt.LargeImage")));
			this.ribAddArt.Name = "ribAddArt";
			this.ribAddArt.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribAddArt.SmallImage")));
			this.ribAddArt.Text = "ribbonButton2";
			this.ribAddArt.Click += new System.EventHandler(this.ribAddArt_Click);
			// 
			// ribbonPanel2
			// 
			this.ribbonPanel2.Name = "ribbonPanel2";
			this.ribbonPanel2.Text = "ribbonPanel2";
			// 
			// ribbonTab2
			// 
			this.ribbonTab2.Name = "ribbonTab2";
			this.ribbonTab2.Text = "ribbonTab2";
			// 
			// dgvMaster
			// 
			this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvMaster.Location = new System.Drawing.Point(0, 124);
			this.dgvMaster.Margin = new System.Windows.Forms.Padding(2);
			this.dgvMaster.Name = "dgvMaster";
			this.dgvMaster.RowTemplate.Height = 24;
			this.dgvMaster.Size = new System.Drawing.Size(836, 416);
			this.dgvMaster.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(836, 540);
			this.Controls.Add(this.dgvMaster);
			this.Controls.Add(this.ribbon1);
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton ribbonButton1;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonTab ribbonTab2;
        private System.Windows.Forms.DataGridView dgvMaster;
		private System.Windows.Forms.RibbonButton ribAddArt;
	}
}

