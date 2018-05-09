using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AutoWelding.control;
using AutoWelding.awdatabase;
using AutoWelding.system;
using AutoWelding.uicontrol;
using AutoWelding.engine;
using System.Reflection;
using System.IO;
using System.Timers;
using System.Threading;
using AutoWelding.test;

namespace AutoWelding
{

    public struct AwCoordinate
    {
        public double leftStep;
        public double rightStep;
        public double frontStep;
    };

    public struct VppData
    {
        public AwCoordinate coordinate;
        public float value;
    };

    public struct UpVppData
    {
        public AwUpCoordinate coordinate;
        public float value;
    };

    public struct AwUpCoordinate
    {
        public double xStep;
        public double yStep;
    };


    public struct CouplingRetData
    {
        public float vpp;
        public float vpp1;
        public float vpp2;
        public float vpp3;
        public float vpp4;
        public float vpp5;
        public int lx1;
        public int ly1;
        public int lz;
        public int lx2;
        public int ly2;
        public int duration;            //耗时
        public UpVppData upVppData;		//顶部电机信息
        public VppData vppValue;		//底部电机信息
        public float angleA;
        public float angleB;
    };

    enum ControlMsg
    {
        MPP,		//最大功率点信息
        MPP_Angle,	//寻找MPP 点时的角度
        MPP_Buttom, //底部电机信息
        MPP_Up,		//顶部电机信息
        VBR,		//VBR 信息
        VBR_Ir_AD_Zero_Value,
        VBR_Ir_Calib_AD_Value,
        REPLACE_COLLOID_STATUS,
        VAPD_Vbr_3V_Iop_Vpp, //烤胶——设置Vapd=Vbr-3V测量并记录当前Iop/Vpp
        Control_Start,               //开始按键
    };

    public struct UiData //Control与UI通信的消息结构体
    {
        public int data1;
        public int data2;
        public int data3;
        public IntPtr ptr1;
        public IntPtr ptr2;
        public IntPtr ptr3;
        public float fData1;
    };	


    public partial class AutoWelding : Form
    {
        //显示器分辩率
        const int DispXPixel = 1440;
        const int DispYPixel = 900;

        float disRate = 1f;
        float xRate = 1f;
        float yRate = 1f;
        int NewPixel_X = Screen.PrimaryScreen.Bounds.Width;
        int NewPixel_Y = Screen.PrimaryScreen.Bounds.Height;
        // Motor 信息
        TraceSetup traceInfo;

        // 工艺参数
        ProductParameter productParameter;
        ProductData productData = null;

        // 耦合返回参数
        CouplingRetData couplingRetData;
        string prdId = "";

        float Vbr;
        float VAPD_Vbr_3V_Iop_Vpp;

        // 消息类型
        private const int WM_USER = 0x0400;
        private const int KControlMsg = WM_USER + 0x001;
        private const int KControlDebugInfo = WM_USER + 0x002;
        private const int KControlError = WM_USER + 0x003;
        private const int KControlProcessStatus = WM_USER + 0x004;  //工作流程状态
        private const int KControlProcessError = WM_USER + 0x005;  //工作流程状态错误
        private const int KControlTest = WM_USER + 0x100;

        private SystemParam sysParam;

        // UI controls
        private AwPanel imagePanel;
        private AwPanel statusAwPanel;
        private AwPanel vppPanel;
        private AwPanel statisticsPanel;
        private AwPanel SelectLine;

        private AwPanel productParamPanel;
        private AwPanel cmdPanel;
        List<Button> menuButtonList;


        // Excel data write
        private System.Timers.Timer excelTimer;
        List<ExcelDataUnit> excelDataList;
        Mutex excelMutex;


       // Control Error type
        const int Err_InvalidHandler = -1;
        const int Err_Invalid_Node = -20;
        const int Err_Invalid_Param = -21;
        const int Err_Timeout = -22;
        const int Err_Busy = -23;
        const int Err_WaitEvent = -24;
        const int Err_ProcessSquence = -25;
        const int Err_InvalidTraceMehod = -26;
        const int ErrInvalidPosition = -27;


        // Communication error
        const int Err_UnkonwCmd = -50;

        // MPC2810 error
        const int ErrMPC2810_LoadLabFailed = -100;
        const int ErrMPC2810_NoCard = -101;
        const int ErrMPC2810_InitCard = -102;
        const int ErrMPC2810_MoveFaild = -103;
        const int ErrMPC2810_CheckLimitFaild = -104;

        // Ad card error
        const int ErrDSO2090_InitFailed = -110;
        const int ErrDSO2090_InitTimerFailed = -111;
        const int ErrDSO2090_EndTimerFailed = -112;
        const int ErrDSO2090_GetDataFailed = -113;

        // IO error
        const int ErrIo_Failed = -120;
        const int ErrIo_InitTimerFailed = -121;

        //FormTest formTest;

        //agilentTest fmMachine=null;

        public static TC6200P mcTC6200P;
        public static ComBoard mcComBoard;
        public static agilent.Agilent mcAgilent;

        enum ControlErr
        {
            WeldingFailed = 0,  //焊接失败
        };

        public struct ProductTimer
        {
            public int Total;
            public int MoveUp;
            public int FindVpp;
            public int BakeColloid;
            public int MoveDown;
        };

	

        enum ProcessStatus
        {
            ProcessStart = 0,		//开始
            ProcessDone,			//完成
            ProcessPause,			//暂停
            ProcessReleaseChipShellDone //Only for releasig chip shell
        };

