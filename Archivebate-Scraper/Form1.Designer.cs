namespace Archivebate_Scraper
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
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            textBoxID = new TextBox();
            textBoxPageFrom = new TextBox();
            textBoxPageTo = new TextBox();
            buttonDownload = new Button();
            textBoxURLs = new TextBox();
            progressBar1 = new ProgressBar();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(24, 20);
            label1.TabIndex = 0;
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 48);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 0;
            label2.Text = "Page";
            // 
            // textBoxID
            // 
            textBoxID.Location = new Point(72, 12);
            textBoxID.Name = "textBoxID";
            textBoxID.Size = new Size(125, 27);
            textBoxID.TabIndex = 1;
            // 
            // textBoxPageFrom
            // 
            textBoxPageFrom.Location = new Point(72, 45);
            textBoxPageFrom.Name = "textBoxPageFrom";
            textBoxPageFrom.Size = new Size(125, 27);
            textBoxPageFrom.TabIndex = 1;
            textBoxPageFrom.Text = "1";
            // 
            // textBoxPageTo
            // 
            textBoxPageTo.Location = new Point(229, 45);
            textBoxPageTo.Name = "textBoxPageTo";
            textBoxPageTo.Size = new Size(125, 27);
            textBoxPageTo.TabIndex = 1;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(360, 45);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(111, 29);
            buttonDownload.TabIndex = 2;
            buttonDownload.Text = "Start";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // textBoxURLs
            // 
            textBoxURLs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxURLs.Location = new Point(0, 114);
            textBoxURLs.Multiline = true;
            textBoxURLs.Name = "textBoxURLs";
            textBoxURLs.ReadOnly = true;
            textBoxURLs.ScrollBars = ScrollBars.Both;
            textBoxURLs.Size = new Size(495, 462);
            textBoxURLs.TabIndex = 3;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(12, 79);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(471, 29);
            progressBar1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(203, 48);
            label3.Name = "label3";
            label3.Size = new Size(20, 20);
            label3.TabIndex = 5;
            label3.Text = "~";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(495, 576);
            Controls.Add(label3);
            Controls.Add(progressBar1);
            Controls.Add(textBoxURLs);
            Controls.Add(buttonDownload);
            Controls.Add(textBoxPageTo);
            Controls.Add(textBoxPageFrom);
            Controls.Add(textBoxID);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Archibate_Scraper";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textBoxID;
        private TextBox textBoxPageFrom;
        private TextBox textBoxPageTo;
        private Button buttonDownload;
        private TextBox textBoxURLs;
        private ProgressBar progressBar1;
        private Label label3;
    }
}
