using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AutoWelding.system
{
    enum ProcessSteps
    {
        SInit = 0,						//��ʼ��,�ص�����0�㣬�ٵ�Platform point
        //SToAbsolutePoint,				//�ص�����0
        SToPlatFormPoint,				//��Platform ��
        SClampShellChip,				//��оƬ�Ϳ���
        SClampShell,					//�п��壬оƬ
        SClampChip,						//��оƬ		
        SToWorkPlatformPoint,			//������ƽ̨��
        SReleaseColloid,				//�������������������
        SMoveToVppPoint,				//�ƶ�������ʵ�
        SBakeColloid,					//����
        SReleaseShellChip,				//�ɿ�оƬ�Ϳ���
        SReleaseShell,					//�ɿ�����
        SReleaseChip,					//�ɿ�оƬ
        SIdle,							//����״̬
        SFindVBR,						//����VBR


        SProductTimer = 200,
        SProcessStart = 250,              //��ʼ (���ڿ�ʼ����ʹ��)
        SAdjustColloidMode = 251,		//��ʼ����
    };

    public enum AccountType
    {
        Admin,
        Developer,
        Operator,
    }

    public class AccountPermision
    {
        public AccountType accountType;
        public UInt32 permition;    
    }

    public class PermitionItems {
        public const UInt32 paramSetting = 0x01 << 0;     //��������
        public const UInt32 pressButton = 0x01 << 1;      //��������
        public const UInt32 adjustColloid = 0x01 << 2;    //����
        public const UInt32 debug = 0x01 << 3;            //����
        public const UInt32 debugInfo = 0x01 << 4;        //������Ϣ
        public const UInt32 fileManager = 0x01 << 5;      //�ļ�����
        public const UInt32 accountManager = 0x01 << 6;   //�����趨
        public const UInt32 newProductBat = 0x01 << 7;    //����������
        public const UInt32 systemInfo = 0x01 << 8;       //ϵͳ��Ϣ
        public const UInt32 register = 0x01 << 9;         //ע��
        public const UInt32 CheckingCode = 0xAA800000;                       //1010101010
        public const UInt32 CheckingCodeMask = 0xFFC00000;
        
    }



    public class SystemParam
    {     
        //public Debuglog debugLog;

        // UI Colors
        Color sysBackgroundColor = Color.FromArgb(150, 216, 228, 248);
        Color controlBackgroundColor = Color.FromArgb(255, 171, 205, 230);//Color.FromArgb(255, 199, 192, 155);
        Color controlFrameColor = Color.FromArgb(255, 171, 205, 230);//Color.FromArgb(255, 203, 191, 125);
        Color processBackgroundColor = Color.FromArgb(255, 7, 142, 0);
        // version
        const string version = "�ۺϲ���ϵͳ 2.0.1.1 2014-9-10";
        const string cpyRight = "�ɶ������Ƽ����޹�˾   ��Ȩ����";
        string controlVersion = "2.0.1.1";
        string currentPath = "";
        private AccountType accountType;
        private List<AccountPermision> accountPermisionList;        //��¼�����û���permision
        private List<string> pwdList;
        private static SystemParam instance;
        private List<MsgReceiverInterface> receiverList;

        public List<string> PwdList {
            get { return pwdList; }
        }

        public AccountType AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        public string CurrentPath
        {
            get { return currentPath; }
        }

        public string ControlVersion
        {
            get { return controlVersion; }
            set { controlVersion = value; }
        }

        public string Version
        {
            get { return version; }
        }

        public Color SysBackgroundColor
        {
            get { return sysBackgroundColor; }
        }

        public Color ControlBackgroundColor
        {
            get { return controlBackgroundColor; }
        }

        public Color ProcessBackgroundColor
        {
            get { return processBackgroundColor; }
        }

        public List<AccountPermision> AccountPermisionList
        {
            get { return accountPermisionList; }
        }

        public Color ControlFrameColor
        {
            get { return controlFrameColor; }
        }

        public string CpyRight {
            get { return cpyRight; }
        }

        private SystemParam()
        {
            accountType = AccountType.Operator;
            pwdList = new List<string>();            
            currentPath = System.Environment.CurrentDirectory;
            InitPermisionList();
            receiverList = new List<MsgReceiverInterface>();
        }

        public void RegisterReceiver(MsgReceiverInterface rev)
        {
            foreach (MsgReceiverInterface receiver in receiverList)
            {
                if (rev == receiver)
                    return;
            }

            receiverList.Add(rev);
        }

        public void UnregisterReceiver(MsgReceiverInterface rev)
        {
            foreach (MsgReceiverInterface receiver in receiverList)
            {
                if (rev == receiver)
                {
                    receiverList.Remove(receiver);
                    return;
                }
            }
        }

        public void ReceiverHandleMsg(ref System.Windows.Forms.Message msg)
        {
            foreach (MsgReceiverInterface rev in receiverList)
            {
                rev.HandleMsg(ref msg);
            }
        }

        public static SystemParam GetInstance()
        {
            if (instance == null)
                instance = new SystemParam();
            return instance;
        }

        private void InitPermisionList()
        {
            accountPermisionList = new List<AccountPermision>();
            AccountPermision ap = new AccountPermision();

            // administrator
            ap.accountType = AccountType.Admin;
            ap.permition = PermitionItems.CheckingCode | PermitionItems.accountManager| PermitionItems.adjustColloid | PermitionItems.debug |
                PermitionItems.debugInfo| PermitionItems.fileManager| PermitionItems.newProductBat| PermitionItems.paramSetting|
                PermitionItems.pressButton| PermitionItems.register | PermitionItems.systemInfo; // All permision
            accountPermisionList.Add(ap);

            // Engineer
            ap = new AccountPermision();
            ap.accountType = AccountType.Developer;
            ap.permition = PermitionItems.CheckingCode | PermitionItems.adjustColloid | PermitionItems.debug |
                PermitionItems.fileManager | PermitionItems.newProductBat | PermitionItems.paramSetting |
                PermitionItems.register | PermitionItems.systemInfo; 
            accountPermisionList.Add(ap);

            // Operator
            ap = new AccountPermision();
            ap.accountType = AccountType.Operator;
            ap.permition = PermitionItems.CheckingCode | PermitionItems.adjustColloid | PermitionItems.newProductBat | 
                PermitionItems.register | PermitionItems.systemInfo;
            accountPermisionList.Add(ap);
        }

        public void InitDebugLog()
        {
        }

        public AccountPermision CurrentAccountPermision()
        {
            foreach (AccountPermision ap in accountPermisionList)
            {
                if (ap.accountType == accountType)
                {
                    return ap;
                }
            }

            AccountPermision defaultAp = new AccountPermision();
            defaultAp.accountType = AccountType.Operator;
            defaultAp.permition = PermitionItems.CheckingCode | PermitionItems.adjustColloid | PermitionItems.newProductBat |
                PermitionItems.register | PermitionItems.systemInfo;

            return defaultAp;
        }

        public AccountPermision GetAccountPermission(AccountType at)
        {
            foreach (AccountPermision ap in accountPermisionList)
            {
                if (ap.accountType == at)
                {
                    return ap;
                }
            }

            AccountPermision defaultAp = new AccountPermision();
            defaultAp.accountType = AccountType.Operator;
            defaultAp.permition = PermitionItems.CheckingCode | PermitionItems.adjustColloid | PermitionItems.newProductBat |
                PermitionItems.register | PermitionItems.systemInfo;

            return defaultAp;

        }

        public void UpdateAccountPermission(AccountPermision permission)
        {
            foreach (AccountPermision ap in accountPermisionList)
            {
                if (ap.accountType == permission.accountType)
                {
                    ap.permition = permission.permition;
                   //accountPermisionList[accountPermisionList.IndexOf(ap)].permition = permission.permition;
                    
                    return;
                }
            }
        }
    }
}
