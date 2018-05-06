using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Reflection; // 引用这个才能使用Missing字段 
using AutoWelding.system;
using AutoWelding.control;

namespace AutoWelding.awdatabase
{
    public class AwExcel
    {
        private Microsoft.Office.Interop.Excel._Application xls_exp = null;
        private Microsoft.Office.Interop.Excel._Workbook xls_book = null;
        private Microsoft.Office.Interop.Excel._Worksheet xls_sheet = null;
        private string error = "";
        private int index = -1;
        private string fName;
        private int startIndex = 0;

        public string Error {
            get { return error; }
        }

        public AwExcel()
        {
             
        }

        ~AwExcel()
        {
            Close();
        }

        public void Close()
        {
            if (xls_sheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject((object)xls_sheet);
                xls_sheet = null;
            }

            if (xls_book != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject((object)xls_book);
                xls_book = null;
            }

            if (xls_exp != null)
            {
                xls_exp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject((object)xls_exp);
                xls_exp = null;
            }

            System.GC.Collect();
            
        }

        public int CreateDataBase( string fileName, string productId )
        {            
            try
            {
                xls_exp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xls_book = xls_exp.Workbooks.Add(true);
                xls_sheet = (Microsoft.Office.Interop.Excel._Worksheet)xls_book.Worksheets.get_Item(1);
                object misValue = System.Reflection.Missing.Value;

                xls_sheet.Cells[1, 1] = "产品ID";
                xls_sheet.Cells[1, 2] = "productId";
                xls_sheet.Cells[2, 1] = "测试序号";
                xls_sheet.Cells[2, 2] = "VPP(MV)";
                xls_sheet.Cells[2, 3] = "耦合时间";
                xls_sheet.Cells[3, 1] = "生产信息";
                xls_sheet.Cells[4, 1] = "Index";
                xls_sheet.Cells[4, 2] = "产品编号";
                xls_sheet.Cells[4, 3] = "Vpp/Iop(mV/uA)";
                xls_sheet.Cells[4, 4] = "Vbr(V)";
                xls_sheet.Cells[4, 5] = "LX1(um)";
                xls_sheet.Cells[4, 6] = "LY1(um)";
                xls_sheet.Cells[4, 7] = "LZ(um)";
                xls_sheet.Cells[4, 8] = "LX2(um)";
                xls_sheet.Cells[4, 9] = "LY2(um)";
                xls_sheet.Cells[4, 10] = "Coupling Time(S)";
                xls_sheet.Cells[4, 11] = "Total Time(S)";
                xls_sheet.Cells[4, 12] = "Date in producted";
                xls_sheet.Cells[4, 13] = "Vpp/Iop_10V(mV/uA)";
                xls_sheet.Cells[4, 14] = "Vpp/Iop_Check(mV/uA)";
                fName = fileName;                


                xls_book.Saved = true;
                //xls_exp.ActiveWorkbook.SaveCopyAs(fileName);
                xls_book.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                
                startIndex = 5;
                index = 5;
                Close();
            }
            catch (Exception ee)
            {
                Close();
                error = ee.Message;
                return -1;
            }
            return 0;
        }

