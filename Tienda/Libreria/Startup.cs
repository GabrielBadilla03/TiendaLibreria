using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Libreria.Startup))]
namespace Libreria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
