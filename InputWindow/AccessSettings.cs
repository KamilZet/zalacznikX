using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Configuration;
using System.Reflection;

namespace InputWindow
{
    public class AccessSettings
    {
        public AccessSettings()
        {
            Uri UriAssemblyFolder = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            string appPath = UriAssemblyFolder.LocalPath + @"\" + "zalacznikX.exe";
            cm = ConfigurationManager.OpenExeConfiguration(appPath);
        }
        private System.Configuration.Configuration cm = null;
        public Configuration GetConfig
        {
            get { return cm; }
        }

        public static string MailAddress
        {
            get
            {
                return EncryptSettings.ToInsecureString(EncryptSettings.DecryptString(InputWindow.Properties.Settings.Default.MailAddress));
                
            }
            set
            {
                InputWindow.Properties.Settings.Default.MailAddress = EncryptSettings.EncryptString(EncryptSettings.ToSecureString(value));
                InputWindow.Properties.Settings.Default.Save();
            }
        }
        public static SecureString MailPswd
        {
            get
            {
                return EncryptSettings.DecryptString(InputWindow.Properties.Settings.Default.MailPswd);
            }
            set
            {
                InputWindow.Properties.Settings.Default.MailPswd = EncryptSettings.EncryptString(value);
                InputWindow.Properties.Settings.Default.Save();
            }
        

        }
    }
}
