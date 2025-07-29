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
            txtSignalR = new TextBox();
            label1 = new Label();
            txtSheetId = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // txtSignalR
            // 
            txtSignalR.Location = new Point(36, 80);
            txtSignalR.Multiline = true;
            txtSignalR.Name = "txtSignalR";
            txtSignalR.ScrollBars = ScrollBars.Vertical;
            txtSignalR.Size = new Size(717, 199);
            txtSignalR.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 55);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 2;
            label1.Text = "Logs";
            label1.Click += label1_Click;
            // 
            // txtSheetId
            // 
            txtSheetId.Location = new Point(192, 17);
            txtSheetId.Name = "txtSheetId";
            txtSheetId.Size = new Size(381, 23);
            txtSheetId.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(592, 17);
            button1.Name = "button1";
            button1.Size = new Size(74, 23);
            button1.TabIndex = 5;
            button1.Text = "Nhận";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(678, 17);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "Thoát";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 21);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 7;
            label2.Text = "Google Sheet ID";
            label2.Click += label2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(785, 297);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(txtSheetId);
            Controls.Add(label1);
            Controls.Add(txtSignalR);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtSignalR;
        private Label label1;
        private TextBox txtSheetId;
        private Button button1;
        private Button button2;
        private Label label2;
    }
}
