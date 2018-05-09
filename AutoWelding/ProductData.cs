using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AutoWelding.awdatabase;
using AutoWelding.system;
using AutoWelding.control;

namespace AutoWelding
{
    public class ProductData
    {
        //AwExcel awExcle = null;
        int passedCount = 0;
        int failedCount = 0;
        SystemParam systemParam;

        public int PassedCount {
            get { return passedCount; }
            set { passedCount = value; }
        }

        public int FailedCount {
            get { return failedCount; }
            set { failedCount = value; }
        }

        public double PassedRate
        {
            get
            {
                if ((passedCount + failedCount) == 0)
                {
                    return 0;
                }
                return (double)passedCount / (double)(passedCount + failedCount);
            }
        }

        public ProductData( ref SystemParam sysParam )
        {
            systemParam = sysParam;
        }

        public int CreateExcelFile()
        {
            return 0;
            //awExcle = new AwExcel();
            //string dataPath = systemParam.CurrentPath + "\\data";

            //if (!Directory.Exists(dataPath))
            //{
            //    Directory.CreateDirectory(dataPath);
            //}

            //return awExcle.CreateDataBase(dataPath + "\\" + DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") + ".xls", DateTime.Now.ToString());
        }

        public int CreateExcelFile(ProductBatInfo prdBatInfo, ProductParameter prdParm)
        {
            return 0;
            //awExcle = new AwExcel();
            //string dataPath = "";

            //if (prdParm.DataPath.Length == 0)
            //    dataPath = systemParam.CurrentPath + "\\data";
            //else
            //    dataPath = prdParm.DataPath;

            //if (!Directory.Exists(dataPath))
            //{
            //    Directory.CreateDirectory(dataPath);
            //}

            ////客户编号:QE123,流水号：124343,随工单号:GH323,文件名设置为：QE123-124343-GH323.XLS
            //string fileName = "";

            //return awExcle.CreateDataBase(dataPath + "\\" + fileName, prdParm, prdBatInfo);
        }

        public int InsertEntry(ref ExcelDataUnit dataUnit)
        {
            return 0 /*awExcle.InsertEntry(ref dataUnit)*/;
        }

        public void Close()
        {
            //awExcle.Close();
        }
    }
}
