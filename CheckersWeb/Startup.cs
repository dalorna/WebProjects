using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CheckersWeb.Startup))]
namespace CheckersWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
