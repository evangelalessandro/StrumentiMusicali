using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace StrumentiMusicali.App.View.BaseControl
{
	partial class BaseGridViewGeneric<TBaseItem, TController, TEntity>
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
			this.gcControl = new DevExpress.XtraGrid.GridControl();
			this.dgvRighe = new DevExpress.XtraGrid.Views.Grid.GridView();
			((System.ComponentModel.ISupportInitialize)(this.gcControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).BeginInit();
			this.SuspendLayout();
			// 
			// gridControl1
			// 
			this.gcControl.Location = new System.Drawing.Point(8, -15);
			this.gcControl.MainView = this.dgvRighe;
			this.gcControl.Name = "gridControl1";
			this.gcControl.Size = new System.Drawing.Size(411, 244);
			this.gcControl.TabIndex = 0;
			this.gcControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvRighe});
			this.gcControl.Dock = System.Windows.Forms.DockStyle.Fill;
			//
			// gridView1
			// 
			this.dgvRighe.GridControl = this.gcControl;
			this.dgvRighe.Name = "gridView1";
			// 
			// BaseGridViewGeneric
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gcControl);
			this.Name = "BaseGridViewGeneric";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(427, 229);
			((System.ComponentModel.ISupportInitialize)(this.gcControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvRighe)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		internal GridControl gcControl;
		internal GridView dgvRighe;
	}
}
