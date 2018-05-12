using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;

namespace AutoWelding
{
 public   class TC6200P
    {
        public  SerialPort comBoard;
        List<byte> buffer = new List<byte>(4096);
        StringBuilder builder = new StringBuilder();
        Stopwatch swNow = new Stopwatch();
        String dataString = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_iNum"></param>
        public TC6200P(string strCom)
        {
            comBoard = new SerialPort(strCom);
            comBoard.BaudRate = 9600;
            comBoard.DataBits = 8; //数据位
            comBoard.Parity = 0; //奇偶校验
            comBoard.StopBits = StopBits.One;//停止位
            comBoard.ReadTimeout = 1000; //读超时 
        }
        public bool getString(ref string data)
        {
            data = null;
            Stopwatch swNow = new Stopwatch();
            swNow.Restart();
            DateTime dt = DateTime.Now;
            while (true)
            {
                if (swNow.ElapsedMilliseconds > 2000)
                {
                    buffer = new List<byte>(4096);
                    return false;
                }
                int nn = comBoard.BytesToRead;
                byte[] buf = new byte[nn];
                if (nn < 2) continue;
                comBoard.Read(buf, 0, nn);//读取缓冲数据
                buffer.AddRange(buf);
                #region 读取数据
                if (buffer[buffer.Count - 2] == 0x0D && buffer[buffer.Count - 1] == 0x0A)//0x0A="/n"
                {
                    byte[] bufOut = new byte[buffer.Count];
                    for (int i = 0; i < buffer.Count; i++)
                    {
                        bufOut[i] = buffer[i];
                    }
                    data = builder.Append(Encoding.ASCII.GetString(bufOut)).ToString();
                    data = data.Replace("\r\n", "\n");
                    string[] get = data.Split('\n');
                    if (get.Length >= 2)
                    {
                        data = get[get.Length - 2];
                    }
                    else
                    {
                        data = get[0];
                    }
                    buffer = new List<byte>(4096);
                    builder.Clear();
                    return true;
                }
                #endregion
            }
        }
        public bool IsHandshaked;
        public bool Handshake()
        {
            try
            {
                IsHandshaked = false;
                comBoard.Open();
                comBoard.WriteLine("*IDN?" + '\n');

                string strReturn = "";
                if (getString(ref strReturn))///
                {
                    IsHandshaked = true;
                    return true;
                }
                comBoard.Close();
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void changeCom(string strCom)
        {
            if (comBoard.IsOpen) comBoard.Close();
            comBoard.PortName = strCom;
        }
        public bool SetVolt(float _vlot)
        {
            string strSend = "SOUR:VOLT" + _vlot.ToString("0.00")+'\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool SetVoltLimHight(float H_Volot)
        {
            string strSend = "SOUR:VOLT:LIMIT:HIGH" + H_Volot.ToString("0.00") + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool SetVoltLimLow(float L_Volot)
        {
            string strSend = "SOUR:VOLT:LIMIT:LOW" + L_Volot.ToString("0.00") + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public   bool SetCurrLimH(float _Curr)
        {
            string strSend = "SOUR:CURR:LIMIT:HIGH" + _Curr.ToString("0.00") + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool SetCurrLimL(float _Curr)
        {
            string strSend = "SOUR:CURR:LIMIT:LOW" + _Curr.ToString("0.00") + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool SetOn()
        {
            string strSend = "CONFigure:OUTPut ON" + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool SetOff()
        {
            string strSend = "CONFigure:OUTPut OFF" + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
        public bool Cls()
        {
            string strSend = "*CLS" + '\n';
            comBoard.WriteLine(strSend);
            return true;
        }
    }
    public class ComBoard
    {
        public SerialPort comBoard;
        public enum emType
        {
            Vgs=0,
            Vds=1,
            Vgd=2,
        }
        public ComBoard(string strCom)
        {
            comBoard = new SerialPort(strCom);
            comBoard.BaudRate = 9600;
            comBoard.DataBits = 8; //数据位
            comBoard.Parity = 0; //奇偶校验
            comBoard.StopBits = StopBits.One;//停止位
            comBoard.ReadTimeout = 1000; //读超时 
            comBoard.DataReceived += new SerialDataReceivedEventHandler(CommDataReceived);
        }
        public void changeCom(string strCom)
        {
            if (comBoard.IsOpen) comBoard.Close();
            comBoard.PortName = strCom;
        }
        /// <summary>
        /// 串口接收，新线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //Comm.BytesToRead中为要读入的字节长度
                int len = comBoard.BytesToRead;
                byte[] readBuffer = new byte[len];
                comBoard.Read(readBuffer, 0, len); //将数据读入缓存
                //处理readBuffer中的数据，自定义处理过程
                //string msg = encoding.GetString(readBuffer, 0, len); //获取出入库产品编号
                //DialogForm.Show("接收到的信息", msg);
            }
            catch (Exception ex)
            {
               
            }
        }
        /// <summary>
        /// 产品类型切换
        /// </summary>
        public bool SelectType(emType type)
        {
            byte[] sendByte = new byte[14];
            switch (type)
            {
                case emType.Vgs:
                    sendByte =new byte[14]{0x11,0x77,0,0,0x0c,0,0,0,0,0,0,0x0d,0x0d,0x0d};
                    break;
                case emType.Vds:
                    sendByte =new byte[14]{0x11,0x77,0x10,0x48,0,0,0,0,0,0,0,0x0d,0x0d,0x0d};
                    break;
                case emType.Vgd:
                    sendByte =new byte[14]{0x11,0x77,0x02,0x0c,0,0,0,0,0,0,0,0x0d,0x0d,0x0d};
                    break;
                default:
                    break;
            }
            comBoard.Write(sendByte, 0, sendByte.Length);
            return true;
        }
        public bool IsHandshaked;
        public bool Handshake()
        {
            try
            {
                IsHandshaked = false;
                comBoard.Open();
                IsHandshaked = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
            
        }
}

}
