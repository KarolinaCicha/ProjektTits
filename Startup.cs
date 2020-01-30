using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjektTitsOI.Startup))]
namespace ProjektTitsOI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