        public int CreateDataBase(string fileName, ProductParameter prdParm, ProductBatInfo prdBatInfo)
        {
            try
            {
                fName = fileName;
                // Check if the file is existing
                xls_exp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (System.IO.File.Exists(fileName))
                {
                    xls_book = xls_exp.Workbooks.Open(fileName, Missing.Value,
                        false, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value, true,
                        Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
                    if (xls_book != null)
                    {
                        // find the last index
                        xls_sheet = (Microsoft.Office.Interop.Excel._Worksheet)xls_book.Worksheets.get_Item(1);
                        int i = xls_sheet.UsedRange.Rows.Count;
                        if (i >= 9)
                            startIndex = 9;
                        else
                            startIndex = i+1;
                        index = i+1;
                        Close();
                        return 0;
                    }
                }

                // Create a new file
                //xls_exp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xls_book = xls_exp.Workbooks.Add(true);
                xls_sheet = (Microsoft.Office.Interop.Excel._Worksheet)xls_book.Worksheets.get_Item(1);
                object misValue = System.Reflection.Missing.Value;

                //xls_sheet.Cells[1, 1] = "客户编号";
                //xls_sheet.Cells[1, 2] = prdBatInfo.customCode;
                //xls_sheet.Cells[2, 1] = "随工单号";
                //xls_sheet.Cells[2, 2] = prdBatInfo.suigongCode;
                //xls_sheet.Cells[3, 1] = "操作员";
                //xls_sheet.Cells[3, 2] = prdBatInfo.operatorCode;
                //xls_sheet.Cells[4, 1] = "入纤功率";
                //xls_sheet.Cells[4, 2] = prdBatInfo.ruQianPower.ToString() + "dBm";
                //xls_sheet.Cells[5, 1] = "机器号";
                //xls_sheet.Cells[5, 2] = prdParm.MachineCode;
                //xls_sheet.Cells[6, 1] = "测试序号";
                //xls_sheet.Cells[6, 2] = "VPP(MV)";
                //xls_sheet.Cells[6, 3] = "耦合时间";
                xls_sheet.Cells[6, 1] = "目标值(mV/uA)";
                xls_sheet.Cells[6, 2] = prdParm.MinimumVpp.ToString();
                xls_sheet.Cells[7, 1] = "生产信息";
                xls_sheet.Cells[8, 1] = "Index";
                xls_sheet.Cells[8, 2] = "产品编号";
                xls_sheet.Cells[8, 3] = "Vpp(mV)/Iop(uA)";
                xls_sheet.Cells[8, 4] = "Vbr(V)";
                xls_sheet.Cells[8, 5] = "LX1(um)";
                xls_sheet.Cells[8, 6] = "LY1(um)";
                xls_sheet.Cells[8, 7] = "LZ(um)";
                xls_sheet.Cells[8, 8] = "LX2(um)";
                xls_sheet.Cells[8, 9] = "LY2(um)";
                xls_sheet.Cells[8, 10] = "Coupling Time(S)";
                xls_sheet.Cells[8, 11] = "Total Time(S)";
                xls_sheet.Cells[8, 12] = "Date in producted";
                xls_sheet.Cells[8, 13] = "Vpp/Iop_10V(mV/uA)";
                xls_sheet.Cells[8, 14] = "Vpp/Iop_Check(mV/uA)";


                xls_book.Saved = true;
                xls_book.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xls_book.Save();
                startIndex = 9;
                index = 9;
                Close();
            }
            catch (Exception ee)
            {
                Close();
                error = ee.Message;
                return -1;
            }
            return 0;
        }


        public int InsertEntry(ref ExcelDataUnit unitData )
        {
            CouplingRetData retData = unitData.couplingRetData;
            double totalTime = unitData.timeCost;
            if (index < 0)
                return index;
            xls_exp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            xls_book = xls_exp.Workbooks.Open(fName, Missing.Value, 
                false, Missing.Value,Missing.Value, Missing.Value, 
                Missing.Value, Missing.Value, Missing.Value, true, 
                Missing.Value, Missing.Value, Missing.Value, 
                Missing.Value, Missing.Value);
            xls_sheet = (Microsoft.Office.Interop.Excel._Worksheet)xls_book.Worksheets.get_Item(1);            

            xls_sheet.Cells[index, 1] = Convert.ToString(index - startIndex + 1);
            xls_sheet.Cells[index, 2] = unitData.prdId;
            xls_sheet.Cells[index, 3] = retData.vpp.ToString();            
            xls_sheet.Cells[index, 4] = unitData.Vbr.ToString("F2");
            xls_sheet.Cells[index, 5] = retData.lx1.ToString();
            xls_sheet.Cells[index, 6] = retData.ly1.ToString();
            xls_sheet.Cells[index, 7] = retData.lz.ToString();
            xls_sheet.Cells[index, 8] = retData.lx2.ToString();
            xls_sheet.Cells[index, 9] = retData.ly2.ToString();
            xls_sheet.Cells[index, 10] = Convert.ToString(((double)retData.duration) / 1000000);
            xls_sheet.Cells[index, 11] = totalTime.ToString();
            xls_sheet.Cells[index, 12] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            xls_sheet.Cells[index, 13] = unitData.couplingRetData.vpp1.ToString();
            xls_sheet.Cells[index, 14] = unitData.VAPD_Vbr3V_Iop_Vpp.ToString();

            object misValue = System.Reflection.Missing.Value;
//            if (index == 3)
//            {
                xls_sheet.Columns.AutoFit();
//            }

            xls_book.Saved = true;
            //xls_exp.ActiveWorkbook.SaveCopyAs(fName);
            xls_book.Save();
            //xls_book.SaveAs(fName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            Close();
            return index++;
        }

        
    }
}
