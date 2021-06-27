using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.BaseServices
{
    public interface IIPPHelper
    {
        public bool IsIP(string ip);
        public string GetRequestIP();
        public string GetMACIP();
    }
}
