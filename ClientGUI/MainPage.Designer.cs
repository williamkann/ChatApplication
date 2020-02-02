namespace ClientGUI
{
    partial class MainPage
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
            this.usernameLabel = new System.Windows.Forms.Label();
            this.logoutLinkLabel = new System.Windows.Forms.LinkLabel();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.topicList = new System.Windows.Forms.ListBox();
            this.conversationTextbox = new System.Windows.Forms.TextBox();
            this.joinButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameLabel.Location = new System.Drawing.Point(27, 9);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(0, 29);
            this.usernameLabel.TabIndex = 0;
            this.usernameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // logoutLinkLabel
            // 
            this.logoutLinkLabel.AutoSize = true;
            this.logoutLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoutLinkLabel.Location = new System.Drawing.Point(919, 9);
            this.logoutLinkLabel.Name = "logoutLinkLabel";
            this.logoutLinkLabel.Size = new System.Drawing.Size(90, 29);
            this.logoutLinkLabel.TabIndex = 1;
            this.logoutLinkLabel.TabStop = true;
            this.logoutLinkLabel.Text = "Logout";
            this.logoutLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.logoutLinkLabel_LinkClicked);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(316, 448);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(546, 22);
            this.messageTextBox.TabIndex = 2;
            this.messageTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(883, 447);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(115, 23);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // topicList
            // 
            this.topicList.FormattingEnabled = true;
            this.topicList.ItemHeight = 16;
            this.topicList.Location = new System.Drawing.Point(32, 65);
            this.topicList.Name = "topicList";
            this.topicList.Size = new System.Drawing.Size(220, 324);
            this.topicList.TabIndex = 5;
            this.topicList.SelectedIndexChanged += new System.EventHandler(this.topicList_SelectedIndexChanged);
            // 
            // conversationTextbox
            // 
            this.conversationTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conversationTextbox.Location = new System.Drawing.Point(316, 65);
            this.conversationTextbox.Multiline = true;
            this.conversationTextbox.Name = "conversationTextbox";
            this.conversationTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.conversationTextbox.Size = new System.Drawing.Size(551, 339);
            this.conversationTextbox.TabIndex = 6;
            this.conversationTextbox.TextChanged += new System.EventHandler(this.conversationTextbox_TextChanged);
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(883, 418);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(115, 23);
            this.joinButton.TabIndex = 7;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 541);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.conversationTextbox);
            this.Controls.Add(this.topicList);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.logoutLinkLabel);
            this.Controls.Add(this.usernameLabel);
            this.Name = "MainPage";
            this.Text = "MainPage";
            this.Load += new System.EventHandler(this.MainPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.LinkLabel logoutLinkLabel;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.ListBox topicList;
        private System.Windows.Forms.TextBox conversationTextbox;
        private System.Windows.Forms.Button joinButton;
    }
}