namespace AutoWelding
{
    partial class FormIoControl
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
            this.comboBoxCard = new System.Windows.Forms.ComboBox();
            this.labelCard = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxCard
            // 
            this.comboBoxCard.FormattingEnabled = true;
            this.comboBoxCard.Location = new System.Drawing.Point(126, 12);
            this.comboBoxCard.Name = "comboBoxCard";
            this.comboBoxCard.Size = new System.Drawing.Size(121, 20);
            this.comboBoxCard.TabIndex = 0;
            // 
            // labelCard
            // 
            this.labelCard.AutoSize = true;
            this.labelCard.Location = new System.Drawing.Point(52, 15);
            this.labelCard.Name = "labelCard";
            this.labelCard.Size = new System.Drawing.Size(59, 12);
            this.labelCard.TabIndex = 1;
            this.labelCard.Text = "IO 卡选择";
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(383, 314);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "退出";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FormIoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 394);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.labelCard);
            this.Controls.Add(this.comboBoxCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormIoControl";
            this.Text = "IO 监制";
            this.Load += new System.EventHandler(this.FormIoControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCard;
        private System.Windows.Forms.Label labelCard;
        private System.Windows.Forms.Button buttonExit;

    }
}