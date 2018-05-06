using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Security.Permissions;

namespace AutoWelding.engine
{
    
    public class AwRegistry
    {
        string hardwareInfo = "";
        Crypt crypt;
        string encryptStr = "(+3,/eKs";
        string decrypStr = "J90k8&c?";
        const string awRegistryFolder = "SOFTWARE\\AUTOWELDING";
        const string awKey = "awRegister";
        SystemInfo sysInfo;

        public AwRegistry()
        {
            sysInfo = new SystemInfo();
            hardwareInfo += sysInfo.ProcessorId;
            hardwareInfo += sysInfo.HardDiskId;

            crypt = new Crypt();
        }

        public string CreateRegistryInfo()
        {

            string registryStr = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") +hardwareInfo;
            registryStr = crypt.EncryptDES(registryStr, encryptStr);
            return registryStr;
        }

        public int CheckRegisterInfo()
        {
            string regData;
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(awRegistryFolder, false);
            if (regKey == null)
                return -1;

            object regObj = regKey.GetValue(awKey);
            if (regObj == null)
                return -1;

            regData = regObj.ToString();

            string decryptStr = crypt.DecryptDES(regData, decrypStr);

            if (decryptStr.Length < 19)
                return -2;

            if (decryptStr.Substring(19) != sysInfo.ProcessorId + sysInfo.HardDiskId)
            {
                return -2;
            }
            return 0;
        }

        public int Register( string regStr )
        {

            // Step 1 check the validation of the regString
            if (regStr.Length == 0)
                return -1;

            string decryptStr = crypt.DecryptDES(regStr, decrypStr);

            if (decryptStr.Length < 19)
                return -1;

            if (decryptStr.Substring(19) != sysInfo.ProcessorId + sysInfo.HardDiskId)
            {
                return -1;
            }

            // save it to windows register table
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(awRegistryFolder, true);
            if (regKey == null)
            {
                regKey = Registry.LocalMachine.CreateSubKey(awRegistryFolder, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (regKey == null)
                return -2;           


            regKey.SetValue(awKey, regStr);
            return 0;
        }
    }
}