        public AutoWelding()
        {
            // 读取
            productParameter = ProductParameter.GetInstance();
            couplingRetData = new CouplingRetData();
            string strCom1="com1", strCom2="com2";
            openSave.RegistryOp.GetValue("com_TC6200P", ref strCom1);
            openSave.RegistryOp.GetValue("com_ComBoard", ref strCom2);
            mcTC6200P = new TC6200P(strCom1);
            mcComBoard = new ComBoard(strCom2);
            InitializeComponent();
            sysParam = SystemParam.GetInstance();
            excelDataList = new List<ExcelDataUnit>();
            sysParam.InitDebugLog();
            excelMutex = new Mutex();
            menuButtonList = new List<Button>();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = true;
            //sysParam.ControlFrameColor = SystemColors.ButtonFace;
            fmTest();

        }

        /**********************************************************************************************
         * discription: InitWelding
         * 
         * 
         ***********************************************************************************************/
        void InitWelding()
        {


            // 读取trace 配置信息
            traceInfo = new TraceSetup();

            // 获取版本信息
            // Step 6 发送命令初始化位置


            // Step 1 提示信息            



            // Step 4 获取trace method        


            // Step 5 发送配置信息

            // Check maximum position
            List<object> setupInfo = new List<object>();
            setupInfo.Add(traceInfo);
            setupInfo.Add(productParameter);

            InitExcelTimer();
        }

        private void InitExcelTimer()
        {

            excelTimer = new System.Timers.Timer(1000);
            excelTimer.Elapsed += new ElapsedEventHandler(ExcelTimeEvent);
            excelTimer.AutoReset = false;
            excelTimer.Enabled = false;
        }

        /**********************************************************************************************
         * discription: 更新状态
         * 
         * 
         ***********************************************************************************************/
        private void ExcelTimeEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            excelTimer.Stop();
            ExcelDataUnit excelUnitData = GetExcelData();

            if (excelUnitData == null)
                return;

            if (productData == null)
            {
                productData = new ProductData(ref sysParam);
                productData.CreateExcelFile();
            }

            try
            {
                productData.InsertEntry(ref excelUnitData);
            }
            catch (Exception ee)
            {
                MessageBox.Show("写入生产数据出错! " + ee.Message, "错误");
            }

            excelTimer.Start();
        }

        /**********************************************************************************************
         * discription: 命令接收函数
         * 
         * 
         ***********************************************************************************************/
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case KControlTest:
             //       textBox1.Text = m.LParam.ToString();
                    break;
                case KControlDebugInfo:
                    break;
                case KControlProcessStatus:
                    ProcessControlStatusMsg(ref m);
                    break;
                case KControlProcessError:
                    ProcessControlStatusErrorMsg( ref m);
                    break;
                case KControlError:
                    ProcessControlError(ref m);
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /**********************************************************************************************
         * discription: 处理control命令
         * 
         * 
         ***********************************************************************************************/


        /*
        void ShowReplaceColloidResult(int status)
        {            
            string msg = "";
            if (status == 0)
            {
                msg = "换胶已完成！";
            }
            else if (status == 1)
            {
                msg = "可以开始换胶！";
            }
            else
            {
                msg = "换胶未知状态:" + status.ToString();
            }

            buttonReplaceColloid.Text = status == 0 ? "更换胶水" : "结束换胶";

            FormNote note = new FormNote(msg);
            note.StartPosition = FormStartPosition.CenterScreen;
            note.ShowDialog();
        
        }
         * */

        void AppendExcelData( CouplingRetData retData, double timeCost, float Vbr, float VAPDVbr3VIopVpp, string prdId)
        {
            ExcelDataUnit dataUnit = new ExcelDataUnit();
            dataUnit.couplingRetData = retData;
            dataUnit.timeCost = timeCost;
            dataUnit.Vbr = Vbr;
            dataUnit.VAPD_Vbr3V_Iop_Vpp = VAPDVbr3VIopVpp;
            dataUnit.prdId = prdId;

            excelMutex.WaitOne();
            excelDataList.Add(dataUnit);
            excelMutex.ReleaseMutex();
            excelTimer.Start();
        }

        ExcelDataUnit GetExcelData()
        {
            ExcelDataUnit ret = null;

            excelMutex.WaitOne();
            
            if (excelDataList.Count > 0)
            {
                ret = excelDataList[0];
                excelDataList.RemoveAt(0);
            }            
            excelMutex.ReleaseMutex();
            return ret;
        }

        /**********************************************************************************************
         * discription: 处理流程状态
         * 
         * 
         ***********************************************************************************************/
        private void ProcessControlStatusMsg(ref Message m)
        {     
        
        }

        private void ProcessControlStatusErrorMsg(ref Message m)
        {

       

        }

        private void ProcessControlError(ref Message m)
        {
     
        }


        private void FreshProductStatistics()
        {
            txDie.Text = productData.PassedCount.ToString();

        }
        private void regist()
            {
            // Init permision
            Login formLogin = new Login(ref sysParam);
            formLogin.InitPermision();
        }

        private void fmTest()
        {
            xRate = (float)NewPixel_X / DispXPixel;
            yRate = (float)NewPixel_Y / DispYPixel;
            disRate = xRate > yRate ? yRate : xRate;
            // hide menu
            InitControls();
            InitMenuPanel();
            regist();

        }

        private void AutoWelding_Load(object sender, EventArgs e)
        {
            if (productParameter.ReleaseColloidBeforeCoupling)
            {
                ChangeTheProcessOrder();
            }

            //CreateNewProductBat();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Visible = false;
            menuStrip1.Visible = false;
            
            //InitControls();
            //InitMenuPanel();
            //regist();


            Visible = true;
            //timer1.Enabled = true;
        }

