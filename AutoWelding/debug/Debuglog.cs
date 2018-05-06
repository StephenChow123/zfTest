using System;
using System.Collections.Generic;
using System.Text;

namespace AutoWelding.debug
{
    public class Debuglog
    {
        private string debugInfo;
        //private ControlLog logForm = null;
        const int MaxLogSize = 1024 * 256;

        public string DebugInfo {
            get { return debugInfo; }        
        }

        /*********************************************************************************************
        *function: ���캯��
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public Debuglog()
        {
            debugInfo = "";
            Debuglog log = this;
            logForm = new ControlLog( ref log );
        }

        /*********************************************************************************************
        *function: ��ʾ
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public void AppendDebugInfo( string log )
        {
            if ((debugInfo.Length + log.Length) > MaxLogSize)
            {
                int len = debugInfo.Length - log.Length;
                if (len > 0)
                {
                    debugInfo = debugInfo.Substring(len);
                }
                else
                {
                    debugInfo = "";
                }
            }

            if (logForm != null)
                logForm.AppendDebugInfo(log, MaxLogSize);

            debugInfo += log;
        }

        /*********************************************************************************************
        *function: ��ʾ
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public void ShowDebugForm()
        {
            if (logForm == null)
            {
                Debuglog log = this;
                logForm = new ControlLog(ref log);
                logForm.Show();
            }
            else
            {
                logForm.Show();
            }
        }

        /*********************************************************************************************
        *function: �ر�Form, ControlLog�ص�����
        *input value: 
        *output:      
        *return value: 
        *
        *********************************************************************************************/
        public void ClosedForm()
        {
            logForm = null;
        }
    }
}
