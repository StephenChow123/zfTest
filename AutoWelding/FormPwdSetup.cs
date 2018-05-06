using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AutoWelding.uicontrol;
using AutoWelding.system;
using AutoWelding.engine;
using AutoWelding.awdatabase;

namespace AutoWelding
{
    public partial class FormPwdSetup : Form
    {
        private AwPanel loginPanel;
        private SystemParam sysParam;
        string keyStr = "&+3k.eKs";
        string adminDefaultPwd = "autoWelding20120625";
        Crypt crypt;
        AwDbInterface xmlDb;
        public FormPwdSetup(ref SystemParam systemParam)
        {
            sysParam = systemParam;
            loginPanel = new AwPanel();
            crypt = new Crypt();
            xmlDb = AwDbFactory.CreateDBEngine();
            InitializeComponent();
        }

        private void FormPwdSetup_Load(object sender, EventArgs e)
        {
            InitControls();
            InitPermision();
        }

        private void InitControls()
        {
            LayoutInit();
            groupBoxAccount.Enabled = false;
        }

        private void LayoutInit()
        {
            Point point = new Point(20, 20);
            loginPanel.BorderColor = sysParam.ControlFrameColor;
            loginPanel.BorderWidth = 3;
            loginPanel.TitleBackground.Height = 30;
            loginPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            loginPanel.Title.Text = "密码重置";

            loginPanel.Parent = this;
            loginPanel.Width = this.Width - point.X * 2;
            loginPanel.Height = this.Height * 4 / 5;
            loginPanel.Location = point;

            point.X = 25;
            point.Y = 25;
            panel1.Location = point;
            panel1.Parent = loginPanel;

            point = loginPanel.Location;
            point.X += loginPanel.Width - buttonOK.Width;
            point.Y += loginPanel.Height + 10;
            buttonOK.Location = point;
        }

