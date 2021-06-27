using Drypoint.Unity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.SharedModels
{
    public class ApiResponse
    {
        public int Status { get; set; } = 200;
        public string Value { get; set; } = "";
        public MessageModel<string> MessageModel = new MessageModel<string>() { };

        public ApiResponse(StatusCode apiCode, string msg = null)
        {
            switch (apiCode)
            {
                case StatusCode.Status401Unauthorized:
                    {
                        Status = 401;
                        Value = "很抱歉，您无权访问该接口，请确保已经登录!";
                    }
                    break;
                case StatusCode.Status403Forbidden:
                    {
                        Status = 403;
                        Value = "很抱歉，您的访问权限等级不够，联系管理员!";
                    }
                    break;
                case StatusCode.Status404NotFound:
                    {
                        Status = 404;
                        Value = "资源不存在!";
                    }
                    break;
                case StatusCode.Status500InternalServerError:
                    {
                        Status = 500;
                        Value = msg;
                    }
                    break;
            }

            MessageModel = new MessageModel<string>()
            {
                Status = Status,
                Message = Value,
                Success = apiCode != StatusCode.Status1Ok
            };
        }
    }

   
}
