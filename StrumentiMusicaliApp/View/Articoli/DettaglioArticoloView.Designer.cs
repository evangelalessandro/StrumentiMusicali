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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.PanelImage = new System.Windows.Forms.FlowLayoutPanel();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuImpostaPrincipale = new System.Windows.Forms.ToolStripMenuItem();
			this.diminuisciPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aumentaPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rimuoviImmagineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(692, 325);
			this.tabControl1.TabIndex = 29;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPage1.Size = new System.Drawing.Size(684, 299);
			this.tabPage1.TabIndex = 2;
			this.tabPage1.Text = "Scheda articolo";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.PanelImage);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tabPage2.Size = new System.Drawing.Size(684, 299);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Immagini";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// PanelImage
			// 
			this.PanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelImage.Location = new System.Drawing.Point(2, 2);
			this.PanelImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.PanelImage.Name = "PanelImage";
			this.PanelImage.Size = new System.Drawing.Size(680, 295);
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
			// DettaglioArticoloView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(692, 325);
			this.Name = "DettaglioArticoloView";
			this.Size = new System.Drawing.Size(692, 325);
			this.Load += new System.EventHandler(this.frmArticolo_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.FlowLayoutPanel PanelImage;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuImpostaPrincipale;
		private System.Windows.Forms.ToolStripMenuItem diminuisciPrioritàToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aumentaPrioritàToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rimuoviImmagineToolStripMenuItem;
		private System.Windows.Forms.TabPage tabPage1;
	}
}