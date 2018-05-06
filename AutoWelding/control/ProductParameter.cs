using System;
using System.Collections.Generic;
using System.Text;
using AutoWelding.awdatabase;

namespace AutoWelding.control
{
    public struct ProductBatInfo
    {
        public string PartNumber;           //
        public string AssemblyLot;          //
        public string Dietype;         //
        public string WaferLot;         //
        public string Package;             //
        public int SampleSize;             //
    }

    public struct ReleaseColloidTime
    {
        public int front;
        public int left;
        public int right;
    }

    public enum ApApdType
    { 
        AD,
        APD,
    }

    public class ApdParam
    {
        //获取0位AD值
        public float Ir_AD_Zero_Value = 0.010f;//AD的0值偏移电压
        //AD校准值
        public float Ir_Calib_Value = 15.5f;//@volIn1=4.279f 万用表测得值
        public float Ir_Calib_AD_Value = 4.297f;	//校准时AD采样值
        //Calibration Value
        public float Vapd_Max_Value = 56.8f; //@volOut1 = 0.050f
        public float Vapd_Max_DA_Value = 0.050f;//校准时DA输出值
        public float Vapd_Min_Value = 9.9f;//@ volOut1 = 1.800f
        public float Vapd_Min_DA_Value = 1.800f;//@ volOut1 = 1.800f　校准时DA输出值

        public float Vapd_Upper_Limit = 55.000f;//@ 设置查找VBR范围的最大值
        public float Vapd_Lower_Limit = 30.000f;//@ 设置查找VBR范围的最小值 

        public float Vbr_Judge = 10.000f;//

        public int VbrType = 1;     // 1 Vbr-3，2 Vbr-4，3 Vbr*0.9

    };

    public class ProductParameter
    {
        float minimumVpp;                                  //最小Vpp
        int delayMoveOutReleaseColloidDIo;       //延时伸出点胶针
        int delayReleaseColloid;                 //延时出胶
        ReleaseColloidTime releaseColloid;                      //出胶时间
        int delayMoveBackReleaseColloidDIo;      //点胶针延时收回
        int bakeColloid;                         //烤胶时间
        int adAdjust;                            //AD 修正百分比
        string machineCode;                      //机器编号
        bool releaseColloidBeforeCoupling = false;       //耦合前点胶
        bool findCenter = false;                         //找到最大值后执行找中心
        float lxLyDownVoltage;                            //LXLY的降压
        float  lzDownVoltage;                             //LZ的降压
        int lxLyMin;                                    //lxLy 最小值
        int lxLyMax;                                    //LxLy 最大值
        int lzMin;                                      //LZ 最小值
        int lzMax;                                      //LZ 最大值
        int zMin;                                       //Z 最小值
        int zMax;                                       //Z 最大值
        uint vbrConfig = (0x01<<1) | (0x01 << 3) | (0x01) << 4;					// bit 0 Vbr*0.9, bit 1 Vbr-3, bit 2 Vbr-4, bit 3 Sync/Async, bit 4 APD or AD
		int vbrMax = 5;						//Vbr最大值
		int vbrMin = 1;						//Vbr最小值
        string dataPath="";                                //数据保存路径

        bool storeSinglePrdId = false;                      //记录每只产品编号
        bool fnsr = false;
        bool tocan = false;                                 //耦合时对TOCAN供电

        private AwDbInterface xmlDb;
        ProductBatInfo prdBatInfo;                      //生产批次信息
        ApdParam apdParm;

        private static ProductParameter instance;

        public ApdParam APDParam
        {
            get { return apdParm; }
        }


        public ProductBatInfo PrdBatInfo
        {
            get { return prdBatInfo; }
            set { prdBatInfo = value; }
        }

        public float MinimumVpp {
            get { return minimumVpp; }
            set { minimumVpp = value; }
        }

        public int DelayMoveOutReleaseColloidDIo
        {
            get { return delayMoveOutReleaseColloidDIo; }
            set { delayMoveOutReleaseColloidDIo = value; }
        }

        public int DelayReleaseColloid
        {
            get { return delayReleaseColloid; }
            set { delayReleaseColloid = value; }
        }

        public ReleaseColloidTime ReleaseColloid
        {
            get { return releaseColloid; }
            set { releaseColloid = value; }
        }

