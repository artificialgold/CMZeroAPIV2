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
using CMZero.API.Messages.Exceptions.Organisations;

namespace Api.Controllers
{
    public class CollectionController : ApiController
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                var collection = _collectionService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, collection);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        public HttpResponseMessage Post(Collection collection)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.Created, _collectionService.Create(collection));
            }
            catch (CollectionNameAlreadyExistsException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.CollectionNameAlreadyExists
                        });
            }
            catch (ApplicationIdNotPartOfOrganisationException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = ReasonPhrases.ApplicationNotPartOfOrganisation
                    });
            }
            catch (OrganisationIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = ReasonPhrases.OrganisationIdNotValid
                        });
            }
        }

        public HttpResponseMessage Put(Collection collection)
        {
            try
            {
                collection = _collectionService.Update(collection);
                return Request.CreateResponse(HttpStatusCode.OK, collection);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            catch (ApplicationIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            ReasonPhrase =
                                ReasonPhrases.ApplicationIdNotValid
                        });
            }
            catch (OrganisationIdNotValidException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase =
                           ReasonPhrases.OrganisationIdNotValid
                    });
            }
        }

        public HttpResponseMessage GetByApiKey(string apiKey)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _collectionService.GetCollectionsByApiKey(apiKey));
            }
            catch (ApiKeyNotValidException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = ReasonPhrases.ApiKeyNotValid
                    });
            }
        }
    }
}
