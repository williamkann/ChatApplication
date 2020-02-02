namespace ClientGUI
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
            this.password = new System.Windows.Forms.Label();
            this.login = new System.Windows.Forms.Label();
            this.passwordText = new System.Windows.Forms.TextBox();
            this.loginText = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.createAccountButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Location = new System.Drawing.Point(254, 203);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(69, 17);
            this.password.TabIndex = 9;
            this.password.Text = "Password";
            // 
            // login
            // 
            this.login.AutoSize = true;
            this.login.Location = new System.Drawing.Point(269, 146);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(43, 17);
            this.login.TabIndex = 8;
            this.login.Text = "Login";
            // 
            // passwordText
            // 
            this.passwordText.Location = new System.Drawing.Point(354, 198);
            this.passwordText.Name = "passwordText";
            this.passwordText.Size = new System.Drawing.Size(192, 22);
            this.passwordText.TabIndex = 7;
            // 
            // loginText
            // 
            this.loginText.Location = new System.Drawing.Point(354, 141);
            this.loginText.Name = "loginText";
            this.loginText.Size = new System.Drawing.Size(192, 22);
            this.loginText.TabIndex = 6;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(457, 261);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(111, 35);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // createAccountButton
            // 
            this.createAccountButton.Location = new System.Drawing.Point(295, 261);
            this.createAccountButton.Name = "createAccountButton";
            this.createAccountButton.Size = new System.Drawing.Size(119, 34);
            this.createAccountButton.TabIndex = 11;
            this.createAccountButton.Text = "Create Account";
            this.createAccountButton.UseVisualStyleBackColor = true;
            this.createAccountButton.Click += new System.EventHandler(this.createAccountButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.createAccountButton);
            this.Controls.Add(this.password);
            this.Controls.Add(this.login);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.loginText);
            this.Controls.Add(this.ok);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Label login;
        private System.Windows.Forms.TextBox passwordText;
        private System.Windows.Forms.TextBox loginText;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button createAccountButton;
    }
}