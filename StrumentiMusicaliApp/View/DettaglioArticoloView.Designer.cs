using StrumentiMusicali.App.CustomComponents;

namespace StrumentiMusicali.App.Forms
{
	partial class DettaglioArticoloView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lblID = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblCategoria = new System.Windows.Forms.Label();
			this.lblCondizione = new System.Windows.Forms.Label();
			this.lblMarca = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cboCategoria = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cboCondizione = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.txtTitolo = new StrumentiMusicali.App.CustomComponents.AutoCompleteTextBox();
			this.txtFiltroCategoria = new StrumentiMusicali.App.CustomComponents.AutoCompleteTextBox();
			this.txtMarca = new StrumentiMusicali.App.CustomComponents.AutoCompleteTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cboReparto = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.pnlTesto = new System.Windows.Forms.Panel();
			this.txtTesto = new StrumentiMusicali.App.CustomComponents.AutoCompleteTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.chkCaricainMercatino = new System.Windows.Forms.CheckBox();
			this.chkCaricainEcommerce = new System.Windows.Forms.CheckBox();
			this.txtPrezzoBarrato = new System.Windows.Forms.NumericUpDown();
			this.txtPrezzo = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.chkPrezzoARichiesta = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txtGiacenza = new System.Windows.Forms.NumericUpDown();
			this.chkBoxProposte = new System.Windows.Forms.CheckBox();
			this.chkUsaAnnuncioTurbo = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.PanelImage = new System.Windows.Forms.FlowLayoutPanel();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuImpostaPrincipale = new System.Windows.Forms.ToolStripMenuItem();
			this.diminuisciPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aumentaPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rimuoviImmagineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.txtCodiceABarre = new StrumentiMusicali.App.CustomComponents.AutoCompleteTextBox();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.pnlTesto.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtGiacenza)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(11, 10);
			this.lblID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(21, 16);
			this.lblID.TabIndex = 0;
			this.lblID.Text = "ID";
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(40, 7);
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
			this.lblCategoria.Location = new System.Drawing.Point(370, 76);
			this.lblCategoria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCategoria.Name = "lblCategoria";
			this.lblCategoria.Size = new System.Drawing.Size(67, 16);
			this.lblCategoria.TabIndex = 2;
			this.lblCategoria.Text = "Categoria";
			// 
			// lblCondizione
			// 
			this.lblCondizione.AutoSize = true;
			this.lblCondizione.Location = new System.Drawing.Point(11, 47);
			this.lblCondizione.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblCondizione.Name = "lblCondizione";
			this.lblCondizione.Size = new System.Drawing.Size(75, 16);
			this.lblCondizione.TabIndex = 4;
			this.lblCondizione.Text = "Condizione";
			// 
			// lblMarca
			// 
			this.lblMarca.AutoSize = true;
			this.lblMarca.Location = new System.Drawing.Point(391, 10);
			this.lblMarca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMarca.Name = "lblMarca";
			this.lblMarca.Size = new System.Drawing.Size(46, 16);
			this.lblMarca.TabIndex = 6;
			this.lblMarca.Text = "Marca";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 113);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Titolo";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new System.Drawing.Point(5, 5);
			this.label2.Margin = new System.Windows.Forms.Padding(10);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(10);
			this.label2.Size = new System.Drawing.Size(63, 36);
			this.label2.TabIndex = 10;
			this.label2.Text = "Testo";
			// 
			// cboCategoria
			// 
			this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCategoria.FormattingEnabled = true;
			this.cboCategoria.Location = new System.Drawing.Point(445, 73);
			this.cboCategoria.Name = "cboCategoria";
			this.cboCategoria.Size = new System.Drawing.Size(436, 24);
			this.cboCategoria.TabIndex = 4;
			this.cboCategoria.Tag = "Categoria";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(11, 76);
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
			this.cboCondizione.Location = new System.Drawing.Point(118, 42);
			this.cboCondizione.Name = "cboCondizione";
			this.cboCondizione.Size = new System.Drawing.Size(239, 24);
			this.cboCondizione.TabIndex = 5;
			this.cboCondizione.Tag = "Condizione";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.txtTitolo);
			this.panel2.Controls.Add(this.txtFiltroCategoria);
			this.panel2.Controls.Add(this.txtMarca);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.cboReparto);
			this.panel2.Controls.Add(this.cboCondizione);
			this.panel2.Controls.Add(this.lblID);
			this.panel2.Controls.Add(this.txtID);
			this.panel2.Controls.Add(this.label9);
			this.panel2.Controls.Add(this.lblCategoria);
			this.panel2.Controls.Add(this.lblCondizione);
			this.panel2.Controls.Add(this.cboCategoria);
			this.panel2.Controls.Add(this.lblMarca);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(909, 146);
			this.panel2.TabIndex = 31;
			// 
			// txtTitolo
			// 
			this.txtTitolo.Location = new System.Drawing.Point(60, 110);
			this.txtTitolo.Name = "txtTitolo";
			this.txtTitolo.Size = new System.Drawing.Size(821, 22);
			this.txtTitolo.TabIndex = 33;
			this.txtTitolo.Tag = "Titolo";
			this.txtTitolo.Values = null;
			// 
			// txtFiltroCategoria
			// 
			this.txtFiltroCategoria.Location = new System.Drawing.Point(118, 76);
			this.txtFiltroCategoria.Name = "txtFiltroCategoria";
			this.txtFiltroCategoria.Size = new System.Drawing.Size(239, 22);
			this.txtFiltroCategoria.TabIndex = 32;
			this.txtFiltroCategoria.Tag = "";
			this.txtFiltroCategoria.Values = null;
			// 
			// txtMarca
			// 
			this.txtMarca.Location = new System.Drawing.Point(445, 7);
			this.txtMarca.Name = "txtMarca";
			this.txtMarca.Size = new System.Drawing.Size(436, 22);
			this.txtMarca.TabIndex = 31;
			this.txtMarca.Tag = "Marca";
			this.txtMarca.Values = null;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(380, 44);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(57, 16);
			this.label6.TabIndex = 30;
			this.label6.Text = "Reparto";
			// 
			// cboReparto
			// 
			this.cboReparto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboReparto.FormattingEnabled = true;
			this.cboReparto.Location = new System.Drawing.Point(445, 44);
			this.cboReparto.Name = "cboReparto";
			this.cboReparto.Size = new System.Drawing.Size(436, 24);
			this.cboReparto.TabIndex = 29;
			this.cboReparto.Tag = "Reparto";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(923, 556);
			this.tabControl1.TabIndex = 29;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.pnlTesto);
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(915, 527);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Dati Articolo";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// pnlTesto
			// 
			this.pnlTesto.Controls.Add(this.txtTesto);
			this.pnlTesto.Controls.Add(this.label2);
			this.pnlTesto.Cursor = System.Windows.Forms.Cursors.Default;
			this.pnlTesto.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTesto.Location = new System.Drawing.Point(3, 149);
			this.pnlTesto.Name = "pnlTesto";
			this.pnlTesto.Padding = new System.Windows.Forms.Padding(5);
			this.pnlTesto.Size = new System.Drawing.Size(909, 238);
			this.pnlTesto.TabIndex = 33;
			// 
			// txtTesto
			// 
			this.txtTesto.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTesto.Location = new System.Drawing.Point(68, 5);
			this.txtTesto.Multiline = true;
			this.txtTesto.Name = "txtTesto";
			this.txtTesto.Size = new System.Drawing.Size(836, 228);
			this.txtTesto.TabIndex = 34;
			this.txtTesto.Tag = "Testo";
			this.txtTesto.Values = null;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.txtCodiceABarre);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.chkCaricainMercatino);
			this.panel1.Controls.Add(this.chkCaricainEcommerce);
			this.panel1.Controls.Add(this.txtPrezzoBarrato);
			this.panel1.Controls.Add(this.txtPrezzo);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.chkPrezzoARichiesta);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.txtGiacenza);
			this.panel1.Controls.Add(this.chkBoxProposte);
			this.panel1.Controls.Add(this.chkUsaAnnuncioTurbo);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 387);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(909, 137);
			this.panel1.TabIndex = 32;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(315, 98);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(98, 16);
			this.label5.TabIndex = 28;
			this.label5.Text = "Codice a Barre";
			// 
			// chkCaricainMercatino
			// 
			this.chkCaricainMercatino.AutoSize = true;
			this.chkCaricainMercatino.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkCaricainMercatino.Location = new System.Drawing.Point(545, 40);
			this.chkCaricainMercatino.Name = "chkCaricainMercatino";
			this.chkCaricainMercatino.Size = new System.Drawing.Size(141, 20);
			this.chkCaricainMercatino.TabIndex = 26;
			this.chkCaricainMercatino.Tag = "CaricainMercatino";
			this.chkCaricainMercatino.Text = "Carica in Mercatino";
			this.chkCaricainMercatino.UseVisualStyleBackColor = true;
			// 
			// chkCaricainEcommerce
			// 
			this.chkCaricainEcommerce.AutoSize = true;
			this.chkCaricainEcommerce.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkCaricainEcommerce.Location = new System.Drawing.Point(528, 14);
			this.chkCaricainEcommerce.Name = "chkCaricainEcommerce";
			this.chkCaricainEcommerce.Size = new System.Drawing.Size(158, 20);
			this.chkCaricainEcommerce.TabIndex = 25;
			this.chkCaricainEcommerce.Tag = "CaricainEcommerce";
			this.chkCaricainEcommerce.Text = "Carica In Ecommerce ";
			this.chkCaricainEcommerce.UseVisualStyleBackColor = true;
			// 
			// txtPrezzoBarrato
			// 
			this.txtPrezzoBarrato.DecimalPlaces = 2;
			this.txtPrezzoBarrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPrezzoBarrato.ForeColor = System.Drawing.Color.Red;
			this.txtPrezzoBarrato.Location = new System.Drawing.Point(118, 52);
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
			this.txtPrezzo.Size = new System.Drawing.Size(169, 29);
			this.txtPrezzo.TabIndex = 8;
			this.txtPrezzo.Tag = "Prezzo";
			this.txtPrezzo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtPrezzo.ThousandsSeparator = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(19, 18);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Prezzo";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(19, 60);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "Prezzo Barrato";
			// 
			// chkPrezzoARichiesta
			// 
			this.chkPrezzoARichiesta.AutoSize = true;
			this.chkPrezzoARichiesta.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkPrezzoARichiesta.Location = new System.Drawing.Point(298, 17);
			this.chkPrezzoARichiesta.Name = "chkPrezzoARichiesta";
			this.chkPrezzoARichiesta.Size = new System.Drawing.Size(139, 20);
			this.chkPrezzoARichiesta.TabIndex = 10;
			this.chkPrezzoARichiesta.Tag = "PrezzoARichiesta";
			this.chkPrezzoARichiesta.Text = "Prezzo A Richiesta";
			this.chkPrezzoARichiesta.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(45, 98);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 16);
			this.label8.TabIndex = 24;
			this.label8.Text = "Giacenza";
			// 
			// txtGiacenza
			// 
			this.txtGiacenza.Enabled = false;
			this.txtGiacenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGiacenza.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtGiacenza.Location = new System.Drawing.Point(118, 90);
			this.txtGiacenza.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtGiacenza.Name = "txtGiacenza";
			this.txtGiacenza.Size = new System.Drawing.Size(169, 29);
			this.txtGiacenza.TabIndex = 13;
			this.txtGiacenza.Tag = "Giacenza";
			this.txtGiacenza.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtGiacenza.ThousandsSeparator = true;
			// 
			// chkBoxProposte
			// 
			this.chkBoxProposte.AutoSize = true;
			this.chkBoxProposte.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkBoxProposte.Location = new System.Drawing.Point(733, 40);
			this.chkBoxProposte.Name = "chkBoxProposte";
			this.chkBoxProposte.Size = new System.Drawing.Size(108, 20);
			this.chkBoxProposte.TabIndex = 11;
			this.chkBoxProposte.Tag = "BoxProposte";
			this.chkBoxProposte.Text = "Box Proposte";
			this.chkBoxProposte.UseVisualStyleBackColor = true;
			// 
			// chkUsaAnnuncioTurbo
			// 
			this.chkUsaAnnuncioTurbo.AutoSize = true;
			this.chkUsaAnnuncioTurbo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUsaAnnuncioTurbo.Location = new System.Drawing.Point(692, 14);
			this.chkUsaAnnuncioTurbo.Name = "chkUsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.Size = new System.Drawing.Size(149, 20);
			this.chkUsaAnnuncioTurbo.TabIndex = 12;
			this.chkUsaAnnuncioTurbo.Tag = "UsaAnnuncioTurbo";
			this.chkUsaAnnuncioTurbo.Text = "Usa Annuncio Turbo";
			this.chkUsaAnnuncioTurbo.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.PanelImage);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1097, 693);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Immagini";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// PanelImage
			// 
			this.PanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelImage.Location = new System.Drawing.Point(3, 3);
			this.PanelImage.Name = "PanelImage";
			this.PanelImage.Size = new System.Drawing.Size(1091, 687);
			this.PanelImage.TabIndex = 1;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuImpostaPrincipale,
            this.diminuisciPrioritàToolStripMenuItem,
            this.aumentaPrioritàToolStripMenuItem,
            this.rimuoviImmagineToolStripMenuItem});
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
			// rimuoviImmagineToolStripMenuItem
			// 
			this.rimuoviImmagineToolStripMenuItem.Image = global::StrumentiMusicali.App.Properties.Resources.Delete;
			this.rimuoviImmagineToolStripMenuItem.Name = "rimuoviImmagineToolStripMenuItem";
			this.rimuoviImmagineToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.rimuoviImmagineToolStripMenuItem.Text = "Rimuovi Immagine";
			// 
			// txtCodiceABarre
			// 
			this.txtCodiceABarre.Location = new System.Drawing.Point(420, 95);
			this.txtCodiceABarre.Name = "txtCodiceABarre";
			this.txtCodiceABarre.Size = new System.Drawing.Size(471, 22);
			this.txtCodiceABarre.TabIndex = 34;
			this.txtCodiceABarre.Tag = "CodiceAbarre";
			this.txtCodiceABarre.Values = null;
			// 
			// DettaglioArticoloView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(923, 556);
			this.Name = "DettaglioArticoloView";
			this.Size = new System.Drawing.Size(923, 556);
			this.Load += new System.EventHandler(this.frmArticolo_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.pnlTesto.ResumeLayout(false);
			this.pnlTesto.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzoBarrato)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPrezzo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtGiacenza)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblCategoria;
		private System.Windows.Forms.Label lblCondizione;
		private System.Windows.Forms.Label lblMarca;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		 
	 
		private System.Windows.Forms.ComboBox cboCategoria;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboCondizione;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown txtPrezzoBarrato;
		private System.Windows.Forms.NumericUpDown txtPrezzo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkPrezzoARichiesta;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown txtGiacenza;
		private System.Windows.Forms.CheckBox chkBoxProposte;
		private System.Windows.Forms.CheckBox chkUsaAnnuncioTurbo;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.FlowLayoutPanel PanelImage;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuImpostaPrincipale;
		private System.Windows.Forms.ToolStripMenuItem diminuisciPrioritàToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aumentaPrioritàToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rimuoviImmagineToolStripMenuItem;
		private System.Windows.Forms.CheckBox chkCaricainEcommerce;
		private System.Windows.Forms.CheckBox chkCaricainMercatino;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cboReparto;
		private System.Windows.Forms.Panel pnlTesto;
		private System.Windows.Forms.Label label5;
		private AutoCompleteTextBox txtMarca;
		private AutoCompleteTextBox txtTitolo;
		private AutoCompleteTextBox txtFiltroCategoria;
		private AutoCompleteTextBox txtTesto;
		private AutoCompleteTextBox txtCodiceABarre;
	}
}