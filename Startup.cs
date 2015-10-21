using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TempoProxy.Startup))]
namespace TempoProxy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
