using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.Messages.Exceptions.Collections;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.API.Messages.Exceptions.Organisations;

namespace CMZero.API.ServiceAgent
{
    public abstract class BaseServiceAgent
    {
        private readonly HttpClient _client;

        protected string _baseUri;

        protected BaseServiceAgent(HttpClient client)
        {
            _client = client;
        }

        protected T GetResult<T>(HttpRequestMessage request)
        {
            HttpResponseMessage response = _client.SendAsync(request).Result;

            return GetResultFromResponse<T>(response);
        }

        private T GetResultFromResponse<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ItemNotFoundException();
            }

            HandleBadResponses(response);

            try
            {
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<T>(new[] { new JsonMediaTypeFormatter() }).Result;
            }
            catch (HttpRequestException ex)
            {
                throw new BadResponseException(ex.Message, ex);
            }
        }

        private void HandleBadResponses(HttpResponseMessage response)
        {
            HandleBadRequest(response);
            HandleConflict(response);
            HandleForbidden(response);
            HandleUnauthorised(response);
            HandleServiceUnavailable(response);
        }

        private void HandleBadRequest(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                if (response.ReasonPhrase == ReasonPhrases.OrganisationIdDoesNotExist) throw new OrganisationDoesNotExistException();
                if (response.ReasonPhrase == ReasonPhrases.CollectionNameAlreadyExists) throw new CollectionNameAlreadyExistsException();
                if (response.ReasonPhrase == ReasonPhrases.ApplicationNotPartOfOrganisation) throw new ApplicationIdNotPartOfOrganisationException();
                if (response.ReasonPhrase == ReasonPhrases.OrganisationIdNotValid) throw new OrganisationIdNotValidException();
                if (response.ReasonPhrase == ReasonPhrases.ApplicationIdNotValid) throw new ApplicationIdNotValidException();
                if (response.ReasonPhrase == ReasonPhrases.CollectionIdDoesNotExist) throw new CollectionIdNotValidException();

                var validationErrors = response.Content.ReadAsAsync<ValidationErrors>(new[] { new JsonMediaTypeFormatter() }).Result;

                throw new BadRequestException(validationErrors);
            }
        }

        private void HandleConflict(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ConflictException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleForbidden(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleUnauthorised(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AccessUnauthorisedException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleServiceUnavailable(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private string GetContentAsString(HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return string.Empty;
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        protected HttpRequestMessage CreatePostRequest<T>(T content, string path)
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = path };

            return new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
                       {
                           Content = new ObjectContent(typeof(T), content, new JsonMediaTypeFormatter())
                       };
        }

        protected HttpRequestMessage CreatePutRequest<T>(T content, string path)
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = path };

            return new HttpRequestMessage(HttpMethod.Put, uriBuilder.Uri)
                       {
                           Content = new ObjectContent(typeof(T), content, new JsonMediaTypeFormatter())
                       };
        }

        protected T CheckResult<T>(HttpRequestMessage request)
        {
            try
            {
                return GetResult<T>(request);
            }
            catch (BaseHttpException ex)
            {
                HandleKnownExceptions(ex);

                throw;
            }
        }

        private void HandleKnownExceptions(BaseHttpException baseHttpException)
        {
            //baseHttpException.ThrowAs<InvalidEmailAddressException>();
            //baseHttpException.ThrowAs<InvalidPointsRequestException>();
            //baseHttpException.ThrowAs<PartnerNotFoundException>();
            //baseHttpException.ThrowAs<ProductionAccessDisabledException>();
            //baseHttpException.ThrowAs<ReservationAlreadyCancelledException>();
            //baseHttpException.ThrowAs<ReservationInThePastException>();
            //baseHttpException.ThrowAs<ReservationNotFoundException>();
            //baseHttpException.ThrowAs<SlotLockAuthenticationException>();
            //baseHttpException.ThrowAs<TimeSlotUnavailableException>();
            //baseHttpException.ThrowAs<UnableToConnectToErbException>();
            //baseHttpException.ThrowAs<UserAccountDeactivatedException>();
            //baseHttpException.ThrowAs<UserAuthenticationException>();
            //baseHttpException.ThrowAs<WrongEmailAddressForReservationException>();

            //var cancellationFailedException = new CancellationFailedException(baseHttpException.Message);

            //if (baseHttpException.Key == cancellationFailedException.Key)
            //{
            //    throw cancellationFailedException;
            //}
        }
    }
}