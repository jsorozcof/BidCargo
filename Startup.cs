using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BidCargo_.Startup))]
namespace BidCargo_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
