using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IndividualProject_BLOG.Startup))]
namespace IndividualProject_BLOG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
