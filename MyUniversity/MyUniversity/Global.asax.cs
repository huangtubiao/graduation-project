using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyUniversity
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigResolver();
        }

        #region 注册控制反转
        private void ConfigResolver()
        {
            Assembly hr_namespace = Assembly.GetExecutingAssembly();


            /*----------------------- Web Api ----------------------*/
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(hr_namespace);

            builder.RegisterAssemblyTypes(hr_namespace).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(hr_namespace).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.Register(c => new MyUniversity.Models.UniversityEntities());

            // Build the container.
            var container = builder.Build();

            // Create the depenedency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            /*----------------------End Web Api --------------------*/


            /*----------------------- MVC -------------------------*/
            var mvcBuilder = new ContainerBuilder();
            mvcBuilder.RegisterControllers(hr_namespace);

            mvcBuilder.RegisterAssemblyTypes(hr_namespace).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            mvcBuilder.RegisterAssemblyTypes(hr_namespace).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            mvcBuilder.Register(c => new MyUniversity.Models.UniversityEntities());

            var mvcContainer = mvcBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(mvcContainer));
            /*---------------------End  MVC -------------------------*/
        }
        #endregion
    }
}