using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AutoWelding.uicontrol;
using  AutoWelding.system;
using AutoWelding.control;

namespace AutoWelding
{
    public partial class FormNewProductBat : Form
    {

        private SystemParam sysParam;
        ProductBatInfo prdBatInfo;

        public ProductBatInfo PrdBatInfo
        {
            get { return prdBatInfo; }
        }

        public FormNewProductBat(ref SystemParam param, ProductBatInfo batInfo)
        {
            sysParam = param;
            prdBatInfo = new ProductBatInfo();
            prdBatInfo = batInfo;
            InitializeComponent();
        }

        private void FormNewProductBat_Load(object sender, EventArgs e)
        {
            InitUiParamPanel();
            ShowBatInfo();
        }

        private void ShowBatInfo()
        {
            txPartNum.Text = prdBatInfo.PartNumber;
            txWafelLot.Text = prdBatInfo.WaferLot;
            txAssLot.Text = prdBatInfo.AssemblyLot;
            txPackage.Text = prdBatInfo.Package;
            txDie.Text = prdBatInfo.Dietype;
            txSampleSize.Text = prdBatInfo.SampleSize.ToString();
        }

        void InitUiParamPanel()
        {
            //Point point = new Point(20, 20);

            //point.Y = 40;
            //point.X = paramPanel.Location.X + paramPanel.Width - buttonCancel.Width;
            //point.Y = paramPanel.Location.Y + paramPanel.Height + 15;
            //buttonCancel.Location = point;
            //point.X -= buttonOK.Width + 10;
            //buttonOK.Location = point;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (txPartNum.Text.Length == 0)
            {
                MessageBox.Show("Pls input PartNum infor!", "提示");
                return;
            }
            if (txWafelLot.Text.Length == 0)
            {
                MessageBox.Show("Pls input WafeLot infor!", "提示");
                return;
            }
            if (txAssLot.Text.Length == 0)
            {
                MessageBox.Show("Pls input Assembly Lot infor!", "提示");
                return;
            }
            if (txPackage.Text.Length == 0)
            {
                MessageBox.Show("Pls input Package infor!", "提示");
                return;
            }
            if (txDie.Text.Length == 0)
            {
                MessageBox.Show("Pls input Die type infor!", "提示");
                return;
            }
            try
            {
               int _smSize= Convert.ToInt32(txSampleSize.Text);
                if(_smSize<=0)
                {
                    MessageBox.Show("Pls input Sample Size infor!", "错误");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Pls input Sample Size infor!", "错误");
                return;
            }
            prdBatInfo.PartNumber = txPartNum.Text;
            prdBatInfo.WaferLot = txWafelLot.Text;
            prdBatInfo.AssemblyLot = txAssLot.Text;
            prdBatInfo.Package = txPackage.Text;
            prdBatInfo.Dietype = txDie.Text;
            prdBatInfo.SampleSize = Convert.ToInt32(txSampleSize.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}