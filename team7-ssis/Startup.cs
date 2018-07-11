using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(team7_ssis.Startup))]
namespace team7_ssis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
