using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using MyUniversity.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace MyUniversity.Controllers
{
    public class messageController : baseController
    {
        private loginUser loginUser { get; set; }
        private ILastChatService _lastChatService { get; set; }
        private IContactService _contactService { get; set; }
        private IMessageService _messageService { get; set; }
        private IUserService _userService { get; set; }

        public messageController(ILastChatService lastChatService, IContactService contactService, IMessageService messageService, IUserService userService)
        {
            _lastChatService = lastChatService;
            _contactService = contactService;
            _messageService = messageService;
            _userService = userService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图
        public ActionResult index(string group)
        {
            ViewBag.UserName = loginUser.userId;

            List<LastChat> lastChats = _lastChatService.getLastChatsByUserId(loginUser.userId);
            List<LastChat> _lastChats = _lastChatService.get_LastChatsByUserId(loginUser.userId); //若用户的id为lastChatFriendId
            (lastChats as List<LastChat>).AddRange(_lastChats);
            List<LastChatsModel> lastChatsModel = _lastChatService.selectLastChatsModel(lastChats);
            _messageService.caculateUnreadNum(lastChatsModel, loginUser.userId);
            ViewData["lastChats"] = lastChatsModel;

            ViewData["info"] = Session["info"];

            //根据每个最近联系人获取相应的聊天记录
            for (int i = 0; i < lastChats.Count; i++)
            {
                List<Message> messages = _messageService.getMessagesByLastChatId(lastChats[i].lastChatId);
                for (int n = 0; n < messages.Count; n++)
                {
                    if (messages[n].messageSendUserId == loginUser.userId)
                    {
                        messages[n].messageWhoSend = "me";
                    }
                    else
                    {
                        messages[n].messageWhoSend = "you";
                    }
                }
                ViewData[lastChats[i].lastChatId.ToString()] = messages;
            }

            //关联最近聊天人与聊天信息
            List<UserChatModel> userChatModels = _lastChatService.selectUserChatModels(lastChats);
            ViewData["userChatModels"] = userChatModels;

            //我的联系人
            List<Contact> contacts = _contactService.getContactsByUserId(loginUser.userId);
            List<ContactModel> contactModel = _contactService.selectContactsModel(contacts);
            ViewData["contacts"] = contactModel;

            //对陌生人发送私信
            if (group != "send")
            {
                User user = _userService.getUserById(Convert.ToInt32(group));
                ViewData["recieverName"] = user.userName;
            }
            return View();
        }
        #endregion

        #region 获取联系人信息
        public ActionResult getUser(long userId)
        {
            User user = _userService.getUserById(userId);
            SuperiorModel userModel = _userService.selectUserData(user);
            var jsonResult = Util.JsonHelper.Jso_ToJSON(userModel);
            return Json(jsonResult);
        }
        #endregion

        #region 标记信息未已读信息（因为双方在各自的聊天窗体）
        public ActionResult messageIsTrue(Message message)
        {
            Message messages = _messageService.getUnreadMessagesByLidSidRid(message.lastChatId, message.messageSendUserId, loginUser.userId);
            messages.messageIfRead = true;
            if (_messageService.updateMessage(messages))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更新聊天信息
        public ActionResult updateMessage(int lastChatId)
        {
            List<Message> messages = _messageService.getUnReadMessagesByLastChatIdReceiverId(lastChatId, loginUser.userId);
            if (_messageService.updateMessages(messages))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 改变导航条信息通知条数
        public ActionResult changeInfo(string statu)
        {
            Session["info"] = statu;
            JsonResult["boo_success"] = true;
            return Json(JsonResult);
        }
        #endregion

        #region 统计通知信息条数
        public ActionResult caculateInfoNum(int lastChatId)
        {
            int info = 0;
            List<LastChat> lastChats = _lastChatService.getLastChatsByUserId(loginUser.userId);
            List<LastChat> _lastChats = _lastChatService.get_LastChatsByUserId(loginUser.userId); //若用户的id为lastChatFriendId
            (lastChats as List<LastChat>).AddRange(_lastChats);
            List<LastChatsModel> lastChatsModel = _lastChatService.selectLastChatsModel(lastChats);
            _messageService.caculateUnreadNum(lastChatsModel, loginUser.userId);
            foreach (var l in lastChatsModel)
            {
                List<Message> m = _messageService.getUnReadMessagesByLastChatId(l.lastChatId);
                if (m.Count > 0)
                {
                    info++;
                }
            }
            Session["info"] = info;
            JsonResult["boo_success"] = info;
            return Json(JsonResult);
        }
        #endregion

        #region 添加新的联系人
        public ActionResult addContact(long FriendId)
        {
            Contact newContact = new Contact();
            newContact.contactFriendId = FriendId;
            newContact.contactUserId = loginUser.userId;
            newContact.contactTime = DateTime.Now;
            if (_contactService.addNewContact(newContact))
            {
                User user = _userService.getUserById(FriendId);
                ContactModel contactModel = _contactService.selectContactModel(user);
                var jsonResult = Util.JsonHelper.Jso_ToJSON(contactModel);
                return Json(jsonResult);
            }
            else
            {
                var jsonResult = "";
                return Json(jsonResult);
            }
        } 
        #endregion

        #region 获取最近聊天的id,如果有最近聊天记录，则“当前聊天id”显示出最近聊天id,如果不显示的话，导致重复添加最近联系人
        public ActionResult getLastChatId(long userId)
        {
            LastChat lastChat = _lastChatService.getLastChatByUserIdFriId(loginUser.userId, userId);
            if (lastChat != null)
            {
                JsonResult["boo_success"] = lastChat.lastChatId;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }

        } 
        #endregion

        #region 删除联系人
        public ActionResult deleteFriend(long userId)
        {
            Contact contact = _contactService.getContactByFriendUserId(userId, loginUser.userId);
            if (_contactService.deleteContact(contact))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion
      
    }
}
