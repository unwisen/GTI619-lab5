using Microsoft.Owin;
using Owin;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
[assembly: OwinStartupAttribute(typeof(Lab5.Startup))]
namespace Lab5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
