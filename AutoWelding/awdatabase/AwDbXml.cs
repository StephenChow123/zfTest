using System;
using System.Collections.Generic;
using System.Text;
using AutoWelding.control;
using System.Xml;
using AutoWelding.system;

namespace AutoWelding.awdatabase
{
    public class AwXmlDocument : XmlDocument
    {
        private bool isLoaded = false;

        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }
    }

    class AwDbXml: AwDbInterface
    {
        private string dbFileName = "awdata.xml";
        private AwXmlDocument xmlDoc = null;
        private string lastError;

        public string LastError
        {
            get { return lastError; }
        }


        /**********************************************************************************************
         * discription: 构造函数
         * 
         * 
         ***********************************************************************************************/
        public AwDbXml()
        {
            xmlDoc = new AwXmlDocument();
            dbFileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + dbFileName;
            try
            {
                xmlDoc.Load(dbFileName);
            }
            catch
            {
                CreateDbXml();
                try
                {
                    xmlDoc.Load(dbFileName);
                }
                catch (Exception ee)
                {
                    lastError = "AwDbXml:" + ee.Message;
                    return;
                }
            }

            xmlDoc.IsLoaded = true;
        }

        /**********************************************************************************************
         * discription: 存储 motor 配置文件
         * 
         * 
         ***********************************************************************************************/
        public int UpdateMotorInfo(MotorInfo motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("MotorInfo");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "MotorInfo", "");
                    //AxisNum
                    XmlElement xmlElement = xmlDoc.CreateElement("", "AxisNum", "");
                    XmlText xmlText = xmlDoc.CreateTextNode(motorInfo.AxisNum.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //CardNum
                    xmlElement = xmlDoc.CreateElement("", "CardNum", "");
                    xmlText = xmlDoc.CreateTextNode(motorInfo.CardNum.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //Bottom motors
                    xmlElement = xmlDoc.CreateElement("", "BottomMotor", "");
                    xmlElement.SetAttribute("left", motorInfo.BottomMotor.motorLeft.ToString());
                    xmlElement.SetAttribute("right", motorInfo.BottomMotor.motorRight.ToString());
                    xmlElement.SetAttribute("front", motorInfo.BottomMotor.motorFront.ToString());
                    xmlElement.SetAttribute("z", motorInfo.BottomMotor.motorZ.ToString());
                    cgElement.AppendChild(xmlElement);

                    //Up motors
                    xmlElement = xmlDoc.CreateElement("", "UpMotor", "");
                    xmlElement.SetAttribute("left", motorInfo.UpMotor.motorX.ToString());
                    xmlElement.SetAttribute("right", motorInfo.UpMotor.motorY.ToString());
                    cgElement.AppendChild(xmlElement); 

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "AxisNum":
                                    xn.ChildNodes[0].Value = motorInfo.AxisNum.ToString();
                                    break;

                                case "CardNum":
                                    xn.ChildNodes[0].Value = motorInfo.CardNum.ToString();
                                    break;

                                case "BottomMotor":
                                    xn.Attributes["left"].Value = motorInfo.BottomMotor.motorLeft.ToString();
                                    xn.Attributes["right"].Value = motorInfo.BottomMotor.motorRight.ToString();
                                    xn.Attributes["front"].Value = motorInfo.BottomMotor.motorFront.ToString();
                                    xn.Attributes["z"].Value = motorInfo.BottomMotor.motorZ.ToString();
                                    break;

                                case "UpMotor":
                                    xn.Attributes["left"].Value = motorInfo.UpMotor.motorX.ToString();
                                    xn.Attributes["right"].Value = motorInfo.UpMotor.motorY.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
        * discription: 存储 platform point 配置信息
        * 
        * 
        ***********************************************************************************************/
        public int UpdatePlatformPoint(Coordinate motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("PlatformPoint");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "PlatformPoint", "");
                    //Bottom motors
                    XmlElement xmlElement = xmlDoc.CreateElement("", "BottomMotor", "");
                    xmlElement.SetAttribute("left", motorInfo.stepLeft.ToString());
                    xmlElement.SetAttribute("right", motorInfo.stepRight.ToString());
                    xmlElement.SetAttribute("front", motorInfo.stepFront.ToString());
                    xmlElement.SetAttribute("z", motorInfo.stepZ.ToString());
                    cgElement.AppendChild(xmlElement);

                    //Up motors
                    xmlElement = xmlDoc.CreateElement("", "UpMotor", "");
                    xmlElement.SetAttribute("left", motorInfo.stepX.ToString());
                    xmlElement.SetAttribute("right", motorInfo.stepY.ToString());
                    cgElement.AppendChild(xmlElement);

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {                             
                                case "BottomMotor":
                                    xn.Attributes["left"].Value = motorInfo.stepLeft.ToString();
                                    xn.Attributes["right"].Value = motorInfo.stepRight.ToString();
                                    xn.Attributes["front"].Value = motorInfo.stepFront.ToString();
                                    xn.Attributes["z"].Value = motorInfo.stepZ.ToString();
                                    break;

                                case "UpMotor":
                                    xn.Attributes["left"].Value = motorInfo.stepX.ToString();
                                    xn.Attributes["right"].Value = motorInfo.stepY.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
         * discription: 存储 platform point 配置信息
         * 
         * 
         ***********************************************************************************************/
        public int UpdateZZeroPoint(Coordinate motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ZZeroPoint");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "ZZeroPoint", "");
                    //Bottom motors
                    XmlElement xmlElement = xmlDoc.CreateElement("", "BottomMotor", "");
                    xmlElement.SetAttribute("left", motorInfo.stepLeft.ToString());
                    xmlElement.SetAttribute("right", motorInfo.stepRight.ToString());
                    xmlElement.SetAttribute("front", motorInfo.stepFront.ToString());
                    xmlElement.SetAttribute("z", motorInfo.stepZ.ToString());
                    cgElement.AppendChild(xmlElement);                   

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "BottomMotor":
                                    xn.Attributes["left"].Value = motorInfo.stepLeft.ToString();
                                    xn.Attributes["right"].Value = motorInfo.stepRight.ToString();
                                    xn.Attributes["front"].Value = motorInfo.stepFront.ToString();
                                    xn.Attributes["z"].Value = motorInfo.stepZ.ToString();
                                    break;                               
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /*******************************************************************
        * function:创建XMl文档
        * input value:
        * output value:
        ********************************************************************/
        private void CreateDbXml()
        {
            XmlElement cgElement;

            //root node    
            cgElement = xmlDoc.CreateElement("", "AutoWelding", "");
            xmlDoc.AppendChild(cgElement);

            try
            {
                xmlDoc.Save(dbFileName);
            }
            catch (Exception ee)
            {
                lastError = "CreateDbXml:" + ee.Message;
            }
        }
        /**********************************************************************************************
         * discription: 读取 motor 配置文件
         * 
         * 
         ***********************************************************************************************/
        public int GetMotorInfo(ref MotorInfo motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;          
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("MotorInfo");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "AxisNum":
                                    motorInfo.AxisNum = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "CardNum":
                                    motorInfo.CardNum = Convert.ToInt32(  xn.ChildNodes[0].Value );
                                    break;

                                case "BottomMotor":
                                    S_BottomMotor bottomMotors = new S_BottomMotor();
                                    bottomMotors.motorLeft = Convert.ToInt32(xn.Attributes["left"].Value);
                                    bottomMotors.motorRight = Convert.ToInt32(xn.Attributes["right"].Value);
                                    bottomMotors.motorFront = Convert.ToInt32(xn.Attributes["front"].Value);
                                    bottomMotors.motorZ = Convert.ToInt32(xn.Attributes["z"].Value);
                                    motorInfo.BottomMotor = bottomMotors;
                                    break;

                                case "UpMotor":
                                    S_UpMotor upMotors = new S_UpMotor();
                                    upMotors.motorX = Convert.ToInt32(xn.Attributes["left"].Value);
                                    upMotors.motorY = Convert.ToInt32(xn.Attributes["right"].Value);
                                    motorInfo.UpMotor = upMotors;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }               
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
        * discription: 读取Platform point 信息
        * 
        * 
        ***********************************************************************************************/
        public int GetPlatformPoint(ref Coordinate motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("PlatformPoint");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {                             
                                case "BottomMotor":
                                    motorInfo.stepLeft = Convert.ToDouble(xn.Attributes["left"].Value);
                                    motorInfo.stepRight = Convert.ToDouble(xn.Attributes["right"].Value);
                                    motorInfo.stepFront = Convert.ToDouble(xn.Attributes["front"].Value);
                                    motorInfo.stepZ = Convert.ToDouble(xn.Attributes["z"].Value);  
                                    break;

                                case "UpMotor":
                                    motorInfo.stepX = Convert.ToDouble(xn.Attributes["left"].Value);
                                    motorInfo.stepY = Convert.ToDouble(xn.Attributes["right"].Value);                                   
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        public int GetZZeroPoint(ref Coordinate motorInfo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ZZeroPoint");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "BottomMotor":
                                    motorInfo.stepLeft = Convert.ToDouble(xn.Attributes["left"].Value);
                                    motorInfo.stepRight = Convert.ToDouble(xn.Attributes["right"].Value);
                                    motorInfo.stepFront = Convert.ToDouble(xn.Attributes["front"].Value);
                                    motorInfo.stepZ = Convert.ToDouble(xn.Attributes["z"].Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;        
        }

        /**********************************************************************************************
        * discription: 存储 max Position 配置信息
        * 
        * 
        ***********************************************************************************************/
        public int UpdateMaxPosition(MaxMinCoordinate maxPosition)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("MaxPosition");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "MaxPosition", "");
                    //Bottom motors
                    XmlElement xmlElement = xmlDoc.CreateElement("", "BottomMotor", "");
                    xmlElement.SetAttribute("left", maxPosition.stepLeft.ToString());
                    xmlElement.SetAttribute("right", maxPosition.stepRight.ToString());
                    xmlElement.SetAttribute("front", maxPosition.stepFront.ToString());
                    xmlElement.SetAttribute("z", maxPosition.stepZ.ToString());
                    cgElement.AppendChild(xmlElement);

                    //Up motors
                    xmlElement = xmlDoc.CreateElement("", "UpMotor", "");
                    xmlElement.SetAttribute("left", maxPosition.stepX.ToString());
                    xmlElement.SetAttribute("right", maxPosition.stepY.ToString());
                    xmlElement.SetAttribute("leftMin", maxPosition.stepMinX.ToString());
                    xmlElement.SetAttribute("rightMin", maxPosition.stepMinY.ToString());
                    cgElement.AppendChild(xmlElement);

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "BottomMotor":
                                    xn.Attributes["left"].Value = maxPosition.stepLeft.ToString();
                                    xn.Attributes["right"].Value = maxPosition.stepRight.ToString();
                                    xn.Attributes["front"].Value = maxPosition.stepFront.ToString();
                                    xn.Attributes["z"].Value = maxPosition.stepZ.ToString();
                                    break;

                                case "UpMotor":
                                    xn.Attributes["left"].Value = maxPosition.stepX.ToString();
                                    xn.Attributes["right"].Value = maxPosition.stepY.ToString();
                                    xn.Attributes["leftMin"].Value = maxPosition.stepMinX.ToString();
                                    xn.Attributes["rightMin"].Value = maxPosition.stepMinY.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
        * discription: 读取Platform point 信息
        * 
        * 
        ***********************************************************************************************/
        public int GetMaxPosition(ref MaxMinCoordinate maxPosition)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("MaxPosition");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "BottomMotor":
                                    maxPosition.stepLeft = Convert.ToDouble(xn.Attributes["left"].Value);
                                    maxPosition.stepRight = Convert.ToDouble(xn.Attributes["right"].Value);
                                    maxPosition.stepFront = Convert.ToDouble(xn.Attributes["front"].Value);
                                    maxPosition.stepZ = Convert.ToDouble(xn.Attributes["z"].Value);
                                    break;

                                case "UpMotor":
                                    maxPosition.stepX = Convert.ToDouble(xn.Attributes["left"].Value);
                                    maxPosition.stepY = Convert.ToDouble(xn.Attributes["right"].Value);
                                    maxPosition.stepMinX = Convert.ToDouble(xn.Attributes["leftMin"].Value);
                                    maxPosition.stepMinY = Convert.ToDouble(xn.Attributes["rightMin"].Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;
        }
        /**********************************************************************************************
         * discription: 存储 active trace method info
         * 
         * 
         ***********************************************************************************************/
        public int UpdateActiveTraceMethod(S_TraceMethod traceMethod)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("TraceMethod");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "TraceMethod", "");

                    XmlElement xmlElement = xmlDoc.CreateElement("", "ActiveMethod", "");                    
                    xmlElement.SetAttribute("id", traceMethod.id.ToString());
                    xmlElement.SetAttribute("name", traceMethod.name);
                    cgElement.AppendChild(xmlElement);

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "ActiveMethod":
                                    xn.Attributes["id"].Value = traceMethod.id.ToString();
                                    xn.Attributes["name"].Value = traceMethod.name;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
         * discription: 读取 active trace method info
         *
         * 
         ***********************************************************************************************/
        public int GetActiveTraceMethod(ref S_TraceMethod traceMethod)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("TraceMethod");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "ActiveMethod":
                                   traceMethod.id = Convert.ToInt32(xn.Attributes["id"].Value);
                                   traceMethod.name = xn.Attributes["name"].Value;                                  
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;
        }

        /**********************************************************************************************
         * discription: 读取生产工艺 信息
         * 
         * 
         ***********************************************************************************************/
        public int GetProductParameters(ref ProductParameter productParam)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ProductParameter");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "MiniVpp":
                                    productParam.MinimumVpp = Convert.ToSingle(xn.ChildNodes[0].Value);
                                    break;

                                case "DelayMoveOutReleaseColloidDIo":
                                    productParam.DelayMoveOutReleaseColloidDIo = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "DelayReleaseColloid":
                                    productParam.DelayReleaseColloid = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "FrontReleaseColloid":
                                    productParam.SetFrontReleaseColloid( Convert.ToInt32(xn.ChildNodes[0].Value));
                                    break;

                                case "LeftReleaseColloid":
                                    productParam.SetLeftReleaseColloid(Convert.ToInt32(xn.ChildNodes[0].Value));
                                    break;

                                case "RightReleaseColloid":
                                    productParam.SetRightReleaseColloid(Convert.ToInt32(xn.ChildNodes[0].Value));
                                    break;

                                case "DelayMoveBackReleaseColloidDIo":
                                    productParam.DelayMoveBackReleaseColloidDIo = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "BakeColloid":
                                    productParam.BakeColloid = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "AdAdjust":
                                    productParam.AdAdjust = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "MachineCode":
                                    productParam.MachineCode = Convert.ToString(xn.ChildNodes[0].Value);
                                    break;

                                case "ReleaseColloidBeforeCoupling":
                                    productParam.ReleaseColloidBeforeCoupling = Convert.ToBoolean(xn.ChildNodes[0].Value);
                                    break;

                                case "FindCenter":
                                    productParam.FindCenter = Convert.ToBoolean(xn.ChildNodes[0].Value);
                                    break;

                                case "LxLyDownVoltage":
                                    productParam.LxLyDownVoltage = Convert.ToSingle(xn.ChildNodes[0].Value);
                                    break;

                                case "LzDownVoltage":
                                    productParam.LzDownVoltage = Convert.ToSingle(xn.ChildNodes[0].Value);
                                    break;

                                case "LxLyMin":
                                    productParam.LxLyMin = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "LxLyMax":
                                    productParam.LxLyMax = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "LzMin":
                                    productParam.LzMin = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "LzMax":
                                    productParam.LzMax = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "ZMin":
                                    productParam.ZMin = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;

                                case "ZMax":
                                    productParam.ZMax = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;
                                case "DataPath":
                                    productParam.DataPath = Convert.ToString(xn.ChildNodes[0].Value);
                                    break;
                                case "AdApd":
                                    productParam.VbrConfig = Convert.ToUInt32(xn.ChildNodes[0].Value);
                                    break;
                                case "StoreSinglePrdId":
                                    productParam.StoreSinglePrdId = Convert.ToBoolean(xn.ChildNodes[0].Value);
                                    break;
                                case "FNSR":
                                    productParam.FNSR = Convert.ToBoolean(xn.ChildNodes[0].Value);
                                    break;
                                case "TOCAN":
                                    productParam.TOCAN = Convert.ToBoolean(xn.ChildNodes[0].Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;        
        }

        /**********************************************************************************************
         * discription: 存储 product 配置信息
         * 
         * 
         ***********************************************************************************************/
        public int UpdateProductParameter(ProductParameter productParameter)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ProductParameter");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "ProductParameter", "");
                    // VPP
                    XmlElement xmlElement = xmlDoc.CreateElement("", "MiniVpp", "");
                    XmlText xmlText = xmlDoc.CreateTextNode(productParameter.MinimumVpp.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    // delay move out IO
                    xmlElement = xmlDoc.CreateElement("", "DelayMoveOutReleaseColloidDIo", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.DelayMoveOutReleaseColloidDIo.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //delay release 
                    xmlElement = xmlDoc.CreateElement("", "DelayReleaseColloid", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.DelayReleaseColloid.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    // release colloid
                    xmlElement = xmlDoc.CreateElement("", "FrontReleaseColloid", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ReleaseColloid.front.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    xmlElement = xmlDoc.CreateElement("", "LeftReleaseColloid", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ReleaseColloid.left.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    xmlElement = xmlDoc.CreateElement("", "RightReleaseColloid", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ReleaseColloid.right.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //delay move back
                    xmlElement = xmlDoc.CreateElement("", "DelayMoveBackReleaseColloidDIo", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.DelayMoveBackReleaseColloidDIo.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //bake colloid
                    xmlElement = xmlDoc.CreateElement("", "BakeColloid", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.BakeColloid.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //AdAdjust
                    xmlElement = xmlDoc.CreateElement("", "AdAdjust", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.AdAdjust.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //MachineCode
                    xmlElement = xmlDoc.CreateElement("", "MachineCode", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.MachineCode.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //ReleaseColloidBeforeCoupling
                    xmlElement = xmlDoc.CreateElement("", "ReleaseColloidBeforeCoupling", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ReleaseColloidBeforeCoupling.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //FindCenter
                    xmlElement = xmlDoc.CreateElement("", "FindCenter", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.FindCenter.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //LxLyDownVoltage
                    xmlElement = xmlDoc.CreateElement("", "LxLyDownVoltage", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LxLyDownVoltage.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //LzDownVoltage
                    xmlElement = xmlDoc.CreateElement("", "LzDownVoltage", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LzDownVoltage.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //LxLyMin
                    xmlElement = xmlDoc.CreateElement("", "LxLyMin", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LxLyMin.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);


                    //LxLyMax
                    xmlElement = xmlDoc.CreateElement("", "LxLyMax", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LxLyMax.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);


                    //LzMin
                    xmlElement = xmlDoc.CreateElement("", "LzMin", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LzMin.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //LzMax
                    xmlElement = xmlDoc.CreateElement("", "LzMax", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.LzMax.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //ZMin
                    xmlElement = xmlDoc.CreateElement("", "ZMin", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ZMin.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //ZMax
                    xmlElement = xmlDoc.CreateElement("", "ZMax", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.ZMax.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //AdApd
                    xmlElement = xmlDoc.CreateElement("", "AdApd", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.VbrConfig.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //StoreSinglePrdId
                    xmlElement = xmlDoc.CreateElement("", "StoreSinglePrdId", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.StoreSinglePrdId.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //FNSR
                    xmlElement = xmlDoc.CreateElement("", "FNSR", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.FNSR.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);


                    //TOCAN
                    xmlElement = xmlDoc.CreateElement("", "TOCAN", "");
                    xmlText = xmlDoc.CreateTextNode(productParameter.TOCAN.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "MiniVpp":
                                    xn.ChildNodes[0].Value = productParameter.MinimumVpp.ToString();
                                    break;

                                case "DelayMoveOutReleaseColloidDIo":
                                    xn.ChildNodes[0].Value = productParameter.DelayMoveOutReleaseColloidDIo.ToString();
                                    break;

                                case "DelayReleaseColloid":
                                    xn.ChildNodes[0].Value = productParameter.DelayReleaseColloid.ToString();
                                    break;

                                case "FrontReleaseColloid":
                                    xn.ChildNodes[0].Value = productParameter.ReleaseColloid.front.ToString();
                                    break;

                                case "LeftReleaseColloid":
                                    xn.ChildNodes[0].Value = productParameter.ReleaseColloid.left.ToString();
                                    break;

                                case "RightReleaseColloid":
                                    xn.ChildNodes[0].Value = productParameter.ReleaseColloid.right.ToString();
                                    break;

                                case "DelayMoveBackReleaseColloidDIo":
                                    xn.ChildNodes[0].Value = productParameter.DelayMoveBackReleaseColloidDIo.ToString();
                                    break;
                                case "BakeColloid":
                                    xn.ChildNodes[0].Value = productParameter.BakeColloid.ToString();
                                    break;

                                case "AdAdjust":
                                    xn.ChildNodes[0].Value = productParameter.AdAdjust.ToString();
                                    break;

                                case "MachineCode":
                                    xn.ChildNodes[0].Value = productParameter.MachineCode.ToString();
                                    break;

                                case "ReleaseColloidBeforeCoupling":
                                    xn.ChildNodes[0].Value = productParameter.ReleaseColloidBeforeCoupling.ToString();
                                    break;

                                case "FindCenter":
                                    xn.ChildNodes[0].Value = productParameter.FindCenter.ToString();
                                    break;

                                case "LxLyDownVoltage":
                                    xn.ChildNodes[0].Value = productParameter.LxLyDownVoltage.ToString();
                                    break;

                                case "LzDownVoltage":
                                    xn.ChildNodes[0].Value = productParameter.LzDownVoltage.ToString();
                                    break;

                                case "LxLyMin":
                                    xn.ChildNodes[0].Value = productParameter.LxLyMin.ToString();
                                    break;

                                case "LxLyMax":
                                    xn.ChildNodes[0].Value = productParameter.LxLyMax.ToString();
                                    break;

                                case "LzMin":
                                    xn.ChildNodes[0].Value = productParameter.LzMin.ToString();
                                    break;

                                case "LzMax":
                                    xn.ChildNodes[0].Value = productParameter.LzMax.ToString();
                                    break;

                                case "ZMin":
                                    xn.ChildNodes[0].Value = productParameter.ZMin.ToString();
                                    break;

                                case "ZMax":
                                    xn.ChildNodes[0].Value = productParameter.ZMax.ToString();
                                    break;

                                case "AdApd":
                                    xn.ChildNodes[0].Value = productParameter.VbrConfig.ToString();
                                    break;

                                case "StoreSinglePrdId":
                                    xn.ChildNodes[0].Value = productParameter.StoreSinglePrdId.ToString();
                                    break;
                                case "FNSR":
                                    xn.ChildNodes[0].Value = productParameter.FNSR.ToString();
                                    break;
                                case "TOCAN":                                
                                    xn.ChildNodes[0].Value = productParameter.TOCAN.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        
        }

        /**********************************************************************************************
         * discription: 读取密码
         * 
         * 
         ***********************************************************************************************/
        public int GetAccountInfo(ref string pwd, ref string pme, ref string pmo)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("Account");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "PWD":
                                    pwd = Convert.ToString(xn.ChildNodes[0].Value);
                                    break;
                                case "PME":
                                    pme = Convert.ToString(xn.ChildNodes[0].Value);
                                    break;
                                case "PMO":
                                    pmo = Convert.ToString(xn.ChildNodes[0].Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;        
        }

        /**********************************************************************************************
         * discription: 写密码
         * 
         * 
         ***********************************************************************************************/
        public int UpdatePwd(string pwd, string devPermission, string opPermission)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("Account");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "Account", "");
                    // VPP
                    XmlElement xmlElement = xmlDoc.CreateElement("", "PWD", "");
                    XmlText xmlText = xmlDoc.CreateTextNode(pwd);
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    // PME
                    xmlElement = xmlDoc.CreateElement("", "PME", "");
                    xmlText = xmlDoc.CreateTextNode(devPermission);
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    // PMO
                    xmlElement = xmlDoc.CreateElement("", "PMO", "");
                    xmlText = xmlDoc.CreateTextNode(opPermission);
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement); 

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "PWD":
                                    xn.ChildNodes[0].Value = pwd;
                                    break;
                                case "PME":
                                    xn.ChildNodes[0].Value = devPermission;
                                    break;
                                case "PMO":
                                    xn.ChildNodes[0].Value = opPermission;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;        
        
        
        }

        public int UpdatePermission(string devPermission, string opPermission)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("Account");
                if (cgDBList.Count == 0)
                {
                    return -1;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "PME":
                                    xn.ChildNodes[0].Value = devPermission;
                                    break;
                                case "PMO":
                                    xn.ChildNodes[0].Value = opPermission;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;


        }

        /**********************************************************************************************
        * discription: 存储 生产批次 信息
        * 
        * 
        ***********************************************************************************************/
        public int UpdateProductBatInfo(ProductBatInfo prdBatInfo)
        {
            openSave.RegistryOp.SaveValue("PartNumber", prdBatInfo.PartNumber);
            openSave.RegistryOp.SaveValue("AssemblyLot", prdBatInfo.AssemblyLot);
            openSave.RegistryOp.SaveValue("Dietype", prdBatInfo.Dietype);
            openSave.RegistryOp.SaveValue("WaferLot", prdBatInfo.WaferLot);
            openSave.RegistryOp.SaveValue("Package", prdBatInfo.Package);
            openSave.RegistryOp.SaveValue("SampleSize", prdBatInfo.SampleSize);

            return 0;
        }

        /**********************************************************************************************
        * discription: 读取Platform point 信息
        * 
        * 
        ***********************************************************************************************/
        public int GetProductBatInfo(ref ProductBatInfo prdBatInfo)
        {
            openSave.RegistryOp.GetValue("PartNumber",ref  prdBatInfo.PartNumber);
            openSave.RegistryOp.GetValue("AssemblyLot",ref prdBatInfo.AssemblyLot);
            openSave.RegistryOp.GetValue("Dietype",ref prdBatInfo.Dietype);
            openSave.RegistryOp.GetValue("WaferLot",ref prdBatInfo.WaferLot);
            openSave.RegistryOp.GetValue("Package",ref prdBatInfo.Package);
            openSave.RegistryOp.GetValue("SampleSize", ref prdBatInfo.SampleSize);
            return 0;
        }

        /**********************************************************************************************
          * discription: 读取apd 信息
          * 
          * 
          ***********************************************************************************************/
        public int GetAPDParameters(ref ApdParam apdParam)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ApdParam");
                if (cgDBList.Count == 0)
                {
                    return 0;
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "Ir_AD_Zero_Value":
                                    apdParam.Ir_AD_Zero_Value = Convert.ToSingle(xn.ChildNodes[0].Value);
                                    break;
                                case "Ir_Calib_Value":
                                    apdParam.Ir_Calib_Value = Convert.ToSingle(xn.ChildNodes[0].Value);
                                    break;
                                case "Ir_Calib_AD_Value":
                                    apdParam.Ir_Calib_AD_Value = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Max_Value":
                                    apdParam.Vapd_Max_Value = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Max_DA_Value":
                                    apdParam.Vapd_Max_DA_Value = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Min_Value":
                                    apdParam.Vapd_Min_Value = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Min_DA_Value":
                                    apdParam.Vapd_Min_DA_Value = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Upper_Limit":
                                    apdParam.Vapd_Upper_Limit = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vapd_Lower_Limit":
                                    apdParam.Vapd_Lower_Limit = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "Vbr_Judge":
                                    apdParam.Vbr_Judge = Convert.ToSingle(xn.ChildNodes[0].Value);;
                                    break;
                                case "VbrType":
                                    apdParam.VbrType = Convert.ToInt32(xn.ChildNodes[0].Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return -2;
            }
            return 0;        
        }

        /**********************************************************************************************
         * discription: 存储 apd 配置信息
         * 
         * 
         ***********************************************************************************************/
        public int UpdateAPDParameters(ApdParam apdParam)
        {
            if (xmlDoc.IsLoaded == false)
                return -1;

            XmlElement cgElement;
            try
            {
                XmlNodeList cgDBList = xmlDoc.GetElementsByTagName("ApdParam");
                if (cgDBList.Count == 0)
                {
                    cgElement = xmlDoc.CreateElement("", "ApdParam", "");
                    //客户编号
                    XmlElement xmlElement = xmlDoc.CreateElement("", "Ir_AD_Zero_Value", "");
                    XmlText xmlText = xmlDoc.CreateTextNode(apdParam.Ir_AD_Zero_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //随工单号
                    xmlElement = xmlDoc.CreateElement("", "Ir_Calib_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Ir_Calib_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //流水单号
                    xmlElement = xmlDoc.CreateElement("", "Ir_Calib_AD_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Ir_Calib_AD_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //操作员号
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Max_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Max_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Max_DA_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Max_DA_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Min_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Min_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Min_DA_Value", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Min_DA_Value.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Upper_Limit", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Upper_Limit.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vapd_Lower_Limit", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vapd_Lower_Limit.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    //入纤功率
                    xmlElement = xmlDoc.CreateElement("", "Vbr_Judge", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.Vbr_Judge.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);


                    xmlElement = xmlDoc.CreateElement("", "VbrType", "");
                    xmlText = xmlDoc.CreateTextNode(apdParam.VbrType.ToString());
                    xmlElement.AppendChild(xmlText);
                    cgElement.AppendChild(xmlElement);

                    xmlDoc.DocumentElement.AppendChild(cgElement);
                }
                else
                {

                    foreach (XmlNode yn in cgDBList)
                    {
                        foreach (XmlNode xn in yn.ChildNodes)
                        {
                            switch (xn.Name)
                            {
                                case "Ir_AD_Zero_Value":
                                    xn.ChildNodes[0].Value = apdParam.Ir_AD_Zero_Value.ToString();
                                    break;
                                case "Ir_Calib_Value":
                                    xn.ChildNodes[0].Value = apdParam.Ir_Calib_Value.ToString();
                                    break;
                                case "Ir_Calib_AD_Value":
                                     xn.ChildNodes[0].Value = apdParam.Ir_Calib_AD_Value.ToString(); ;
                                    break;
                                case "Vapd_Max_Value":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Max_Value.ToString(); ;
                                    break;
                                case "Vapd_Max_DA_Value":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Max_DA_Value.ToString(); ;
                                    break;
                                case "Vapd_Min_Value":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Min_Value.ToString(); ;
                                    break;
                                case "Vapd_Min_DA_Value":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Min_DA_Value.ToString(); ;
                                    break;
                                case "Vapd_Upper_Limit":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Upper_Limit.ToString(); ;
                                    break;
                                case "Vapd_Lower_Limit":
                                     xn.ChildNodes[0].Value = apdParam.Vapd_Lower_Limit.ToString(); ;
                                    break;
                                case "Vbr_Judge":
                                     xn.ChildNodes[0].Value = apdParam.Vbr_Judge.ToString(); ;
                                    break;
                                case "VbrType":
                                    xn.ChildNodes[0].Value = apdParam.VbrType.ToString(); ;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                xmlDoc.Save(dbFileName);
            }
            catch
            {
                return -2;
            }
            return 0;
        }

    } //end class
}