        private bool CheckRegistry()
        {
            bool isRegistered = true;

            AwRegistry awRegister = new AwRegistry();
            switch (awRegister.CheckRegisterInfo())
            { 
                case 0:
                    break;
                case -1:
                    EnableControls(false);
                    MessageBox.Show("未注册，请先注册!","提示");
                    isRegistered = false;
                    break;
                case -2:
                    EnableControls(false);
                    MessageBox.Show("注册信息错误，请重新注册!", "提示");
                    isRegistered = false;
                    break;
            
            }

            if (!isRegistered)
            {
                FormRegister formRegister = new FormRegister( ref awRegister );
                formRegister.StartPosition = FormStartPosition.CenterScreen;
                if (formRegister.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
            }


            EnableControls(true);
            
            // remove register button
            List<string> exCmdList = new List<string>();
            exCmdList.Add("注册");
            SetmenuButtonLists(exCmdList);

            return true;
        }

        private void EnableControls(bool isRegistered)
        {
            cmdPanel.Enabled = isRegistered;

            foreach (Button button in menuButtonList)
            {
                switch (button.Text)
                { 
                    case "电机卡设置":
                    case "AD卡设置":
                    case "参数设置":
                    case "测试":
                    case "调试信息":
                    case "登录":
                        button.Enabled = isRegistered;
                        break;
                }
            }

        }

        private void InitControls()
        {

            
            InitMiddleControls();
            InitBottomControls();
           
        }
        /// <summary>
        /// 生产统计
        /// </summary>
        void InitStatisticsPanel()
        {
            Point point = new Point();
            statisticsPanel = new AwPanel();
            point.X = SelectLine.Location.X+ SelectLine.Width+10;
            point.Y = SelectLine.Location.Y;
            statisticsPanel.Location = point;

            statisticsPanel.BorderColor = sysParam.ControlFrameColor;
            statisticsPanel.BorderWidth = 3;
            statisticsPanel.Width = (int)(190f*xRate);
            statisticsPanel.Height = imagePanel.Height; 
            statisticsPanel.Parent = this;
            statisticsPanel.TitleBackground.Height = 30;
            statisticsPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;

            Font fn = new Font("华文楷体", (float)14, FontStyle.Bold);
            statisticsPanel.Title.Font = fn;
            statisticsPanel.Title.ForeColor = Color.FromArgb(255, 255, 255);

            statisticsPanel.Title.Text = "生产统计";

            panelStatistics.Width = statisticsPanel.Width - 20;
            panelStatistics.Height = statisticsPanel.Height - statisticsPanel.TitleBackground.Height - 20;
            panelStatistics.Parent = statisticsPanel;

            point.X = 10;
            point.Y = statisticsPanel.TitleBackground.Height + 10;

            panelStatistics.Location = point;
/*
            point.X = (panelStatistics.Width - panelInerStatistics.Width) / 2;
            point.Y = (panelStatistics.Height - panelInerStatistics.Height) / 2;
            panelInerStatistics.Location = point;
*/        
        }
        /// <summary>
        /// 手动选择曲线
        /// </summary>
        void InitSelcetLine()
        {
            Point point = new Point();
            SelectLine = new AwPanel();
            point.X = imagePanel.Location.X + imagePanel.Width + 10;
            point.Y = imagePanel.Location.Y;
            SelectLine.Location = point;

            SelectLine.BorderColor = sysParam.ControlFrameColor;
            SelectLine.BorderWidth = 3;
            SelectLine.Width = (int)(190f * xRate);
            SelectLine.Height = imagePanel.Height;
            SelectLine.Parent = this;
            SelectLine.TitleBackground.Height = 30;
            SelectLine.TitleBackground.BackColor = sysParam.ControlBackgroundColor;

            Font fn = new Font("华文楷体", (float)14, FontStyle.Bold);
            SelectLine.Title.Font = fn;
            SelectLine.Title.ForeColor = Color.FromArgb(255, 255, 255);

            SelectLine.Title.Text = "曲线选择";

            panelSelect.Width = SelectLine.Width - 20;
            panelSelect.Height = SelectLine.Height - SelectLine.TitleBackground.Height - 20;
            panelSelect.Parent = SelectLine;

            point.X = 10;
            point.Y = SelectLine.TitleBackground.Height + 10;

            panelSelect.Location = point;
            /*
                        point.X = (panelStatistics.Width - panelInerStatistics.Width) / 2;
                        point.Y = (panelStatistics.Height - panelInerStatistics.Height) / 2;
                        panelInerStatistics.Location = point;
            */
        }

        void InitproductParamPanel()
        { 
        
            Point point = new Point();
            productParamPanel = new AwPanel();
            point.X = (int)(21f * xRate);
            point.Y = (int)(530f * yRate);
            productParamPanel.Location = point;

            productParamPanel.BorderColor = sysParam.ControlFrameColor;
            productParamPanel.BorderWidth = 3;
            productParamPanel.Width = imagePanel.Width;
            productParamPanel.Height = NewPixel_Y - productParamPanel.Location.Y - 10;
            productParamPanel.Parent = this;
            productParamPanel.TitleBackground.Height = 30;
            productParamPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;
            Font fn = new Font("华文楷体", (float)14, FontStyle.Bold);
            productParamPanel.Title.Font = fn;
            productParamPanel.Title.ForeColor = Color.FromArgb(255,255,255);
            productParamPanel.Title.Text = "工艺参数";


            //panelParam.Width = productParamPanel.Width - 20;
            //panelParam.Height = productParamPanel.Height - productParamPanel.TitleBackground.Height - 20;
            //panelParam.Parent = productParamPanel;

            //point.X = 10;
            //point.Y = productParamPanel.TitleBackground.Height + 10;

            //panelParam.Location = point;


        }

        void FreshProductParameterValues()
        {

        }

        void InitBottomControls()
        {
            //InitproductParamPanel();
            FreshProductParameterValues();
            
            // permission
            //  panelPermission.Visible = false;
        }

        private Point Relocate(Point point)
        {
            return new Point((int)(point.X * xRate), (int)(point.Y * yRate));
        }

        private Size ReSize(Size size)
        {
            return new Size((int)(size.Width * xRate), (int)(size.Height * yRate));
        }
        /// <summary>
        /// 图片显示区设置
        /// </summary>
        void InitImagePanel()
        {
            // pic box
            Point point = new Point();
            imagePanel = new AwPanel();
            point.X = (int)(statusAwPanel.Location.X);
            point.Y = (int)(statusAwPanel.Location.Y+statusAwPanel.Height+10);
            imagePanel.Location = point;
            imagePanel.BorderColor = sysParam.ControlFrameColor;
            imagePanel.BorderWidth = 3;
            imagePanel.Width = (int)(836f * xRate);
            imagePanel.Height = (int)(580f * yRate);
            imagePanel.Parent = this;
            imagePanel.TitleBackground.Height = 30;
            imagePanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;

            Font fn = new Font("华文楷体", (float)14, FontStyle.Bold);
            imagePanel.Title.Font = fn;
            imagePanel.Title.ForeColor = Color.FromArgb(255, 255, 255);

            imagePanel.Title.Text = "图像提示";

            scatterGraph1.Width = imagePanel.Width - 20;
            scatterGraph1.Height = imagePanel.Height - imagePanel.TitleBackground.Height - 20;
            scatterGraph1.Parent = imagePanel;

            pictureBox.Width = imagePanel.Width - 20;
            pictureBox.Height = imagePanel.Height - imagePanel.TitleBackground.Height - 20;
            point.X = 10;
            point.Y = imagePanel.TitleBackground.Height + 10;
            pictureBox.Location = point;
            pictureBox.Parent = imagePanel;
            pictureBox.Focus();

            scatterGraph1.Location = point;
            scatterGraph1.Parent = imagePanel;
            scatterGraph1.Focus();
        }
        private void UpFont(Control _panl1)
        {
            foreach (Control ctr in _panl1.Controls)
            {
                if (ctr.Controls.Count > 0)
                {
                    UpFont(ctr);
                }
                else 
                {
                    float oldSize = ctr.Font.Size;
                    if (ctr is Label || ctr is CheckBox)
                    {
                        Font lFont = new Font("华文楷体", (float)disRate * oldSize, FontStyle.Bold);
                        ctr.Font = lFont;
                        ctr.ForeColor = SystemColors.MenuHighlight;
                    }
                    else
                    {
                        Font lFont = new Font("宋体", (float)disRate * oldSize, FontStyle.Bold);
                        ctr.Font = lFont;
                    }
                   
                }
            }
        }
        /// <summary>
        /// 表头参数设置区
        /// </summary>
        void InitStatusPanel()
        {
            Point point = new Point();
            statusAwPanel = new AwPanel();
            point.X = panelSysTitleBackgroud.Location.X+10; 
            point.Y = (int)(panelSysTitleBackgroud.Height +10);
            statusAwPanel.Location = point;

            statusAwPanel.BorderColor = sysParam.ControlFrameColor;
            statusAwPanel.BorderWidth = 3;
            statusAwPanel.Width = (int)(NewPixel_X-20);
            statusAwPanel.Height =(int)( 160*yRate);//184@1440x900
            statusAwPanel.Parent = this;
            statusAwPanel.TitleBackground.Height = 30;
            statusAwPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;

            Font fn = new Font("华文楷体", (float)14, FontStyle.Bold);
            statusAwPanel.Title.Font = fn;
            statusAwPanel.Title.ForeColor = Color.FromArgb(255, 255, 255);

            statusAwPanel.Title.Text = "当前状态";

            panelStatus.Width = statusAwPanel.Width - 20;
            panelStatus.Height = statusAwPanel.Height - statusAwPanel.TitleBackground.Height - 20;
            panelStatus.Parent = statusAwPanel;
            point.X = statusAwPanel.Location.X;
            point.Y = statusAwPanel.TitleBackground.Height +10;

            panelStatus.Location = point;

            DrawProcessLines();


        }

        void InitParameters()
        {


/*
            point.Y = (panelCurrentParam.Height - textBoxX.Height) / 2 + 10;
            labelX.Location = new Point(labelX.Location.X, point.Y);
            textBoxX.Location = new Point(textBoxX.Location.X, point.Y);
            labelY.Location = new Point(labelY.Location.X, point.Y);
            textBoxY.Location = new Point(textBoxY.Location.X, point.Y);
            labelVPP.Location = new Point(labelVPP.Location.X, point.Y);
            textBoxPPV.Location = new Point(textBoxPPV.Location.X, point.Y);
*/

/*
            point.Y = (panelBottomPlatform.Height - textBoxOffset.Height) / 2 + 10;
            labelOffSet.Location = new Point(labelOffSet.Location.X, point.Y);
            textBoxOffset.Location = new Point(textBoxOffset.Location.X, point.Y);
            labelLean.Location = new Point(labelLean.Location.X, point.Y);
            textBoxLean.Location = new Point(textBoxLean.Location.X, point.Y);
            labelZ.Location = new Point(labelZ.Location.X, point.Y);
            textBoxZ.Location = new Point(textBoxZ.Location.X, point.Y);

*/
        }

        void InitMiddleControls()
        {
            InitSysTitle();
            InitStatusPanel();
            InitImagePanel();
            InitSelcetLine();
            InitStatisticsPanel();
            InitCmdButtons();
            UpFont(panel2);
            UpFont(panelStatus);
            UpFont(panelSelect);
            //InitParameters();        
        }


        void InitSysTitle()
        {
            Point point = new Point(0,0);
            panelSysTitleBackgroud.Location = point;
            panelSysTitleBackgroud.Width = NewPixel_X;
            panelSysTitleBackgroud.Height =(int) (120 *yRate);

            point.X = (panelSysTitleBackgroud.Width - panelSysTitle.Width) / 2;
            point.Y = (panelSysTitleBackgroud.Height - panelSysTitle.Height - 20 ) / 2;
            panelSysTitle.Location = point;

           // InitMenuPanel();
        }
        /// <summary>
        /// 按钮布局
        /// </summary>
        void InitCmdButtons()
        {            
            Point point = new Point();

            cmdPanel = new AwPanel();
            point.X = (int)(statisticsPanel.Location.X + statisticsPanel.Width+10);
            point.Y = imagePanel.Location.Y;
            cmdPanel.Location = point;
            cmdPanel.BorderColor = sysParam.ControlFrameColor;
            cmdPanel.BorderWidth = 2;
            cmdPanel.Width = (int)(205 * yRate);
            cmdPanel.Height = statisticsPanel.Location.Y + statisticsPanel.Height - cmdPanel.Location.Y;
            cmdPanel.Parent = this;
            cmdPanel.TitleBackground.Height = 30;
            cmdPanel.TitleBackground.BackColor = sysParam.ControlBackgroundColor;

            Font fn = new Font("华文楷体", (float)16, FontStyle.Bold);
            cmdPanel.Title.Font = fn;
            cmdPanel.Title.ForeColor = Color.FromArgb(255, 255, 255);

            cmdPanel.Title.Text = "命令";

            int invervalY = cmdPanel.Height - cmdPanel.TitleBackground.Height;
            invervalY = (invervalY - (70) * 7) / 8;
            point.X = (cmdPanel.Width - 150) / 2;
            point.Y = cmdPanel.TitleBackground.Height + invervalY;

            buttonStart.Width = 150;
            buttonStart.Height = 70;
            buttonStart.Location = point;
            buttonStart.Parent = cmdPanel;


            buttonClamp.Width = 150;
            buttonClamp.Height = 70;
            point.Y += buttonStart.Height + invervalY;
            buttonClamp.Location = point;
            buttonClamp.Parent = cmdPanel;

            buttonFix.Width = 150;
            buttonFix.Height = 70;
            point.Y += buttonClamp.Height + invervalY;
            buttonFix.Location = point;
            buttonFix.Parent = cmdPanel;

            buttonProductParameter.Width = 150;
            buttonProductParameter.Height = 70;
            point.Y += buttonFix.Height + invervalY;
            buttonProductParameter.Location = point;
            buttonProductParameter.Parent = cmdPanel;


            buttonNewPrdBat.Width = 150;
            buttonNewPrdBat.Height = 70;
            point.Y += buttonProductParameter.Height + invervalY;
            buttonNewPrdBat.Location = point;
            buttonNewPrdBat.Parent = cmdPanel;

            buttonStop.Width = 150;
            buttonStop.Height = 70;
            point.Y += buttonNewPrdBat.Height + invervalY;
            buttonStop.Location = point;
            buttonStop.Parent = cmdPanel;

            buttonReplaceColloid.Width = 150;
            buttonReplaceColloid.Height = 70;
            point.Y += buttonStop.Height + invervalY;
            buttonReplaceColloid.Location = point;
            buttonReplaceColloid.Parent = cmdPanel;

        
        }

        private void AxisSetupMenu_Click(object sender, EventArgs e)
        {
      

            if (traceInfo.IsUpdated)
            {
                traceInfo.UpdateDbInfo();
                MessageBox.Show("该设置已保存，会在重启本程序之后有效！请将中止当前一切操作，并检测平台后重启程序！","提示"); 
                traceInfo.IsUpdated = false;
            }
        }
        private bool autoTest()
        {
            for (int i = 0; i <  productParameter.PrdBatInfo.SampleSize; i++)
            {
               
            }
            return true;
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (productParameter.StoreSinglePrdId)
            {

            }
            else
            {
                prdId = "";
            }
            // Data base test
            //DabaBaseTest();

            ClearProductData();
            buttonReplaceColloid.Enabled = false;
           // MessageBox.Show("Msg result=" + ret.ToString());
        }

        void ClearProductData()
        {
            txSampleSize.Text = "";

        }

        void DabaBaseTest()
        {
            traceInfo.MotorsInfo.AxisNum = 8;
            traceInfo.MotorsInfo.CardNum = 9;

            S_UpMotor upMotor = new S_UpMotor();
            upMotor.motorX = 1;
            upMotor.motorY = 2;
            traceInfo.MotorsInfo.UpMotor = upMotor;

            traceInfo.UpdateDbInfo();

        }

        private void ToolStripMenuItemDebugInfo_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemFileManagement_Click(object sender, EventArgs e)
        {
            string dataPath = "";

          //  string dataPath = sysParam.CurrentPath + "\\data";
            if(productParameter.DataPath=="")
            {
                dataPath = sysParam.CurrentPath + "\\data";
            }
            else
            {
                dataPath = productParameter.DataPath;
            }

            if (!Directory.Exists(dataPath))
            {
                MessageBox.Show("无生产文件!","提示");
                return;
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = dataPath;
            fileDialog.Filter = "xcel files (*.xls)|*.xls";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Process.Start(fileDialog.FileName);
            }
        }

        private void ToolStripMenuItemPWDSetup_Click(object sender, EventArgs e)
        {
            FormPwdSetup pwdSetup = new FormPwdSetup(ref sysParam);
            pwdSetup.StartPosition = FormStartPosition.CenterScreen;
            pwdSetup.ShowDialog();
        }

        private void ToolStripMenuItemLogout_Click(object sender, EventArgs e)
        {
            sysParam.AccountType = AccountType.Operator;
            switch (sysParam.AccountType)
            {
                case AccountType.Admin:
                    labelAccountInfo.Text = "管理员";
                    break;
                case AccountType.Developer:
                    labelAccountInfo.Text = "工程师";
                    break;
                case AccountType.Operator:
                    labelAccountInfo.Text = "操作员";
                    break;
            }

            List<string> excludeList = new List<string>();
            excludeList.Add("登出");
            SetmenuButtonLists(excludeList);
        }

        private void buttonClamp_Click(object sender, EventArgs e)
        {
            agilentTest fmMachine = new agilentTest();
            fmMachine.ShowDialog();
        }

        private void buttonFix_Click(object sender, EventArgs e)
        {

        }

        public void FormTestClose()
        {
          
        }

        private void ToolStripMenuItemTestForm_Click(object sender, EventArgs e)
        {

        }

        private void MenuButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //static int ii = 5;
        private void buttonAdjust_Click(object sender, EventArgs e)
        {           
            /*
            FormPwdSetup pwdSetup = new FormPwdSetup(ref sysParam);
            pwdSetup.ShowDialog();*/
            CreateNewProductBat();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            AdjustColloid();
        }

        void AdjustColloid()
        {
            ClearProductData();
        }

        private void buttonProductParameter_Click(object sender, EventArgs e)
        {
            //FormIoControl ioControlForm = new FormIoControl( ref sysParam );
            //ioControlForm.StartPosition = FormStartPosition.CenterScreen;
            //ioControlForm.ShowDialog();
            bool releaseColloidOrder = productParameter.ReleaseColloidBeforeCoupling;


            if(releaseColloidOrder != productParameter.ReleaseColloidBeforeCoupling)
            {
                ChangeTheProcessOrder();
            }
        }

        private void buttonNewProductBat_Click(object sender, EventArgs e)
        {
            CreateNewProductBat();
        }

        private void CreateNewProductBat()
        {
            FormNewProductBat newPrdBat = new FormNewProductBat(ref sysParam,productParameter.PrdBatInfo);
            newPrdBat.StartPosition = FormStartPosition.CenterScreen;
            if (newPrdBat.ShowDialog() == DialogResult.OK)
            {
                if (productData != null)
                    productData.Close();
                productData = new ProductData(ref sysParam);
                productData.CreateExcelFile(newPrdBat.PrdBatInfo, productParameter);
                productParameter.PrdBatInfo = newPrdBat.PrdBatInfo;
                productParameter.UpdateProductBatInfo();
                ShowProductInfo(newPrdBat.PrdBatInfo);
            }
            else
            {
                if (productData != null)
                    productData.Close();
                productData = new ProductData(ref sysParam);
                productData.CreateExcelFile(productParameter.PrdBatInfo, productParameter);
                ShowProductInfo(productParameter.PrdBatInfo);
            }
        }

        private void ShowProductInfo(ProductBatInfo prdBatInfo)
        {
            txPartNum.Text = prdBatInfo.PartNumber;
            txWafelLot.Text = prdBatInfo.WaferLot;
            txAssLot.Text = prdBatInfo.AssemblyLot;
            txPackage.Text = prdBatInfo.Package;
            txDie.Text = prdBatInfo.Dietype;
            txSampleSize.Text = prdBatInfo.SampleSize.ToString();

        }

        private void MenuButtonSystem_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout(ref sysParam);
            formAbout.StartPosition = FormStartPosition.CenterScreen;
            formAbout.ShowDialog();
        }

