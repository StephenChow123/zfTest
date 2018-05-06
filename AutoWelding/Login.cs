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
    public partial class Login : Form
    {
        private AwPanel loginPanel;
        private SystemParam sysParam;
        string keyStr = "&+3k.eKs";
        string adminDefaultPwd = "autoWelding20120625";
        Crypt crypt;
        AwDbInterface xmlDb;
        List<string> pwdList;
        AccountType accountType;

        public AccountType AccountType
        {
            get { return accountType; }
        }

        public Login(ref SystemParam paramSys)
        {
            sysParam = paramSys;
            loginPanel = new AwPanel();
            crypt = new Crypt();
            xmlDb = AwDbFactory.CreateDBEngine();
            pwdList = new List<string>();
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void InitControl()
        {
            LayoutInit();
            InitUserInfo();
        }

        private void LayoutInit()
        {
            Point point = new Point(20, 20);
            loginPanel.BorderColor = sysParam.ControlFrameColor;
            loginPanel.BorderWidth = 3;
            loginPanel.TitleBackground.Height = 30;
            loginPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            loginPanel.Title.Text = "登录信息";

            loginPanel.Parent = this;
            loginPanel.Width = this.Width - point.X * 2;
            loginPanel.Height = this.Height * 3 / 5;
            loginPanel.Location = point;

            //point.X = 20;
            //point.Y += loginPanel.Title.Height + 30;
            //labelUserName.Location = point;
            //labelUserName.Parent = loginPanel;

            //point.X += 100;
            //comboBoxUserName.Location = point;
            //comboBoxUserName.Parent = loginPanel;

            //point = labelUserName.Location;
            //point.Y += labelUserName.Height + 30;
            //labelPwd.Location = point;
            //labelPwd.Parent = loginPanel;

            //point.X += 100;
            //textBoxPwd.Location = point;
            //textBoxPwd.Width = comboBoxUserName.Width;
            //textBoxPwd.Parent = loginPanel;

            //point.X = loginPanel.Location.X + loginPanel.Width - buttonLogin.Width;
            //point.Y = loginPanel.Location.Y + loginPanel.Height + 20;
            //buttonLogin.Location = point;
        }

        public void InitPermision()
        {
            string pwd = "";
            string pme = "";
            string pmo = "";

            xmlDb.GetAccountInfo(ref pwd, ref pme, ref pmo);

            try
            {
                pme = crypt.DecryptDES(pme, keyStr);
                pmo = crypt.DecryptDES(pmo, keyStr);
                UInt32 pmePermission = Convert.ToUInt32(pme);
                UInt32 pmoPermission = Convert.ToUInt32(pmo);

                if ((pmePermission & PermitionItems.CheckingCodeMask) == PermitionItems.CheckingCode)
                {
                    AccountPermision ap = new AccountPermision();
                    ap.accountType = AccountType.Developer;
                    ap.permition = pmePermission;
                    sysParam.UpdateAccountPermission(ap);
                }

                if ((pmoPermission & PermitionItems.CheckingCodeMask) == PermitionItems.CheckingCode)
                {
                    AccountPermision ap = new AccountPermision();
                    ap.accountType = AccountType.Operator;
                    ap.permition = pmoPermission;
                    sysParam.UpdateAccountPermission(ap);
                }
            }
            catch { }
        
        }

        private void InitUserInfo()
        {
            string pwd = "";
            string pme = "";
            string pmo = "";

            xmlDb.GetAccountInfo(ref pwd, ref pme, ref pmo);           

            if (pwd.Length == 0)
            {
                comboBoxUserName.Items.Add("管理员");
                pwdList.Add(adminDefaultPwd);
                return;
            }

            pwd = crypt.DecryptDES(pwd, keyStr);
            String[] pwds = pwd.Split('\n');

            if (pwds.GetLength(0) != 3)
            {
                MessageBox.Show("账号错误,请与发行商联系!", "错误");
                return;
            }

            comboBoxUserName.Items.Add("管理员");
            comboBoxUserName.Items.Add("工程师");
            //comboBoxUserName.Items.Add("操作员");
            pwdList.Add(pwds[0]);
            pwdList.Add(pwds[1]);
            //pwdList.Add(pwds[2]);

            sysParam.PwdList.Add(pwds[0]);
            sysParam.PwdList.Add(pwds[1]);
            //sysParam.PwdList.Add(pwds[2]);

            comboBoxUserName.SelectedIndex = 0;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {            
            if (sysParam.PwdList.Count == 0 && textBoxPwd.Text == adminDefaultPwd)
            {
                accountType = AccountType.Admin;
                sysParam.AccountType = accountType;
            
            }
            else if (sysParam.PwdList.Count == 0 && textBoxPwd.Text != adminDefaultPwd)
            {
                MessageBox.Show("密码错误!", "提示");
                return;
            }
            else if (textBoxPwd.Text == sysParam.PwdList[comboBoxUserName.SelectedIndex])
            {
                switch (comboBoxUserName.SelectedIndex)
                {
                    case 0:
                        accountType = AccountType.Admin;
                        sysParam.AccountType = accountType;
                        break;
                    case 1:
                        accountType = AccountType.Developer;
                        sysParam.AccountType = accountType;
                        break;
                    case 2:
                        accountType = AccountType.Operator;
                        break;
                    default:
                        break;
                }
            }
            else {
                if (comboBoxUserName.Text == "管理员")
                    MessageBox.Show("密码错误!", "提示");
                return;
            }

           
            DialogResult = DialogResult.OK;
        }
    }
}