using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AutoWelding
{
public class Win32Api
    {
        
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        /// <summary>
        /// �Զ���Ľṹ
        /// </summary>
        public struct AW_lParam
        {
            public int i;
        }
        /// <summary>
        /// ʹ��COPYDATASTRUCT�������ַ���
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            int lParam          //����2
        );

        
        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            ref AW_lParam lParam //����2
        );
        
        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            ref  COPYDATASTRUCT lParam  //����2
        );

        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            int lParam            // ����2
        );
        
        
        
        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            ref AW_lParam lParam //����2
        );
        
        //�첽��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // ��Ϣ�����Ĵ��ڵľ��
            int Msg,            // ��ϢID
            int wParam,         // ����1
            ref  COPYDATASTRUCT lParam  // ����2
        );

    }
}