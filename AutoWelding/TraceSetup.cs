using System;
using System.Collections.Generic;
using System.Text;
using AutoWelding.control;
using AutoWelding.awdatabase;

namespace AutoWelding
{
     public struct S_TraceMethod
    {
         public int id;
         public string name;
         public bool activeMethod;
    }

    public class TraceSetup
    {
        private List<S_TraceMethod> traceMethodList;
        private S_TraceMethod lastActiveMethod;
        private MotorInfo motorInfo;
        private AwDbInterface xmlDb;
        private Coordinate platformPoint;
        private Coordinate zZeroPoint;
        private MaxMinCoordinate maxPosition; //限位
        private bool isUpdated;

        const double maxtBottomSingleStep = 5000;  //底部电机运动时，任两个电机移动差值不能大于该值

        //获取Trace method
        public List<S_TraceMethod> TraceMehodList
        {
            get { return traceMethodList; }
        }

        //获取MotorInfo
        public MotorInfo MotorsInfo
        {
            get { return motorInfo; }
            set { motorInfo = value; }
        }

        //是否需要更新参数到控制模块
        public bool IsUpdated
        {
            get { return isUpdated; }
            set { isUpdated = value; }
        }

        //Active trace
        public S_TraceMethod LastActiveMethod
        {
            get { return lastActiveMethod; }
       //     set { lastActiveMethod = value; }
        }

        public Coordinate PlatformPoint
        {
            get { return platformPoint; }
            set { platformPoint = value; }
        }

        public Coordinate ZZeroPoint
        {
            get { return zZeroPoint; }
            set { zZeroPoint = value; }
        }

        public MaxMinCoordinate MaxPosition
        {
            get { return maxPosition; }
            set { maxPosition = value; }
        }

        public double MaxtBottomSingleStep
        {
            get { return maxtBottomSingleStep; }
        }


        /**********************************************************************************************
         * discription: 构造函数
         * 
         * 
         ***********************************************************************************************/
        public TraceSetup()
        {
            xmlDb = AwDbFactory.CreateDBEngine();
            motorInfo = new MotorInfo();
            traceMethodList = new List<S_TraceMethod>();
            lastActiveMethod = new S_TraceMethod();
            platformPoint = new Coordinate();
            maxPosition = new MaxMinCoordinate();
            zZeroPoint = new Coordinate();

            xmlDb.GetMotorInfo(ref motorInfo);
            xmlDb.GetActiveTraceMethod(ref lastActiveMethod);
            xmlDb.GetPlatformPoint(ref platformPoint);
            xmlDb.GetMaxPosition(ref maxPosition);
            xmlDb.GetZZeroPoint(ref zZeroPoint);
            isUpdated = false;
            
        }

        /**********************************************************************************************
         * discription: 更新database
         * 
         * 
         ***********************************************************************************************/
        public int UpdateDbInfo()
        {
            int ret = 0;
            if ((ret = xmlDb.UpdateMotorInfo(motorInfo)) < 0)
            {
                return -1;
            }
            if ((ret = xmlDb.UpdatePlatformPoint(platformPoint)) < 0)
                return -2;

            if ((ret = xmlDb.UpdateMaxPosition(maxPosition)) < 0)
                return -3;

            if ((ret = xmlDb.UpdateZZeroPoint(zZeroPoint)) < 0)
                return -4;

            foreach (S_TraceMethod method in traceMethodList)
            {
                if (method.activeMethod)
                {
                    if ((ret = xmlDb.UpdateActiveTraceMethod(method)) < 0)
                        return ret;
                }
            }

            return 0;
        }

        /**********************************************************************************************
         * discription: 添加trace method
         * 
         * 
         ***********************************************************************************************/
        public int AddTraceMethod(ref S_TraceMethod method)
        {
            if (method.id == lastActiveMethod.id)
            {
                method.activeMethod = true;
            }
            else
            {
                method.activeMethod = false;
            }

            traceMethodList.Add(method);
            return 0;
        }

        /**********************************************************************************************
         * discription: 设置active trace method
         * 
         * 
         ***********************************************************************************************/
        public int SetActiveTraceMethod(int traceId)
        {
            int ret = -1;
            S_TraceMethod item;

            for (int i = 0; i < traceMethodList.Count; i++)
            {
                item = traceMethodList[i];

                if (item.id == traceId)
                {
                    ret = 0;
                    item.activeMethod = true;
                    traceMethodList[i] = item;
                }
                else
                {
                    item.activeMethod = false;
                    traceMethodList[i] = item;
                }
            }

            return ret;
        }

        public int GetActiveTraceId()
        {
            foreach (S_TraceMethod item in traceMethodList)
            {
                if (item.activeMethod)
                    return item.id;
            }

            return 0;
        }

