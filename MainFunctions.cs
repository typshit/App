using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;
public class AppFunctions
{
    private static readonly string apiKey = "7c18252bdbbcd7";  // Replace with your IPinfo API key

    public static string GetIpAddress()
    {
        using HttpClient client = new HttpClient();
        return client.GetStringAsync("https://api.ipify.org").Result;
    }

    public static async Task GeoLocateAndDetectVPN()
    {
        string ip = GetIpAddress();
        using HttpClient client = new HttpClient();
        string apiUrl = $"https://ipinfo.io/{ip}?token={apiKey}";
        string jsonResult = await client.GetStringAsync(apiUrl);

        Console.WriteLine("IP Information:");
        Console.WriteLine(jsonResult);
    }
}
public class VMDetection
{
    public static bool IsVirtualMachine()
    {
        return CheckBIOSManufacturer() || CheckDiskDrive() || CheckMACAddress() || CheckCPUFeatures();
    }

    // Check for known VM BIOS manufacturers
    private static bool CheckBIOSManufacturer()
    {
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS"))
        {
            foreach (var obj in searcher.Get())
            {
                string manufacturer = obj["Manufacturer"]?.ToString().ToLower() ?? string.Empty;
                if (manufacturer.Contains("vmware") || manufacturer.Contains("virtualbox") || 
                    manufacturer.Contains("qemu") || manufacturer.Contains("xen"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Check for known VM disk drives
    private static bool CheckDiskDrive()
    {
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
        {
            foreach (var obj in searcher.Get())
            {
                string model = obj["Model"]?.ToString().ToLower() ?? string.Empty;
                if (model.Contains("vmware") || model.Contains("virtualbox") || 
                    model.Contains("qemu") || model.Contains("xen"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Check for known VM MAC address prefixes
    private static bool CheckMACAddress()
    {
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE MACAddress IS NOT NULL"))
        {
            foreach (var obj in searcher.Get())
            {
                string macAddress = obj["MACAddress"]?.ToString() ?? string.Empty;
                if (macAddress.StartsWith("00:05:69") || // VMware
                    macAddress.StartsWith("00:0C:29") || // VMware
                    macAddress.StartsWith("00:1C:14") || // VMware
                    macAddress.StartsWith("08:00:27"))   // VirtualBox
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Check for CPU features that indicate virtualization
    private static bool CheckCPUFeatures()
    {
        string[] cpuInfo = GetCpuInfo();
        foreach (string info in cpuInfo)
        {
            if (info.Contains("hypervisor"))
            {
                return true;
            }
        }
        return false;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_INFO
    {
        public ushort processorArchitecture;
        public ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;
        public IntPtr maximumApplicationAddress;
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;
    }

    private static string[] GetCpuInfo()
    {
        SYSTEM_INFO systemInfo = new SYSTEM_INFO();
        GetSystemInfo(ref systemInfo);

        string[] cpuInfo = new string[2];
        cpuInfo[0] = $"Architecture: {systemInfo.processorArchitecture}";
        cpuInfo[1] = $"Hypervisor Present: {systemInfo.processorType}";

        return cpuInfo;
    }
}


