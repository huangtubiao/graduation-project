using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.SignalR
{
    /// <summary>
    /// 接口：客户端js函数的原型，后台不要实现 
    /// </summary>
    public interface IHubClient
    {
        void recieveMessage(MessageModel msg);
        void stopConnect();
        void hello();
    }
}