        private void MenuButtonRegistry_Click(object sender, EventArgs e)
        {
            AwRegistry register = new AwRegistry();
            //MessageBox.Show("Registry string=" + register.CreateRegistryInfo());
            FormRegister formRegister = new FormRegister( ref register);
            formRegister.StartPosition = FormStartPosition.CenterScreen;
            if (formRegister.ShowDialog() == DialogResult.OK)
            {
                EnableControls(true);

                List<string> exCmdList = new List<string>();
                exCmdList.Add("注册");
                SetmenuButtonLists(exCmdList);

                // get start
                InitWelding();  
            }
        }

        private void MenuButtonLogin_Click(object sender, EventArgs e)
        {
            // login
            Login login = new Login(ref sysParam);
            login.StartPosition = FormStartPosition.CenterScreen;
            if (login.ShowDialog() != DialogResult.OK)
            {                
                return;
            }

            switch (sysParam.AccountType)
            {
                case AccountType.Admin:
                    labelAccountInfo.Text = "管理员";
                    break;
                case AccountType.Developer:
                    labelAccountInfo.Text = "工程师";
                    break;
                case AccountType.Operator:
                    labelAccountInfo.Text = "操作员";
                    break;
            }

            List<string> excludeList = new List<string>();
            excludeList.Add("登录");
            SetmenuButtonLists(excludeList);
        }

        
        /*
               private void button1_Click(object sender, EventArgs e)
                {
                    int mainControlThreadId = weldingCtrl.WeldingInit();
                    textBox1.Text = mainControlThreadId.ToString();
                    controlHandle = new IntPtr(mainControlThreadId);
                    // send test msg
                    int i = Win32Api.PostMessage(controlHandle, 0, 11, 11);
                    textBox1.Text += "  " + i.ToString();
                }
                private void button2_Click(object sender, EventArgs e)
                {
                    RetData retData = new RetData();
                    int i = weldingCtrl.SendCommands(CmdType.CMD_INIT_MPC2810,ref retData);
                    textBox1.Text = i.ToString();
                    if (i < 0)
                    {
                        MessageBox.Show(retData.errMsg);
                    }
                }

                private void button3_Click(object sender, EventArgs e)
                {
                    MotorInfo motorInfo = new MotorInfo();
                    motorInfo.AxisNum = 12;
                    motorInfo.CardNum = 3;
                    S_BottomMotor bottomMotor;
                    bottomMotor.motorLeft = 4;
                    bottomMotor.motorRight = 5;
                    bottomMotor.motorFront = 6;
                    motorInfo.BottomMotor = bottomMotor;

                    AwDbInterface xmlDb = AwDbFactory.CreateDBEngine();
                    xmlDb.UpdateMotorInfo(motorInfo);
                }

                private void button4_Click(object sender, EventArgs e)
                {
                    RetData retData = new RetData();
                    int i = weldingCtrl.SendCommands(CmdType.CMD_TRACE_METHOD, ref retData);
                    textBox1.Text = "total trace:" + i.ToString();

                    foreach (int item in retData.tracePool)
                    {
                        textBox1.Text += " Trace id:" + item.ToString();
                    }
                    if (i < 0)
                    {
                        MessageBox.Show(retData.errMsg);
                    }
                }
         */

