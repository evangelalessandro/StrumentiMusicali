namespace StrumentiMusicali.App.View.BaseControl
{
	partial class BaseGridViewGeneric<TBaseItem, TController, TEntity>
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
			this.dgvRighe = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvRighe
			// 
			this.dgvRighe.AllowUserToAddRows = false;
			this.dgvRighe.AllowUserToDeleteRows = false;
			this.dgvRighe.AllowUserToOrderColumns = true;
			this.dgvRighe.AllowUserToResizeRows = false;
			this.dgvRighe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRighe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvRighe.Location = new System.Drawing.Point(5, 5);
			this.dgvRighe.Margin = new System.Windows.Forms.Padding(2);
			this.dgvRighe.MultiSelect = false;
			this.dgvRighe.Name = "dgvRighe";
			this.dgvRighe.ReadOnly = true;
			this.dgvRighe.RowTemplate.Height = 24;
			this.dgvRighe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvRighe.Size = new System.Drawing.Size(417, 219);
			this.dgvRighe.TabIndex = 3;
			// 
			// FatturaRigheListView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvRighe);
			this.Name = "FatturaRigheListView";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(427, 229);
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.DataGridView dgvRighe;
	}
}
