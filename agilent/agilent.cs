using System;
using System.Collections.Generic;
using System.Text;
using Ivi.Visa.Interop;
using System.Windows.Forms;
using System.Threading;

namespace agilent
{
    public class Agilent
    {
        public FormattedIO488 ioDmm;
        public Agilent()
        {
            try
            {
                //create the formatted io object
                ioDmm = new FormattedIO488Class();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("FormattedIO488Class object creation failure. " + ex.Source + "  " + ex.Message, "GPIBMeasConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void closeIO()
        {
            try
            {
                ioDmm.IO.Close();
            }
            catch (Exception)
            {

                //throw;
            }
        }
        ~Agilent()
        {
            //close the instrument session
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            closeIO();
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        public string strAddress="NULL";
        public bool InitIO(string txtAddress)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                //create the resource manager and open a session with the instrument specified on txtAddress
                ResourceManager grm = new ResourceManager();
                ioDmm.IO = (IMessage)grm.Open(txtAddress, AccessMode.NO_LOCK, 2000, "");
                ioDmm.IO.Timeout = 7000;
                strAddress = txtAddress;
                //Enable UI
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return true;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Open failed on " + txtAddress + " " + ex.Source + "  " + ex.Message, "GPIBMeasConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ioDmm.IO = null;
                return false;
            }
        }
        public string Measure()
        {
            string dbResult;
            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                //Reset the dmm
                ioDmm.WriteString("*RST", true);
                //Clear the dmm registers

                ioDmm.WriteString("*CLS", true);
                ioDmm.WriteString("TRIG:SOUR BUS", true);
                ioDmm.WriteString("ABORT", true);
                ioDmm.WriteString("INIT", true);
                ioDmm.WriteString("TRIGGER:IMMEDIATE", true);

                ioDmm.WriteString("TRIG:SOUR BUS", true);

                // Set meter to 1 amp ac range
                ioDmm.WriteString("FETCH?", true);
                Thread.Sleep(1);
                //dbResult = (double)ioDmm.ReadNumber(IEEEASCIIType.ASCIIType_Any, true);

                dbResult = ioDmm.ReadString();
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return dbResult;

            }
            catch (SystemException ex)
            {
                MessageBox.Show("Measure command failed. " + ex.Source + "  " + ex.Message, "GPIB_Meas_Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        public double GetMeasure()
        {
            string dbResult;
            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                //Reset the dmm
                ioDmm.WriteString("*RST", true);
                //Clear the dmm registers

                ioDmm.WriteString("*CLS", true);
                ioDmm.WriteString("TRIG:SOUR BUS", true);
                ioDmm.WriteString("ABORT", true);
                ioDmm.WriteString("INIT", true);
                ioDmm.WriteString("TRIGGER:IMMEDIATE", true);

                ioDmm.WriteString("TRIG:SOUR BUS", true);

                // Set meter to 1 amp ac range
                ioDmm.WriteString("FETCH?", true);
                Thread.Sleep(1);
                //dbResult = (double)ioDmm.ReadNumber(IEEEASCIIType.ASCIIType_Any, true);

                dbResult = ioDmm.ReadString();
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                double xReturn = Convert.ToDouble(dbResult);
                return xReturn;

            }
            catch (SystemException ex)
            {
                MessageBox.Show("Measure command failed. " + ex.Source + "  " + ex.Message, "GPIB_Meas_Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        public void config()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                //Reset the dmm
                ioDmm.WriteString("*RST", true);
                //Clear the dmm registers
                ioDmm.WriteString("*CLS", true);
                ioDmm.WriteString("TRIG:SOUR BUS", true);
                ioDmm.WriteString("ABORT", true);
                ioDmm.WriteString("INIT", true);
                ioDmm.WriteString("TRIGGER:IMMEDIATE", true);
                Thread.Sleep(10);
                ioDmm.WriteString("DISP:PAGE MEAS", true);
                ioDmm.WriteString("FUNC:IMP CPD", true);
                ioDmm.WriteString("FUNC:IMP:RANG:AUTO ON", true);
                ioDmm.WriteString("FREQ 1MHZ", true);
                ioDmm.WriteString("BIAS:VOLT 0V", true);
                ioDmm.WriteString("VOLT 100MV", true);

                ioDmm.WriteString("APER MED", true);

                ioDmm.WriteString("DISP:PAGE CSET", true);
                Thread.Sleep(1000);
                ioDmm.WriteString("DISP:PAGE MEAS", true);

                ioDmm.WriteString("DISP:LINE " + '"' + "FZP INIT OK" + '"', true);
                Thread.Sleep(100);
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Configure command failed. " + ex.Source + "  " + ex.Message, "GPIBMeasConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void adjust()
        {
            ioDmm.WriteString("*RST", true);
            //Clear the dmm registers
            ioDmm.WriteString("*CLS", true);
            ioDmm.WriteString("TRIG:SOUR BUS", true);
            ioDmm.WriteString("DISP:PAGE MESET", true);
            ioDmm.WriteString("FUNC:IMP CPD", true);
            ioDmm.WriteString("FUNC:IMP:RANG:AUTO ON", true);
            ioDmm.WriteString("FREQ 1MHZ", true);
            ioDmm.WriteString("BIAS:VOLT 0V", true);
            ioDmm.WriteString("VOLT 100MV", true);
            ioDmm.WriteString("APER MED", true);
            Thread.Sleep(1000);
            ioDmm.WriteString("DISP:PAGE CSET", true);//校准页面
            ioDmm.WriteString("CORR:OPEN:STAT ON", true);
            ioDmm.WriteString("CORR:SHOR:STAT ON", true);
            ioDmm.WriteString("CORR:LOAD:STAT OFF", true);
            ioDmm.WriteString("CORR:LENG 1", true);
            ioDmm.WriteString("CORR:METH MULT", true);
            ioDmm.WriteString("CORR:LOAD:STAT OFF", true);
            ioDmm.WriteString("CORR:USE 10", true);
            ioDmm.WriteString("CORR:SPOT1:STAT ON", true);
            ioDmm.WriteString("CORR:SPOT1:FREQ 1MHZ", true);
            ioDmm.WriteString("CORR:SPOT1:OPEN", true);
            Thread.Sleep(10000);
            ioDmm.WriteString("CORR:SPOT1:SHOR", true);
            Thread.Sleep(10000);
            Thread.Sleep(1000);
            ioDmm.WriteString("DISP:PAGE MEAS", true);
            ioDmm.WriteString("DISP:LINE " + '"' + "FZP INIT OK" + '"', true);
            Thread.Sleep(100);
        }
        public void readOrder(string fileName)
        {
            
        }
    }
}
