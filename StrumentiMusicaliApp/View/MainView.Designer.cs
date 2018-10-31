﻿namespace StrumentiMusicali.App
{
    partial class MainView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
			this.ribbon1 = new System.Windows.Forms.Ribbon();
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
			this.ribAddArt = new System.Windows.Forms.RibbonButton();
			this.ribEditArt = new System.Windows.Forms.RibbonButton();
			this.ribDelete = new System.Windows.Forms.RibbonButton();
			this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
			this.ribbonButton2 = new System.Windows.Forms.RibbonButton();
			this.ribArtDuplicate = new System.Windows.Forms.RibbonButton();
			this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
			this.ribCerca = new System.Windows.Forms.RibbonButton();
			this.ribbonPanel4 = new System.Windows.Forms.RibbonPanel();
			this.ribCaricaMagazzino = new System.Windows.Forms.RibbonButton();
			this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
			this.ribPnlInvioArticoli = new System.Windows.Forms.RibbonPanel();
			this.ribInvioDati = new System.Windows.Forms.RibbonButton();
			this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
			this.ribImportArticoli = new System.Windows.Forms.RibbonButton();
			this.ribbonButton3 = new System.Windows.Forms.RibbonButton();
			this.ribImportFattureAccess = new System.Windows.Forms.RibbonButton();
			this.ribTabFatture = new System.Windows.Forms.RibbonTab();
			this.ribPnlFatturazione = new System.Windows.Forms.RibbonPanel();
			this.ribBtnApriFatturazione = new System.Windows.Forms.RibbonButton();
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			this.pnlArticoli = new System.Windows.Forms.Panel();
			this.pnlCerca = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCerca = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.ribbonOrbOptionButton1 = new System.Windows.Forms.RibbonOrbOptionButton();
			this.ribbonOrbOptionButton2 = new System.Windows.Forms.RibbonOrbOptionButton();
			this.rbiTabSetting = new System.Windows.Forms.RibbonTab();
			this.ribPnlSetting = new System.Windows.Forms.RibbonPanel();
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
			this.ribbon1.OrbDropDown.Visible = false;
			this.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2010;
			// 
			// 
			// 
			this.ribbon1.QuickAccessToolbar.Visible = false;
			this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			this.ribbon1.Size = new System.Drawing.Size(851, 167);
			this.ribbon1.TabIndex = 0;
			this.ribbon1.Tabs.Add(this.ribbonTab1);
			this.ribbon1.Tabs.Add(this.ribbonTab2);
			this.ribbon1.Tabs.Add(this.ribTabFatture);
			this.ribbon1.Tabs.Add(this.rbiTabSetting);
			this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(6, 26, 20, 0);
			this.ribbon1.TabSpacing = 3;
			this.ribbon1.Text = "ribbon1";
			// 
			// ribbonTab1
			// 
			this.ribbonTab1.Name = "ribbonTab1";
			this.ribbonTab1.Panels.Add(this.ribbonPanel1);
			this.ribbonTab1.Panels.Add(this.ribbonPanel2);
			this.ribbonTab1.Panels.Add(this.ribbonPanel4);
			this.ribbonTab1.Text = "Articoli";
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.Items.Add(this.ribAddArt);
			this.ribbonPanel1.Items.Add(this.ribEditArt);
			this.ribbonPanel1.Items.Add(this.ribDelete);
			this.ribbonPanel1.Items.Add(this.ribArtDuplicate);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "Comandi Articoli";
			// 
			// ribAddArt
			// 
			this.ribAddArt.Image = global::StrumentiMusicali.App.Properties.Resources.Add;
			this.ribAddArt.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Add;
			this.ribAddArt.Name = "ribAddArt";
			this.ribAddArt.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribAddArt.SmallImage")));
			this.ribAddArt.Text = "Crea";
			this.ribAddArt.Click += new System.EventHandler(this.ribAddArt_Click);
			// 
			// ribEditArt
			// 
			this.ribEditArt.Image = global::StrumentiMusicali.App.Properties.Resources.Penna;
			this.ribEditArt.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Penna;
			this.ribEditArt.Name = "ribEditArt";
			this.ribEditArt.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribEditArt.SmallImage")));
			this.ribEditArt.Text = "Vedi\\Modifica";
			this.ribEditArt.Click += new System.EventHandler(this.ribEditArt_Click);
			// 
			// ribDelete
			// 
			this.ribDelete.DropDownItems.Add(this.ribbonButton1);
			this.ribDelete.DropDownItems.Add(this.ribbonButton2);
			this.ribDelete.Image = global::StrumentiMusicali.App.Properties.Resources.Delete;
			this.ribDelete.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Delete;
			this.ribDelete.Name = "ribDelete";
			this.ribDelete.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribDelete.SmallImage")));
			this.ribDelete.Text = "Elimina";
			// 
			// ribbonButton1
			// 
			this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
			this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
			this.ribbonButton1.Name = "ribbonButton1";
			this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
			this.ribbonButton1.Text = "ribbonButton1";
			// 
			// ribbonButton2
			// 
			this.ribbonButton2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.Image")));
			this.ribbonButton2.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.LargeImage")));
			this.ribbonButton2.Name = "ribbonButton2";
			this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
			this.ribbonButton2.Text = "ribbonButton2";
			// 
			// ribArtDuplicate
			// 
			this.ribArtDuplicate.Image = global::StrumentiMusicali.App.Properties.Resources.Duplicate;
			this.ribArtDuplicate.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Duplicate;
			this.ribArtDuplicate.Name = "ribArtDuplicate";
			this.ribArtDuplicate.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribArtDuplicate.SmallImage")));
			this.ribArtDuplicate.Text = "Duplica";
			this.ribArtDuplicate.Click += new System.EventHandler(this.ribArtDuplicate_Click);
			// 
			// ribbonPanel2
			// 
			this.ribbonPanel2.Items.Add(this.ribCerca);
			this.ribbonPanel2.Name = "ribbonPanel2";
			this.ribbonPanel2.Text = "Altro";
			// 
			// ribCerca
			// 
			this.ribCerca.CheckOnClick = true;
			this.ribCerca.FlashIntervall = 50;
			this.ribCerca.Image = global::StrumentiMusicali.App.Properties.Resources.Cerca;
			this.ribCerca.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Cerca;
			this.ribCerca.Name = "ribCerca";
			this.ribCerca.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribCerca.SmallImage")));
			this.ribCerca.Text = "Cerca";
			// 
			// ribbonPanel4
			// 
			this.ribbonPanel4.Items.Add(this.ribCaricaMagazzino);
			this.ribbonPanel4.Name = "ribbonPanel4";
			this.ribbonPanel4.Text = "Carico Scarico Magazzino";
			// 
			// ribCaricaMagazzino
			// 
			this.ribCaricaMagazzino.Image = ((System.Drawing.Image)(resources.GetObject("ribCaricaMagazzino.Image")));
			this.ribCaricaMagazzino.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribCaricaMagazzino.LargeImage")));
			this.ribCaricaMagazzino.Name = "ribCaricaMagazzino";
			this.ribCaricaMagazzino.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribCaricaMagazzino.SmallImage")));
			this.ribCaricaMagazzino.Text = "Apri";
			// 
			// ribbonTab2
			// 
			this.ribbonTab2.Name = "ribbonTab2";
			this.ribbonTab2.Panels.Add(this.ribPnlInvioArticoli);
			this.ribbonTab2.Panels.Add(this.ribbonPanel3);
			this.ribbonTab2.Text = "Import\\Export";
			// 
			// ribPnlInvioArticoli
			// 
			this.ribPnlInvioArticoli.Items.Add(this.ribInvioDati);
			this.ribPnlInvioArticoli.Name = "ribPnlInvioArticoli";
			this.ribPnlInvioArticoli.Text = "Invio articoli";
			// 
			// ribInvioDati
			// 
			this.ribInvioDati.Image = global::StrumentiMusicali.App.Properties.Resources.Upload;
			this.ribInvioDati.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Upload;
			this.ribInvioDati.Name = "ribInvioDati";
			this.ribInvioDati.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribInvioDati.SmallImage")));
			this.ribInvioDati.Text = "Invio Dati";
			// 
			// ribbonPanel3
			// 
			this.ribbonPanel3.ButtonMoreEnabled = false;
			this.ribbonPanel3.ButtonMoreVisible = false;
			this.ribbonPanel3.Items.Add(this.ribImportArticoli);
			this.ribbonPanel3.Items.Add(this.ribImportFattureAccess);
			this.ribbonPanel3.Name = "ribbonPanel3";
			this.ribbonPanel3.Text = "Import";
			// 
			// ribImportArticoli
			// 
			this.ribImportArticoli.DropDownItems.Add(this.ribbonButton3);
			this.ribImportArticoli.Image = global::StrumentiMusicali.App.Properties.Resources.Import;
			this.ribImportArticoli.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Import;
			this.ribImportArticoli.Name = "ribImportArticoli";
			this.ribImportArticoli.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribImportArticoli.SmallImage")));
			this.ribImportArticoli.Text = "Articoli mercatino excel csv";
			// 
			// ribbonButton3
			// 
			this.ribbonButton3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.Image")));
			this.ribbonButton3.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.LargeImage")));
			this.ribbonButton3.Name = "ribbonButton3";
			this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
			this.ribbonButton3.Text = "ribbonButton3";
			// 
			// ribImportFattureAccess
			// 
			this.ribImportFattureAccess.Image = global::StrumentiMusicali.App.Properties.Resources.Fatture_48;
			this.ribImportFattureAccess.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Fatture_48;
			this.ribImportFattureAccess.Name = "ribImportFattureAccess";
			this.ribImportFattureAccess.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribImportFattureAccess.SmallImage")));
			this.ribImportFattureAccess.Text = "Fatture Access";
			// 
			// ribTabFatture
			// 
			this.ribTabFatture.Name = "ribTabFatture";
			this.ribTabFatture.Panels.Add(this.ribPnlFatturazione);
			this.ribTabFatture.Text = "Fatturazione";
			// 
			// ribPnlFatturazione
			// 
			this.ribPnlFatturazione.ButtonMoreEnabled = false;
			this.ribPnlFatturazione.ButtonMoreVisible = false;
			this.ribPnlFatturazione.Items.Add(this.ribBtnApriFatturazione);
			this.ribPnlFatturazione.Name = "ribPnlFatturazione";
			this.ribPnlFatturazione.Text = "Comandi";
			// 
			// ribBtnApriFatturazione
			// 
			this.ribBtnApriFatturazione.Image = ((System.Drawing.Image)(resources.GetObject("ribBtnApriFatturazione.Image")));
			this.ribBtnApriFatturazione.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribBtnApriFatturazione.LargeImage")));
			this.ribBtnApriFatturazione.Name = "ribBtnApriFatturazione";
			this.ribBtnApriFatturazione.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribBtnApriFatturazione.SmallImage")));
			this.ribBtnApriFatturazione.Text = "Apri fatturazione";
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
			this.dgvMaster.Size = new System.Drawing.Size(841, 404);
			this.dgvMaster.TabIndex = 1;
			this.dgvMaster.DoubleClick += new System.EventHandler(this.dgvMaster_DoubleClick);
			// 
			// pnlArticoli
			// 
			this.pnlArticoli.Controls.Add(this.dgvMaster);
			this.pnlArticoli.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlArticoli.Location = new System.Drawing.Point(0, 249);
			this.pnlArticoli.Name = "pnlArticoli";
			this.pnlArticoli.Padding = new System.Windows.Forms.Padding(5);
			this.pnlArticoli.Size = new System.Drawing.Size(851, 414);
			this.pnlArticoli.TabIndex = 2;
			// 
			// pnlCerca
			// 
			this.pnlCerca.Controls.Add(this.label1);
			this.pnlCerca.Controls.Add(this.txtCerca);
			this.pnlCerca.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pnlCerca.Location = new System.Drawing.Point(0, 167);
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
			this.splitter1.Location = new System.Drawing.Point(0, 238);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(851, 11);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// ribbonOrbOptionButton1
			// 
			this.ribbonOrbOptionButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton1.Image")));
			this.ribbonOrbOptionButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton1.LargeImage")));
			this.ribbonOrbOptionButton1.Name = "ribbonOrbOptionButton1";
			this.ribbonOrbOptionButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton1.SmallImage")));
			this.ribbonOrbOptionButton1.Text = "ribbonOrbOptionButton1";
			// 
			// ribbonOrbOptionButton2
			// 
			this.ribbonOrbOptionButton2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton2.Image")));
			this.ribbonOrbOptionButton2.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton2.LargeImage")));
			this.ribbonOrbOptionButton2.Name = "ribbonOrbOptionButton2";
			this.ribbonOrbOptionButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbOptionButton2.SmallImage")));
			this.ribbonOrbOptionButton2.Text = "ribbonOrbOptionButton2";
			// 
			// rbiTabSetting
			// 
			this.rbiTabSetting.Name = "rbiTabSetting";
			this.rbiTabSetting.Panels.Add(this.ribPnlSetting);
			this.rbiTabSetting.Text = "Impostazioni";
			// 
			// ribPnlSetting
			// 
			this.ribPnlSetting.Name = "ribPnlSetting";
			this.ribPnlSetting.Text = "ribbonPanel5";
			// 
			// MainView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 663);
			this.Controls.Add(this.pnlArticoli);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pnlCerca);
			this.Controls.Add(this.ribbon1);
			this.IsMdiContainer = true;
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MainView";
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
		private System.Windows.Forms.RibbonButton ribbonButton1;
		private System.Windows.Forms.RibbonButton ribbonButton2;
		private System.Windows.Forms.RibbonButton ribArtDuplicate;
		private System.Windows.Forms.RibbonOrbOptionButton ribbonOrbOptionButton1;
		private System.Windows.Forms.RibbonOrbOptionButton ribbonOrbOptionButton2;
		private System.Windows.Forms.RibbonPanel ribPnlInvioArticoli;
		private System.Windows.Forms.RibbonButton ribInvioDati;
		private System.Windows.Forms.RibbonPanel ribbonPanel3;
		private System.Windows.Forms.RibbonButton ribImportArticoli;
		private System.Windows.Forms.RibbonPanel ribbonPanel4;
		private System.Windows.Forms.RibbonButton ribCaricaMagazzino;
		private System.Windows.Forms.RibbonButton ribbonButton3;
		private System.Windows.Forms.RibbonButton ribImportFattureAccess;
		private System.Windows.Forms.RibbonTab ribTabFatture;
		private System.Windows.Forms.RibbonPanel ribPnlFatturazione;
		private System.Windows.Forms.RibbonButton ribBtnApriFatturazione;
		private System.Windows.Forms.RibbonTab rbiTabSetting;
		private System.Windows.Forms.RibbonPanel ribPnlSetting;
	}
}

