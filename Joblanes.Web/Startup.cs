using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web.Joblanes.Startup))]
namespace Web.Joblanes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
