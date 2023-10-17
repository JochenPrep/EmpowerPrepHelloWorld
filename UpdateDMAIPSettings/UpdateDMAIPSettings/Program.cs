using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpdateDMAIPSettings
{
    internal class Program
    {
        static void Main()
        {
            string newIP = Dns.GetHostName();
            Console.WriteLine("Your IP is "+newIP+" (press enter to continue)");
            Console.ReadLine();

            UpdateFileContent("C:\\Skyline DataMiner\\SLCloud.xml", "(?<=\\<URI\\>)(.*)(?=\\<\\/URI\\>)", newIP);
            UpdateFileContent("C:\\Skyline DataMiner\\DMS.xml", "(?<=\\<DMA ip\\=\\\")(.*)(?=\\\" timestamp)", newIP);
            UpdateFileContent("C:\\Skyline DataMiner\\NATS\\nats-streaming-server\\nats-server.config", "(?<=server_name: )(.*)", newIP);

            Console.WriteLine();
            Console.WriteLine("Succesfully updated IPs");
            Console.ReadLine();
        }

        private static void UpdateFileContent(string filePath, string pattern, string newIP)
        {
            Console.WriteLine("Updating " + filePath);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist, skipping it");
                return;
            }
            string fileContent = File.ReadAllText(filePath);
            string newfileContent = Regex.Replace(fileContent, pattern, newIP);
            File.WriteAllText(filePath, newfileContent);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