        /**********************************************************************************************
         * discription: 检测 电机卡轴配置
         * 
         * 
         ***********************************************************************************************/
        public int CheckMotorsAxis()
        {

            if (GetActiveTraceId() != 2)
            {
                if (motorInfo.BottomMotor.motorLeft <= 0 || motorInfo.BottomMotor.motorRight <= 0 ||
                    motorInfo.BottomMotor.motorFront <= 0 || motorInfo.UpMotor.motorX <= 0 ||
                    motorInfo.UpMotor.motorY <= 0)
                {
                    return -1;
                }

                if (motorInfo.UpMotor.motorX == motorInfo.UpMotor.motorY ||
                   motorInfo.UpMotor.motorX == motorInfo.BottomMotor.motorFront ||
                   motorInfo.UpMotor.motorX == motorInfo.BottomMotor.motorLeft ||
                   motorInfo.UpMotor.motorX == motorInfo.BottomMotor.motorRight ||
                   motorInfo.UpMotor.motorX == motorInfo.BottomMotor.motorFront)
                {
                    //MessageBox.Show("顶部电机 X 与其它电机设置冲突!");
                    return -2;
                }

                if (motorInfo.UpMotor.motorY == motorInfo.UpMotor.motorX ||
                    motorInfo.UpMotor.motorY == motorInfo.BottomMotor.motorFront ||
                    motorInfo.UpMotor.motorY == motorInfo.BottomMotor.motorLeft ||
                    motorInfo.UpMotor.motorY == motorInfo.BottomMotor.motorRight ||
                    motorInfo.UpMotor.motorY == motorInfo.BottomMotor.motorFront)
                {
                    // MessageBox.Show("顶部电机 Y 与其它电机设置冲突!");
                    return -3;
                }

                if (motorInfo.BottomMotor.motorFront == motorInfo.UpMotor.motorX ||
                    motorInfo.BottomMotor.motorFront == motorInfo.UpMotor.motorY ||
                    motorInfo.BottomMotor.motorFront == motorInfo.BottomMotor.motorLeft ||
                    motorInfo.BottomMotor.motorFront == motorInfo.BottomMotor.motorRight)
                {
                    //MessageBox.Show("底部前方电机与其它电机设置冲突!");
                    return -4;
                }

                if (motorInfo.BottomMotor.motorLeft == motorInfo.UpMotor.motorX ||
                    motorInfo.BottomMotor.motorLeft == motorInfo.UpMotor.motorY ||
                    motorInfo.BottomMotor.motorLeft == motorInfo.BottomMotor.motorFront ||
                    motorInfo.BottomMotor.motorLeft == motorInfo.BottomMotor.motorRight)
                {
                    // MessageBox.Show("底部左边电机与其它电机设置冲突!");
                    return -5;
                }

                if (motorInfo.BottomMotor.motorRight == motorInfo.UpMotor.motorX ||
                    motorInfo.BottomMotor.motorRight == motorInfo.UpMotor.motorY ||
                    motorInfo.BottomMotor.motorRight == motorInfo.BottomMotor.motorFront ||
                    motorInfo.BottomMotor.motorRight == motorInfo.BottomMotor.motorLeft)
                {
                    //MessageBox.Show("底部右边电机与其它电机设置冲突!");
                    return -6;
                }
            }
            else
            {

                if (motorInfo.BottomMotor.motorZ <= 0 || motorInfo.UpMotor.motorX <= 0 ||
                    motorInfo.UpMotor.motorY <= 0)
                {
                    return -1;
                }

                if (motorInfo.UpMotor.motorX == motorInfo.UpMotor.motorY ||
                   motorInfo.UpMotor.motorX == motorInfo.BottomMotor.motorZ)
                {
                    //MessageBox.Show("顶部电机 X 与其它电机设置冲突!");
                    return -2;
                }

                if (motorInfo.UpMotor.motorY == motorInfo.UpMotor.motorX ||
                    motorInfo.UpMotor.motorY == motorInfo.BottomMotor.motorZ)
                {
                    // MessageBox.Show("顶部电机 Y 与其它电机设置冲突!");
                    return -3;
                }               
            }

            return 0;
        }

        /**********************************************************************************************
         * discription: 检测 限位配置
         * 
         * 
         ***********************************************************************************************/
        public int CheckMaxPosition()
        {
            int traceIndex = 0;
            foreach(S_TraceMethod item in traceMethodList)
            {
                if(item.activeMethod)
                {
                    traceIndex = traceMethodList.IndexOf(item) + 1;
                    break;
                }
            }

            if (maxPosition.stepX <= 0 || maxPosition.stepY <= 0)
            {
               // MessageBox.Show("顶部电机限位错误，请检查!");
                return -1;
            }


            if (traceIndex == 1)
            {
                if (maxPosition.stepLeft <= 0 || maxPosition.stepRight <= 0 || maxPosition.stepFront <= 0)
                {
                    // MessageBox.Show("底部电机限位参数错误，请更正输入参数");
                    return -2;
                }


                if (Math.Abs(maxPosition.stepLeft - maxPosition.stepRight) > MaxtBottomSingleStep ||
                    Math.Abs(maxPosition.stepLeft - maxPosition.stepFront) > MaxtBottomSingleStep ||
                    Math.Abs(maxPosition.stepRight - maxPosition.stepFront) > MaxtBottomSingleStep)
                {
                    //MessageBox.Show("限位 Step 差值不能大于" + MaxtBottomSingleStep.ToString());
                    return -3;
                }
            }
            else
            {
                if (maxPosition.stepZ <= 0 )
                {
                    // MessageBox.Show("底部电机限位参数错误，请更正输入参数");
                    return -2;
                }               
            }

            return 0;
        }
    }
}
