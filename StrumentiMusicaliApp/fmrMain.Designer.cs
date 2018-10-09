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
			this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
			this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			this.pnlArticoli = new System.Windows.Forms.Panel();
			this.pnlCerca = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCerca = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.ribAddArt = new System.Windows.Forms.RibbonButton();
			this.ribEditArt = new System.Windows.Forms.RibbonButton();
			this.ribDelete = new System.Windows.Forms.RibbonButton();
			this.ribCerca = new System.Windows.Forms.RibbonButton();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			this.pnlArticoli.SuspendLayout();
			this.pnlCerca.SuspendLayout();
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
			this.ribbon1.Size = new System.Drawing.Size(851, 154);
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
			this.ribbonTab1.Text = "Articoli";
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.Items.Add(this.ribAddArt);
			this.ribbonPanel1.Items.Add(this.ribEditArt);
			this.ribbonPanel1.Items.Add(this.ribDelete);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "Comandi Articoli";
			// 
			// ribbonPanel2
			// 
			this.ribbonPanel2.Items.Add(this.ribCerca);
			this.ribbonPanel2.Name = "ribbonPanel2";
			this.ribbonPanel2.Text = "Altro";
			// 
			// ribbonTab2
			// 
			this.ribbonTab2.Name = "ribbonTab2";
			this.ribbonTab2.Text = "ribbonTab2";
			// 
			// dgvMaster
			// 
			this.dgvMaster.AllowUserToAddRows = false;
			this.dgvMaster.AllowUserToDeleteRows = false;
			this.dgvMaster.AllowUserToOrderColumns = true;
			this.dgvMaster.AllowUserToResizeRows = false;
			this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvMaster.Location = new System.Drawing.Point(5, 5);
			this.dgvMaster.Margin = new System.Windows.Forms.Padding(2);
			this.dgvMaster.MultiSelect = false;
			this.dgvMaster.Name = "dgvMaster";
			this.dgvMaster.ReadOnly = true;
			this.dgvMaster.RowTemplate.Height = 24;
			this.dgvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMaster.Size = new System.Drawing.Size(841, 417);
			this.dgvMaster.TabIndex = 1;
			// 
			// pnlArticoli
			// 
			this.pnlArticoli.Controls.Add(this.dgvMaster);
			this.pnlArticoli.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlArticoli.Location = new System.Drawing.Point(0, 236);
			this.pnlArticoli.Name = "pnlArticoli";
			this.pnlArticoli.Padding = new System.Windows.Forms.Padding(5);
			this.pnlArticoli.Size = new System.Drawing.Size(851, 427);
			this.pnlArticoli.TabIndex = 2;
			// 
			// pnlCerca
			// 
			this.pnlCerca.Controls.Add(this.label1);
			this.pnlCerca.Controls.Add(this.txtCerca);
			this.pnlCerca.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pnlCerca.Location = new System.Drawing.Point(0, 154);
			this.pnlCerca.Name = "pnlCerca";
			this.pnlCerca.Size = new System.Drawing.Size(851, 71);
			this.pnlCerca.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Testo";
			// 
			// txtCerca
			// 
			this.txtCerca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCerca.Location = new System.Drawing.Point(76, 23);
			this.txtCerca.Name = "txtCerca";
			this.txtCerca.Size = new System.Drawing.Size(763, 22);
			this.txtCerca.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.PowderBlue;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 225);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(851, 11);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// ribAddArt
			// 
			this.ribAddArt.Image = ((System.Drawing.Image)(resources.GetObject("ribAddArt.Image")));
			this.ribAddArt.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribAddArt.LargeImage")));
			this.ribAddArt.Name = "ribAddArt";
			this.ribAddArt.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribAddArt.SmallImage")));
			this.ribAddArt.Text = "Crea";
			this.ribAddArt.Click += new System.EventHandler(this.ribAddArt_Click);
			// 
			// ribEditArt
			// 
			this.ribEditArt.Image = global::SturmentiMusicaliApp.Properties.Resources.Penna;
			this.ribEditArt.LargeImage = global::SturmentiMusicaliApp.Properties.Resources.Penna;
			this.ribEditArt.Name = "ribEditArt";
			this.ribEditArt.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribEditArt.SmallImage")));
			this.ribEditArt.Text = "Vedi\\Modifica";
			this.ribEditArt.Click += new System.EventHandler(this.ribEditArt_Click);
			// 
			// ribDelete
			// 
			this.ribDelete.Image = ((System.Drawing.Image)(resources.GetObject("ribDelete.Image")));
			this.ribDelete.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribDelete.LargeImage")));
			this.ribDelete.Name = "ribDelete";
			this.ribDelete.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribDelete.SmallImage")));
			this.ribDelete.Text = "Elimina";
			// 
			// ribCerca
			// 
			this.ribCerca.Image = global::SturmentiMusicaliApp.Properties.Resources.Cerca;
			this.ribCerca.LargeImage = global::SturmentiMusicaliApp.Properties.Resources.Cerca;
			this.ribCerca.Name = "ribCerca";
			this.ribCerca.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribCerca.SmallImage")));
			this.ribCerca.Text = "Cerca";
			// 
			// fmrMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 663);
			this.Controls.Add(this.pnlArticoli);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pnlCerca);
			this.Controls.Add(this.ribbon1);
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "fmrMain";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			this.pnlArticoli.ResumeLayout(false);
			this.pnlCerca.ResumeLayout(false);
			this.pnlCerca.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonTab ribbonTab2;
        private System.Windows.Forms.DataGridView dgvMaster;
		private System.Windows.Forms.RibbonButton ribAddArt;
		private System.Windows.Forms.RibbonButton ribEditArt;
		private System.Windows.Forms.RibbonButton ribDelete;
		private System.Windows.Forms.RibbonPanel ribbonPanel2;
		private System.Windows.Forms.RibbonButton ribCerca;
		private System.Windows.Forms.Panel pnlArticoli;
		private System.Windows.Forms.Panel pnlCerca;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCerca;
	}
}

