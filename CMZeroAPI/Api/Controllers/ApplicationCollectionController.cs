using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;

namespace Api.Controllers
{
    public class ApplicationCollectionController : ApiController
    {
        private readonly ICollectionService _collectionService;

        public ApplicationCollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public HttpResponseMessage Get(string apikey)
        {
//_collectionService.GetCollectionsForApplication()
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }
    }
}
