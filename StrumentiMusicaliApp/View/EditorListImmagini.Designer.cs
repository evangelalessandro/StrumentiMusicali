namespace StrumentiMusicali.App.View
{
    partial class EditorListImmagini
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

         

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PanelImage = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuImpostaPrincipale = new System.Windows.Forms.ToolStripMenuItem();
            this.diminuisciPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aumentaPrioritàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rimuoviImmagineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelImage
            // 
            this.PanelImage.AllowDrop = true;
            this.PanelImage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelImage.Location = new System.Drawing.Point(0, 0);
            this.PanelImage.Margin = new System.Windows.Forms.Padding(2);
            this.PanelImage.Name = "PanelImage";
            this.PanelImage.Size = new System.Drawing.Size(742, 512);
            this.PanelImage.TabIndex = 2;
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
            // EditorListImmagini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelImage);
            this.Name = "EditorListImmagini";
            this.Size = new System.Drawing.Size(742, 512);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel PanelImage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuImpostaPrincipale;
        private System.Windows.Forms.ToolStripMenuItem diminuisciPrioritàToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aumentaPrioritàToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rimuoviImmagineToolStripMenuItem;
    }
}
