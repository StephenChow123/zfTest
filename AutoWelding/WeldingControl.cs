using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AutoWelding.control;

namespace AutoWelding
{
    public class AngleMove
    {
        public double angleA;
        public double angleB;
    }

    public class StepMove
    {
        public double xStep;
        public double yStep;

        public double leftStep;
        public double rightStep;
        public double frontStep;
        public double zStep;
    }

    public struct TraceInfo 
    {
        public int Id;
        public string name;
    };

    public class RetData
    {
        public int axisNum;
        public int cardNum;
        public List<TraceInfo> tracePool;
        public string str;
        public string errMsg;
        public Coordinate coordinate;

        public RetData()
        {
            tracePool = new List<TraceInfo>();
        }
    }

    public enum CmdType
    {
        CMD_GET_VERSION = 99,			// 获取软件版本
        CMD_INIT_MPC2810 = 100,          //初始化MPC2810卡
        CMD_GET_TRACE_METHOD,            //获取Trace 信息
        CMD_WELDING_SETUP,             	 //配置Trace 参数
        CMD_GET_COORDINATE,               //获取当前坐标
        CMD_SET_PRODUCT_PARAMETER,        //设置工艺参数
        CMD_GET_VPP,                     //获取VPP Value
        CMD_GET_OUTPUT_IO_STATE,           //获取Out put io 状态
        CMD_SET_OUTPUT_IO,
        CMD_GET_INPUT_IO_STATE,            //获取out put io state
        CMD_BREAK_TO_WORK_PLATPOINT,       //由于出错而回到工作平台点
        CMD_EXIT,

        CMD_VPP_START = 120,                   //开始寻找VPP
        CMD_OPERATE_FIX = 121,                 //夹住壳体
        CMD_OPERATE_CLAMP = 122,                   //夹住芯片
        CMD_INIT_PLATFORM = 123,			//初始化平始
        CMD_MOVETO_WORKPOINT = 124,			//移动到校准点

        CMD_ADJUST_COLLOID = 130,           //对针模式
        CMD_END_ADJUST_COLLOID = 131,           //结束对针模式

        CMD_ADJUST_VAPD_LOW = 140,              //APD 低电压校准
        CMD_ADJUST_VAPD_HIGH = 141,             //APD 高电压校准
        CMD_ADJUST_VAPD_IR_1 = 142,             // Ir 反向电流第一次校准
        CMD_ADJUST_VAPD_IR_2 = 143,             // Ir 反向电流第二次校准
        CMD_ADJUST_VBR_CURRENT = 144,           // VBR 判定电流
        CMD_ADJUST_FIND_VBR = 145,              // 查找VBR
        CMD_ADJUST_VAL_OUTPUT = 146,            // 查找VBR
        CMD_APD_PARM = 147,

        CMD_OPERATE_REPLACE_COLLOID = 148,       //换胶
        CMD_OPERATE_END_REPLACE_COLLOID = 149,       //换胶




        //Test command
        CMD_OPERATE_ANGLEMOVE = 500,                //角度移动
        CMD_OPERATE_UPMOVE = 501,                //顶部电机移动
        CMD_OPERATE_BOTTOMMOVE = 502,                //底部电机移动
        CMD_OPERATE_UPMOVE_REGULATORPOINT = 503,    //移动到顶部校准点
        CMD_OPERATE_DOWNMOVE_REGULATORPOINT = 504,    //移动到底部校准点
        CMD_OPERATE_RESETPLATFORMPOINT = 505,	        //重置工作平台点
        CMD_OPERATE_END_ADJUST_COLLOID = 506,	        //结束对针
        CMD_ADJUST_BAKE_COLLOID = 507,  				//烤胶校准
        CMD_ADJUST_RELEASE_COLLOID = 508,				//出胶校准

        CMD_OPERATE_START_AD = 509,					//打开APD
        CMD_OPERAE_STOP_AD = 510,                    //关闭APD  
        CMD_ADJUST_RELEASE_COLLOID2 = 511,          //出胶校准， 以参数设置中的参数调试出胶，而且不会伸胶针
    }

    public class WeldingControl
    {
        [DllImport("control.dll")]
        private static extern int InitControl(IntPtr data);
        [DllImport("control.dll")]
        private static extern int UICommands(int command, IntPtr cmdData);
        [DllImport("control.dll")]        
        private static extern int ReleaseCommunicationData(IntPtr data);

