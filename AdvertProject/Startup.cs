using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdvertProject.Startup))]
namespace AdvertProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
