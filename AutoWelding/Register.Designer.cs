namespace AutoWelding
{
    partial class FormRegister
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelRegInfo = new System.Windows.Forms.Label();
            this.textBoxRegInfo = new System.Windows.Forms.TextBox();
            this.labelRegCode = new System.Windows.Forms.Label();
            this.textBoxRegCode = new System.Windows.Forms.TextBox();
            this.buttonRegistry = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelTip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelRegInfo
            // 
            this.labelRegInfo.AutoSize = true;
            this.labelRegInfo.Location = new System.Drawing.Point(14, 66);
            this.labelRegInfo.Name = "labelRegInfo";
            this.labelRegInfo.Size = new System.Drawing.Size(53, 12);
            this.labelRegInfo.TabIndex = 0;
            this.labelRegInfo.Text = "注册信息";
            // 
            // textBoxRegInfo
            // 
            this.textBoxRegInfo.Location = new System.Drawing.Point(73, 46);
            this.textBoxRegInfo.Multiline = true;
            this.textBoxRegInfo.Name = "textBoxRegInfo";
            this.textBoxRegInfo.ReadOnly = true;
            this.textBoxRegInfo.Size = new System.Drawing.Size(250, 50);
            this.textBoxRegInfo.TabIndex = 1;
            // 
            // labelRegCode
            // 
            this.labelRegCode.AutoSize = true;
            this.labelRegCode.Location = new System.Drawing.Point(12, 135);
            this.labelRegCode.Name = "labelRegCode";
            this.labelRegCode.Size = new System.Drawing.Size(41, 12);
            this.labelRegCode.TabIndex = 2;
            this.labelRegCode.Text = "注册码";
            // 
            // textBoxRegCode
            // 
            this.textBoxRegCode.Location = new System.Drawing.Point(73, 116);
            this.textBoxRegCode.Multiline = true;
            this.textBoxRegCode.Name = "textBoxRegCode";
            this.textBoxRegCode.Size = new System.Drawing.Size(250, 50);
            this.textBoxRegCode.TabIndex = 3;
            // 
            // buttonRegistry
            // 
            this.buttonRegistry.Location = new System.Drawing.Point(128, 177);
            this.buttonRegistry.Name = "buttonRegistry";
            this.buttonRegistry.Size = new System.Drawing.Size(75, 23);
            this.buttonRegistry.TabIndex = 4;
            this.buttonRegistry.Text = "注册";
            this.buttonRegistry.UseVisualStyleBackColor = true;
            this.buttonRegistry.Click += new System.EventHandler(this.buttonRegistry_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(248, 177);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 5;
            this.buttonExit.Text = "退出";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelTip
            // 
            this.labelTip.AutoSize = true;
            this.labelTip.Location = new System.Drawing.Point(14, 17);
            this.labelTip.Name = "labelTip";
            this.labelTip.Size = new System.Drawing.Size(41, 12);
            this.labelTip.TabIndex = 6;
            this.labelTip.Text = "label1";
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 207);
            this.Controls.Add(this.labelTip);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonRegistry);
            this.Controls.Add(this.textBoxRegCode);
            this.Controls.Add(this.labelRegCode);
            this.Controls.Add(this.textBoxRegInfo);
            this.Controls.Add(this.labelRegInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormRegister";
            this.Text = "注册";
            this.Load += new System.EventHandler(this.FormRegister_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRegInfo;
        private System.Windows.Forms.TextBox textBoxRegInfo;
        private System.Windows.Forms.Label labelRegCode;
        private System.Windows.Forms.TextBox textBoxRegCode;
        private System.Windows.Forms.Button buttonRegistry;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label labelTip;
    }
}