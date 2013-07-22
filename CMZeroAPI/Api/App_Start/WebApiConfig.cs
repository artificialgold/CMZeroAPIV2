using System.Web.Http;

namespace Api.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "CollectionContentAreasByCollectionId",
                "contentarea/collection/{collectionId}",
                new { controller = "ContentArea", action = "GetByCollectionId" });

            config.Routes.MapHttpRoute(
                "CollectionContentAreasByNameAndApiKey",
                "contentarea/collection/",
                new { controller = "ContentArea", action = "GetByCollectionNameAndApiKey" });

            config.Routes.MapHttpRoute(
                "CollectionsByApiKey",
                "apikey/{apikey}/collections/",
                new { controller = "Collection", action = "GetByApiKey" });


            config.Routes.MapHttpRoute(
                name: "DefaultApi", routeTemplate: "{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }
    }
}
