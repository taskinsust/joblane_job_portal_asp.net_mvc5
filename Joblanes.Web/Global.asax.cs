using System.Configuration;
using System.IO;
using Web.Joblanes;
using Web.Joblanes.Context;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web.Joblanes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new ApplicationDbContext.DropCreateAlwaysInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

#if DEBUG

            //Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContext.DropCreateAlwaysInitializer());
#endif
        }
    }
}
