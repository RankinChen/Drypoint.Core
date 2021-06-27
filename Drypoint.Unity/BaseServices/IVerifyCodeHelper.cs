using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.BaseServices
{
    public interface IVerifyCodeHelper
    {
        public string GetBase64String(out string verifyCode, int length = 4);
    }
}
