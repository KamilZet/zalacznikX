using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using InputWindow;

namespace zalacznikX
{
    public class EncryptUserData : IUserData
    {
        public ExchangeVersion Version 
        {
            get
            {
                return ExchangeVersion.Exchange2013;
            }
        }

        public string EmailAddress 
        {
            get 
            { 
                return EncryptSettings.ToInsecureString(EncryptSettings.DecryptString(Properties.Settings.Default.MailboxAddress)); 
            }
        }

        public SecureString Password 
        {
            get
            {
                return EncryptSettings.DecryptString(Properties.Settings.Default.MailboxPswd);
            }
        }
        public Uri AutodiscoverUrl { get; set; }
    }
}
