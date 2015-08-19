using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace zalacznikX
{
    public class ConsoleEncryptUserData : ConsoleUserData
    {
        public static void Save()
        {
            Properties.Settings.Default.MailboxAddress = EncryptSettings.EncryptString(EncryptSettings.ToSecureString(UserData.EmailAddress));
            Properties.Settings.Default.MailboxPswd = EncryptSettings.EncryptString(UserData.Password);
        }
        public static string DecryptAddress
        {
            get
            {
                return EncryptSettings.ToInsecureString(EncryptSettings.DecryptString(Properties.Settings.Default.MailboxAddress));
            }
        }
        public static string DecryptPswd()
        {
            return EncryptSettings.ToInsecureString(EncryptSettings.DecryptString(Properties.Settings.Default.MailboxPswd));
        }
        public static SecureString DecryptPswdToSec()
        {
            return EncryptSettings.DecryptString(Properties.Settings.Default.MailboxPswd);
        }
        public static IUserData GetDecryptData()
        {
            UserData.EmailAddress = DecryptAddress;
            UserData.Password = DecryptPswdToSec();
            return UserData;

        }

    }
}
