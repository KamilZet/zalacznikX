using Microsoft.Exchange.WebServices.Data;
using System;
using System.Linq;
using System.Security;

namespace zalacznikX
{
    public class HardUserData : IUserData
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
                return "jzien@euronetworldwide.com";
            }
        }
        public SecureString Password
        {
            get
            {
                SecureString ss = new SecureString();
                "".ToCharArray().ToList().ForEach(c => ss.AppendChar(c));
                return ss;
            }
        }
        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }

}
