namespace WebConfigConverter
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.txtParseResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(28, 21);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(156, 23);
            this.btnLoadConfig.TabIndex = 0;
            this.btnLoadConfig.Text = "convert config appsetting";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // txtParseResult
            // 
            this.txtParseResult.Location = new System.Drawing.Point(28, 61);
            this.txtParseResult.Multiline = true;
            this.txtParseResult.Name = "txtParseResult";
            this.txtParseResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtParseResult.Size = new System.Drawing.Size(1150, 450);
            this.txtParseResult.TabIndex = 1;
            this.txtParseResult.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 559);
            this.Controls.Add(this.txtParseResult);
            this.Controls.Add(this.btnLoadConfig);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.TextBox txtParseResult;
    }
}

