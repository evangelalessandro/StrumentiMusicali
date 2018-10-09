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
			this.lblID = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblCategoria = new System.Windows.Forms.Label();
			this.txtCategoria = new SturmentiMusicaliApp.Forms.AutoCompleteTextBox();
			this.SuspendLayout();
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(36, 79);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(18, 13);
			this.lblID.TabIndex = 0;
			this.lblID.Text = "ID";
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(77, 76);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(239, 20);
			this.txtID.TabIndex = 1;
			this.txtID.Tag = "ID";
			// 
			// lblCategoria
			// 
			this.lblCategoria.AutoSize = true;
			this.lblCategoria.Location = new System.Drawing.Point(12, 110);
			this.lblCategoria.Name = "lblCategoria";
			this.lblCategoria.Size = new System.Drawing.Size(52, 13);
			this.lblCategoria.TabIndex = 2;
			this.lblCategoria.Text = "Categoria";
			// 
			// txtCategoria
			// 
			this.txtCategoria.Location = new System.Drawing.Point(77, 110);
			this.txtCategoria.Name = "txtCategoria";
			this.txtCategoria.Size = new System.Drawing.Size(239, 20);
			this.txtCategoria.TabIndex = 3;
			this.txtCategoria.Tag = "ID";
			this.txtCategoria.Values = null;
			// 
			// frmArticolo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(896, 680);
			this.Controls.Add(this.txtCategoria);
			this.Controls.Add(this.lblCategoria);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.lblID);
			this.Name = "frmArticolo";
			this.Text = "frmArticolo";
			this.Load += new System.EventHandler(this.frmArticolo_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblCategoria;
		private AutoCompleteTextBox txtCategoria;
	}
}