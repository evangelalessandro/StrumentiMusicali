using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

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
			this.txtQta = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblTitoloArticolo = new System.Windows.Forms.Label();
			this.lblTitoloArt = new System.Windows.Forms.Label();
			this.pnlMidle = new System.Windows.Forms.Panel();
			this.lblGiacenzaArticolo = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.dgvRighe = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.listDepositi = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).BeginInit();
			this.pnlMidle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).BeginInit();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCodiceABarre
			// 
			this.txtCodiceABarre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCodiceABarre.Location = new System.Drawing.Point(9, 30);
			this.txtCodiceABarre.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtCodiceABarre.Name = "txtCodiceABarre";
			this.txtCodiceABarre.Size = new System.Drawing.Size(462, 26);
			this.txtCodiceABarre.TabIndex = 1;
			this.txtCodiceABarre.TextChanged += new System.EventHandler(this.txtCodiceABarre_TextChanged);
			// 
			// txtQta
			// 
			this.txtQta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQta.Location = new System.Drawing.Point(27, 169);
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
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(420, 36);
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
			this.label2.Location = new System.Drawing.Point(22, 139);
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
			this.label3.Location = new System.Drawing.Point(4, 0);
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
			this.lblTitoloArticolo.Location = new System.Drawing.Point(13, 8);
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
			this.lblTitoloArt.Location = new System.Drawing.Point(168, 8);
			this.lblTitoloArt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitoloArt.Name = "lblTitoloArt";
			this.lblTitoloArt.Size = new System.Drawing.Size(147, 25);
			this.lblTitoloArt.TabIndex = 10;
			this.lblTitoloArt.Text = "Titolo articolo:";
			// 
			// pnlMidle
			// 
			this.pnlMidle.Controls.Add(this.listDepositi);
			this.pnlMidle.Controls.Add(this.lblGiacenzaArticolo);
			this.pnlMidle.Controls.Add(this.label4);
			this.pnlMidle.Controls.Add(this.lblTitoloArt);
			this.pnlMidle.Controls.Add(this.lblTitoloArticolo);
			this.pnlMidle.Controls.Add(this.txtQta);
			this.pnlMidle.Controls.Add(this.label2);
			this.pnlMidle.Controls.Add(this.label1);
			this.pnlMidle.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMidle.Location = new System.Drawing.Point(0, 77);
			this.pnlMidle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlMidle.Name = "pnlMidle";
			this.pnlMidle.Size = new System.Drawing.Size(1000, 222);
			this.pnlMidle.TabIndex = 11;
			// 
			// lblGiacenzaArticolo
			// 
			this.lblGiacenzaArticolo.AutoSize = true;
			this.lblGiacenzaArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGiacenzaArticolo.ForeColor = System.Drawing.Color.DodgerBlue;
			this.lblGiacenzaArticolo.Location = new System.Drawing.Point(121, 79);
			this.lblGiacenzaArticolo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGiacenzaArticolo.Name = "lblGiacenzaArticolo";
			this.lblGiacenzaArticolo.Size = new System.Drawing.Size(53, 37);
			this.lblGiacenzaArticolo.TabIndex = 12;
			this.lblGiacenzaArticolo.Text = "10";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(22, 36);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(253, 25);
			this.label4.TabIndex = 11;
			this.label4.Text = "Giacenza Totale Articolo:";
			// 
			// gridControl1
			// 
			this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridControl1.Location = new System.Drawing.Point(0, 0);
			this.gridControl1.MainView = this.dgvRighe;
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.Size = new System.Drawing.Size(1000, 669);
			this.gridControl1.TabIndex = 0;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvRighe});
			// 
			// dgvRighe
			// 
			this.dgvRighe.GridControl = this.gridControl1;
			this.dgvRighe.Name = "dgvRighe";
			// 
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.label3);
			this.pnlTop.Controls.Add(this.txtCodiceABarre);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1000, 77);
			this.pnlTop.TabIndex = 14;
			// 
			// listDepositi
			// 
			this.listDepositi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listDepositi.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listDepositi.FormattingEnabled = true;
			this.listDepositi.ItemHeight = 25;
			this.listDepositi.Location = new System.Drawing.Point(425, 64);
			this.listDepositi.Name = "listDepositi";
			this.listDepositi.Size = new System.Drawing.Size(559, 154);
			this.listDepositi.TabIndex = 13;
			// 
			// ScaricoMagazzinoView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlMidle);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.gridControl1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(1000, 669);
			this.Name = "ScaricoMagazzinoView";
			this.Size = new System.Drawing.Size(1000, 669);
			((System.ComponentModel.ISupportInitialize)(this.txtQta)).EndInit();
			this.pnlMidle.ResumeLayout(false);
			this.pnlMidle.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).EndInit();
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtCodiceABarre;
		internal GridControl gridControl1;
		internal GridView dgvRighe;
		private System.Windows.Forms.NumericUpDown txtQta;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblTitoloArticolo;
		private System.Windows.Forms.Label lblTitoloArt;
		private System.Windows.Forms.Panel pnlMidle;
		private System.Windows.Forms.Label lblGiacenzaArticolo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.ListBox listDepositi;
	}
}
