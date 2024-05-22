namespace MobileApiCreateSign
{
    partial class Generate
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.lblApiTitle = new System.Windows.Forms.Label();
            this.lblResultMessage = new System.Windows.Forms.Label();
            this.lblDepositUrl = new System.Windows.Forms.Label();
            this.BtnExample = new System.Windows.Forms.Button();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lblApiName = new System.Windows.Forms.Label();
            this.lblEXProductName = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtDepositUrl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnGenerate.Location = new System.Drawing.Point(367, 91);
            this.BtnGenerate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(96, 71);
            this.BtnGenerate.TabIndex = 8;
            this.BtnGenerate.Text = "產生並複製";
            this.BtnGenerate.UseVisualStyleBackColor = true;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // lblApiTitle
            // 
            this.lblApiTitle.AutoSize = true;
            this.lblApiTitle.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblApiTitle.Location = new System.Drawing.Point(95, 30);
            this.lblApiTitle.Name = "lblApiTitle";
            this.lblApiTitle.Size = new System.Drawing.Size(31, 16);
            this.lblApiTitle.TabIndex = 1;
            this.lblApiTitle.Text = "API :";
            // 
            // lblResultMessage
            // 
            this.lblResultMessage.AutoSize = true;
            this.lblResultMessage.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblResultMessage.Location = new System.Drawing.Point(79, 180);
            this.lblResultMessage.Name = "lblResultMessage";
            this.lblResultMessage.Size = new System.Drawing.Size(47, 16);
            this.lblResultMessage.TabIndex = 3;
            this.lblResultMessage.Text = "Result :";
            // 
            // lblDepositUrl
            // 
            this.lblDepositUrl.AutoSize = true;
            this.lblDepositUrl.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDepositUrl.Location = new System.Drawing.Point(12, 142);
            this.lblDepositUrl.Name = "lblDepositUrl";
            this.lblDepositUrl.Size = new System.Drawing.Size(114, 16);
            this.lblDepositUrl.TabIndex = 7;
            this.lblDepositUrl.Text = "depositUrl (string) :";
            // 
            // BtnExample
            // 
            this.BtnExample.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnExample.Location = new System.Drawing.Point(367, 59);
            this.BtnExample.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExample.Name = "BtnExample";
            this.BtnExample.Size = new System.Drawing.Size(96, 23);
            this.BtnExample.TabIndex = 7;
            this.BtnExample.Text = "範例";
            this.BtnExample.UseVisualStyleBackColor = true;
            this.BtnExample.Click += new System.EventHandler(this.BtnExample_Click);
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserID.Location = new System.Drawing.Point(53, 66);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(73, 16);
            this.lblUserID.TabIndex = 10;
            this.lblUserID.Text = "userID (int) :";
            // 
            // lblApiName
            // 
            this.lblApiName.AutoSize = true;
            this.lblApiName.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblApiName.ForeColor = System.Drawing.Color.Blue;
            this.lblApiName.Location = new System.Drawing.Point(132, 30);
            this.lblApiName.Name = "lblApiName";
            this.lblApiName.Size = new System.Drawing.Size(97, 16);
            this.lblApiName.TabIndex = 12;
            this.lblApiName.Text = "Account/LogOn";
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtMessage.ForeColor = System.Drawing.Color.Red;
            this.txtMessage.Location = new System.Drawing.Point(135, 180);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(328, 206);
            this.txtMessage.TabIndex = 17;
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUserID.Location = new System.Drawing.Point(135, 63);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(212, 23);
            this.txtUserID.TabIndex = 5;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserName.Location = new System.Drawing.Point(13, 103);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(113, 16);
            this.lblUserName.TabIndex = 26;
            this.lblUserName.Text = "userName (string) :";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtUserName.Location = new System.Drawing.Point(135, 103);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(212, 23);
            this.txtUserName.TabIndex = 27;
            // 
            // txtDepositUrl
            // 
            this.txtDepositUrl.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtDepositUrl.Location = new System.Drawing.Point(135, 139);
            this.txtDepositUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDepositUrl.Name = "txtDepositUrl";
            this.txtDepositUrl.Size = new System.Drawing.Size(212, 23);
            this.txtDepositUrl.TabIndex = 28;
            // 
            // Generate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 416);
            this.Controls.Add(this.txtDepositUrl);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblEXProductName);
            this.Controls.Add(this.lblApiName);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.BtnExample);
            this.Controls.Add(this.lblDepositUrl);
            this.Controls.Add(this.lblResultMessage);
            this.Controls.Add(this.lblApiTitle);
            this.Controls.Add(this.BtnGenerate);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Generate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MS-M-產生API參數";
            this.Load += new System.EventHandler(this.GenerateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.Label lblApiTitle;
        private System.Windows.Forms.Label lblResultMessage;
        private System.Windows.Forms.Label lblDepositUrl;
        private System.Windows.Forms.Button BtnExample;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Label lblApiName;
        private System.Windows.Forms.Label lblEXProductName;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtDepositUrl;
    }
}