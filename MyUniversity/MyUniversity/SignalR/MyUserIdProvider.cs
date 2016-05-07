using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.SignalR
{
    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return "25";//反返回当前登陆的用户id.
            ///根据user发送消息的时候判断,比如：服务端发送消息给用户ID为25的客户端，你登陆的帐号通过这个方法返回25，就会收到消息，否则不会收到消息
        }
    }
}
