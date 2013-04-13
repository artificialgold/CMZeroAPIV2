using System.Web.Http;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "CollectionsByApplication",
            //    routeTemplate: "collection/application/{applicationid}",
            //    defaults: new { controller = "CollectionApplication" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi", routeTemplate: "{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }
    }
}
