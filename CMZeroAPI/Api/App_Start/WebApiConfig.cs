using System.Web.Http;

namespace Api.App_Start
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
                "CollectionContentAreasByCollectionId",
                "contentarea/collection/{collectionId}",
                new { controller = "ContentArea", action = "GetByCollectionId" });

            config.Routes.MapHttpRoute(
                "CollectionContentAreasByNameAndApiKey",
                "contentarea/collection/",
                new { controller = "ContentArea", action = "GetByCollectionNameAndApiKey" });

    //        config.Routes.MapHttpRoute(
    //            "ApplicationCollections",
    //            "application/collections/{apikey}",
    //            defaults: new { controller = "ApplicationCollection", action = "Get" });

    //        config.Routes.MapHttpRoute(
    //"ApplicationCollections2",
    //"application/collections/",
    //defaults: new { controller = "ApplicationCollection", action = "Get" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi", routeTemplate: "{controller}/{id}", defaults: new { id = RouteParameter.Optional });

        }
    }
}