        private void InitMenuPanel()
        {
            Panel menuPanel = new Panel();
            menuPanel.Name = "menuPanel";
            menuPanel.Height = 30;
            menuPanel.Width = panelSysTitleBackgroud.Width;
            menuPanel.BackColor = Color.Transparent;            
            Point point = new Point();
            point.X = 0;
            point.Y = panelSysTitleBackgroud.Location.Y + panelSysTitleBackgroud.Height - menuPanel.Height;
            menuPanel.Location = point;
            menuPanel.Parent = panelSysTitleBackgroud;
            menuPanel.Margin = new Padding(0, 0, 0, 0);
            menuPanel.Padding = new Padding(0, 0, 0, 0);
            InitmenuButtonLists(ref menuPanel);
            SetmenuButtonLists( null );
            point = panelSysTitleBackgroud.Location;
            point.X += panelSysTitleBackgroud.Width - 100;
            point.Y += panelSysTitleBackgroud.Height - labelAccountInfo.Height - menuPanel .Height - 20;
            labelAccountInfo.Location = point;
            point.X = labelTimer.Location.X;
            labelTimer.Location = point;
        }

        private void InitmenuButtonLists( ref Panel menuPanel )
        {
            Point point = new Point(0, 0);

            // Step 1 find out the command button number
            int buttonLen = (int)(136f * xRate);
            int number = menuPanel.Width / buttonLen;




            for (int i = 0; i < number; i++)
            {
                Button button = new Button();
                switch (i)
                {
                    case 0:
                        button.Text = "";
                        break;
                    case 1:
                        button.Text = "";
                        break;
                    case 2:
                        button.Text = "其它设置";
                        break;
                    case 3:
                        button.Text = "登陆";
                        break;
                    case 4:
                        button.Text = "系统";
                        break;
                }
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = sysParam.SysBackgroundColor;//buttonLogin.BackColor;
                button.Margin = new Padding(0, 0, 0, 0);
                button.Padding = new Padding(0, 0, 0, 0);
                if (i == number - 1)
                {
                    button.Width = menuPanel.Width - point.X;
                }
                else
                {
                    button.Width = buttonLen + 3;
                }
                button.Height = menuPanel.Height;
                button.Location = point;
                point.X += buttonLen;

                Font fn = new Font("华文楷体", (float)13, FontStyle.Bold);
                button.Font = fn;
                button.ForeColor = Color.FromArgb(0, 51, 153);

                menuPanel.Controls.Add(button);
                menuButtonList.Add(button);
            }
        }

