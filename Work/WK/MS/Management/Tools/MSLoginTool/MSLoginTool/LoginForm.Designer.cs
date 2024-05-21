using System.Windows.Forms;

namespace MSLoginTool
{
    partial class LoginForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.txt_UserPWD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Code = new System.Windows.Forms.TextBox();
            this.btn_Enter = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtAuthenticatorCode = new System.Windows.Forms.TextBox();
            this.lblAuthenticatorCode = new System.Windows.Forms.Label();
            this.chkCreateAndCopy = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "请输入用户名：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(31, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "请输入密码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(114, 17);
            this.txt_UserName.MaxLength = 100;
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(151, 22);
            this.txt_UserName.TabIndex = 0;
            // 
            // txt_UserPWD
            // 
            this.txt_UserPWD.Location = new System.Drawing.Point(114, 51);
            this.txt_UserPWD.MaxLength = 100;
            this.txt_UserPWD.Name = "txt_UserPWD";
            this.txt_UserPWD.PasswordChar = '*';
            this.txt_UserPWD.Size = new System.Drawing.Size(151, 22);
            this.txt_UserPWD.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(55, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "验证码：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_Code
            // 
            this.txt_Code.Location = new System.Drawing.Point(114, 115);
            this.txt_Code.Name = "txt_Code";
            this.txt_Code.ReadOnly = true;
            this.txt_Code.Size = new System.Drawing.Size(232, 22);
            this.txt_Code.TabIndex = 5;
            // 
            // btn_Enter
            // 
            this.btn_Enter.Location = new System.Drawing.Point(271, 51);
            this.btn_Enter.Name = "btn_Enter";
            this.btn_Enter.Size = new System.Drawing.Size(75, 23);
            this.btn_Enter.TabIndex = 3;
            this.btn_Enter.Text = "产生验证码";
            this.btn_Enter.UseVisualStyleBackColor = true;
            this.btn_Enter.Click += new System.EventHandler(this.btn_Enter_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(271, 85);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "拷贝验证码";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtAuthenticatorCode
            // 
            this.txtAuthenticatorCode.Location = new System.Drawing.Point(114, 82);
            this.txtAuthenticatorCode.MaxLength = 6;
            this.txtAuthenticatorCode.Name = "txtAuthenticatorCode";
            this.txtAuthenticatorCode.Size = new System.Drawing.Size(151, 22);
            this.txtAuthenticatorCode.TabIndex = 2;
            // 
            // lblAuthenticatorCode
            // 
            this.lblAuthenticatorCode.Location = new System.Drawing.Point(20, 85);
            this.lblAuthenticatorCode.Name = "lblAuthenticatorCode";
            this.lblAuthenticatorCode.Size = new System.Drawing.Size(88, 12);
            this.lblAuthenticatorCode.TabIndex = 9;
            this.lblAuthenticatorCode.Text = "Google验证码：";
            this.lblAuthenticatorCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCreateAndCopy
            // 
            this.chkCreateAndCopy.AutoSize = true;
            this.chkCreateAndCopy.Checked = true;
            this.chkCreateAndCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateAndCopy.Location = new System.Drawing.Point(271, 20);
            this.chkCreateAndCopy.Name = "chkCreateAndCopy";
            this.chkCreateAndCopy.Size = new System.Drawing.Size(84, 16);
            this.chkCreateAndCopy.TabIndex = 10;
            this.chkCreateAndCopy.Text = "产生后拷贝";
            this.chkCreateAndCopy.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 157);
            this.Controls.Add(this.chkCreateAndCopy);
            this.Controls.Add(this.txtAuthenticatorCode);
            this.Controls.Add(this.lblAuthenticatorCode);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btn_Enter);
            this.Controls.Add(this.txt_Code);
            this.Controls.Add(this.txt_UserPWD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_UserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.Text = "MS后台管理系统验登入工具";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.TextBox txt_UserPWD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Code;
        private System.Windows.Forms.Button btn_Enter;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtAuthenticatorCode;
        private System.Windows.Forms.Label lblAuthenticatorCode;
        private System.Windows.Forms.CheckBox chkCreateAndCopy;        
    }
}

