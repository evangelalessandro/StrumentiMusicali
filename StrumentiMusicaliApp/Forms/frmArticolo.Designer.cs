namespace SturmentiMusicaliApp.Forms
{
	partial class frmArticolo
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmArticolo));
			this.lblID = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblCategoria = new System.Windows.Forms.Label();
			this.lblCondizione = new System.Windows.Forms.Label();
			this.lblMarca = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPrezzo = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPrezzoBarrato = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.chkPrezzoARichiesta = new System.Windows.Forms.CheckBox();
			this.chkBoxProposte = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.chkUsaAnnuncioTurbo = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.ribbon1 = new System.Windows.Forms.Ribbon();
			this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
			this.ribSave = new System.Windows.Forms.RibbonButton();
			this.cboCategoria = new System.Windows.Forms.ComboBox();
			this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
			this.txtTesto = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtTitolo = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtMarca = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtCondizione = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtFiltroCategoria = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.label9 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			this.SuspendLayout();
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(98, 182);
			this.lblID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(21, 16);
			this.lblID.TabIndex = 0;
			this.lblID.Text = "ID";
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(152, 178);
			this.txtID.Margin = new System.Windows.Forms.Padding(4);
			this.txtID.Name = "txtID";
			this.txtID.ReadOnly = true;
			this.txtID.Size = new System.Drawing.Size(317, 22);
			this.txtID.TabIndex = 1;
			this.txtID.Tag = "ID";
			// 
			// lblCategoria
			// 
			this.lblCategoria.AutoSize = true;
			this.lblCategoria.Location = new System.Drawing.Point(75, 248);
			this.lblCategoria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCategoria.Name = "lblCategoria";
			this.lblCategoria.Size = new System.Drawing.Size(67, 16);
			this.lblCategoria.TabIndex = 2;
			this.lblCategoria.Text = "Categoria";
			// 
			// lblCondizione
			// 
			this.lblCondizione.AutoSize = true;
			this.lblCondizione.Location = new System.Drawing.Point(471, 252);
			this.lblCondizione.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCondizione.Name = "lblCondizione";
			this.lblCondizione.Size = new System.Drawing.Size(75, 16);
			this.lblCondizione.TabIndex = 4;
			this.lblCondizione.Text = "Condizione";
			// 
			// lblMarca
			// 
			this.lblMarca.AutoSize = true;
			this.lblMarca.Location = new System.Drawing.Point(500, 182);
			this.lblMarca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMarca.Name = "lblMarca";
			this.lblMarca.Size = new System.Drawing.Size(46, 16);
			this.lblMarca.TabIndex = 6;
			this.lblMarca.Text = "Marca";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(98, 285);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Titolo";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(88, 324);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Testo";
			// 
			// txtPrezzo
			// 
			this.txtPrezzo.DecimalPlaces = 2;
			this.txtPrezzo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrezzo.ForeColor = System.Drawing.Color.SeaGreen;
			this.txtPrezzo.Location = new System.Drawing.Point(150, 574);
			this.txtPrezzo.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtPrezzo.Name = "txtPrezzo";
			this.txtPrezzo.Size = new System.Drawing.Size(192, 29);
			this.txtPrezzo.TabIndex = 12;
			this.txtPrezzo.Tag = "Prezzo";
			this.txtPrezzo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPrezzo.ThousandsSeparator = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(96, 582);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Prezzo";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(376, 581);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "PrezzoBarrato";
			// 
			// txtPrezzoBarrato
			// 
			this.txtPrezzoBarrato.DecimalPlaces = 2;
			this.txtPrezzoBarrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrezzoBarrato.ForeColor = System.Drawing.Color.Red;
			this.txtPrezzoBarrato.Location = new System.Drawing.Point(474, 574);
			this.txtPrezzoBarrato.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtPrezzoBarrato.Name = "txtPrezzoBarrato";
			this.txtPrezzoBarrato.Size = new System.Drawing.Size(225, 29);
			this.txtPrezzoBarrato.TabIndex = 14;
			this.txtPrezzoBarrato.Tag = "PrezzoBarrato";
			this.txtPrezzoBarrato.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPrezzoBarrato.ThousandsSeparator = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(706, 581);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 17;
			this.label5.Tag = "";
			this.label5.Text = "Prezzo A Richiesta";
			// 
			// chkPrezzoARichiesta
			// 
			this.chkPrezzoARichiesta.AutoSize = true;
			this.chkPrezzoARichiesta.Location = new System.Drawing.Point(860, 583);
			this.chkPrezzoARichiesta.Name = "chkPrezzoARichiesta";
			this.chkPrezzoARichiesta.Size = new System.Drawing.Size(15, 14);
			this.chkPrezzoARichiesta.TabIndex = 18;
			this.chkPrezzoARichiesta.Tag = "PrezzoARichiesta";
			this.chkPrezzoARichiesta.UseVisualStyleBackColor = true;
			// 
			// chkBoxProposte
			// 
			this.chkBoxProposte.AutoSize = true;
			this.chkBoxProposte.Location = new System.Drawing.Point(150, 624);
			this.chkBoxProposte.Name = "chkBoxProposte";
			this.chkBoxProposte.Size = new System.Drawing.Size(15, 14);
			this.chkBoxProposte.TabIndex = 20;
			this.chkBoxProposte.Tag = "BoxProposte";
			this.chkBoxProposte.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(51, 622);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 16);
			this.label6.TabIndex = 19;
			this.label6.Tag = "";
			this.label6.Text = "Box Proposte";
			// 
			// chkUsaAnnuncioTurbo
			// 
			this.chkUsaAnnuncioTurbo.AutoSize = true;
			this.chkUsaAnnuncioTurbo.Location = new System.Drawing.Point(474, 622);
			this.chkUsaAnnuncioTurbo.Name = "chkUsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.Size = new System.Drawing.Size(15, 14);
			this.chkUsaAnnuncioTurbo.TabIndex = 22;
			this.chkUsaAnnuncioTurbo.Tag = "UsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(334, 622);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(130, 16);
			this.label7.TabIndex = 21;
			this.label7.Tag = "";
			this.label7.Text = "Usa Annuncio Turbo";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(75, 665);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 16);
			this.label8.TabIndex = 24;
			this.label8.Text = "Giacenza";
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numericUpDown3.ForeColor = System.Drawing.SystemColors.WindowText;
			this.numericUpDown3.Location = new System.Drawing.Point(150, 657);
			this.numericUpDown3.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(144, 29);
			this.numericUpDown3.TabIndex = 23;
			this.numericUpDown3.Tag = "Giacenza";
			this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown3.ThousandsSeparator = true;
			// 
			// ribbon1
			// 
			this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.ribbon1.Location = new System.Drawing.Point(0, 0);
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
			// 
			// 
			// 
			this.ribbon1.QuickAccessToolbar.Items.Add(this.ribbonButton1);
			this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			this.ribbon1.Size = new System.Drawing.Size(1002, 150);
			this.ribbon1.TabIndex = 25;
			this.ribbon1.Tabs.Add(this.ribbonTab1);
			this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
			this.ribbon1.Text = "ribbon1";
			// 
			// ribbonButton1
			// 
			this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
			this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
			this.ribbonButton1.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Compact;
			this.ribbonButton1.Name = "ribbonButton1";
			this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
			this.ribbonButton1.Text = "ribbonButton1";
			// 
			// ribbonTab1
			// 
			this.ribbonTab1.Name = "ribbonTab1";
			this.ribbonTab1.Panels.Add(this.ribbonPanel1);
			this.ribbonTab1.Panels.Add(this.ribbonPanel2);
			this.ribbonTab1.Text = "Articolo";
			// 
			// ribbonPanel1
			// 
			this.ribbonPanel1.Items.Add(this.ribSave);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "Strumenti";
			// 
			// ribSave
			// 
			this.ribSave.Image = ((System.Drawing.Image)(resources.GetObject("ribSave.Image")));
			this.ribSave.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribSave.LargeImage")));
			this.ribSave.Name = "ribSave";
			this.ribSave.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribSave.SmallImage")));
			this.ribSave.Text = "Salva";
			// 
			// cboCategoria
			// 
			this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCategoria.FormattingEnabled = true;
			this.cboCategoria.Location = new System.Drawing.Point(152, 248);
			this.cboCategoria.Name = "cboCategoria";
			this.cboCategoria.Size = new System.Drawing.Size(317, 24);
			this.cboCategoria.TabIndex = 26;
			this.cboCategoria.Tag = "Categoria";
			// 
			// ribbonPanel2
			// 
			this.ribbonPanel2.Name = "ribbonPanel2";
			this.ribbonPanel2.Text = "Filtra Categoria";
			// 
			// txtTesto
			// 
			this.txtTesto.Location = new System.Drawing.Point(150, 324);
			this.txtTesto.Margin = new System.Windows.Forms.Padding(4);
			this.txtTesto.Multiline = true;
			this.txtTesto.Name = "txtTesto";
			this.txtTesto.Size = new System.Drawing.Size(724, 231);
			this.txtTesto.TabIndex = 11;
			this.txtTesto.Tag = "Testo";
			this.txtTesto.Values = null;
			// 
			// txtTitolo
			// 
			this.txtTitolo.Location = new System.Drawing.Point(152, 285);
			this.txtTitolo.Margin = new System.Windows.Forms.Padding(4);
			this.txtTitolo.MaxLength = 100;
			this.txtTitolo.Name = "txtTitolo";
			this.txtTitolo.Size = new System.Drawing.Size(722, 22);
			this.txtTitolo.TabIndex = 9;
			this.txtTitolo.Tag = "Titolo";
			this.txtTitolo.Values = null;
			// 
			// txtMarca
			// 
			this.txtMarca.Location = new System.Drawing.Point(557, 179);
			this.txtMarca.Margin = new System.Windows.Forms.Padding(4);
			this.txtMarca.MaxLength = 100;
			this.txtMarca.Name = "txtMarca";
			this.txtMarca.Size = new System.Drawing.Size(317, 22);
			this.txtMarca.TabIndex = 7;
			this.txtMarca.Tag = "Marca";
			this.txtMarca.Values = null;
			// 
			// txtCondizione
			// 
			this.txtCondizione.Location = new System.Drawing.Point(557, 248);
			this.txtCondizione.Margin = new System.Windows.Forms.Padding(4);
			this.txtCondizione.Name = "txtCondizione";
			this.txtCondizione.Size = new System.Drawing.Size(317, 22);
			this.txtCondizione.TabIndex = 5;
			this.txtCondizione.Tag = "Condizione";
			this.txtCondizione.Values = null;
			// 
			// txtFiltroCategoria
			// 
			this.txtFiltroCategoria.Location = new System.Drawing.Point(152, 219);
			this.txtFiltroCategoria.Margin = new System.Windows.Forms.Padding(4);
			this.txtFiltroCategoria.MaxLength = 100;
			this.txtFiltroCategoria.Name = "txtFiltroCategoria";
			this.txtFiltroCategoria.Size = new System.Drawing.Size(317, 22);
			this.txtFiltroCategoria.TabIndex = 27;
			this.txtFiltroCategoria.Tag = "";
			this.txtFiltroCategoria.Values = null;
			this.txtFiltroCategoria.TextChanged += new System.EventHandler(this.txtFiltroCategoria_TextChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(37, 221);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(99, 16);
			this.label9.TabIndex = 28;
			this.label9.Text = "Filtro Categoria";
			// 
			// frmArticolo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1002, 689);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtFiltroCategoria);
			this.Controls.Add(this.cboCategoria);
			this.Controls.Add(this.ribbon1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.numericUpDown3);
			this.Controls.Add(this.chkUsaAnnuncioTurbo);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkBoxProposte);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.chkPrezzoARichiesta);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtPrezzoBarrato);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtPrezzo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTesto);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtTitolo);
			this.Controls.Add(this.txtMarca);
			this.Controls.Add(this.lblMarca);
			this.Controls.Add(this.txtCondizione);
			this.Controls.Add(this.lblCondizione);
			this.Controls.Add(this.lblCategoria);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.lblID);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "frmArticolo";
			this.Text = " ";
			this.Load += new System.EventHandler(this.frmArticolo_Load);
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblCategoria;
		private AutoCompleteTextBox txtCondizione;
		private System.Windows.Forms.Label lblCondizione;
		private AutoCompleteTextBox txtMarca;
		private System.Windows.Forms.Label lblMarca;
		private AutoCompleteTextBox txtTitolo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private AutoCompleteTextBox txtTesto;
		private System.Windows.Forms.NumericUpDown txtPrezzo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown txtPrezzoBarrato;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkPrezzoARichiesta;
		private System.Windows.Forms.CheckBox chkBoxProposte;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkUsaAnnuncioTurbo;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.Ribbon ribbon1;
		private System.Windows.Forms.RibbonTab ribbonTab1;
		private System.Windows.Forms.RibbonPanel ribbonPanel1;
		private System.Windows.Forms.RibbonButton ribSave;
		private System.Windows.Forms.RibbonButton ribbonButton1;
		private System.Windows.Forms.ComboBox cboCategoria;
		private System.Windows.Forms.RibbonPanel ribbonPanel2;
		private AutoCompleteTextBox txtFiltroCategoria;
		private System.Windows.Forms.Label label9;
	}
}