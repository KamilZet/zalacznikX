using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zalacznikX
{
    public class PocoUserData : IUserData
    {
        public Microsoft.Exchange.WebServices.Data.ExchangeVersion Version
        {
            get { return ExchangeVersion.Exchange2013;}
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public System.Security.SecureString Password
        {
            get;
            set;
        }

        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }
}
