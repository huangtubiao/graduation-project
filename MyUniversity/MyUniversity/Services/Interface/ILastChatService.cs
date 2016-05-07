using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface ILastChatService 
    {
        List<LastChat> getLastChatsByUserId(long userId);
        List<LastChat> get_LastChatsByUserId(long userId);

        List<UserChatModel> selectUserChatModels(List<LastChat> lastChats);
        List<LastChatsModel> selectLastChatsModel(List<LastChat> lastChats);

        LastChat getLastChatById(int lastChatId);
        LastChat getLastChatByUserIdFriId(long lastChatUserId, long lastChatfriendId);

        bool updateLastChat(LastChat lastChat);
        bool addNewLastChat(LastChat newLastChat);
    }
}
