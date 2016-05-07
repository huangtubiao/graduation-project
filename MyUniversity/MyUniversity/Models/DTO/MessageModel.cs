using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class MessageModel
    {
        /// <summary>
        /// 无参构造，消息类型初始化为"1"
        /// </summary>
        public MessageModel()
        {
            MsgType = "1";//默认为普通消息
        }
        /// <summary>
        /// 接收消息的用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 发送消息的名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 接收消息的组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 接收消息的连接ID
        /// </summary>
        public string ConnectionID { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息类型，1 普通，2 跳转
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 链接（类型为：跳转时需要）
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 发送消息者头像
        /// </summary>
        public string UserImg { get; set; }
        /// <summary>
        /// 当前最近对话id
        /// </summary>
        public string lastChatId { get; set; }
    }
}
