using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class LastChatService : ILastChatService
    {
        public ILastChatRepository _lastChatRepository { get; private set; }
        private loginUser loginUser { get; set; }

        public LastChatService(ILastChatRepository lastChatRepository)
        {
            this._lastChatRepository = lastChatRepository;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 条件检索
        public List<LastChat> getLastChatsByUserId(long userId)
        {
            return _lastChatRepository.Get(o => o.lastChatUserId == userId).OrderByDescending(o => o.lastChatTime).ToList();
        }

        public List<LastChat> get_LastChatsByUserId(long userId)
        {
            return _lastChatRepository.Get(o => o.lastChatfriendId == userId).OrderByDescending(o => o.lastChatTime).ToList();
        }

        public LastChat getLastChatById(int lastChatId)
        {
            return _lastChatRepository.Get(o => o.lastChatId == lastChatId).FirstOrDefault();
        }

        public LastChat getLastChatByUserIdFriId(long lastChatUserId, long lastChatfriendId)
        {
            return _lastChatRepository.Get(o => o.lastChatUserId == lastChatUserId && o.lastChatfriendId == lastChatfriendId).FirstOrDefault();
        }
        #endregion

        #region 关联最近聊天人与聊天信息
        public List<UserChatModel> selectUserChatModels(List<LastChat> lastChats)
        {
            List<UserChatModel> userChatModel = new List<UserChatModel>();
            foreach (var l in lastChats)
            {
                userChatModel.Add(new UserChatModel
                {
                    userchat = l.lastChatId
                });
            }
            return userChatModel;
        } 
        #endregion

        #region 筛选出最近聊天信息
        public List<LastChatsModel> selectLastChatsModel(List<LastChat> lastChats)
        {
            List<LastChatsModel> lastChatsModel = new List<LastChatsModel>();
            foreach (var l in lastChats)
            {
                if (l.lastChatUserId == loginUser.userId)
                {
                    lastChatsModel.Add(new LastChatsModel
                    {
                        lastChatId = l.lastChatId,
                        userImg = l.User1.userImg,
                        userName = l.User1.userName,
                        lastChatContent = l.lastChatContent,
                        lastChatfriendId = l.lastChatfriendId
                    });
                }
                else
                {
                    lastChatsModel.Add(new LastChatsModel
                    {
                        lastChatId = l.lastChatId,
                        userImg = l.User.userImg,
                        userName = l.User.userName,
                        lastChatContent = l.lastChatContent,
                        lastChatfriendId = l.lastChatUserId
                    });
                }
            }
            return lastChatsModel;
        } 
        #endregion

        #region 更新最近聊天信息
        public bool updateLastChat(LastChat lastChat)
        {
            try
            {
                _lastChatRepository.Update(lastChat);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 添加新的最近联系人
        public bool addNewLastChat(LastChat newLastChat)
        {
            try
            {
                _lastChatRepository.Add(newLastChat);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
