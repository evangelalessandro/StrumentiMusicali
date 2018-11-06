namespace StrumentiMusicali.App.View
{
	partial class ScaricoMagazzinoView
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
			this.txtCodiceABarre = new System.Windows.Forms.TextBox();
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			this.txtQta = new System.Windows.Forms.NumericUpDown();
			this.cboDeposito = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblTitoloArticolo = new System.Windows.Forms.Label();
			this.lblTitoloArt = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCodiceABarre
			// 
			this.txtCodiceABarre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCodiceABarre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCodiceABarre.Location = new System.Drawing.Point(18, 37);
			this.txtCodiceABarre.Name = "txtCodiceABarre";
			this.txtCodiceABarre.Size = new System.Drawing.Size(855, 26);
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
			this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvMaster.Location = new System.Drawing.Point(0, 160);
			this.dgvMaster.Margin = new System.Windows.Forms.Padding(2);
			this.dgvMaster.MultiSelect = false;
			this.dgvMaster.Name = "dgvMaster";
			this.dgvMaster.ReadOnly = true;
			this.dgvMaster.RowTemplate.Height = 24;
			this.dgvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMaster.Size = new System.Drawing.Size(893, 275);
			this.dgvMaster.TabIndex = 2;
			// 
			// txtQta
			// 
			this.txtQta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQta.Location = new System.Drawing.Point(196, 83);
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
			this.cboDeposito.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDeposito.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDeposito.FormattingEnabled = true;
			this.cboDeposito.Location = new System.Drawing.Point(438, 82);
			this.cboDeposito.Name = "cboDeposito";
			this.cboDeposito.Size = new System.Drawing.Size(436, 32);
			this.cboDeposito.TabIndex = 5;
			this.cboDeposito.Tag = "Categoria";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(322, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 25);
			this.label1.TabIndex = 6;
			this.label1.Text = "Deposito";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(97, 85);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 25);
			this.label2.TabIndex = 7;
			this.label2.Text = "Quantità";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(13, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(229, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Codice a barre articolo";
			// 
			// lblTitoloArticolo
			// 
			this.lblTitoloArticolo.AutoSize = true;
			this.lblTitoloArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArticolo.Location = new System.Drawing.Point(27, 127);
			this.lblTitoloArticolo.Name = "lblTitoloArticolo";
			this.lblTitoloArticolo.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArticolo.TabIndex = 9;
			this.lblTitoloArticolo.Text = "Titolo articolo:";
			// 
			// lblTitoloArt
			// 
			this.lblTitoloArt.AutoSize = true;
			this.lblTitoloArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArt.Location = new System.Drawing.Point(180, 127);
			this.lblTitoloArt.Name = "lblTitoloArt";
			this.lblTitoloArt.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArt.TabIndex = 10;
			this.lblTitoloArt.Text = "Titolo articolo:";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.lblTitoloArt);
			this.panel1.Controls.Add(this.txtCodiceABarre);
			this.panel1.Controls.Add(this.lblTitoloArticolo);
			this.panel1.Controls.Add(this.txtQta);
			this.panel1.Controls.Add(this.cboDeposito);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(893, 160);
			this.panel1.TabIndex = 11;
			// 
			// ScaricoMagazzino
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvMaster);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(893, 435);
			this.Name = "ScaricoMagazzino";
			this.Size = new System.Drawing.Size(893, 435);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtCodiceABarre;
		private System.Windows.Forms.DataGridView dgvMaster;
		private System.Windows.Forms.NumericUpDown txtQta;
		private System.Windows.Forms.ComboBox cboDeposito;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTitoloArticolo;
		private System.Windows.Forms.Label lblTitoloArt;
		private System.Windows.Forms.Panel panel1;
	}
}
