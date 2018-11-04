using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialMarketplace.Startup))]
namespace SocialMarketplace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
