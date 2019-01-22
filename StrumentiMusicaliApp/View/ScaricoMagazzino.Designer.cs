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
			this.pnlTop = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.lblGiacenzaArticolo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).BeginInit();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCodiceABarre
			// 
			this.txtCodiceABarre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCodiceABarre.Location = new System.Drawing.Point(25, 44);
			this.txtCodiceABarre.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtCodiceABarre.Name = "txtCodiceABarre";
			this.txtCodiceABarre.Size = new System.Drawing.Size(462, 26);
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
			this.dgvMaster.Location = new System.Drawing.Point(0, 227);
			this.dgvMaster.MultiSelect = false;
			this.dgvMaster.Name = "dgvMaster";
			this.dgvMaster.ReadOnly = true;
			this.dgvMaster.RowTemplate.Height = 24;
			this.dgvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvMaster.Size = new System.Drawing.Size(1000, 442);
			this.dgvMaster.TabIndex = 2;
			// 
			// txtQta
			// 
			this.txtQta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQta.Location = new System.Drawing.Point(123, 86);
			this.txtQta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtQta.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.txtQta.Name = "txtQta";
			this.txtQta.Size = new System.Drawing.Size(180, 31);
			this.txtQta.TabIndex = 3;
			this.txtQta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtQta.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
			this.txtQta.ValueChanged += new System.EventHandler(this.txtQta_ValueChanged);
			// 
			// cboDeposito
			// 
			this.cboDeposito.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDeposito.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDeposito.FormattingEnabled = true;
			this.cboDeposito.Location = new System.Drawing.Point(425, 86);
			this.cboDeposito.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboDeposito.Name = "cboDeposito";
			this.cboDeposito.Size = new System.Drawing.Size(559, 32);
			this.cboDeposito.TabIndex = 5;
			this.cboDeposito.Tag = "Categoria";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(320, 88);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 25);
			this.label1.TabIndex = 6;
			this.label1.Text = "Deposito";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(22, 88);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 25);
			this.label2.TabIndex = 7;
			this.label2.Text = "Quantità";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(20, 14);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(229, 25);
			this.label3.TabIndex = 8;
			this.label3.Text = "Codice a barre articolo";
			// 
			// lblTitoloArticolo
			// 
			this.lblTitoloArticolo.AutoSize = true;
			this.lblTitoloArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArticolo.Location = new System.Drawing.Point(22, 141);
			this.lblTitoloArticolo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitoloArticolo.Name = "lblTitoloArticolo";
			this.lblTitoloArticolo.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArticolo.TabIndex = 9;
			this.lblTitoloArticolo.Text = "Titolo articolo:";
			// 
			// lblTitoloArt
			// 
			this.lblTitoloArt.AutoSize = true;
			this.lblTitoloArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitoloArt.Location = new System.Drawing.Point(177, 141);
			this.lblTitoloArt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitoloArt.Name = "lblTitoloArt";
			this.lblTitoloArt.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArt.TabIndex = 10;
			this.lblTitoloArt.Text = "Titolo articolo:";
			// 
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.lblGiacenzaArticolo);
			this.pnlTop.Controls.Add(this.label4);
			this.pnlTop.Controls.Add(this.label3);
			this.pnlTop.Controls.Add(this.lblTitoloArt);
			this.pnlTop.Controls.Add(this.txtCodiceABarre);
			this.pnlTop.Controls.Add(this.lblTitoloArticolo);
			this.pnlTop.Controls.Add(this.txtQta);
			this.pnlTop.Controls.Add(this.cboDeposito);
			this.pnlTop.Controls.Add(this.label2);
			this.pnlTop.Controls.Add(this.label1);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1000, 227);
			this.pnlTop.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(22, 185);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(253, 25);
			this.label4.TabIndex = 11;
			this.label4.Text = "Giacenza Totale Articolo:";
			// 
			// lblGiacenzaArticolo
			// 
			this.lblGiacenzaArticolo.AutoSize = true;
			this.lblGiacenzaArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGiacenzaArticolo.ForeColor = System.Drawing.Color.DodgerBlue;
			this.lblGiacenzaArticolo.Location = new System.Drawing.Point(283, 185);
			this.lblGiacenzaArticolo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGiacenzaArticolo.Name = "lblGiacenzaArticolo";
			this.lblGiacenzaArticolo.Size = new System.Drawing.Size(39, 29);
			this.lblGiacenzaArticolo.TabIndex = 12;
			this.lblGiacenzaArticolo.Text = "10";
			// 
			// ScaricoMagazzinoView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvMaster);
			this.Controls.Add(this.pnlTop);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(1000, 669);
			this.Name = "ScaricoMagazzinoView";
			this.Size = new System.Drawing.Size(1000, 669);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).EndInit();
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
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
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblGiacenzaArticolo;
		private System.Windows.Forms.Label label4;
	}
}
