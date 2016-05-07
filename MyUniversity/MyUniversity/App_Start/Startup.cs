using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using MyUniversity.SignalR;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(MyUniversity.App_Start.Startup))]

namespace MyUniversity.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var idProvider = new MyUserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
