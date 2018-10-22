namespace StrumentiMusicali.App.View
{
	partial class ScaricoMagazzino
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

		#region Codice generato da Progettazione componenti

		/// <summary> 
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaricoMagazzino));
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
			this.ribCerca = new System.Windows.Forms.RibbonButton();
			this.ribCarica = new System.Windows.Forms.RibbonButton();
			this.ribScarica = new System.Windows.Forms.RibbonButton();
			this.txtCodiceABarre = new System.Windows.Forms.TextBox();
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			this.ribbonSeparator1 = new System.Windows.Forms.RibbonSeparator();
			this.txtQta = new System.Windows.Forms.NumericUpDown();
			this.cboDeposito = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblTitoloArticolo = new System.Windows.Forms.Label();
			this.lblTitoloArt = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbon1
			// 
			// 
			// 
			// 
			this.ribbon1.OrbDropDown.BorderRoundness = 8;
			this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
			this.ribbon1.OrbDropDown.Name = "";
			this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 447);
			this.ribbon1.OrbDropDown.TabIndex = 0;
			this.ribbon1.Size = new System.Drawing.Size(885, 157);
			this.ribbon1.Tabs.Add(this.ribbonTab1);
			// 
			// ribbonTab1
			// 
			this.ribbonTab1.Name = "ribbonTab1";
			this.ribbonTab1.Panels.Add(this.ribbonPanel1);
			this.ribbonTab1.Text = "Scarico magazzino";
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.Items.Add(this.ribCerca);
			this.ribbonPanel1.Items.Add(this.ribCarica);
			this.ribbonPanel1.Items.Add(this.ribScarica);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "";
			// 
			// ribCerca
			// 
			this.ribCerca.DropDownItems.Add(this.ribCarica);
			this.ribCerca.DropDownItems.Add(this.ribScarica);
			this.ribCerca.Image = global::StrumentiMusicali.App.Properties.Resources.Cerca;
			this.ribCerca.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Cerca;
			this.ribCerca.Name = "ribCerca";
			this.ribCerca.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribCerca.SmallImage")));
			this.ribCerca.Text = "Cerca";
			// 
			// ribCarica
			// 
			this.ribCarica.DropDownItems.Add(this.ribbonSeparator1);
			this.ribCarica.Image = global::StrumentiMusicali.App.Properties.Resources.Add;
			this.ribCarica.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Add;
			this.ribCarica.Name = "ribCarica";
			this.ribCarica.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribCarica.SmallImage")));
			this.ribCarica.Text = "Carica a magazzino";
			// 
			// ribScarica
			// 
			this.ribScarica.Image = global::StrumentiMusicali.App.Properties.Resources.Remove;
			this.ribScarica.LargeImage = global::StrumentiMusicali.App.Properties.Resources.Remove;
			this.ribScarica.Name = "ribScarica";
			this.ribScarica.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribScarica.SmallImage")));
			this.ribScarica.Text = "Scarica da magazzino";
			// 
			// txtCodiceABarre
			// 
			this.txtCodiceABarre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCodiceABarre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCodiceABarre.Location = new System.Drawing.Point(17, 197);
			this.txtCodiceABarre.Name = "txtCodiceABarre";
			this.txtCodiceABarre.Size = new System.Drawing.Size(856, 26);
			this.txtCodiceABarre.TabIndex = 1;
			this.txtCodiceABarre.TextChanged += new System.EventHandler(this.txtCodiceABarre_TextChanged);
			// 
			// dgvMaster
			// 
			this.dgvMaster.AllowUserToAddRows = false;
			this.dgvMaster.AllowUserToDeleteRows = false;
			this.dgvMaster.AllowUserToOrderColumns = true;
			this.dgvMaster.AllowUserToResizeRows = false;
			this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dgvMaster.Location = new System.Drawing.Point(0, 334);
			this.dgvMaster.Margin = new System.Windows.Forms.Padding(2);
			this.dgvMaster.MultiSelect = false;
			this.dgvMaster.Name = "dgvMaster";
			this.dgvMaster.ReadOnly = true;
			this.dgvMaster.RowTemplate.Height = 24;
			this.dgvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMaster.Size = new System.Drawing.Size(885, 240);
			this.dgvMaster.TabIndex = 2;
			// 
			// ribbonSeparator1
			// 
			this.ribbonSeparator1.Name = "ribbonSeparator1";
			// 
			// txtQta
			// 
			this.txtQta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQta.Location = new System.Drawing.Point(195, 243);
			this.txtQta.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.txtQta.Name = "txtQta";
			this.txtQta.Size = new System.Drawing.Size(120, 31);
			this.txtQta.TabIndex = 3;
			this.txtQta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtQta.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
			// 
			// cboDeposito
			// 
			this.cboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDeposito.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDeposito.FormattingEnabled = true;
			this.cboDeposito.Location = new System.Drawing.Point(437, 242);
			this.cboDeposito.Name = "cboDeposito";
			this.cboDeposito.Size = new System.Drawing.Size(436, 32);
			this.cboDeposito.TabIndex = 5;
			this.cboDeposito.Tag = "Categoria";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(321, 244);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 25);
			this.label1.TabIndex = 6;
			this.label1.Text = "Deposito";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(96, 245);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 25);
			this.label2.TabIndex = 7;
			this.label2.Text = "Quantità";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(12, 169);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(229, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Codice a barre articolo";
			// 
			// lblTitoloArticolo
			// 
			this.lblTitoloArticolo.AutoSize = true;
			this.lblTitoloArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArticolo.Location = new System.Drawing.Point(26, 287);
			this.lblTitoloArticolo.Name = "lblTitoloArticolo";
			this.lblTitoloArticolo.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArticolo.TabIndex = 9;
			this.lblTitoloArticolo.Text = "Titolo articolo:";
			// 
			// lblTitoloArt
			// 
			this.lblTitoloArt.AutoSize = true;
			this.lblTitoloArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArt.Location = new System.Drawing.Point(179, 287);
			this.lblTitoloArt.Name = "lblTitoloArt";
			this.lblTitoloArt.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArt.TabIndex = 10;
			this.lblTitoloArt.Text = "Titolo articolo:";
			// 
			// ScaricoMagazzino
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(885, 574);
			this.Controls.Add(this.lblTitoloArt);
			this.Controls.Add(this.lblTitoloArticolo);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboDeposito);
			this.Controls.Add(this.txtQta);
			this.Controls.Add(this.dgvMaster);
			this.Controls.Add(this.txtCodiceABarre);
			this.Name = "ScaricoMagazzino";
			this.Controls.SetChildIndex(this.ribbon1, 0);
			this.Controls.SetChildIndex(this.txtCodiceABarre, 0);
			this.Controls.SetChildIndex(this.dgvMaster, 0);
			this.Controls.SetChildIndex(this.txtQta, 0);
			this.Controls.SetChildIndex(this.cboDeposito, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.lblTitoloArticolo, 0);
			this.Controls.SetChildIndex(this.lblTitoloArt, 0);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RibbonTab ribbonTab1;
		private System.Windows.Forms.RibbonPanel ribbonPanel1;
		private System.Windows.Forms.RibbonButton ribCerca;
		private System.Windows.Forms.RibbonButton ribCarica;
		private System.Windows.Forms.RibbonButton ribScarica;
		private System.Windows.Forms.TextBox txtCodiceABarre;
		private System.Windows.Forms.DataGridView dgvMaster;
		private System.Windows.Forms.RibbonSeparator ribbonSeparator1;
		private System.Windows.Forms.NumericUpDown txtQta;
		private System.Windows.Forms.ComboBox cboDeposito;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTitoloArticolo;
		private System.Windows.Forms.Label lblTitoloArt;
	}
}
