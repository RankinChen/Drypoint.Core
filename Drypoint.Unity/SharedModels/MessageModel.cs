using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.SharedModels
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public MessageModel<T> BuildSuccessMessage(string message = "")
        {
            return BuildMessage(true, default, message);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="response">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public MessageModel<T> BuildSuccessMessage(T response, string message = "")
        {
            return BuildMessage(true, response, message);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public MessageModel<T> BuildFailMessage(string message = "")
        {
            return BuildMessage(false, default, message);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="response">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public MessageModel<T> BuildFailMessage(T response, string message = "")
        {
            return BuildMessage(false, response, message);
        }
        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="success">失败/成功</param>
        /// <param name="response">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        private MessageModel<T> BuildMessage(bool success, T response, string message = "")
        {
            return new MessageModel<T>() { Success = success, Response = response, Message = message, };
        }
    }

    public class MessageModel : MessageModel<object>
    {

    }
}
