using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BOS.Startup))]
namespace BOS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
