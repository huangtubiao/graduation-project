using Microsoft.AspNet.SignalR;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.SignalR
{
    /// <summary>
    /// 可供后台调用的函数
    /// </summary>
    public class ChatHubService
    {
        private readonly static Lazy<ChatHubService> _instance = new Lazy<ChatHubService>(() => new ChatHubService());

        public static ChatHubService Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private readonly IHubContext<IHubClient> _hubContext;
        private ChatHubService()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<IHubClient>("ChatHub");
        }
        /// <summary>
        /// 发送消息到指定组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        public void SendToGroup(MessageModel msg)
        {
            _hubContext.Clients.Group(msg.GroupName).recieveMessage(msg);
        }
        public void SendToConnection(MessageModel msg)
        {
            _hubContext.Clients.Client(msg.ConnectionID).recieveMessage(msg);
        }
        public void sendToAll(MessageModel msg)
        {
            // Call the broadcastMessage method to update clients.
            _hubContext.Clients.All.recieveMessage(msg);
        }
    }
}
