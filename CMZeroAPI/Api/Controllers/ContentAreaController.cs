using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;

namespace Api.Controllers
{
    public class ContentAreaController : ApiController
    {
        private readonly IContentAreaService _contentAreaService;

        public ContentAreaController(IContentAreaService contentAreaService)
        {
            _contentAreaService = contentAreaService;
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                ContentArea application = _contentAreaService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, application);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]ContentArea contentArea)
        {
            try
            {
                contentArea = _contentAreaService.Create(contentArea);

                return Request.CreateResponse(HttpStatusCode.Created, contentArea);
            }
            catch (ContentAreaNameAlreadyExistsInCollectionException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.ContentAreaNameAlreadyExistsInCollection
                        });
            }
            catch (CollectionIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.CollectionIdDoesNotExist
                        });
            }
            catch (CollectionIdNotPartOfApplicationException)
            {
                throw new HttpResponseException(new HttpResponseMessage
                                                    {
                                                        StatusCode = HttpStatusCode.BadRequest,
                                                        ReasonPhrase = ReasonPhrases.CollectionNotPartOfApplication
                                                    });
            }
        }

        // PUT api/values/5
        public HttpResponseMessage Put([FromBody]ContentArea contentArea)
        {
            try
            {
                contentArea = _contentAreaService.Update(contentArea);
                return Request.CreateResponse(HttpStatusCode.OK, contentArea);
            }
            catch (ApplicationIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            ReasonPhrase = ReasonPhrases.ApplicationIdNotValid,
                            StatusCode = HttpStatusCode.BadRequest
                        });
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (CollectionIdNotPartOfApplicationException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.CollectionNotPartOfApplication
                        });
            }
        }

        public HttpResponseMessage GetByCollectionId(string collectionId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _contentAreaService.GetByCollection(collectionId));
            }
            catch (CollectionIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.CollectionIdDoesNotExist
                        });
            }
        }

        public HttpResponseMessage GetByCollectionNameAndApiKey(string apiKey, string collectionName)
        {
            try
            {
                return Request.CreateResponse(
                    HttpStatusCode.OK, _contentAreaService.GetByCollectionNameAndApiKey(apiKey, collectionName));
            }
            catch (ApiKeyNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.ApiKeyNotValid
                        });
            }
            catch (CollectionNameNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.CollectionNameNotValidException
                        });
            }
        }
    }
}
