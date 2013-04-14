using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.Messages.Responses.Collections;

namespace Api.Controllers
{
    public class CollectionController : ApiController
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public GetCollectionResponse Get(string id)
        {
            try
            {
                var collection = _collectionService.GetById(id);
                return new GetCollectionResponse { Collection = collection };
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        public PostCollectionResponse Post(Collection collection)
        {
            try
            {
                return new PostCollectionResponse { Collection = _collectionService.Create(collection) };
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
        }

        public PutCollectionResponse Put(Collection collection)
        {
            try
            {
                collection = _collectionService.Update(collection);
                return new PutCollectionResponse { Collection = collection };
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
    }
}
