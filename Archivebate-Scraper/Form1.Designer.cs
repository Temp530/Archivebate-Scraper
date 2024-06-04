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
            textBoxID.TabIndex = 0;
            // 
            // textBoxPageFrom
            // 
            textBoxPageFrom.Location = new Point(72, 45);
            textBoxPageFrom.Name = "textBoxPageFrom";
            textBoxPageFrom.Size = new Size(125, 27);
            textBoxPageFrom.TabIndex = 5;
            textBoxPageFrom.Text = "1";
            textBoxPageFrom.KeyDown += textBoxOnlyNum_KeyDown;
            textBoxPageFrom.KeyPress += textBoxOnlyNum_KeyPress;
            // 
            // textBoxPageTo
            // 
            textBoxPageTo.Location = new Point(229, 45);
            textBoxPageTo.Name = "textBoxPageTo";
            textBoxPageTo.Size = new Size(125, 27);
            textBoxPageTo.TabIndex = 1;
            textBoxPageTo.KeyDown += textBoxOnlyNum_KeyDown;
            textBoxPageTo.KeyPress += textBoxOnlyNum_KeyPress;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(360, 45);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(111, 29);
            buttonDownload.TabIndex = 4;
            buttonDownload.Text = "Start";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // textBoxSuccessURLs
            // 
            textBoxSuccessURLs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxSuccessURLs.Location = new Point(69, 3);
            textBoxSuccessURLs.Multiline = true;
            textBoxSuccessURLs.Name = "textBoxSuccessURLs";
            textBoxSuccessURLs.ReadOnly = true;
            textBoxSuccessURLs.ScrollBars = ScrollBars.Both;
            textBoxSuccessURLs.Size = new Size(399, 193);
            textBoxSuccessURLs.TabIndex = 8;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(11, 209);
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
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Dock = DockStyle.Bottom;
            labelResult.Location = new Point(0, 644);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(131, 20);
            labelResult.TabIndex = 6;
            labelResult.Text = "Success: 0 / Fail: 0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 103);
            label4.Name = "label4";
            label4.Size = new Size(103, 20);
            label4.TabIndex = 7;
            label4.Text = "Page Counter";
            // 
            // textBoxCounterCount
            // 
            textBoxCounterCount.Location = new Point(14, 126);
            textBoxCounterCount.Name = "textBoxCounterCount";
            textBoxCounterCount.Size = new Size(125, 27);
            textBoxCounterCount.TabIndex = 2;
            textBoxCounterCount.Text = "2";
            textBoxCounterCount.TextChanged += textBoxCounterCount_TextChanged;
            textBoxCounterCount.KeyDown += textBoxOnlyNum_KeyDown;
            textBoxCounterCount.KeyPress += textBoxOnlyNum_KeyPress;
            // 
            // checkBoxHeadless
            // 
            checkBoxHeadless.AutoSize = true;
            checkBoxHeadless.Checked = true;
            checkBoxHeadless.CheckState = CheckState.Checked;
            checkBoxHeadless.Location = new Point(12, 77);
            checkBoxHeadless.Name = "checkBoxHeadless";
            checkBoxHeadless.Size = new Size(91, 24);
            checkBoxHeadless.TabIndex = 6;
            checkBoxHeadless.Text = "Headless";
            checkBoxHeadless.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 154);
            label5.Name = "label5";
            label5.Size = new Size(91, 20);
            label5.TabIndex = 7;
            label5.Text = "URL Scraper";
            // 
            // textBoxScraperCount
            // 
            textBoxScraperCount.Location = new Point(14, 178);
            textBoxScraperCount.Name = "textBoxScraperCount";
            textBoxScraperCount.Size = new Size(125, 27);
            textBoxScraperCount.TabIndex = 3;
            textBoxScraperCount.Text = "24";
            textBoxScraperCount.KeyDown += textBoxOnlyNum_KeyDown;
            textBoxScraperCount.KeyPress += textBoxOnlyNum_KeyPress;
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
            tableLayoutPanel1.Location = new Point(12, 244);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(471, 398);
            tableLayoutPanel1.TabIndex = 10;
            // 
            // textBoxFailURLs
            // 
            textBoxFailURLs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFailURLs.Location = new Point(69, 202);
            textBoxFailURLs.Multiline = true;
            textBoxFailURLs.Name = "textBoxFailURLs";
            textBoxFailURLs.ReadOnly = true;
            textBoxFailURLs.ScrollBars = ScrollBars.Both;
            textBoxFailURLs.Size = new Size(399, 193);
            textBoxFailURLs.TabIndex = 9;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 199);
            label7.Name = "label7";
            label7.Size = new Size(32, 20);
            label7.TabIndex = 5;
            label7.Text = "Fail";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 0);
            label6.Name = "label6";
            label6.Size = new Size(60, 20);
            label6.TabIndex = 4;
            label6.Text = "Success";
            // 
            // checkBoxOnlyPageURL
            // 
            checkBoxOnlyPageURL.AutoSize = true;
            checkBoxOnlyPageURL.Location = new Point(117, 78);
            checkBoxOnlyPageURL.Margin = new Padding(3, 2, 3, 2);
            checkBoxOnlyPageURL.Name = "checkBoxOnlyPageURL";
            checkBoxOnlyPageURL.Size = new Size(131, 24);
            checkBoxOnlyPageURL.TabIndex = 7;
            checkBoxOnlyPageURL.Text = "Page URL only";
            checkBoxOnlyPageURL.UseVisualStyleBackColor = true;
            checkBoxOnlyPageURL.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(495, 664);
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
