namespace AutoWelding
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelSysTitleBackgroud = new System.Windows.Forms.Panel();
            this.labelTimer = new System.Windows.Forms.Label();
            this.labelAccountInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelSysTitleBackgroud.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelSysTitleBackgroud, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.15009F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.95637F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.89354F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1317, 860);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelSysTitleBackgroud
            // 
            this.panelSysTitleBackgroud.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelSysTitleBackgroud.BackgroundImage")));
            this.panelSysTitleBackgroud.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSysTitleBackgroud.Controls.Add(this.labelTimer);
            this.panelSysTitleBackgroud.Controls.Add(this.labelAccountInfo);
            this.panelSysTitleBackgroud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSysTitleBackgroud.Location = new System.Drawing.Point(4, 4);
            this.panelSysTitleBackgroud.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSysTitleBackgroud.Name = "panelSysTitleBackgroud";
            this.panelSysTitleBackgroud.Size = new System.Drawing.Size(1309, 148);
            this.panelSysTitleBackgroud.TabIndex = 32;
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.BackColor = System.Drawing.Color.Transparent;
            this.labelTimer.Location = new System.Drawing.Point(27, 183);
            this.labelTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(71, 18);
            this.labelTimer.TabIndex = 23;
            this.labelTimer.Text = "label22";
            // 
            // labelAccountInfo
            // 
            this.labelAccountInfo.AutoSize = true;
            this.labelAccountInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelAccountInfo.Location = new System.Drawing.Point(1752, 183);
            this.labelAccountInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAccountInfo.Name = "labelAccountInfo";
            this.labelAccountInfo.Size = new System.Drawing.Size(62, 18);
            this.labelAccountInfo.TabIndex = 22;
            this.labelAccountInfo.Text = "操作员";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 860);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form3";
            this.Text = "Form3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelSysTitleBackgroud.ResumeLayout(false);
            this.panelSysTitleBackgroud.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelSysTitleBackgroud;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.Label labelAccountInfo;
    }
}