        private void InitPermision()
        { 
            // Admin            
            AccountPermision ap = sysParam.GetAccountPermission(AccountType.Admin);
            checkBoxAdminParamSetting.Checked = (ap.permition & PermitionItems.paramSetting) != 0;
            checkBoxAdminPressKey.Checked = (ap.permition & PermitionItems.pressButton) != 0;
            checkBoxAdminAdjustColloid.Checked = (ap.permition & PermitionItems.pressButton) != 0;
            checkBoxAdminDebug.Checked = (ap.permition & PermitionItems.debug) != 0;
            checkBoxAdminDebugInfo.Checked = (ap.permition & PermitionItems.debugInfo) != 0;
            checkBoxAdminFileManager.Checked = (ap.permition & PermitionItems.fileManager) != 0;
            checkBoxAdminAccountManager.Checked = (ap.permition & PermitionItems.accountManager) != 0;
            checkBoxAdminNewPrdBat.Checked = (ap.permition & PermitionItems.newProductBat) != 0;
            checkBoxAdminSystem.Checked = (ap.permition & PermitionItems.systemInfo) != 0;
            checkBoxAdminRegister.Checked = (ap.permition & PermitionItems.register) != 0;
            panelAdminPermision.Enabled = false;

            // Developer
            ap = sysParam.GetAccountPermission(AccountType.Developer);
            checkBoxDevParamSetting.Checked = (ap.permition & PermitionItems.paramSetting) != 0;
            checkBoxDevPressKey.Checked = (ap.permition & PermitionItems.pressButton) != 0;
            checkBoxDevAdjustColloid.Checked = (ap.permition & PermitionItems.adjustColloid) != 0;
            checkBoxDevDebug.Checked = (ap.permition & PermitionItems.debug) != 0;
            checkBoxDevDebugInfo.Checked = (ap.permition & PermitionItems.debugInfo) != 0;
            checkBoxDevFileManager.Checked = (ap.permition & PermitionItems.fileManager) != 0;
            checkBoxDevAccountManager.Checked = (ap.permition & PermitionItems.accountManager) != 0;
            checkBoxDevNewPrdBat.Checked = (ap.permition & PermitionItems.newProductBat) != 0;
            checkBoxDevSystem.Checked = (ap.permition & PermitionItems.systemInfo) != 0;
            checkBoxDevRegister.Checked = (ap.permition & PermitionItems.register) != 0;

            // Operator
            ap = sysParam.GetAccountPermission(AccountType.Operator);
            checkBoxOpParamSetting.Checked = (ap.permition & PermitionItems.paramSetting) != 0;
            checkBoxOpPressKey.Checked = (ap.permition & PermitionItems.pressButton) != 0;
            checkBoxOpAdjustColloid.Checked = (ap.permition & PermitionItems.adjustColloid) != 0;
            checkBoxOpDebug.Checked = (ap.permition & PermitionItems.debug) != 0;
            checkBoxOpDebugInfo.Checked = (ap.permition & PermitionItems.debugInfo) != 0;
            checkBoxOpFileManager.Checked = (ap.permition & PermitionItems.fileManager) != 0;
            checkBoxOpAccountManager.Checked = (ap.permition & PermitionItems.accountManager) != 0;
            checkBoxOpNewPrdBat.Checked = (ap.permition & PermitionItems.newProductBat) != 0;
            checkBoxOpSystem.Checked = (ap.permition & PermitionItems.systemInfo) != 0;
            checkBoxOpRegister.Checked = (ap.permition & PermitionItems.register) != 0;
            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string pwdString = "";
            // Check original admin pwd
            if (checkBoxModifyPwd.Checked)
            {
                if (textBoxOriginal.Text == "" && sysParam.PwdList.Count != 0)
                {
                    MessageBox.Show("请输入原管理员密码!", "提示");
                    return;
                }

                // Check new admin pwd
                if (textBoxAdmin.Text == "")
                {
                    MessageBox.Show("请输入管理员密码!", "提示");
                    return;
                }

                if (textBoxAdmin.Text != textBoxAdminConfirm.Text)
                {
                    MessageBox.Show("管理员密码错误,请重输!", "提示");
                    textBoxAdminConfirm.Text = "";
                    textBoxAdmin.Text = "";
                    return;
                }

                // check developer pwd

                if (textBoxDeveloper.Text == "")
                {
                    MessageBox.Show("请输入工程师密码!", "提示");
                    return;
                }

                if (textBoxDeveloper.Text != textBoxDeveloperConfirm.Text)
                {
                    MessageBox.Show("工程师密码错误,请重输!", "提示");
                    textBoxDeveloper.Text = "";
                    textBoxDeveloperConfirm.Text = "";
                    return;
                }

                // check operator pwd

                if (textBoxOperator.Text == "")
                {
                    MessageBox.Show("请输入操作员密码!", "提示");
                    return;
                }

                if (textBoxOperator.Text != textBoxOperatorConfirm.Text)
                {
                    MessageBox.Show("操作员密码错误,请重输!", "提示");
                    textBoxOperator.Text = "";
                    textBoxOperatorConfirm.Text = "";
                    return;
                }

                // check if the original pwd is valid
                if (sysParam.PwdList.Count > 0 && textBoxOriginal.Text != sysParam.PwdList[0])
                {
                    MessageBox.Show("原管理员密码错误!", "错误");
                    textBoxOriginal.Text = "";
                    return;
                }

                pwdString = textBoxAdmin.Text + "\n" + textBoxDeveloper.Text + "\n" + textBoxOperator.Text;
            }

            // Dev Permision
            UInt32 permission = PermitionItems.CheckingCode;

            permission |= checkBoxDevParamSetting.Checked ? PermitionItems.paramSetting : 0x0;
            permission |= checkBoxDevPressKey.Checked ? PermitionItems.pressButton : 0x0;
            permission |= checkBoxDevAdjustColloid.Checked ? PermitionItems.adjustColloid : 0x0;
            permission |= checkBoxDevDebug.Checked ? PermitionItems.debug : 0x0;
            permission |= checkBoxDevDebugInfo.Checked ? PermitionItems.debugInfo : 0x0;
            permission |= checkBoxDevFileManager.Checked ? PermitionItems.fileManager : 0x0;
            permission |= checkBoxDevAccountManager.Checked ? PermitionItems.accountManager : 0x0;
            permission |= checkBoxDevNewPrdBat.Checked ? PermitionItems.newProductBat : 0x0;
            permission |= checkBoxDevSystem.Checked ? PermitionItems.systemInfo : 0x0;
            permission |= checkBoxDevRegister.Checked ? PermitionItems.register : 0x0;

            string PME = permission.ToString();
            AccountPermision ap = new AccountPermision();
            ap.accountType = AccountType.Developer;
            ap.permition = permission;
            sysParam.UpdateAccountPermission(ap);

            permission = PermitionItems.CheckingCode;

            permission |= checkBoxOpParamSetting.Checked ? PermitionItems.paramSetting : 0x0;
            permission |= checkBoxOpPressKey.Checked ? PermitionItems.pressButton : 0x0;
            permission |= checkBoxOpAdjustColloid.Checked ? PermitionItems.adjustColloid : 0x0;
            permission |= checkBoxOpDebug.Checked ? PermitionItems.debug : 0x0;
            permission |= checkBoxOpDebugInfo.Checked ? PermitionItems.debugInfo : 0x0;
            permission |= checkBoxOpFileManager.Checked ? PermitionItems.fileManager : 0x0;
            permission |= checkBoxOpAccountManager.Checked ? PermitionItems.accountManager : 0x0;
            permission |= checkBoxOpNewPrdBat.Checked ? PermitionItems.newProductBat : 0x0;
            permission |= checkBoxOpSystem.Checked ? PermitionItems.systemInfo : 0x0;
            permission |= checkBoxOpRegister.Checked ? PermitionItems.register : 0x0;

            string PMO = permission.ToString();
            ap.accountType = AccountType.Operator;
            ap.permition = permission;
            sysParam.UpdateAccountPermission(ap);

            //UpdatePermission
            if (checkBoxModifyPwd.Checked)
                xmlDb.UpdatePwd(crypt.EncryptDES(pwdString, keyStr), crypt.EncryptDES(PME, keyStr), crypt.EncryptDES(PMO, keyStr));
            else
                xmlDb.UpdatePermission(crypt.EncryptDES(PME, keyStr), crypt.EncryptDES(PMO, keyStr));

            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkBoxModifyPwd_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAccount.Enabled = checkBoxModifyPwd.Checked;
        }
        
    }
}