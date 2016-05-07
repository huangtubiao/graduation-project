using Microsoft.AspNet.SignalR;
using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.SignalR
{
    /// <summary>
    /// 前台js调用的函数，2.0.2发下版本没有 函数模板，要升级signalR程序包到2.1.2以上。 
    /// </summary>
    public class ChatHub : Hub<IHubClient> //服务端接收客户端的调用，然后再回复给特定的客户端
    {
        UniversityEntities db = new UniversityEntities();

        public void Hello()
        {
            Clients.All.hello();
        }
        /// <summary>
        /// 所有客户端的消息接收函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void sendToAll(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.recieveMessage(new MessageModel() { MsgType = "1", UserName = name, Message = message });
        }
        /// <summary>
        /// 所有客户端断开连接的函数，断开所有连接
        /// </summary>
        public void disconnectAll()
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.stopConnect();
        }
        /// <summary>
        /// 发送给特定连接
        /// </summary>
        /// <param name="ConnectID"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void sendToConnectID(string ConnectID, string name, string message)
        {
            Clients.Client(ConnectID).recieveMessage(new MessageModel() { MsgType = "1", UserName = name, Message = message });
            //Clients.Group(,)
        }
        /// <summary>
        /// 发送给特定组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="name">发送者</param>
        /// <param name="message">消息内容</param>
        public void sendToGroup(string groupName, string name, string message, string userimg, string lastChatId)
        {
            //接受方没有在线，把信息保存到数据库，并标记为未读（a.没有最近聊天记录，则添加新的最近聊天记录和聊天信息；b.有最近聊天记录，
            //则添加新的聊天信息，并更新最近聊天记录）
            if (lastChatId != "") //有最近聊天记录
            {
                //添加新的聊天记录
                int lastChatint = Convert.ToInt32(lastChatId);
                Message newMesssge = new Message();
                newMesssge.lastChatId = lastChatint;
                newMesssge.messageContent = message;
                newMesssge.messageReceiveUserId = Convert.ToInt32(groupName);
                newMesssge.messageSendUserId = Convert.ToInt32(name);
                newMesssge.messageTime = DateTime.Now;
                db.Message.Add(newMesssge);
                //更新最近聊天信息
                LastChat uplastChat = db.LastChat.Where(o => o.lastChatId == lastChatint).FirstOrDefault();
                uplastChat.lastChatContent = message;
                db.SaveChanges();

                Clients.Group(groupName).recieveMessage(new MessageModel() { MsgType = "1", UserName = name, Message = message, UserImg = userimg, lastChatId = lastChatId });
            }
            else
            {
                //添加新的最近联系人
                LastChat newLastChat = new LastChat();
                newLastChat.lastChatContent = message;
                newLastChat.lastChatfriendId = Convert.ToInt32(groupName);
                newLastChat.lastChatTime = DateTime.Now;
                newLastChat.lastChatUserId = Convert.ToInt32(name);
                db.LastChat.Add(newLastChat);
                db.SaveChanges();
                //添加新的聊天记录
                Message newMesssge = new Message();
                newMesssge.messageContent = message;
                newMesssge.messageReceiveUserId = Convert.ToInt32(groupName);
                newMesssge.messageSendUserId = Convert.ToInt32(name);
                newMesssge.messageTime = DateTime.Now;
                int id = db.LastChat.Where(o => o.lastChatfriendId == newLastChat.lastChatfriendId && o.lastChatUserId == newLastChat.lastChatUserId).FirstOrDefault().lastChatId;
                newMesssge.lastChatId = id;
                db.Message.Add(newMesssge);
                db.SaveChanges();

                string stringId = Convert.ToString(id);
                Clients.Group(groupName).recieveMessage(new MessageModel() { MsgType = "1", UserName = name, Message = message, UserImg = userimg, lastChatId = stringId });
            }
        }
        /// <summary>
        /// 发送给特定用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void sendToUser(string userID, string name, string message)
        {
            Clients.User(userID).recieveMessage(new MessageModel() { MsgType = "1", UserName = name, Message = message });
        }
        /// <summary>
        /// 把当前连接加入组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public Task joinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }
        /// <summary>
        /// 把当前连接移出组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public Task leaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }


    }
}
