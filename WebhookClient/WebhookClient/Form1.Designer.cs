using System.Threading.Tasks;

namespace WebhookClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private async Task InitializeComponent()
        {
            textBoxLogs = new RichTextBox();
            labelLogs = new Label();
            textBoxSheetId = new TextBox();
            buttonJoin = new Button();
            buttonExit = new Button();
            labelSheetId = new Label();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxLogs
            // 
            textBoxLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxLogs.BackColor = Color.FromArgb(250, 250, 250);
            textBoxLogs.BorderStyle = BorderStyle.FixedSingle;
            textBoxLogs.Font = new Font("Consolas", 9F);
            textBoxLogs.Location = new Point(25, 85);
            textBoxLogs.Name = "textBoxLogs";
            textBoxLogs.ReadOnly = true;
            textBoxLogs.ScrollBars = RichTextBoxScrollBars.Vertical;
            textBoxLogs.Size = new Size(669, 230);
            textBoxLogs.TabIndex = 4;
            textBoxLogs.Text = "";
            // 
            // labelLogs
            // 
            labelLogs.AutoSize = true;
            labelLogs.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelLogs.Location = new Point(25, 60);
            labelLogs.Name = "labelLogs";
            labelLogs.Size = new Size(68, 19);
            labelLogs.TabIndex = 3;
            labelLogs.Text = "📜 Logs:";
            // 
            // textBoxSheetId
            // 
            textBoxSheetId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSheetId.BorderStyle = BorderStyle.FixedSingle;
            textBoxSheetId.Location = new Point(152, 18);
            textBoxSheetId.Name = "textBoxSheetId";
            textBoxSheetId.Size = new Size(431, 23);
            textBoxSheetId.TabIndex = 1;
            textBoxSheetId.Text = Properties.Settings.Default.SheetId;
            // 
            // buttonJoin
            // 
            buttonJoin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonJoin.BackColor = Color.FromArgb(76, 175, 80);
            buttonJoin.Cursor = Cursors.Hand;
            buttonJoin.FlatAppearance.BorderSize = 0;
            buttonJoin.FlatStyle = FlatStyle.Flat;
            buttonJoin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonJoin.ForeColor = Color.White;
            buttonJoin.Location = new Point(604, 15);
            buttonJoin.Name = "buttonJoin";
            buttonJoin.Size = new Size(90, 28);
            buttonJoin.TabIndex = 2;
            buttonJoin.Text = "Kết nối";
            buttonJoin.UseVisualStyleBackColor = false;
            buttonJoin.Click += button1_Click;
            // 
            // buttonExit
            // 
            buttonExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonExit.BackColor = Color.Gray;
            buttonExit.Cursor = Cursors.Hand;
            buttonExit.FlatAppearance.BorderSize = 0;
            buttonExit.FlatStyle = FlatStyle.Flat;
            buttonExit.ForeColor = Color.White;
            buttonExit.Location = new Point(604, 325);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(90, 28);
            buttonExit.TabIndex = 5;
            buttonExit.Text = "Thoát";
            buttonExit.UseVisualStyleBackColor = false;
            buttonExit.Click += button2_Click;
            // 
            // labelSheetId
            // 
            labelSheetId.AutoSize = true;
            labelSheetId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelSheetId.Location = new Point(25, 20);
            labelSheetId.Name = "labelSheetId";
            labelSheetId.Size = new Size(121, 19);
            labelSheetId.TabIndex = 0;
            labelSheetId.Text = "Google Sheet ID:";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 358);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(720, 22);
            statusStrip.TabIndex = 6;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(117, 17);
            statusLabel.Text = "Kết nối: Chưa kết nối";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(720, 380);
            Controls.Add(labelSheetId);
            Controls.Add(textBoxSheetId);
            Controls.Add(buttonJoin);
            Controls.Add(labelLogs);
            Controls.Add(textBoxLogs);
            Controls.Add(buttonExit);
            Controls.Add(statusStrip);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Google Sheet Notifier";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.RichTextBox textBoxLogs;
        private System.Windows.Forms.Label labelLogs;
        private System.Windows.Forms.TextBox textBoxSheetId;
        private System.Windows.Forms.Button buttonJoin;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelSheetId;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}
