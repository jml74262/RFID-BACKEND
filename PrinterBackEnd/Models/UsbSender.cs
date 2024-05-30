using Microsoft.Win32;
using PrinterBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace PrinterBackEnd.Models
{
    internal class USBSender
    {
        public static UsbInfo[] GetActiveDeviceNames()
        {
            return GetDeviceList(USBIDs.Valid_VIDs);
        }

        private static UsbInfo[] GetDeviceList(int[] VIDs)
        {
            string[] array = default(string[]);
            string[] array2 = default(string[]);
            int usbDeviceList = USBPort.GetUsbDeviceList(ref array, ref array2);

            var devices = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%SATO%'")
                            .Get()
                            .OfType<ManagementObject>();

            // Filter for "SATO" and get the first one
            var satoPrinter = devices
                .FirstOrDefault(device => device["Name"]?.ToString().Contains("SATO") == true);


            List<UsbInfo> list = new List<UsbInfo>();
            for (int i = 0; i < usbDeviceList; i++)
            {
                string vid;
                string pid;
                string sid = ExtractIDs(array2[i], out vid, out pid);
                if (!string.IsNullOrEmpty(vid) && !string.IsNullOrEmpty(pid))
                {
                    int vidInt;
                    if (int.TryParse(vid, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out vidInt))
                    {
                        if (Array.Exists(VIDs, e => e == vidInt))
                        {
                            string name = array2[i];
                            string portName = GetPortName(vid, pid, sid);
                            if (portName != null)
                            {
                                name = portName;
                            }
                            list.Add(new UsbInfo(name, array2[i]));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to parse VID: {vid}");
                    }
                }
            }
            return list.ToArray();
        }

        private static string ExtractIDs(string input, out string vid, out string pid)
        {
            string text = "";
            vid = "";
            pid = "";
            try
            {
                Match match = Regex.Match(input, "usb#vid_([\\da-fA-F]+)&pid_([\\da-fA-F]+)#(.*)#{");
                if (match.Groups.Count >= 4)
                {
                    vid = match.Groups[1].Value;
                    pid = match.Groups[2].Value;
                    text = match.Groups[3].Value;
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                vid = "";
                pid = "";
                text = "Error";
                Console.WriteLine($"Error extracting IDs: {ex.Message}");
            }
            return text;
        }

        private static string GetPortName(string vid, string pid, string sid)
        {
            string text = null;
            try
            {
                using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\DeviceClasses\\{28d78fad-5a12-11d1-ae5b-0000f803a8c2}");
                for (int i = 0; i < registryKey.SubKeyCount; i++)
                {
                    string text2 = registryKey.GetSubKeyNames()[i].ToUpper();
                    string text3 = "VID_" + vid.ToString().PadLeft(4, '0') + "&PID_" + pid.ToString().PadLeft(4, '0') + "#" + sid;
                    if (text2.Contains(text3.ToUpper()))
                    {
                        text = registryKey.OpenSubKey(text2 + "\\#\\Device Parameters").GetValue("Port Description").ToString();
                        if (text.Contains("SATO") && !text.Contains("SATO "))
                        {
                            text = text.Replace("SATO", "SATO ");
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                text = null;
                Console.WriteLine($"Error getting port name: {ex.Message}");
            }
            return text;
        }
    }

    internal static class USBIDs
    {
        public static readonly int[] Valid_VIDs = { /* Add valid VIDs here */ };
    }

    internal static class USBPort
    {
        public static int GetUsbDeviceList(ref string[] array, ref string[] array2)
        {
            // Implement this method to get the list of USB devices
            // For demonstration, returning dummy data
            array = new string[] { "USB\\VID_1234&PID_5678\\0001" };
            array2 = new string[] { "USB Device 1" };
            return 1;
        }
    }
}
