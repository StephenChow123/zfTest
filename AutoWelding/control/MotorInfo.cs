using System;
using System.Collections.Generic;
using System.Text;

namespace AutoWelding.control
{
    public struct S_BottomMotor
    {
        public int motorLeft;
        public int motorRight;
        public int motorFront;
        public int motorZ;      // 三轴时使用
    }

    public struct S_UpMotor
    {
        public int motorX;
        public int motorY;
    }

    public struct Coordinate
    {
        public double stepX;
        public double stepY;
        public double stepLeft;
        public double stepRight;
        public double stepFront;
        public double stepZ;
    }

    public struct MaxMinCoordinate
    {
        public double stepX;
        public double stepY;
        public double stepMinX;
        public double stepMinY;

        public double stepLeft;
        public double stepRight;
        public double stepFront;
        public double stepZ;
    
    }

    public class MotorInfo
    {
        private int axisNum;
        private int cardNum;
        private S_BottomMotor bottomMotor;
        private S_UpMotor upMotor;


        // 设置Axis Num数
        public int AxisNum
        {
            get { return axisNum; }
            set { axisNum = value; }
        }

        // 设置Card Num
        public int CardNum
        {
            get { return cardNum; }
            set { cardNum = value; }
        }

        // 获取/设置bottom motors
        public S_BottomMotor BottomMotor
        {
            get { return bottomMotor; }
            set { bottomMotor = value; }
        }

        // 获取/设置上层两个motors
        public S_UpMotor UpMotor
        {
            get { return upMotor; }
            set { upMotor = value; }
        }

        /**********************************************************************************************
         * discription: 初始化，默认为Mysql
         * 
         * 
         ***********************************************************************************************/
        public MotorInfo()
        {
            axisNum = 0;
            cardNum = 0;
        }
         
    }
}