        private static WeldingControl instance;

        public static WeldingControl GetInstance()
        {
            if (instance == null)
                instance = new WeldingControl();
            return instance;
        }

        private WeldingControl()
        {
        
        }

        public int WeldingInit(ref RetData retData)
        {
            IntPtr surData = Marshal.AllocHGlobal(255);
            int ret = 0;

            ret = InitControl(surData);

            if ( ret > 0)
            {
                DealWithInitMPCData(ref retData, ref surData, ret);
            }

           Marshal.FreeHGlobal(surData);
           return ret;
        }

        /*********************************************************************************************
        *function: 发送命令
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public int SendCommands(CmdType command, ref RetData retData,object sendData)
        {
            IntPtr surData = Marshal.AllocHGlobal(1024);
            int ret = 0;
            try
            {
                switch (command)
                {
                    case CmdType.CMD_WELDING_SETUP:

                        WeldingSetupData(ref surData, (TraceSetup)((List<object>)sendData)[0], (ProductParameter)((List<object>)sendData)[1]);
                        break;

                    // Test command
                    case CmdType.CMD_OPERATE_ANGLEMOVE:
                        {
                            double[] angleArray = new double[2];
                            angleArray[0] = ((AngleMove)sendData).angleA;
                            angleArray[1] = ((AngleMove)sendData).angleB;
                            Marshal.Copy(angleArray, 0, surData, 2);
                        }
                        break;
                    case CmdType.CMD_OPERATE_UPMOVE:
                        {
                            double[] upArray = new double[2];
                            upArray[0] = ((StepMove)sendData).xStep;
                            upArray[1] = ((StepMove)sendData).yStep;
                            Marshal.Copy(upArray, 0, surData, 2);
                        }
                        break;
                    case CmdType.CMD_OPERATE_BOTTOMMOVE:
                        {
                            double[] downArray = new double[4];
                            downArray[0] = ((StepMove)sendData).leftStep;
                            downArray[1] = ((StepMove)sendData).rightStep;
                            downArray[2] = ((StepMove)sendData).frontStep;
                            downArray[3] = ((StepMove)sendData).zStep;
                            Marshal.Copy(downArray, 0, surData, 4);
                        }
                        break;
                    case CmdType.CMD_SET_PRODUCT_PARAMETER:
                        {
                            int productParamOffset = 0;
                            ProductParameter productParameter = (ProductParameter)sendData;

                            float[] outputvalue = new float[1];
                            outputvalue[0] = productParameter.MinimumVpp;
                            Marshal.Copy(outputvalue, 0, surData, 1);

                            UInt32 startPos = (UInt32)surData + sizeof(float);

                            
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.DelayMoveOutReleaseColloidDIo);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.DelayReleaseColloid);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.ReleaseColloid.front);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), productParameter.ReleaseColloid.left);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), productParameter.ReleaseColloid.right);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.DelayMoveBackReleaseColloidDIo);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.BakeColloid);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.AdAdjust);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), Convert.ToInt32(productParameter.ReleaseColloidBeforeCoupling));
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), Convert.ToInt32(productParameter.FindCenter));

                            outputvalue[0] = productParameter.LxLyDownVoltage;
                            Marshal.Copy(outputvalue, 0, (IntPtr)(startPos + (productParamOffset) * sizeof(int)), 1);
                            startPos += sizeof(float);

                            outputvalue[0] = productParameter.LzDownVoltage;
                            Marshal.Copy(outputvalue, 0, (IntPtr)(startPos + (productParamOffset) * sizeof(int)), 1);
                            startPos += sizeof(float);

                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.LxLyMin);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.LxLyMax);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.LzMin);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.LzMax);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int) ), productParameter.ZMin);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), productParameter.ZMax);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), (int)productParameter.VbrConfig);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), productParameter.VbrMax);
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), productParameter.VbrMin);

                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), Convert.ToInt32(productParameter.FNSR));
                            Marshal.WriteInt32((IntPtr)(startPos + (productParamOffset++) * sizeof(int)), Convert.ToInt32(productParameter.TOCAN));
                        }
                        break;
                    case CmdType.CMD_SET_OUTPUT_IO:
                        {
                            int outPutIoOffset = 0;
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), retData.axisNum);
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), retData.cardNum);
                        }
                        break;
                    case CmdType.CMD_OPERATE_RESETPLATFORMPOINT:
                        {
                            TraceSetup traceInfo = (TraceSetup)sendData;
                            double[] platformPoint = new double[10];
                            platformPoint[0] = traceInfo.PlatformPoint.stepX;
                            platformPoint[1] = traceInfo.PlatformPoint.stepY;
                            platformPoint[2] = traceInfo.PlatformPoint.stepLeft;
                            platformPoint[3] = traceInfo.PlatformPoint.stepRight;
                            platformPoint[4] = traceInfo.PlatformPoint.stepFront;
                            platformPoint[5] = traceInfo.PlatformPoint.stepZ;
                            platformPoint[6] = traceInfo.ZZeroPoint.stepLeft;
                            platformPoint[7] = traceInfo.ZZeroPoint.stepRight;
                            platformPoint[8] = traceInfo.ZZeroPoint.stepFront;
                            platformPoint[9] = traceInfo.ZZeroPoint.stepZ;
                            Marshal.Copy(platformPoint, 0, surData, 10);
                        }
                        break;
                    case CmdType.CMD_ADJUST_BAKE_COLLOID:
                        {
                            int outPutIoOffset = 0;
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), retData.axisNum);
                        }
                        break;
                    case CmdType.CMD_ADJUST_RELEASE_COLLOID:
                    case CmdType.CMD_ADJUST_RELEASE_COLLOID2:
                        {
                            int outPutIoOffset = 0;
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), ((int)retData.coordinate.stepFront));
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), ((int)retData.coordinate.stepLeft));
                            Marshal.WriteInt32((IntPtr)((UInt32)surData + (outPutIoOffset++) * sizeof(int)), ((int)retData.coordinate.stepRight));                        
                        }
                        break;

                    case CmdType.CMD_ADJUST_VAL_OUTPUT:
                        {
                            float[] outputvalue = new float[1];
                            outputvalue[0] = (float)retData.coordinate.stepFront;
                            Marshal.Copy(outputvalue, 0, surData, 1);
                        }
                        break;

                    case CmdType.CMD_ADJUST_VBR_CURRENT:
                        {
                            float[] current = new float[1];
                            current[0] = (float)retData.coordinate.stepFront;
                            Marshal.Copy(current, 0, surData, 1);
                        }
                        break;
                    case CmdType.CMD_APD_PARM:
                        {
                            ProductParameter pm = ProductParameter.GetInstance();
                            float[] apdParams = new float[11];
                            apdParams[0] = pm.APDParam.Ir_AD_Zero_Value;
                            apdParams[1] = pm.APDParam.Ir_Calib_Value;
                            apdParams[2] = pm.APDParam.Ir_Calib_AD_Value;
                            apdParams[3] = pm.APDParam.Vapd_Max_Value;
                            apdParams[4] = pm.APDParam.Vapd_Max_DA_Value;
                            apdParams[5] = pm.APDParam.Vapd_Min_Value;
                            apdParams[6] = pm.APDParam.Vapd_Min_DA_Value;
                            apdParams[7] = pm.APDParam.Vapd_Upper_Limit;
                            apdParams[8] = pm.APDParam.Vapd_Lower_Limit;
                            apdParams[9] = pm.APDParam.Vbr_Judge;
                            apdParams[10] = (float)pm.APDParam.VbrType;
                            Marshal.Copy(apdParams, 0, surData, 11);
                        }
                        break;
                    default:                        
                        break;
                }

                ret = UICommands(Convert.ToInt32(command), surData);
               
                switch (command)
                {
                    case CmdType.CMD_INIT_MPC2810:
                        DealWithInitMPCData(ref retData, ref surData, ret);
                        break;
                    case CmdType.CMD_GET_TRACE_METHOD:
                        DealWithTraceMethodsData(ref retData, ref surData, ret);
                        break;
                    case CmdType.CMD_WELDING_SETUP:
                        if (ret < 0)
                        {
                            retData.errMsg = "耦合配置错误,ID=" + ret.ToString();
                        }
                        break;
                    case CmdType.CMD_GET_COORDINATE:
                        DealWithInitCoordinateData(ref retData, ref surData);
                        break;
                    case CmdType.CMD_GET_VERSION:
                        retData.str = Marshal.PtrToStringAnsi(surData);
                        break;
                    case CmdType.CMD_SET_PRODUCT_PARAMETER:
                        if (ret < 0)
                        {
                            retData.errMsg = "工艺参数配置错误,ID=" + ret.ToString();
                        }
                        break;
                    case CmdType.CMD_GET_VPP:
                        float[] vpp = new float[1];
                        Marshal.Copy(surData, vpp, 0, 1);
                        retData.coordinate.stepFront = vpp[0];
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ee)
            {
                string error = ee.Message;
            }

            Marshal.FreeHGlobal(surData);
            return ret;
        }

        /*********************************************************************************************
        *function: 处理MPC2810命令
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        private void DealWithInitCoordinateData(ref RetData retData, ref IntPtr surData)
        {
            double[] coordinateArray = new double[6];
            Marshal.Copy(surData, coordinateArray, 0, 6);
            retData.coordinate = new Coordinate();
            retData.coordinate.stepX = coordinateArray[0];
            retData.coordinate.stepY = coordinateArray[1];
            retData.coordinate.stepLeft = coordinateArray[2];
            retData.coordinate.stepRight = coordinateArray[3];
            retData.coordinate.stepFront = coordinateArray[4];
            retData.coordinate.stepZ = coordinateArray[5];
        }

        /*********************************************************************************************
        *function: 处理MPC2810命令
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        private void DealWithInitMPCData(ref RetData retData, ref IntPtr surData, int retValue)
        {
            if (retValue < 0)
            {
                retData.errMsg = Marshal.PtrToStringAnsi(surData);
            }
            else
            {
                retData.axisNum = Marshal.ReadInt32(surData);
                retData.cardNum = Marshal.ReadInt32((IntPtr)((UInt32)surData + 4));
            }
        }

        /*********************************************************************************************
        *function: 处理trace methods
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        private void DealWithTraceMethodsData(ref RetData retData, ref IntPtr surData, int retValue)
        {
            if (retValue <= 0)
            {
                retData.errMsg = Marshal.PtrToStringAnsi(surData);
            }
            else
            {
                int itemLen = 0;
                IntPtr intPtr = surData;

                for (int i = 0; i < retValue; i++)
                {
                    // get item length
                    itemLen = Marshal.ReadInt32(intPtr);
                    intPtr = (IntPtr)((UInt32)intPtr + sizeof(int));

                    TraceInfo traceItem = new TraceInfo();
                    
                    // get id
                    traceItem.Id = Marshal.ReadInt32(intPtr);
                    intPtr = (IntPtr)((UInt32)intPtr + sizeof(int));

                    // get name
                    traceItem.name = Marshal.PtrToStringAnsi(intPtr, itemLen - sizeof(int));
                    intPtr = intPtr = (IntPtr)((UInt32)intPtr + itemLen - sizeof(int));

                    // append new trace info
                    retData.tracePool.Add(traceItem);


                }
            }
        }

        /*********************************************************************************************
        *function:Welding setup data
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        private void WeldingSetupData( ref IntPtr weldingData,TraceSetup traceInfo, ProductParameter productParameter)
        { 
            // trace method
            int offset = 0;
            int activeMethodId = -1;
            foreach(S_TraceMethod method in traceInfo.TraceMehodList)
            {
                if(method.activeMethod)
                {
                    activeMethodId = method.id;
                }
            }
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), activeMethodId);

            // XF_1 to XF_3 motors and z motor
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.BottomMotor.motorLeft);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.BottomMotor.motorFront);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.BottomMotor.motorRight);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.BottomMotor.motorZ);

            // XY_X to XY_Y motors
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.UpMotor.motorX);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (offset++) * sizeof(int)), traceInfo.MotorsInfo.UpMotor.motorY);

            // Platform Point
            double[] platformPoint = new double[10];
            platformPoint[0] = traceInfo.PlatformPoint.stepX;
            platformPoint[1] = traceInfo.PlatformPoint.stepY;
            platformPoint[2] = traceInfo.PlatformPoint.stepLeft;
            platformPoint[3] = traceInfo.PlatformPoint.stepRight;
            platformPoint[4] = traceInfo.PlatformPoint.stepFront;
            platformPoint[5] = traceInfo.PlatformPoint.stepZ;
            platformPoint[6] = traceInfo.ZZeroPoint.stepLeft;
            platformPoint[7] = traceInfo.ZZeroPoint.stepRight;
            platformPoint[8] = traceInfo.ZZeroPoint.stepFront;
            platformPoint[9] = traceInfo.ZZeroPoint.stepZ;

            Marshal.Copy(platformPoint,0, (IntPtr)((UInt32)weldingData + offset * sizeof(int)), 10);

            offset = offset * sizeof(int) + 10 * sizeof(double);

            // 限位
            platformPoint[0] = traceInfo.MaxPosition.stepX;
            platformPoint[1] = traceInfo.MaxPosition.stepY;
            platformPoint[2] = traceInfo.MaxPosition.stepLeft;
            platformPoint[3] = traceInfo.MaxPosition.stepRight;
            platformPoint[4] = traceInfo.MaxPosition.stepFront;
            platformPoint[5] = traceInfo.MaxPosition.stepMinX;
            platformPoint[6] = traceInfo.MaxPosition.stepMinY;
            platformPoint[7] = traceInfo.MaxPosition.stepZ;
            Marshal.Copy(platformPoint, 0, (IntPtr)((UInt32)weldingData + offset), 8);

            offset += 8 * sizeof(double);

            //product parameter

            float[] outputvalue = new float[1];
            outputvalue[0] = productParameter.MinimumVpp;
            Marshal.Copy(outputvalue, 0, (IntPtr)((UInt32)weldingData + offset), 1);

            offset += sizeof(float);

            int productParamOffset = 0;            
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.DelayMoveOutReleaseColloidDIo);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.DelayReleaseColloid);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.ReleaseColloid.front);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.ReleaseColloid.left);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.ReleaseColloid.right);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.DelayMoveBackReleaseColloidDIo);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.BakeColloid);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.AdAdjust);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), Convert.ToInt32(productParameter.ReleaseColloidBeforeCoupling));
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), Convert.ToInt32(productParameter.FindCenter));

            outputvalue[0] = productParameter.LxLyDownVoltage;
            Marshal.Copy(outputvalue, 0, (IntPtr)((UInt32)weldingData +(productParamOffset) * sizeof(int) + offset), 1);
            offset += sizeof(float);

            outputvalue[0] = productParameter.LzDownVoltage;
            Marshal.Copy(outputvalue, 0, (IntPtr)((UInt32)weldingData + (productParamOffset) * sizeof(int) + offset), 1);
            offset += sizeof(float); 

            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.LxLyMin);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.LxLyMax);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.LzMin);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.LzMax);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.ZMin);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.ZMax);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), (int)productParameter.VbrConfig);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.VbrMax);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), productParameter.VbrMin);
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), Convert.ToInt32(productParameter.FNSR));
            Marshal.WriteInt32((IntPtr)((UInt32)weldingData + (productParamOffset++) * sizeof(int) + offset), Convert.ToInt32(productParameter.TOCAN));

            offset += productParamOffset * sizeof(int);

            //
            ProductParameter pm = ProductParameter.GetInstance();
            float[] apdParams = new float[11];
            apdParams[0] = pm.APDParam.Ir_AD_Zero_Value;
            apdParams[1] = pm.APDParam.Ir_Calib_Value;
            apdParams[2] = pm.APDParam.Ir_Calib_AD_Value;
            apdParams[3] = pm.APDParam.Vapd_Max_Value;
            apdParams[4] = pm.APDParam.Vapd_Max_DA_Value;
            apdParams[5] = pm.APDParam.Vapd_Min_Value;
            apdParams[6] = pm.APDParam.Vapd_Min_DA_Value;
            apdParams[7] = pm.APDParam.Vapd_Upper_Limit;
            apdParams[8] = pm.APDParam.Vapd_Lower_Limit;
            apdParams[9] = pm.APDParam.Vbr_Judge;
            apdParams[10] = pm.APDParam.VbrType;
            Marshal.Copy(apdParams, 0, (IntPtr)((UInt32)weldingData + offset), 11);

            offset += sizeof(float) * 11;
        }

        /*********************************************************************************************
        *function:Free communication data
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public void FreeControlData(IntPtr data)
        {
            ReleaseCommunicationData(data);
        }

    }
}
