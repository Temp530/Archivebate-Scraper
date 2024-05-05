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
            textBoxSuccessURLs = new TextBox();
            progressBar1 = new ProgressBar();
            label3 = new Label();
            labelResult = new Label();
            label4 = new Label();
            textBoxCounterCount = new TextBox();
            checkBoxHeadless = new CheckBox();
            label5 = new Label();
            textBoxScraperCount = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            textBoxFailURLs = new TextBox();
            label7 = new Label();
            label6 = new Label();
            checkBoxOnlyPageURL = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 19);
            label1.Name = "label1";
            label1.Size = new Size(30, 25);
            label1.TabIndex = 0;
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 60);
            label2.Name = "label2";
            label2.Size = new Size(52, 25);
            label2.TabIndex = 0;
            label2.Text = "Page";
            // 
            // textBoxID
            // 
            textBoxID.Location = new Point(80, 15);
            textBoxID.Margin = new Padding(3, 4, 3, 4);
            textBoxID.Name = "textBoxID";
            textBoxID.Size = new Size(138, 31);
            textBoxID.TabIndex = 1;
            // 
            // textBoxPageFrom
            // 
            textBoxPageFrom.Location = new Point(80, 56);
            textBoxPageFrom.Margin = new Padding(3, 4, 3, 4);
            textBoxPageFrom.Name = "textBoxPageFrom";
            textBoxPageFrom.Size = new Size(138, 31);
            textBoxPageFrom.TabIndex = 1;
            textBoxPageFrom.Text = "1";
            // 
            // textBoxPageTo
            // 
            textBoxPageTo.Location = new Point(254, 56);
            textBoxPageTo.Margin = new Padding(3, 4, 3, 4);
            textBoxPageTo.Name = "textBoxPageTo";
            textBoxPageTo.Size = new Size(138, 31);
            textBoxPageTo.TabIndex = 1;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(400, 56);
            buttonDownload.Margin = new Padding(3, 4, 3, 4);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(123, 36);
            buttonDownload.TabIndex = 2;
            buttonDownload.Text = "Start";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // textBoxSuccessURLs
            // 
            textBoxSuccessURLs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSuccessURLs.Location = new Point(85, 4);
            textBoxSuccessURLs.Margin = new Padding(3, 4, 3, 4);
            textBoxSuccessURLs.Multiline = true;
            textBoxSuccessURLs.Name = "textBoxSuccessURLs";
            textBoxSuccessURLs.ReadOnly = true;
            textBoxSuccessURLs.ScrollBars = ScrollBars.Both;
            textBoxSuccessURLs.Size = new Size(435, 240);
            textBoxSuccessURLs.TabIndex = 3;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(12, 261);
            progressBar1.Margin = new Padding(3, 4, 3, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(523, 36);
            progressBar1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(226, 60);
            label3.Name = "label3";
            label3.Size = new Size(25, 25);
            label3.TabIndex = 5;
            label3.Text = "~";
            // 
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Dock = DockStyle.Bottom;
            labelResult.Location = new Point(0, 805);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(161, 25);
            labelResult.TabIndex = 6;
            labelResult.Text = "Success: 0 / Fail: 0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 129);
            label4.Name = "label4";
            label4.Size = new Size(122, 25);
            label4.TabIndex = 7;
            label4.Text = "Page Counter";
            // 
            // textBoxCounterCount
            // 
            textBoxCounterCount.Location = new Point(16, 158);
            textBoxCounterCount.Margin = new Padding(3, 4, 3, 4);
            textBoxCounterCount.Name = "textBoxCounterCount";
            textBoxCounterCount.Size = new Size(138, 31);
            textBoxCounterCount.TabIndex = 8;
            textBoxCounterCount.Text = "2";
            // 
            // checkBoxHeadless
            // 
            checkBoxHeadless.AutoSize = true;
            checkBoxHeadless.Checked = true;
            checkBoxHeadless.CheckState = CheckState.Checked;
            checkBoxHeadless.Location = new Point(13, 96);
            checkBoxHeadless.Margin = new Padding(3, 4, 3, 4);
            checkBoxHeadless.Name = "checkBoxHeadless";
            checkBoxHeadless.Size = new Size(111, 29);
            checkBoxHeadless.TabIndex = 9;
            checkBoxHeadless.Text = "Headless";
            checkBoxHeadless.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 193);
            label5.Name = "label5";
            label5.Size = new Size(112, 25);
            label5.TabIndex = 7;
            label5.Text = "URL Scraper";
            // 
            // textBoxScraperCount
            // 
            textBoxScraperCount.Location = new Point(15, 222);
            textBoxScraperCount.Margin = new Padding(3, 4, 3, 4);
            textBoxScraperCount.Name = "textBoxScraperCount";
            textBoxScraperCount.Size = new Size(138, 31);
            textBoxScraperCount.TabIndex = 8;
            textBoxScraperCount.Text = "24";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(textBoxSuccessURLs, 1, 0);
            tableLayoutPanel1.Controls.Add(textBoxFailURLs, 1, 1);
            tableLayoutPanel1.Controls.Add(label7, 0, 1);
            tableLayoutPanel1.Controls.Add(label6, 0, 0);
            tableLayoutPanel1.Location = new Point(13, 305);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(523, 497);
            tableLayoutPanel1.TabIndex = 10;
            // 
            // textBoxFailURLs
            // 
            textBoxFailURLs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFailURLs.Location = new Point(85, 252);
            textBoxFailURLs.Margin = new Padding(3, 4, 3, 4);
            textBoxFailURLs.Multiline = true;
            textBoxFailURLs.Name = "textBoxFailURLs";
            textBoxFailURLs.ReadOnly = true;
            textBoxFailURLs.ScrollBars = ScrollBars.Both;
            textBoxFailURLs.Size = new Size(435, 241);
            textBoxFailURLs.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 248);
            label7.Name = "label7";
            label7.Size = new Size(38, 25);
            label7.TabIndex = 5;
            label7.Text = "Fail";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 0);
            label6.Name = "label6";
            label6.Size = new Size(76, 25);
            label6.TabIndex = 4;
            label6.Text = "Success";
            // 
            // checkBoxOnlyPageURL
            // 
            checkBoxOnlyPageURL.AutoSize = true;
            checkBoxOnlyPageURL.Location = new Point(130, 97);
            checkBoxOnlyPageURL.Name = "checkBoxOnlyPageURL";
            checkBoxOnlyPageURL.Size = new Size(157, 29);
            checkBoxOnlyPageURL.TabIndex = 11;
            checkBoxOnlyPageURL.Text = "Page URL only";
            checkBoxOnlyPageURL.UseVisualStyleBackColor = true;
            checkBoxOnlyPageURL.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 830);
            Controls.Add(checkBoxOnlyPageURL);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(checkBoxHeadless);
            Controls.Add(textBoxScraperCount);
            Controls.Add(label5);
            Controls.Add(textBoxCounterCount);
            Controls.Add(label4);
            Controls.Add(labelResult);
            Controls.Add(label3);
            Controls.Add(progressBar1);
            Controls.Add(buttonDownload);
            Controls.Add(textBoxPageTo);
            Controls.Add(textBoxPageFrom);
            Controls.Add(textBoxID);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Archivebate_Scraper";
            FormClosing += Form1_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
        private TextBox textBoxSuccessURLs;
        private ProgressBar progressBar1;
        private Label label3;
        private Label labelResult;
        private Label label4;
        private TextBox textBoxCounterCount;
        private CheckBox checkBoxHeadless;
        private Label label5;
        private TextBox textBoxScraperCount;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBoxFailURLs;
        private Label label7;
        private Label label6;
        private CheckBox checkBoxOnlyPageURL;
    }
}
