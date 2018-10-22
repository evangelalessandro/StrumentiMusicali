namespace StrumentiMusicali.App.View.Base
{
	partial class BaseDataControl
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
			this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
			this.ribbon1 = new System.Windows.Forms.Ribbon();
			this.SuspendLayout();
			// 
			// ribbonTab1
			// 
			this.ribbonTab1.Name = "ribbonTab1";
			this.ribbonTab1.Text = "Articolo";
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
			this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
			this.ribbon1.Size = new System.Drawing.Size(1078, 141);
			this.ribbon1.TabIndex = 0;
			this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
			this.ribbon1.Text = "ribbon1";
			// 
			// BaseDataControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1078, 510);
			this.Controls.Add(this.ribbon1);
			this.KeyPreview = true;
			this.Name = "BaseDataControl";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RibbonTab ribbonTab1;
		public System.Windows.Forms.Ribbon ribbon1;
	}
}
