using System;
using System.Management;
using System.Collections.Generic;
using System.Text;

namespace AutoWelding.engine
{
    class SystemInfo
    {
        private string processorId;
        private string hardDiskId;

        public string ProcessorId
        {
            get { return processorId; }
        }

        public string HardDiskId
        {
            get { return hardDiskId; }
        }

        public SystemInfo()
        {
            processorId = "";
            hardDiskId = "";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if (PC.Name == "ProcessorId")
                    {
                        processorId += PC.Value;
                    }
                }
            }

            searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject share in searcher.Get())
            {
                foreach (PropertyData PC in share.Properties)
                {
                    if (PC.Name == "PNPDeviceID")
                    {
                        hardDiskId += PC.Value;
                    }
                }
            }
        }

    }
}
