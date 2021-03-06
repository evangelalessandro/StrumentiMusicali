﻿namespace StrumentiMusicali.App.View.Login
{
    partial class LoginForm
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
			this.LoginStatusStrip = new System.Windows.Forms.StatusStrip();
			this.LoginProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.LoginFormLayout = new System.Windows.Forms.TableLayoutPanel();
			this.btnCambioPassword = new System.Windows.Forms.Button();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.LoginButton = new System.Windows.Forms.Button();
			this.LoginStatusStrip.SuspendLayout();
			this.LoginFormLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// LoginStatusStrip
			// 
			this.LoginStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoginProgressBar});
			this.LoginStatusStrip.Location = new System.Drawing.Point(0, 301);
			this.LoginStatusStrip.Name = "LoginStatusStrip";
			this.LoginStatusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
			this.LoginStatusStrip.Size = new System.Drawing.Size(629, 36);
			this.LoginStatusStrip.TabIndex = 0;
			// 
			// LoginProgressBar
			// 
			this.LoginProgressBar.Name = "LoginProgressBar";
			this.LoginProgressBar.Size = new System.Drawing.Size(200, 30);
			// 
			// LoginFormLayout
			// 
			this.LoginFormLayout.BackColor = System.Drawing.Color.Transparent;
			this.LoginFormLayout.ColumnCount = 2;
			this.LoginFormLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.LoginFormLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.LoginFormLayout.Controls.Add(this.btnCambioPassword, 0, 5);
			this.LoginFormLayout.Controls.Add(this.UsernameLabel, 0, 0);
			this.LoginFormLayout.Controls.Add(this.txtUsername, 0, 1);
			this.LoginFormLayout.Controls.Add(this.PasswordLabel, 0, 2);
			this.LoginFormLayout.Controls.Add(this.txtPassword, 0, 3);
			this.LoginFormLayout.Controls.Add(this.LoginButton, 0, 5);
			this.LoginFormLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LoginFormLayout.Location = new System.Drawing.Point(0, 0);
			this.LoginFormLayout.Margin = new System.Windows.Forms.Padding(6);
			this.LoginFormLayout.Name = "LoginFormLayout";
			this.LoginFormLayout.RowCount = 6;
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.77131F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.72558F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.77131F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.72558F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.160505F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.84571F));
			this.LoginFormLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.LoginFormLayout.Size = new System.Drawing.Size(629, 301);
			this.LoginFormLayout.TabIndex = 1;
			// 
			// btnCambioPassword
			// 
			this.btnCambioPassword.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCambioPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCambioPassword.ForeColor = System.Drawing.Color.White;
			this.btnCambioPassword.Location = new System.Drawing.Point(393, 227);
			this.btnCambioPassword.Margin = new System.Windows.Forms.Padding(6);
			this.btnCambioPassword.Name = "btnCambioPassword";
			this.btnCambioPassword.Size = new System.Drawing.Size(230, 68);
			this.btnCambioPassword.TabIndex = 5;
			this.btnCambioPassword.Text = "Cambia Password";
			this.btnCambioPassword.UseVisualStyleBackColor = true;
			this.btnCambioPassword.Click += new System.EventHandler(this.btnCambioPassword_Click);
			// 
			// UsernameLabel
			// 
			this.UsernameLabel.AutoSize = true;
			this.UsernameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UsernameLabel.ForeColor = System.Drawing.Color.White;
			this.UsernameLabel.Location = new System.Drawing.Point(6, 0);
			this.UsernameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.UsernameLabel.Name = "UsernameLabel";
			this.UsernameLabel.Size = new System.Drawing.Size(375, 44);
			this.UsernameLabel.TabIndex = 0;
			this.UsernameLabel.Text = "Username";
			this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtUsername
			// 
			this.LoginFormLayout.SetColumnSpan(this.txtUsername, 2);
			this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUsername.Location = new System.Drawing.Point(6, 50);
			this.txtUsername.Margin = new System.Windows.Forms.Padding(6);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(617, 31);
			this.txtUsername.TabIndex = 1;
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PasswordLabel.ForeColor = System.Drawing.Color.White;
			this.PasswordLabel.Location = new System.Drawing.Point(6, 97);
			this.PasswordLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(375, 44);
			this.PasswordLabel.TabIndex = 0;
			this.PasswordLabel.Text = "Password";
			this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPassword
			// 
			this.LoginFormLayout.SetColumnSpan(this.txtPassword, 2);
			this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPassword.Location = new System.Drawing.Point(6, 147);
			this.txtPassword.Margin = new System.Windows.Forms.Padding(6);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(617, 31);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// LoginButton
			// 
			this.LoginButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LoginButton.ForeColor = System.Drawing.Color.White;
			this.LoginButton.Location = new System.Drawing.Point(151, 227);
			this.LoginButton.Margin = new System.Windows.Forms.Padding(6);
			this.LoginButton.Name = "LoginButton";
			this.LoginButton.Size = new System.Drawing.Size(230, 68);
			this.LoginButton.TabIndex = 4;
			this.LoginButton.Text = "Login";
			this.LoginButton.UseVisualStyleBackColor = true;
			this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::StrumentiMusicali.App.Properties.Resources.backGroudImageLogin1;
			this.ClientSize = new System.Drawing.Size(629, 337);
			this.Controls.Add(this.LoginFormLayout);
			this.Controls.Add(this.LoginStatusStrip);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Margin = new System.Windows.Forms.Padding(6);
			this.MaximizeBox = false;
			this.Name = "LoginForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.LoginStatusStrip.ResumeLayout(false);
			this.LoginStatusStrip.PerformLayout();
			this.LoginFormLayout.ResumeLayout(false);
			this.LoginFormLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip LoginStatusStrip;
        private System.Windows.Forms.TableLayoutPanel LoginFormLayout;
        private System.Windows.Forms.ToolStripProgressBar LoginProgressBar;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button LoginButton;
		private System.Windows.Forms.Button btnCambioPassword;
	}
}

