namespace EncryptImagesTool
{
    partial class EncryptImagesTool
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
            txtSelectPath = new TextBox();
            btnExecute = new Button();
            btnSelectPath = new Button();
            progress = new ProgressBar();
            lblResult = new Label();
            lblSResult = new Label();
            lblSTotalCount = new Label();
            lblSProcessedCount = new Label();
            lblTotalCount = new Label();
            lblProcessedCount = new Label();
            rdoPickFile = new RadioButton();
            rdoPickDirectory = new RadioButton();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            rdoEncrypt = new RadioButton();
            rdoDescrypt = new RadioButton();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // txtSelectPath
            // 
            txtSelectPath.Location = new Point(20, 122);
            txtSelectPath.Name = "txtSelectPath";
            txtSelectPath.Size = new Size(293, 23);
            txtSelectPath.TabIndex = 1;
            // 
            // btnExecute
            // 
            btnExecute.Location = new Point(195, 76);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(118, 33);
            btnExecute.TabIndex = 2;
            btnExecute.Text = "執行";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += BtnExecute_Click;
            // 
            // btnSelectPath
            // 
            btnSelectPath.Location = new Point(195, 24);
            btnSelectPath.Name = "btnSelectPath";
            btnSelectPath.Size = new Size(118, 34);
            btnSelectPath.TabIndex = 3;
            btnSelectPath.Text = "選取圖片位置";
            btnSelectPath.UseVisualStyleBackColor = true;
            btnSelectPath.Click += BtnSelectPath_Click;
            // 
            // progress
            // 
            progress.Dock = DockStyle.Bottom;
            progress.Location = new Point(0, 332);
            progress.Name = "progress";
            progress.Size = new Size(339, 23);
            progress.TabIndex = 6;
            // 
            // lblResult
            // 
            lblResult.AutoEllipsis = true;
            lblResult.Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblResult.ForeColor = Color.Red;
            lblResult.Location = new Point(20, 211);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(307, 106);
            lblResult.TabIndex = 7;
            lblResult.Text = "-";
            // 
            // lblSResult
            // 
            lblSResult.AutoSize = true;
            lblSResult.Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblSResult.ForeColor = Color.Red;
            lblSResult.Location = new Point(20, 187);
            lblSResult.Name = "lblSResult";
            lblSResult.Size = new Size(105, 24);
            lblSResult.TabIndex = 8;
            lblSResult.Text = "執行結果：";
            // 
            // lblSTotalCount
            // 
            lblSTotalCount.AutoSize = true;
            lblSTotalCount.Location = new Point(20, 159);
            lblSTotalCount.Name = "lblSTotalCount";
            lblSTotalCount.Size = new Size(55, 15);
            lblSTotalCount.TabIndex = 9;
            lblSTotalCount.Text = "總筆數：";
            // 
            // lblSProcessedCount
            // 
            lblSProcessedCount.AutoSize = true;
            lblSProcessedCount.Location = new Point(148, 159);
            lblSProcessedCount.Name = "lblSProcessedCount";
            lblSProcessedCount.Size = new Size(79, 15);
            lblSProcessedCount.TabIndex = 10;
            lblSProcessedCount.Text = "已處理筆數：";
            // 
            // lblTotalCount
            // 
            lblTotalCount.AutoSize = true;
            lblTotalCount.Location = new Point(77, 159);
            lblTotalCount.Name = "lblTotalCount";
            lblTotalCount.Size = new Size(14, 15);
            lblTotalCount.TabIndex = 11;
            lblTotalCount.Text = "0";
            // 
            // lblProcessedCount
            // 
            lblProcessedCount.AutoSize = true;
            lblProcessedCount.Location = new Point(229, 159);
            lblProcessedCount.Name = "lblProcessedCount";
            lblProcessedCount.Size = new Size(14, 15);
            lblProcessedCount.TabIndex = 12;
            lblProcessedCount.Text = "0";
            // 
            // rdoPickFile
            // 
            rdoPickFile.AutoSize = true;
            rdoPickFile.Location = new Point(79, 19);
            rdoPickFile.Name = "rdoPickFile";
            rdoPickFile.Size = new Size(49, 19);
            rdoPickFile.TabIndex = 13;
            rdoPickFile.Text = "檔案";
            rdoPickFile.UseVisualStyleBackColor = true;
            // 
            // rdoPickDirectory
            // 
            rdoPickDirectory.AutoSize = true;
            rdoPickDirectory.Checked = true;
            rdoPickDirectory.Location = new Point(12, 19);
            rdoPickDirectory.Name = "rdoPickDirectory";
            rdoPickDirectory.Size = new Size(61, 19);
            rdoPickDirectory.TabIndex = 14;
            rdoPickDirectory.TabStop = true;
            rdoPickDirectory.Text = "資料夾";
            rdoPickDirectory.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rdoPickFile);
            groupBox1.Controls.Add(rdoPickDirectory);
            groupBox1.Location = new Point(15, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(162, 45);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rdoEncrypt);
            groupBox2.Controls.Add(rdoDescrypt);
            groupBox2.Location = new Point(15, 64);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(162, 45);
            groupBox2.TabIndex = 18;
            groupBox2.TabStop = false;
            // 
            // rdoEncrypt
            // 
            rdoEncrypt.AutoSize = true;
            rdoEncrypt.Checked = true;
            rdoEncrypt.Location = new Point(12, 20);
            rdoEncrypt.Name = "rdoEncrypt";
            rdoEncrypt.Size = new Size(49, 19);
            rdoEncrypt.TabIndex = 13;
            rdoEncrypt.TabStop = true;
            rdoEncrypt.Text = "加密";
            rdoEncrypt.UseVisualStyleBackColor = true;
            // 
            // rdoDescrypt
            // 
            rdoDescrypt.AutoSize = true;
            rdoDescrypt.Location = new Point(79, 19);
            rdoDescrypt.Name = "rdoDescrypt";
            rdoDescrypt.Size = new Size(49, 19);
            rdoDescrypt.TabIndex = 14;
            rdoDescrypt.Text = "解密";
            rdoDescrypt.UseVisualStyleBackColor = true;
            // 
            // EncryptImagesTool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 355);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblProcessedCount);
            Controls.Add(lblTotalCount);
            Controls.Add(lblSProcessedCount);
            Controls.Add(lblSTotalCount);
            Controls.Add(lblSResult);
            Controls.Add(lblResult);
            Controls.Add(progress);
            Controls.Add(btnSelectPath);
            Controls.Add(btnExecute);
            Controls.Add(txtSelectPath);
            Name = "EncryptImagesTool";
            Text = "加解密圖片工具";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtSelectPath;
        private Button btnExecute;
        private Button btnSelectPath;
        private ProgressBar progress;
        private Label lblResult;
        private Label lblSResult;
        private Label lblSTotalCount;
        private Label lblSProcessedCount;
        private Label lblTotalCount;
        private Label lblProcessedCount;
        private RadioButton rdoPickFile;
        private RadioButton rdoPickDirectory;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private RadioButton rdoEncrypt;
        private RadioButton rdoDescrypt;
    }
}