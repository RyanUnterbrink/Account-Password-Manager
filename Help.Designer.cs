namespace Account_Manager
{
    partial class Help
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
            this.invalidLoginsLabel = new System.Windows.Forms.Label();
            this.invalidLoginsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.afkTimerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.afkTimerLabel = new System.Windows.Forms.Label();
            this.hidePassRichTextBox = new System.Windows.Forms.RichTextBox();
            this.hidePassLabel = new System.Windows.Forms.Label();
            this.passOnMinRichTextBox = new System.Windows.Forms.RichTextBox();
            this.passOnMinLabel = new System.Windows.Forms.Label();
            this.clearClipRichTextBox = new System.Windows.Forms.RichTextBox();
            this.clearClipLabel = new System.Windows.Forms.Label();
            this.autoBackupRichTextBox = new System.Windows.Forms.RichTextBox();
            this.autoBackupLabel = new System.Windows.Forms.Label();
            this.invalidLoginsAlertRichTextBox = new System.Windows.Forms.RichTextBox();
            this.invalidLoginsAlertLabel = new System.Windows.Forms.Label();
            this.emailRichTextBox = new System.Windows.Forms.RichTextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.passKeyRichTextBox = new System.Windows.Forms.RichTextBox();
            this.passKeyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // invalidLoginsLabel
            // 
            this.invalidLoginsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invalidLoginsLabel.Location = new System.Drawing.Point(12, 9);
            this.invalidLoginsLabel.Name = "invalidLoginsLabel";
            this.invalidLoginsLabel.Size = new System.Drawing.Size(390, 20);
            this.invalidLoginsLabel.TabIndex = 1;
            this.invalidLoginsLabel.Text = "Invalid Logins (#)";
            this.invalidLoginsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // invalidLoginsRichTextBox
            // 
            this.invalidLoginsRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.invalidLoginsRichTextBox.Location = new System.Drawing.Point(30, 32);
            this.invalidLoginsRichTextBox.Name = "invalidLoginsRichTextBox";
            this.invalidLoginsRichTextBox.ReadOnly = true;
            this.invalidLoginsRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.invalidLoginsRichTextBox.TabIndex = 5;
            this.invalidLoginsRichTextBox.Text = "Exits the app if incorrect password attempts reaches this number.";
            // 
            // afkTimerRichTextBox
            // 
            this.afkTimerRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.afkTimerRichTextBox.Location = new System.Drawing.Point(30, 71);
            this.afkTimerRichTextBox.Name = "afkTimerRichTextBox";
            this.afkTimerRichTextBox.ReadOnly = true;
            this.afkTimerRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.afkTimerRichTextBox.TabIndex = 7;
            this.afkTimerRichTextBox.Text = "Exits the app if no user input is detected for this number of seconds.";
            // 
            // afkTimerLabel
            // 
            this.afkTimerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.afkTimerLabel.Location = new System.Drawing.Point(12, 48);
            this.afkTimerLabel.Name = "afkTimerLabel";
            this.afkTimerLabel.Size = new System.Drawing.Size(390, 20);
            this.afkTimerLabel.TabIndex = 6;
            this.afkTimerLabel.Text = "AFK Timer (s)";
            this.afkTimerLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // hidePassRichTextBox
            // 
            this.hidePassRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hidePassRichTextBox.Location = new System.Drawing.Point(30, 110);
            this.hidePassRichTextBox.Name = "hidePassRichTextBox";
            this.hidePassRichTextBox.ReadOnly = true;
            this.hidePassRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.hidePassRichTextBox.TabIndex = 9;
            this.hidePassRichTextBox.Text = "Hide passwords in account entries by covering them with *******.";
            // 
            // hidePassLabel
            // 
            this.hidePassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hidePassLabel.Location = new System.Drawing.Point(12, 87);
            this.hidePassLabel.Name = "hidePassLabel";
            this.hidePassLabel.Size = new System.Drawing.Size(390, 20);
            this.hidePassLabel.TabIndex = 8;
            this.hidePassLabel.Text = "Hide Passwords";
            this.hidePassLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passOnMinRichTextBox
            // 
            this.passOnMinRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passOnMinRichTextBox.Location = new System.Drawing.Point(30, 149);
            this.passOnMinRichTextBox.Name = "passOnMinRichTextBox";
            this.passOnMinRichTextBox.ReadOnly = true;
            this.passOnMinRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.passOnMinRichTextBox.TabIndex = 11;
            this.passOnMinRichTextBox.Text = "When the app is minimized, ask for password when maximized.";
            // 
            // passOnMinLabel
            // 
            this.passOnMinLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passOnMinLabel.Location = new System.Drawing.Point(12, 126);
            this.passOnMinLabel.Name = "passOnMinLabel";
            this.passOnMinLabel.Size = new System.Drawing.Size(390, 20);
            this.passOnMinLabel.TabIndex = 10;
            this.passOnMinLabel.Text = "Password On Minimize";
            this.passOnMinLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clearClipRichTextBox
            // 
            this.clearClipRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clearClipRichTextBox.Location = new System.Drawing.Point(30, 188);
            this.clearClipRichTextBox.Name = "clearClipRichTextBox";
            this.clearClipRichTextBox.ReadOnly = true;
            this.clearClipRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.clearClipRichTextBox.TabIndex = 13;
            this.clearClipRichTextBox.Text = "Clear the clipboard for erasing Ctrl + V pasting.";
            // 
            // clearClipLabel
            // 
            this.clearClipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearClipLabel.Location = new System.Drawing.Point(12, 165);
            this.clearClipLabel.Name = "clearClipLabel";
            this.clearClipLabel.Size = new System.Drawing.Size(390, 20);
            this.clearClipLabel.TabIndex = 12;
            this.clearClipLabel.Text = "Clear Clipboard On Exit";
            this.clearClipLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // autoBackupRichTextBox
            // 
            this.autoBackupRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.autoBackupRichTextBox.Location = new System.Drawing.Point(30, 237);
            this.autoBackupRichTextBox.Name = "autoBackupRichTextBox";
            this.autoBackupRichTextBox.ReadOnly = true;
            this.autoBackupRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.autoBackupRichTextBox.TabIndex = 15;
            this.autoBackupRichTextBox.Text = "Backup and send your data to your email.";
            // 
            // autoBackupLabel
            // 
            this.autoBackupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoBackupLabel.Location = new System.Drawing.Point(12, 214);
            this.autoBackupLabel.Name = "autoBackupLabel";
            this.autoBackupLabel.Size = new System.Drawing.Size(390, 20);
            this.autoBackupLabel.TabIndex = 14;
            this.autoBackupLabel.Text = "Auto Backup";
            this.autoBackupLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // invalidLoginsAlertRichTextBox
            // 
            this.invalidLoginsAlertRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.invalidLoginsAlertRichTextBox.Location = new System.Drawing.Point(30, 276);
            this.invalidLoginsAlertRichTextBox.Name = "invalidLoginsAlertRichTextBox";
            this.invalidLoginsAlertRichTextBox.ReadOnly = true;
            this.invalidLoginsAlertRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.invalidLoginsAlertRichTextBox.TabIndex = 17;
            this.invalidLoginsAlertRichTextBox.Text = "When the number of invalid logins is reached, an alert report will be emailed.";
            // 
            // invalidLoginsAlertLabel
            // 
            this.invalidLoginsAlertLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invalidLoginsAlertLabel.Location = new System.Drawing.Point(12, 253);
            this.invalidLoginsAlertLabel.Name = "invalidLoginsAlertLabel";
            this.invalidLoginsAlertLabel.Size = new System.Drawing.Size(390, 20);
            this.invalidLoginsAlertLabel.TabIndex = 16;
            this.invalidLoginsAlertLabel.Text = "Invalid Logins Alert";
            this.invalidLoginsAlertLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // emailRichTextBox
            // 
            this.emailRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.emailRichTextBox.Location = new System.Drawing.Point(30, 325);
            this.emailRichTextBox.Name = "emailRichTextBox";
            this.emailRichTextBox.ReadOnly = true;
            this.emailRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.emailRichTextBox.TabIndex = 19;
            this.emailRichTextBox.Text = "Insert your email to use for sending backups and alerts.";
            // 
            // emailLabel
            // 
            this.emailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLabel.Location = new System.Drawing.Point(12, 302);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(390, 20);
            this.emailLabel.TabIndex = 18;
            this.emailLabel.Text = "Email";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passKeyRichTextBox
            // 
            this.passKeyRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passKeyRichTextBox.Location = new System.Drawing.Point(30, 364);
            this.passKeyRichTextBox.Name = "passKeyRichTextBox";
            this.passKeyRichTextBox.ReadOnly = true;
            this.passKeyRichTextBox.Size = new System.Drawing.Size(372, 13);
            this.passKeyRichTextBox.TabIndex = 21;
            this.passKeyRichTextBox.Text = "Insert the smtp pass key associated with this app from your email.";
            // 
            // passKeyLabel
            // 
            this.passKeyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passKeyLabel.Location = new System.Drawing.Point(12, 341);
            this.passKeyLabel.Name = "passKeyLabel";
            this.passKeyLabel.Size = new System.Drawing.Size(390, 20);
            this.passKeyLabel.TabIndex = 20;
            this.passKeyLabel.Text = "Pass Key";
            this.passKeyLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 391);
            this.Controls.Add(this.passKeyRichTextBox);
            this.Controls.Add(this.passKeyLabel);
            this.Controls.Add(this.emailRichTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.invalidLoginsAlertRichTextBox);
            this.Controls.Add(this.invalidLoginsAlertLabel);
            this.Controls.Add(this.autoBackupRichTextBox);
            this.Controls.Add(this.autoBackupLabel);
            this.Controls.Add(this.clearClipRichTextBox);
            this.Controls.Add(this.clearClipLabel);
            this.Controls.Add(this.passOnMinRichTextBox);
            this.Controls.Add(this.passOnMinLabel);
            this.Controls.Add(this.hidePassRichTextBox);
            this.Controls.Add(this.hidePassLabel);
            this.Controls.Add(this.afkTimerRichTextBox);
            this.Controls.Add(this.afkTimerLabel);
            this.Controls.Add(this.invalidLoginsRichTextBox);
            this.Controls.Add(this.invalidLoginsLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Help";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.Help_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label invalidLoginsLabel;
        private System.Windows.Forms.RichTextBox invalidLoginsRichTextBox;
        private System.Windows.Forms.RichTextBox afkTimerRichTextBox;
        private System.Windows.Forms.Label afkTimerLabel;
        private System.Windows.Forms.RichTextBox hidePassRichTextBox;
        private System.Windows.Forms.Label hidePassLabel;
        private System.Windows.Forms.RichTextBox passOnMinRichTextBox;
        private System.Windows.Forms.Label passOnMinLabel;
        private System.Windows.Forms.RichTextBox clearClipRichTextBox;
        private System.Windows.Forms.Label clearClipLabel;
        private System.Windows.Forms.RichTextBox autoBackupRichTextBox;
        private System.Windows.Forms.Label autoBackupLabel;
        private System.Windows.Forms.RichTextBox invalidLoginsAlertRichTextBox;
        private System.Windows.Forms.Label invalidLoginsAlertLabel;
        private System.Windows.Forms.RichTextBox emailRichTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.RichTextBox passKeyRichTextBox;
        private System.Windows.Forms.Label passKeyLabel;
    }
}