using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WSManAutomation;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System.Security;

namespace WinRM_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestMIAPI();
        }

        private static void TestMIAPI()
        {
            //TUTORIAL ON QUERYING DATA: https://learn.microsoft.com/en-us/previous-versions/windows/desktop/wmi_v2/how-to-implement-a-managed-mi-client

            CimCredential credentials = new CimCredential(ImpersonatedAuthenticationMechanism.Kerberos);
            //Attention! Using Basic Auth => no encryption done from WinRM. So then HTTPS has to be set up.
            //WinRM with HTTPS: https://learn.microsoft.com/en-us/troubleshoot/windows-client/system-management-components/configure-winrm-for-https

            CimSessionOptions options = new CimSessionOptions();
            options.AddDestinationCredentials(credentials);
            CimSession cimSession = CimSession.Create("leanderd.skyline.local", options);//For Keberos authenticaion it must be a DNS name, not an IP

            //CimCredential credentials = new CimCredential(PasswordAuthenticationMechanism.Basic, "", "WMIUser", ConvertStringToSecureString("DataMinerForICT123"));
            //CimSessionOptions options = new CimSessionOptions();
            //options.AddDestinationCredentials(credentials);
            //CimSession cimSession = CimSession.Create("slc-h88-g04.skyline.local", options);
            //The client also needs to enable unencrypted traffic!: https://stackoverflow.com/questions/1469791/powershell-v2-remoting-how-do-you-enable-unencrypted-traffic
            //Client + server (both!) machine must be added to the trusted host of the server you want to monitor: https://stackoverflow.com/questions/21548566/how-to-add-more-than-one-machine-to-the-trusted-hosts-list-using-winrm
            //Add the local user to the 'Remote Management Users' group + Allow access to a specific namespace (e.g. CIMV2)

            IEnumerable<CimInstance> queryInstances =  cimSession.QueryInstances(@"root\cimv2",
                            "WQL",
                            @"select name from win32_process");

            foreach (CimInstance cimInstance in queryInstances)
            {
                Console.WriteLine("{0}", cimInstance.CimInstanceProperties["Name"].Value.ToString());
            }
            Console.ReadLine();
        }

        private static SecureString ConvertStringToSecureString(string s)
        {
            SecureString secureString = new SecureString();
            for (int i = 0; i < s.Length; i++)
            {
                secureString.AppendChar(s[i]);
            }
            return secureString;
        }
    }
}
