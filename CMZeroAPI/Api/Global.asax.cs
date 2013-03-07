using System.Reflection;
using System.Web.Http;

using Api.Infrastructure;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BootStrapper.Init(Assembly.GetExecutingAssembly());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}