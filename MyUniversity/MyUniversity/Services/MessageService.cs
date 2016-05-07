using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class MessageService : IMessageService
    {
        public IMessageRepository _messageRepository { get; private set; }

        public MessageService(IMessageRepository messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        #region 条件检索
        public List<Message> getMessagesByLastChatId(int lastChatId)
        {
            return _messageRepository.Get(o => o.lastChatId == lastChatId).OrderBy(o => o.messageTime).ToList();
        }

        public List<Message> getUnReadMessagesByLastChatId(int lastChatId)
        {
            return _messageRepository.Get(o => o.lastChatId == lastChatId && o.messageIfRead == false).ToList();
        }

        public List<Message> getUnReadMessagesByLastChatIdReceiverId(int lastChatId, long userId)
        {
            return _messageRepository.Get(o => o.lastChatId == lastChatId && o.messageIfRead == false && o.messageReceiveUserId == userId).ToList();
        }

        public Message getUnreadMessagesByLidSidRid(int lastChatId, long messageSendUserId, long messageReceiveUserId)
        {
            return _messageRepository.Get(o => o.lastChatId == lastChatId && o.messageSendUserId == messageSendUserId && o.messageReceiveUserId == messageReceiveUserId && o.messageIfRead == false).FirstOrDefault();
        }
        #endregion

        #region 增加新的聊天信息
        public bool addMessage(Message message)
        {
            try
            {
                _messageRepository.Add(message);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 计算信息未读条数
        public void caculateUnreadNum(List<LastChatsModel> lastChatsModel, long userId)
        {
            foreach (var l in lastChatsModel)
            {
                l.messageUnreadNum = _messageRepository.Get(o => o.lastChatId == l.lastChatId && o.messageIfRead == false && o.messageReceiveUserId == userId).Count();
            }
        } 
        #endregion

        #region 更新聊天信息》未读信息改为已读
        public bool updateMessages(List<Message> messages)
        {
            try
            {
                foreach (var m in messages)
                {
                    m.messageIfRead = true;
                    _messageRepository.Update(m);
                }
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        public bool updateMessage(Message message)
        {
            try
            {
                _messageRepository.Update(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