        private void SetmenuButtonLists( List<string> excludeCmdList )
        { 
            // Step 1 remove all the buttons' property
            ResetButton();

            int i = 0;
            // Supper admin
            /*
            if (isIncluded(ref excludeCmdList, "电机卡设置"))
            {
                menuButtonList[i].Text = "电机卡设置";
                menuButtonList[i++].Click += AxisSetupMenu_Click;
            }
             */ 

            //menuButtonList[i++].Text = "AD卡设置";

            if (isIncluded(ref excludeCmdList, "参数设置") && (sysParam.CurrentAccountPermision().permition & PermitionItems.paramSetting) != 0)
            {
                menuButtonList[i].Click += buttonProductParameter_Click;
                menuButtonList[i++].Text = "参数设置";
            }


            if (isIncluded(ref excludeCmdList, "新批次生产") && (sysParam.CurrentAccountPermision().permition & PermitionItems.newProductBat) != 0)
            {
                menuButtonList[i].Click += buttonNewProductBat_Click;
                menuButtonList[i++].Text = "新批次生产";
            }

            if (sysParam.AccountType == AccountType.Operator)
            {
                if (isIncluded(ref excludeCmdList, "登录"))
                {
                    menuButtonList[i].Text = "登录";
                    menuButtonList[i++].Click += MenuButtonLogin_Click;
                }
            }

            //if (sysParam.AccountType == AccountType.Admin || sysParam.AccountType == AccountType.Developer)
            {
                if (isIncluded(ref excludeCmdList, "注 册") && (sysParam.CurrentAccountPermision().permition & PermitionItems.register) != 0)
                {
                    menuButtonList[i].Text = "注 册";
                    menuButtonList[i++].Click += MenuButtonRegistry_Click;
                }

                if (isIncluded(ref excludeCmdList, "调 试") && (sysParam.CurrentAccountPermision().permition & PermitionItems.debug) != 0)
                {
                    menuButtonList[i].Text = "调 试";
                    menuButtonList[i++].Click += ToolStripMenuItemTestForm_Click;
                }

                if (isIncluded(ref excludeCmdList, "调试信息") && (sysParam.CurrentAccountPermision().permition & PermitionItems.debugInfo) != 0)
                {
                    menuButtonList[i].Text = "调试信息";
                    menuButtonList[i++].Click += ToolStripMenuItemDebugInfo_Click;
                }

                if (isIncluded(ref excludeCmdList, "文件管理") && (sysParam.CurrentAccountPermision().permition & PermitionItems.fileManager) != 0)
                {
                    menuButtonList[i].Text = "文件管理";
                    menuButtonList[i++].Click += ToolStripMenuItemFileManagement_Click;
                }

                if (sysParam.AccountType == AccountType.Admin && (sysParam.CurrentAccountPermision().permition & PermitionItems.accountManager) != 0)
                {
                    menuButtonList[i].Text = "密码设定";
                    menuButtonList[i++].Click += ToolStripMenuItemPWDSetup_Click;
                }

                if ( (sysParam.AccountType == AccountType.Admin || sysParam.AccountType == AccountType.Developer) && isIncluded(ref excludeCmdList, "登出"))
                {
                    menuButtonList[i].Text = "登出";
                    menuButtonList[i++].Click += ToolStripMenuItemLogout_Click;
                }

            }

            if (isIncluded(ref excludeCmdList, "系 统") || (sysParam.CurrentAccountPermision().permition & PermitionItems.systemInfo) != 0)
            {
                menuButtonList[i].Text = "系 统";
                menuButtonList[i++].Click += MenuButtonSystem_Click;
            }

            if (isIncluded(ref excludeCmdList, "退 出"))
            {
                menuButtonList[i].Text = "退 出";
                menuButtonList[i++].Click += MenuButtonExit_Click;
            }


            //buttonStart.Enabled = (sysParam.CurrentAccountPermision().permition & PermitionItems.pressButton) != 0;
            //buttonClamp.Enabled = (sysParam.CurrentAccountPermision().permition & PermitionItems.pressButton) != 0;
            //buttonFix.Enabled = (sysParam.CurrentAccountPermision().permition & PermitionItems.pressButton) != 0;
            //buttonNewPrdBat.Enabled = (sysParam.CurrentAccountPermision().permition & PermitionItems.newProductBat) != 0;
            //buttonProductParameter.Enabled = (sysParam.CurrentAccountPermision().permition & PermitionItems.paramSetting) != 0;

        }

