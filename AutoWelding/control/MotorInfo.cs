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
        public int motorZ;      // ����ʱʹ��
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


        // ����Axis Num��
        public int AxisNum
        {
            get { return axisNum; }
            set { axisNum = value; }
        }

        // ����Card Num
        public int CardNum
        {
            get { return cardNum; }
            set { cardNum = value; }
        }

        // ��ȡ/����bottom motors
        public S_BottomMotor BottomMotor
        {
            get { return bottomMotor; }
            set { bottomMotor = value; }
        }

        // ��ȡ/�����ϲ�����motors
        public S_UpMotor UpMotor
        {
            get { return upMotor; }
            set { upMotor = value; }
        }

        /**********************************************************************************************
         * discription: ��ʼ����Ĭ��ΪMysql
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
