namespace StrumentiMusicaliApp.Forms
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmArticolo));
			this.lblID = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblCategoria = new System.Windows.Forms.Label();
			this.lblCondizione = new System.Windows.Forms.Label();
			this.lblMarca = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ribbon1 = new System.Windows.Forms.Ribbon();
			this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
			this.ribSave = new System.Windows.Forms.RibbonButton();
			this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
			this.cboCategoria = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cboCondizione = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.txtFiltroCategoria = new StrumentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtMarca = new StrumentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtTitolo = new StrumentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.txtTesto = new StrumentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtPrezzoBarrato = new System.Windows.Forms.NumericUpDown();
			this.txtPrezzo = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.chkPrezzoARichiesta = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.chkBoxProposte = new System.Windows.Forms.CheckBox();
			this.chkUsaAnnuncioTurbo = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.ribAddImage = new System.Windows.Forms.RibbonButton();
			this.ribRemoveImage = new System.Windows.Forms.RibbonButton();
			this.PanelImage = new System.Windows.Forms.FlowLayoutPanel();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuImpostaPrincipale = new System.Windows.Forms.ToolStripMenuItem();
			this.diminuisciPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aumentaPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(89, 14);
			this.lblID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(21, 16);
			this.lblID.TabIndex = 0;
			this.lblID.Text = "ID";
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(118, 10);
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
			this.lblCategoria.Location = new System.Drawing.Point(43, 70);
			this.lblCategoria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCategoria.Name = "lblCategoria";
			this.lblCategoria.Size = new System.Drawing.Size(67, 16);
			this.lblCategoria.TabIndex = 2;
			this.lblCategoria.Text = "Categoria";
			// 
			// lblCondizione
			// 
			this.lblCondizione.AutoSize = true;
			this.lblCondizione.Location = new System.Drawing.Point(442, 73);
			this.lblCondizione.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCondizione.Name = "lblCondizione";
			this.lblCondizione.Size = new System.Drawing.Size(75, 16);
			this.lblCondizione.TabIndex = 4;
			this.lblCondizione.Text = "Condizione";
			// 
			// lblMarca
			// 
			this.lblMarca.AutoSize = true;
			this.lblMarca.Location = new System.Drawing.Point(443, 14);
			this.lblMarca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMarca.Name = "lblMarca";
			this.lblMarca.Size = new System.Drawing.Size(46, 16);
			this.lblMarca.TabIndex = 6;
			this.lblMarca.Text = "Marca";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(68, 107);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Titolo";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 156);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Testo";
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
			this.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2010;
			// 
			// 
			// 
			this.ribbon1.QuickAccessToolbar.Items.Add(this.ribbonButton1);
			this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			this.ribbon1.Size = new System.Drawing.Size(755, 150);
			this.ribbon1.TabIndex = 25;
			this.ribbon1.Tabs.Add(this.ribbonTab1);
			this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
			this.ribbon1.TabSpacing = 3;
			this.ribbon1.Text = "ribbon1";
			// 
			// ribbonButton1
			// 
			this.ribbonButton1.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Compact;
			this.ribbonButton1.Name = "ribbonButton1";
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
			this.ribbonPanel1.ButtonMoreEnabled = false;
			this.ribbonPanel1.ButtonMoreVisible = false;
			this.ribbonPanel1.Items.Add(this.ribSave);
			this.ribbonPanel1.Name = "ribbonPanel1";
			this.ribbonPanel1.Text = "Strumenti";
			// 
			// ribSave
			// 
			this.ribSave.Image = global::StrumentiMusicaliApp.Properties.Resources.Save;
			this.ribSave.LargeImage = global::StrumentiMusicaliApp.Properties.Resources.Save;
			this.ribSave.Name = "ribSave";
			this.ribSave.Text = "Salva";
			// 
			// ribbonPanel2
			// 
			this.ribbonPanel2.ButtonMoreEnabled = false;
			this.ribbonPanel2.ButtonMoreVisible = false;
			this.ribbonPanel2.Items.Add(this.ribAddImage);
			this.ribbonPanel2.Items.Add(this.ribRemoveImage);
			this.ribbonPanel2.Name = "ribbonPanel2";
			this.ribbonPanel2.Text = "Immagini";
			// 
			// cboCategoria
			// 
			this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCategoria.FormattingEnabled = true;
			this.cboCategoria.Location = new System.Drawing.Point(118, 70);
			this.cboCategoria.Name = "cboCategoria";
			this.cboCategoria.Size = new System.Drawing.Size(317, 24);
			this.cboCategoria.TabIndex = 4;
			this.cboCategoria.Tag = "Categoria";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(11, 43);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(99, 16);
			this.label9.TabIndex = 28;
			this.label9.Text = "Filtro Categoria";
			// 
			// cboCondizione
			// 
			this.cboCondizione.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCondizione.FormattingEnabled = true;
			this.cboCondizione.Location = new System.Drawing.Point(524, 67);
			this.cboCondizione.Name = "cboCondizione";
			this.cboCondizione.Size = new System.Drawing.Size(175, 24);
			this.cboCondizione.TabIndex = 5;
			this.cboCondizione.Tag = "Condizione";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.cboCondizione);
			this.panel2.Controls.Add(this.lblID);
			this.panel2.Controls.Add(this.txtID);
			this.panel2.Controls.Add(this.label9);
			this.panel2.Controls.Add(this.lblCategoria);
			this.panel2.Controls.Add(this.txtFiltroCategoria);
			this.panel2.Controls.Add(this.lblCondizione);
			this.panel2.Controls.Add(this.cboCategoria);
			this.panel2.Controls.Add(this.lblMarca);
			this.panel2.Controls.Add(this.txtMarca);
			this.panel2.Controls.Add(this.txtTitolo);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(741, 146);
			this.panel2.TabIndex = 31;
			// 
			// txtFiltroCategoria
			// 
			this.txtFiltroCategoria.Location = new System.Drawing.Point(118, 41);
			this.txtFiltroCategoria.Margin = new System.Windows.Forms.Padding(4);
			this.txtFiltroCategoria.MaxLength = 100;
			this.txtFiltroCategoria.Name = "txtFiltroCategoria";
			this.txtFiltroCategoria.Size = new System.Drawing.Size(317, 22);
			this.txtFiltroCategoria.TabIndex = 3;
			this.txtFiltroCategoria.Tag = "";
			this.txtFiltroCategoria.Values = null;
			this.txtFiltroCategoria.TextChanged += new System.EventHandler(this.txtFiltroCategoria_TextChanged);
			// 
			// txtMarca
			// 
			this.txtMarca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMarca.Location = new System.Drawing.Point(497, 11);
			this.txtMarca.Margin = new System.Windows.Forms.Padding(4);
			this.txtMarca.MaxLength = 100;
			this.txtMarca.Name = "txtMarca";
			this.txtMarca.Size = new System.Drawing.Size(231, 22);
			this.txtMarca.TabIndex = 2;
			this.txtMarca.Tag = "Marca";
			this.txtMarca.Values = null;
			// 
			// txtTitolo
			// 
			this.txtTitolo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitolo.Location = new System.Drawing.Point(118, 107);
			this.txtTitolo.Margin = new System.Windows.Forms.Padding(4);
			this.txtTitolo.MaxLength = 100;
			this.txtTitolo.Name = "txtTitolo";
			this.txtTitolo.Size = new System.Drawing.Size(608, 22);
			this.txtTitolo.TabIndex = 6;
			this.txtTitolo.Tag = "Titolo";
			this.txtTitolo.Values = null;
			// 
			// txtTesto
			// 
			this.txtTesto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTesto.Location = new System.Drawing.Point(59, 156);
			this.txtTesto.Margin = new System.Windows.Forms.Padding(4);
			this.txtTesto.Multiline = true;
			this.txtTesto.Name = "txtTesto";
			this.txtTesto.Size = new System.Drawing.Size(681, 62);
			this.txtTesto.TabIndex = 7;
			this.txtTesto.Tag = "Testo";
			this.txtTesto.Values = null;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 150);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(755, 353);
			this.tabControl1.TabIndex = 29;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.txtTesto);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(747, 324);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Dati Articolo";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.PanelImage);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(747, 324);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Immagini";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.txtPrezzoBarrato);
			this.panel1.Controls.Add(this.txtPrezzo);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.chkPrezzoARichiesta);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.numericUpDown3);
			this.panel1.Controls.Add(this.chkBoxProposte);
			this.panel1.Controls.Add(this.chkUsaAnnuncioTurbo);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 225);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(741, 96);
			this.panel1.TabIndex = 32;
			// 
			// txtPrezzoBarrato
			// 
			this.txtPrezzoBarrato.DecimalPlaces = 2;
			this.txtPrezzoBarrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrezzoBarrato.ForeColor = System.Drawing.Color.Red;
			this.txtPrezzoBarrato.Location = new System.Drawing.Point(417, 13);
			this.txtPrezzoBarrato.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtPrezzoBarrato.Name = "txtPrezzoBarrato";
			this.txtPrezzoBarrato.Size = new System.Drawing.Size(169, 29);
			this.txtPrezzoBarrato.TabIndex = 9;
			this.txtPrezzoBarrato.Tag = "PrezzoBarrato";
			this.txtPrezzoBarrato.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPrezzoBarrato.ThousandsSeparator = true;
			// 
			// txtPrezzo
			// 
			this.txtPrezzo.DecimalPlaces = 2;
			this.txtPrezzo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrezzo.ForeColor = System.Drawing.Color.SeaGreen;
			this.txtPrezzo.Location = new System.Drawing.Point(118, 14);
			this.txtPrezzo.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtPrezzo.Name = "txtPrezzo";
			this.txtPrezzo.Size = new System.Drawing.Size(152, 29);
			this.txtPrezzo.TabIndex = 8;
			this.txtPrezzo.Tag = "Prezzo";
			this.txtPrezzo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPrezzo.ThousandsSeparator = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(63, 22);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Prezzo";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(317, 21);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "PrezzoBarrato";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(593, 22);
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
			this.chkPrezzoARichiesta.Location = new System.Drawing.Point(720, 24);
			this.chkPrezzoARichiesta.Name = "chkPrezzoARichiesta";
			this.chkPrezzoARichiesta.Size = new System.Drawing.Size(15, 14);
			this.chkPrezzoARichiesta.TabIndex = 10;
			this.chkPrezzoARichiesta.Tag = "PrezzoARichiesta";
			this.chkPrezzoARichiesta.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(334, 62);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 16);
			this.label8.TabIndex = 24;
			this.label8.Text = "Giacenza";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(23, 62);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 16);
			this.label6.TabIndex = 19;
			this.label6.Tag = "";
			this.label6.Text = "Box Proposte";
			// 
			// numericUpDown3
			// 
			this.numericUpDown3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numericUpDown3.ForeColor = System.Drawing.SystemColors.WindowText;
			this.numericUpDown3.Location = new System.Drawing.Point(417, 52);
			this.numericUpDown3.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(169, 29);
			this.numericUpDown3.TabIndex = 13;
			this.numericUpDown3.Tag = "Giacenza";
			this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown3.ThousandsSeparator = true;
			// 
			// chkBoxProposte
			// 
			this.chkBoxProposte.AutoSize = true;
			this.chkBoxProposte.Location = new System.Drawing.Point(118, 64);
			this.chkBoxProposte.Name = "chkBoxProposte";
			this.chkBoxProposte.Size = new System.Drawing.Size(15, 14);
			this.chkBoxProposte.TabIndex = 11;
			this.chkBoxProposte.Tag = "BoxProposte";
			this.chkBoxProposte.UseVisualStyleBackColor = true;
			// 
			// chkUsaAnnuncioTurbo
			// 
			this.chkUsaAnnuncioTurbo.AutoSize = true;
			this.chkUsaAnnuncioTurbo.Location = new System.Drawing.Point(290, 62);
			this.chkUsaAnnuncioTurbo.Name = "chkUsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.Size = new System.Drawing.Size(15, 14);
			this.chkUsaAnnuncioTurbo.TabIndex = 12;
			this.chkUsaAnnuncioTurbo.Tag = "UsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(153, 60);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(130, 16);
			this.label7.TabIndex = 21;
			this.label7.Tag = "";
			this.label7.Text = "Usa Annuncio Turbo";
			// 
			// ribAddImage
			// 
			this.ribAddImage.Image = global::StrumentiMusicaliApp.Properties.Resources.Add;
			this.ribAddImage.LargeImage = global::StrumentiMusicaliApp.Properties.Resources.Add;
			this.ribAddImage.Name = "ribAddImage";
			this.ribAddImage.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribAddImage.SmallImage")));
			this.ribAddImage.Text = "Aggiungi";
			this.ribAddImage.Click += new System.EventHandler(this.ribAddImage_Click);
			// 
			// ribRemoveImage
			// 
			this.ribRemoveImage.Image = global::StrumentiMusicaliApp.Properties.Resources.Delete;
			this.ribRemoveImage.LargeImage = global::StrumentiMusicaliApp.Properties.Resources.Delete;
			this.ribRemoveImage.Name = "ribRemoveImage";
			this.ribRemoveImage.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribRemoveImage.SmallImage")));
			this.ribRemoveImage.Text = "Rimuovi";
			this.ribRemoveImage.Click += new System.EventHandler(this.ribRemoveImage_Click);
			// 
			// PanelImage
			// 
			this.PanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelImage.Location = new System.Drawing.Point(3, 3);
			this.PanelImage.Name = "PanelImage";
			this.PanelImage.Size = new System.Drawing.Size(741, 318);
			this.PanelImage.TabIndex = 1;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuImpostaPrincipale,
            this.diminuisciPrioritàToolStripMenuItem,
            this.aumentaPrioritàToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(206, 92);
			// 
			// menuImpostaPrincipale
			// 
			this.menuImpostaPrincipale.Name = "menuImpostaPrincipale";
			this.menuImpostaPrincipale.Size = new System.Drawing.Size(205, 22);
			this.menuImpostaPrincipale.Text = "Imposta come principale";
			// 
			// diminuisciPrioritàToolStripMenuItem
			// 
			this.diminuisciPrioritàToolStripMenuItem.Name = "diminuisciPrioritàToolStripMenuItem";
			this.diminuisciPrioritàToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.diminuisciPrioritàToolStripMenuItem.Text = "Diminuisci priorità";
			// 
			// aumentaPrioritàToolStripMenuItem
			// 
			this.aumentaPrioritàToolStripMenuItem.Name = "aumentaPrioritàToolStripMenuItem";
			this.aumentaPrioritàToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.aumentaPrioritàToolStripMenuItem.Text = "Aumenta priorità";
			// 
			// frmArticolo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(755, 503);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.ribbon1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(771, 542);
			this.Name = "frmArticolo";
			this.Text = " ";
			this.Load += new System.EventHandler(this.frmArticolo_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblCategoria;
		private System.Windows.Forms.Label lblCondizione;
		private AutoCompleteTextBox txtMarca;
		private System.Windows.Forms.Label lblMarca;
		private AutoCompleteTextBox txtTitolo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private AutoCompleteTextBox txtTesto;
		private System.Windows.Forms.Ribbon ribbon1;
		private System.Windows.Forms.RibbonTab ribbonTab1;
		private System.Windows.Forms.RibbonPanel ribbonPanel1;
		private System.Windows.Forms.RibbonButton ribbonButton1;
		private System.Windows.Forms.ComboBox cboCategoria;
		private System.Windows.Forms.RibbonPanel ribbonPanel2;
		private AutoCompleteTextBox txtFiltroCategoria;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.RibbonButton ribSave;
		private System.Windows.Forms.ComboBox cboCondizione;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown txtPrezzoBarrato;
		private System.Windows.Forms.NumericUpDown txtPrezzo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkPrezzoARichiesta;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.CheckBox chkBoxProposte;
		private System.Windows.Forms.CheckBox chkUsaAnnuncioTurbo;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.RibbonButton ribAddImage;
		private System.Windows.Forms.RibbonButton ribRemoveImage;
		private System.Windows.Forms.FlowLayoutPanel PanelImage;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuImpostaPrincipale;
		private System.Windows.Forms.ToolStripMenuItem diminuisciPrioritàToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aumentaPrioritàToolStripMenuItem;
	}
}