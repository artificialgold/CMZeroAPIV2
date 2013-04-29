using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.API.Messages.Responses.ContentAreas;

namespace Api.Controllers
{
    public class ContentAreaController : ApiController
    {
        private readonly IContentAreaService _contentAreaService;

        public ContentAreaController(IContentAreaService contentAreaService)
        {
            _contentAreaService = contentAreaService;
        }

        public GetContentAreaResponse Get(string id)
        {
            try
            {
                ContentArea application = _contentAreaService.GetById(id);
                return new GetContentAreaResponse { ContentArea = application };
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // POST api/values
        public PostContentAreaResponse Post([FromBody]ContentArea contentArea)
        {
            try
            {
            contentArea = _contentAreaService.Create(contentArea);
            PostContentAreaResponse response = new PostContentAreaResponse { ContentArea = contentArea };

            return response;
            }
            catch (ContentAreaNameAlreadyExistsInCollectionException)
            {
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ReasonPhrases.CollectionNameAlreadyExists });
            }
        }

        // PUT api/values/5
        public PutContentAreaResponse Put([FromBody]ContentArea contentArea)
        {
            //try
            //{
            contentArea = _contentAreaService.Update(contentArea);
            return new PutContentAreaResponse { ContentArea = contentArea };
            //}
            //catch (OrganisationIdNotValidException)
            //{
            //    throw new HttpResponseException(
            //        new HttpResponseMessage
            //            {
            //                ReasonPhrase = ReasonPhrases.OrganisationIdNotValid,
            //                StatusCode = HttpStatusCode.BadRequest
            //            });
            //}
            //catch (ItemNotFoundException)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
        }
    }
}
