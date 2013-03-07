using System;
using System.Net.Http;
using System.Net.Http.Formatting;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Responses;

namespace CMZero.API.ServiceAgent
{
    public class OrganisationsServiceAgent : BaseServiceAgent, IOrganisationsServiceAgent
    {
        private readonly string _baseUri;

        public OrganisationsServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
        {
        }

        public OrganisationsServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public GetOrganisationResponse Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/organisation/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationResponse>(request);
        }

        public GetOrganisationsResponse Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/organisation/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationsResponse>(request);
        }

        public PostOrganisationResponse Post(Organisation organisation)
        {
            HttpRequestMessage request = CreatePostRequest(organisation, "/organisation/");

            return CheckResult<PostOrganisationResponse>(request);
        }

        private HttpRequestMessage CreatePostRequest<T>(T content, string path)
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = path };

            return new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
            {
                Content = new ObjectContent(typeof(T), content, new JsonMediaTypeFormatter())
            };
        }

        private T CheckResult<T>(HttpRequestMessage request)
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
