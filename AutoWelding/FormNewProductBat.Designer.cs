namespace AutoWelding
{
    partial class FormNewProductBat
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txSampleSize = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txDie = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.labelPassed = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txAssLot = new System.Windows.Forms.TextBox();
            this.txPackage = new System.Windows.Forms.TextBox();
            this.txPartNum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txWafelLot = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("宋体", 12F);
            this.buttonOK.Location = new System.Drawing.Point(36, 243);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(113, 51);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("宋体", 12F);
            this.buttonCancel.Location = new System.Drawing.Point(180, 243);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(113, 51);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.44195F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.55805F));
            this.tableLayoutPanel7.Controls.Add(this.txSampleSize, 1, 5);
            this.tableLayoutPanel7.Controls.Add(this.label17, 1, 4);
            this.tableLayoutPanel7.Controls.Add(this.txDie, 0, 5);
            this.tableLayoutPanel7.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.labelPassed, 0, 4);
            this.tableLayoutPanel7.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.txAssLot, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.txPackage, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.txPartNum, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txWafelLot, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.label10, 1, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 6;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(328, 212);
            this.tableLayoutPanel7.TabIndex = 7;
            // 
            // txSampleSize
            // 
            this.txSampleSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txSampleSize.Location = new System.Drawing.Point(155, 178);
            this.txSampleSize.Name = "txSampleSize";
            this.txSampleSize.Size = new System.Drawing.Size(170, 21);
            this.txSampleSize.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("宋体", 12F);
            this.label17.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label17.Location = new System.Drawing.Point(155, 140);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(170, 35);
            this.label17.TabIndex = 0;
            this.label17.Text = "Sample Size";
            // 
            // txDie
            // 
            this.txDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txDie.Location = new System.Drawing.Point(3, 178);
            this.txDie.Name = "txDie";
            this.txDie.Size = new System.Drawing.Size(146, 21);
            this.txDie.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(155, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 35);
            this.label8.TabIndex = 1;
            this.label8.Text = "Wafer Lot#";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPassed
            // 
            this.labelPassed.AutoSize = true;
            this.labelPassed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPassed.Font = new System.Drawing.Font("宋体", 12F);
            this.labelPassed.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelPassed.Location = new System.Drawing.Point(3, 140);
            this.labelPassed.Name = "labelPassed";
            this.labelPassed.Size = new System.Drawing.Size(146, 35);
            this.labelPassed.TabIndex = 0;
            this.labelPassed.Text = "Die type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Location = new System.Drawing.Point(3, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 35);
            this.label9.TabIndex = 2;
            this.label9.Text = "Assembly Lot#";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txAssLot
            // 
            this.txAssLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txAssLot.Location = new System.Drawing.Point(3, 108);
            this.txAssLot.Name = "txAssLot";
            this.txAssLot.Size = new System.Drawing.Size(146, 21);
            this.txAssLot.TabIndex = 5;
            // 
            // txPackage
            // 
            this.txPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txPackage.Location = new System.Drawing.Point(155, 108);
            this.txPackage.Name = "txPackage";
            this.txPackage.Size = new System.Drawing.Size(170, 21);
            this.txPackage.TabIndex = 5;
            // 
            // txPartNum
            // 
            this.txPartNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txPartNum.Location = new System.Drawing.Point(3, 38);
            this.txPartNum.Name = "txPartNum";
            this.txPartNum.Size = new System.Drawing.Size(146, 21);
            this.txPartNum.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 35);
            this.label7.TabIndex = 0;
            this.label7.Text = "Part Number";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txWafelLot
            // 
            this.txWafelLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txWafelLot.Location = new System.Drawing.Point(155, 38);
            this.txWafelLot.Name = "txWafelLot";
            this.txWafelLot.Size = new System.Drawing.Size(170, 21);
            this.txWafelLot.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("宋体", 12F);
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label10.Location = new System.Drawing.Point(155, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 35);
            this.label10.TabIndex = 2;
            this.label10.Text = "Package";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormNewProductBat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(328, 334);
            this.Controls.Add(this.tableLayoutPanel7);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormNewProductBat";
            this.Text = "新生产批次";
            this.Load += new System.EventHandler(this.FormNewProductBat_Load);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TextBox txSampleSize;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txDie;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelPassed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txAssLot;
        private System.Windows.Forms.TextBox txPackage;
        private System.Windows.Forms.TextBox txPartNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txWafelLot;
        private System.Windows.Forms.Label label10;
    }
}