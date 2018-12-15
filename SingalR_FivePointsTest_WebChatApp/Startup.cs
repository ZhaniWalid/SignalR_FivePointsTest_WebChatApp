using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(SingalR_FivePointsTest_WebChatApp.Startup))]
[assembly: OwinStartup(typeof(SingalR_FivePointsTest_WebChatApp.Startup))]
namespace SingalR_FivePointsTest_WebChatApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
