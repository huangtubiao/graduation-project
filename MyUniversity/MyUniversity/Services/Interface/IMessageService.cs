using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IMessageService 
    {
        List<Message> getMessagesByLastChatId(int lastChatId);
        List<Message> getUnReadMessagesByLastChatId(int lastChatId);
        List<Message> getUnReadMessagesByLastChatIdReceiverId(int lastChatId, long userId);
        Message getUnreadMessagesByLidSidRid(int lastChatId, long messageSendUserId, long messageReceiveUserId);

        bool addMessage(Message message);
        bool updateMessages(List<Message> messages);
        bool updateMessage(Message message);

        void caculateUnreadNum(List<LastChatsModel> lastChatsModel, long userId);
    }
}
