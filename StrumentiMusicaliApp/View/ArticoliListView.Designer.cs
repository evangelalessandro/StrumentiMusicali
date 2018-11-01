namespace StrumentiMusicali.App
{
    partial class ArticoliListView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArticoliListView));
			this.dgvMaster = new System.Windows.Forms.DataGridView();
			this.pnlArticoli = new System.Windows.Forms.Panel();
			this.pnlCerca = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCerca = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
			this.pnlArticoli.SuspendLayout();
			this.pnlCerca.SuspendLayout();
			this.SuspendLayout();
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
			this.dgvMaster.Size = new System.Drawing.Size(841, 571);
			this.dgvMaster.TabIndex = 1;
			this.dgvMaster.DoubleClick += new System.EventHandler(this.dgvMaster_DoubleClick);
			// 
			// pnlArticoli
			// 
			this.pnlArticoli.Controls.Add(this.dgvMaster);
			this.pnlArticoli.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlArticoli.Location = new System.Drawing.Point(0, 82);
			this.pnlArticoli.Name = "pnlArticoli";
			this.pnlArticoli.Padding = new System.Windows.Forms.Padding(5);
			this.pnlArticoli.Size = new System.Drawing.Size(851, 581);
			this.pnlArticoli.TabIndex = 2;
			// 
			// pnlCerca
			// 
			this.pnlCerca.Controls.Add(this.label1);
			this.pnlCerca.Controls.Add(this.txtCerca);
			this.pnlCerca.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pnlCerca.Location = new System.Drawing.Point(0, 0);
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
			this.splitter1.Location = new System.Drawing.Point(0, 71);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(851, 11);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// MainView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlArticoli);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pnlCerca);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MainView";
			this.Size = new System.Drawing.Size(851, 663);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
			this.pnlArticoli.ResumeLayout(false);
			this.pnlCerca.ResumeLayout(false);
			this.pnlCerca.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
 
        private System.Windows.Forms.DataGridView dgvMaster;
		private System.Windows.Forms.Panel pnlArticoli;
		private System.Windows.Forms.Panel pnlCerca;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCerca;
		 
	}
}

