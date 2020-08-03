using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BMotionReporting.Startup))]
namespace BMotionReporting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
