using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Controllers
{
    public class HomeController : baseController
    {
        private loginUser loginUser { get; set; }
        private IUserService _userService { get; set; }
        private ILastChatService _lastChatService { get; set; }
        private IMessageService _messageService { get; set; }

        public HomeController(IUserService userService, ILastChatService lastChatService, IMessageService messageService)
        {
            _userService = userService;
            _lastChatService = lastChatService;
            _messageService = messageService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图
        public ActionResult Index()
        {
            //获取信息通知
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                return View();
            }
            else
            {
                int info = 0;
                List<LastChat> lastChats = _lastChatService.getLastChatsByUserId(loginUser.userId);
                List<LastChat> _lastChats = _lastChatService.get_LastChatsByUserId(loginUser.userId); //若用户的id为lastChatFriendId
                (lastChats as List<LastChat>).AddRange(_lastChats);
                List<LastChatsModel> lastChatsModel = _lastChatService.selectLastChatsModel(lastChats);
                _messageService.caculateUnreadNum(lastChatsModel, loginUser.userId);
                foreach (var l in lastChatsModel)
                {
                    List<Message> m = _messageService.getUnReadMessagesByLastChatIdReceiverId(l.lastChatId, loginUser.userId);
                    if (m.Count > 0)
                    {
                        info++;
                    }
                }
                Session["info"] = info;
                ViewData["info"] = Session["info"];
            }

            return View();
        }
        #endregion

        #region 从localstorage取出用户登录信息并保存
        public ActionResult setLogin(long userId)
        {
            User user = _userService.getUserById(userId);
            _userService.setLoginUser(user.userAccount, userId, user.userSchool);
            JsonResult["boo_success"] = true;
            return Json(JsonResult);
        }
        #endregion
    }
}
