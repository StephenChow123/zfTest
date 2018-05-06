using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AutoWelding.engine;

namespace AutoWelding
{
    public partial class FormRegister : Form
    {
        AwRegistry awRegistry;
        public FormRegister(ref AwRegistry reg)
        {
            awRegistry = reg;
            InitializeComponent();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            labelTip.Text = "请将注册信息发送到软件发行商,获取注册码.";

            textBoxRegInfo.Text = awRegistry.CreateRegistryInfo();
        }

        private void buttonRegistry_Click(object sender, EventArgs e)
        {
            int ret = 0;

            if (textBoxRegCode.Text.Length == 0)
            {
                MessageBox.Show("注册码为空!", "提示");
                return;
            }

            if ((ret = awRegistry.Register(textBoxRegCode.Text)) < 0)
            {
                MessageBox.Show("注册失败 " + ret.ToString() + " !", "提示");
                return;
            }

            MessageBox.Show("成功注册", "提示");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}