        private void ResetButton()
        {
            foreach (Button button in menuButtonList)
            {
                button.Text = "";
                ClearEvent(button, "Click");
            }
        }

        void ClearEvent(Control control, string eventname)
        {
            if (control == null) 
                return;
            if (string.IsNullOrEmpty(eventname))
                return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);
            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + eventname, mFieldFlags);
            Delegate d = eventHandlerList[fieldInfo.GetValue(control)];

            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(eventname);

            foreach (Delegate dx in d.GetInvocationList())
                eventInfo.RemoveEventHandler(control, dx);

        }


        private bool isIncluded(ref List<string> excludeCmdList, string cmdName)
        {
            if (excludeCmdList == null)
                return true;

            foreach (string exCmdStr in excludeCmdList)
            {
                if (exCmdStr == cmdName)
                    return false;
            }

            return true;
        }

        private void InitStatusLabels()
        {


            // draw lines
            DrawProcessLines();

        }

        private void DrawProcessLines()
        {
            // line 1
          
        }

        private void ShowBottomPlatformControls(bool isVisible)
        {

        }

        private void ShowProcess( ProcessSteps processStep )
        {
            
        }

        private void AutoWelding_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (excelTimer != null)
                excelTimer.Close();
            
            ExcelDataUnit excelUnitData = null;

            while (true)
            {
                excelUnitData = GetExcelData();

                if (excelUnitData == null)
                    break;

                if (productData == null)
                {
                    productData = new ProductData(ref sysParam);
                    productData.CreateExcelFile();
                }

                try
                {
                    productData.InsertEntry(ref excelUnitData);
                }
                catch
                {
                    
                }

            }

            if (productData != null)
                productData.Close();

            excelMutex.Close();

            Thread.Sleep(2000);
        }

        private void buttonClearPassed_Click(object sender, EventArgs e)
        {
            if (productData == null)
            {
                productData = new ProductData(ref sysParam);
                productData.CreateExcelFile();
            }

            productData.PassedCount = 0;

            FreshProductStatistics();
        }

        private void buttonClearFailed_Click(object sender, EventArgs e)
        {
            if (productData == null)
            {
                productData = new ProductData(ref sysParam);
                productData.CreateExcelFile();
            }

            productData.FailedCount = 0;

            FreshProductStatistics();
        }

        void ChangeTheProcessOrder()
        {            

        }

        private void currentTimetimer_Tick(object sender, EventArgs e)
        {
            labelTimer.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        private void buttonReplaceColloid_Click(object sender, EventArgs e)
        {
            int frontReleaseTime = 0;
            int leftReleaseTime = 0;
            int rightReleaseTime = 0;
            frontReleaseTime = productParameter.ReleaseColloid.front;
            leftReleaseTime = productParameter.ReleaseColloid.left;
            rightReleaseTime = productParameter.ReleaseColloid.right;
            if (frontReleaseTime <= 0)
            {
                MessageBox.Show("无效出胶时间", "提示");
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


        }

        private void AutoWelding_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }  // end class

    public class ExcelDataUnit
    {
        public CouplingRetData couplingRetData;
        public float Vbr;
        public float VAPD_Vbr3V_Iop_Vpp;
        public double timeCost;
        public string prdId; //产品id
    };


}