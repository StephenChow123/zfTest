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
            labelTip.Text = "�뽫ע����Ϣ���͵����������,��ȡע����.";

            textBoxRegInfo.Text = awRegistry.CreateRegistryInfo();
        }

        private void buttonRegistry_Click(object sender, EventArgs e)
        {
            int ret = 0;

            if (textBoxRegCode.Text.Length == 0)
            {
                MessageBox.Show("ע����Ϊ��!", "��ʾ");
                return;
            }

            if ((ret = awRegistry.Register(textBoxRegCode.Text)) < 0)
            {
                MessageBox.Show("ע��ʧ�� " + ret.ToString() + " !", "��ʾ");
                return;
            }

            MessageBox.Show("�ɹ�ע��", "��ʾ");

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