        public int DelayMoveBackReleaseColloidDIo
        {
            get { return delayMoveBackReleaseColloidDIo; }
            set { delayMoveBackReleaseColloidDIo = value; }
        }

        public int BakeColloid
        {
            get { return bakeColloid; }
            set { bakeColloid = value; }
        }

        public int AdAdjust
        {
            get { return adAdjust; }
            set { adAdjust = value; }
        }

        public string MachineCode
        {
            get { return machineCode; }
            set { machineCode = value; }
        }

        public bool ReleaseColloidBeforeCoupling 
        {
            get { return releaseColloidBeforeCoupling; }
            set { releaseColloidBeforeCoupling = value; }
        }

        public bool FindCenter 
        {
            get { return findCenter; }
            set { findCenter = value; }
        }

        public float LxLyDownVoltage
        {
            get { return lxLyDownVoltage; }
            set { lxLyDownVoltage = value; }
        }

        public float LzDownVoltage
        {
            get { return lzDownVoltage; }
            set { lzDownVoltage = value; }
        }

        public int LxLyMin
        {
            get { return lxLyMin; }
            set { lxLyMin = value; }
        }

        public int LxLyMax
        {
            get { return lxLyMax; }
            set { lxLyMax = value; }
        }

        public int LzMin
        {
            get { return lzMin; }
            set { lzMin = value; }
        }

        public int LzMax
        {
            get { return lzMax; }
            set { lzMax = value; }
        }

        public int ZMin
        {
            get { return zMin; }
            set { zMin = value; }
        }

        public int ZMax
        {
            get { return zMax; }
            set { zMax = value; }
        }

        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public uint VbrConfig
        {
            get { return vbrConfig; }
            set { vbrConfig = value; }
        }

        public ApApdType AdApd
        {
            get {
                if (((vbrConfig >> 4) & 0x00000001) == 0x00000000)
                    return ApApdType.AD;
                else
                    return ApApdType.APD;

            }
            set {
                if (value == ApApdType.AD)
                    vbrConfig &= ~((uint)0x00000001 << 4);
                else
                    vbrConfig |= (0x00000001 << 4);
            
            }
        }

        public int VbrMax
        {
            get { return vbrMax; }
            set { vbrMax = value; }
        }

        public int VbrMin
        {
            get { return vbrMin; }
            set {vbrMin = value;}
        }

        public bool StoreSinglePrdId
        {
            get { return storeSinglePrdId; }
            set { storeSinglePrdId = value; }
        }

        public bool FNSR
        {
            get { return fnsr; }
            set { fnsr = value; }
        }

        public bool TOCAN
        {
            get { return tocan; }
            set { tocan = value; }
        }

        /**********************************************************************************************
        * discription: 构造函数
        * 
        * 
        ***********************************************************************************************/
        private ProductParameter()
        {
            apdParm = new ApdParam();
            xmlDb = AwDbFactory.CreateDBEngine();
            ProductParameter prdParameter = this;
            prdBatInfo = new ProductBatInfo();
            xmlDb.GetProductParameters(ref prdParameter);
            xmlDb.GetProductBatInfo(ref prdBatInfo);
            xmlDb.GetAPDParameters(ref apdParm);
        }

        public static ProductParameter GetInstance()
        {
            if (instance == null)
                instance = new ProductParameter();
            return instance;
        }

         /**********************************************************************************************
         * discription: 更新database
         * 
         * 
         ***********************************************************************************************/
        public int UpdateDbInfo()
        {
            return xmlDb.UpdateProductParameter(this);            
        }

        public int UpdateProductBatInfo()
        {
            return xmlDb.UpdateProductBatInfo(prdBatInfo);
        }

        public void SetFrontReleaseColloid(int front)
        {
            releaseColloid.front = front;
        }

        public void SetRightReleaseColloid(int right)
        {
            releaseColloid.right = right;
        }

        public void SetLeftReleaseColloid(int left)
        {
            releaseColloid.left = left;
        }

        public void UpdateApdParam(ApdParam _apdParam)
        {
            this.apdParm = _apdParam;
            xmlDb.UpdateAPDParameters(apdParm);
        }
    }   
}
