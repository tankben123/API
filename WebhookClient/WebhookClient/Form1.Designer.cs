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
            this.textBoxLogs = new RichTextBox();
            this.labelLogs = new Label();
            this.textBoxSheetId = new TextBox();
            this.buttonJoin = new Button();
            this.buttonExit = new Button();
            this.labelSheetId = new Label();
            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ClientSize = new Size(720, 380);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "🔥 Google Sheet Notifier";

            // 
            // labelSheetId
            // 
            this.labelSheetId.AutoSize = true;
            this.labelSheetId.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.labelSheetId.Location = new Point(25, 20);
            this.labelSheetId.Text = "Google Sheet ID:";

            // 
            // textBoxSheetId
            // 
            this.textBoxSheetId.Location = new Point(160, 18);
            this.textBoxSheetId.Size = new Size(400, 25);
            this.textBoxSheetId.Text = Properties.Settings.Default.SheetId;
            this.textBoxSheetId.BorderStyle = BorderStyle.FixedSingle;

            // 
            // buttonJoin
            // 
            this.buttonJoin.Location = new Point(580, 18);
            this.buttonJoin.Size = new Size(90, 28);
            this.buttonJoin.Text = "Kết nối";
            this.buttonJoin.FlatStyle = FlatStyle.Flat;
            this.buttonJoin.FlatAppearance.BorderSize = 0;
            this.buttonJoin.BackColor = Color.FromArgb(76, 175, 80);
            this.buttonJoin.ForeColor = Color.White;
            this.buttonJoin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.buttonJoin.Cursor = Cursors.Hand;
            this.buttonJoin.Click += button1_Click;

            // 
            // labelLogs
            // 
            this.labelLogs.AutoSize = true;
            this.labelLogs.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.labelLogs.Location = new Point(25, 60);
            this.labelLogs.Text = "📜 Logs:";

            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBoxLogs.BackColor = Color.FromArgb(250, 250, 250);
            this.textBoxLogs.Font = new Font("Consolas", 9F);
            this.textBoxLogs.Location = new Point(25, 85);
            this.textBoxLogs.ReadOnly = true;
            this.textBoxLogs.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.textBoxLogs.Size = new Size(645, 230);
            this.textBoxLogs.BorderStyle = BorderStyle.FixedSingle;

            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.buttonExit.Location = new Point(580, 325);
            this.buttonExit.Size = new Size(90, 28);
            this.buttonExit.Text = "Thoát";
            this.buttonExit.FlatStyle = FlatStyle.Flat;
            this.buttonExit.FlatAppearance.BorderSize = 0;
            this.buttonExit.BackColor = Color.Gray;
            this.buttonExit.ForeColor = Color.White;
            this.buttonExit.Cursor = Cursors.Hand;
            this.buttonExit.Click += button2_Click;

            // 
            // statusStrip
            // 
            this.statusStrip.Dock = DockStyle.Bottom;
            this.statusStrip.Items.AddRange(new ToolStripItem[] { this.statusLabel });

            // 
            // statusLabel
            // 
            this.statusLabel.Text = "Kết nối: Chưa kết nối";

            // Add controls to form
            this.Controls.Add(this.labelSheetId);
            this.Controls.Add(this.textBoxSheetId);
            this.Controls.Add(this.buttonJoin);
            this.Controls.Add(this.labelLogs);
            this.Controls.Add(this.textBoxLogs);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.statusStrip);
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
