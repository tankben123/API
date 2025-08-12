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
            textBoxLogs = new TextBox();
            labelLogs = new Label();
            textBoxSheetId = new TextBox();
            buttonJoin = new Button();
            buttonExit = new Button();
            labelSheetId = new Label();
            SuspendLayout();
            // 
            // textBoxLogs
            // 
            textBoxLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxLogs.BackColor = SystemColors.ButtonHighlight;
            textBoxLogs.Location = new Point(36, 80);
            textBoxLogs.Multiline = true;
            textBoxLogs.Name = "textBoxLogs";
            textBoxLogs.ReadOnly = true;
            textBoxLogs.ScrollBars = ScrollBars.Vertical;
            textBoxLogs.Size = new Size(630, 199);
            textBoxLogs.TabIndex = 1;
            // 
            // labelLogs
            // 
            labelLogs.AutoSize = true;
            labelLogs.Location = new Point(36, 55);
            labelLogs.Name = "labelLogs";
            labelLogs.Size = new Size(32, 15);
            labelLogs.TabIndex = 2;
            labelLogs.Text = "Logs";
            // 
            // textBoxSheetId
            // 
            textBoxSheetId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSheetId.Location = new Point(192, 17);
            textBoxSheetId.Name = "textBoxSheetId";
            textBoxSheetId.Size = new Size(381, 23);
            textBoxSheetId.TabIndex = 4;
            textBoxSheetId.Text = Properties.Settings.Default.SheetId; // Load saved value
            textBoxSheetId.TextChanged += (s, e) =>
            {
                Properties.Settings.Default.SheetId = textBoxSheetId.Text; // Save value on change
                Properties.Settings.Default.Save();
            };
            // 
            // buttonJoin
            // 
            buttonJoin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonJoin.Location = new Point(592, 17);
            buttonJoin.Name = "buttonJoin";
            buttonJoin.Size = new Size(74, 23);
            buttonJoin.TabIndex = 5;
            buttonJoin.Text = "Nhận";
            buttonJoin.UseVisualStyleBackColor = true;
            buttonJoin.Click += button1_Click;
            // 
            // buttonExit
            // 
            buttonExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonExit.Location = new Point(592, 285);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(75, 23);
            buttonExit.TabIndex = 6;
            buttonExit.Text = "Thoát";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += button2_Click;
            // 
            // labelSheetId
            // 
            labelSheetId.AutoSize = true;
            labelSheetId.Location = new Point(36, 21);
            labelSheetId.Name = "labelSheetId";
            labelSheetId.Size = new Size(91, 15);
            labelSheetId.TabIndex = 7;
            labelSheetId.Text = "Google Sheet ID";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(703, 321);
            Controls.Add(labelSheetId);
            Controls.Add(buttonExit);
            Controls.Add(buttonJoin);
            Controls.Add(textBoxSheetId);
            Controls.Add(labelLogs);
            Controls.Add(textBoxLogs);
            Name = "Form1";
            Text = "Google Sheet Notifier";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBoxLogs;
        private Label labelLogs;
        private TextBox textBoxSheetId;
        private Button buttonJoin;
        private Button buttonExit;
        private Label labelSheetId;
    }
}
