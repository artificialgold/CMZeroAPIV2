using System;
using System.Net.Http;

using CMZero.API.Messages;
using CMZero.API.Messages.Responses;
using CMZero.API.Messages.Responses.Applications;
using CMZero.API.Messages.Responses.Organisations;

namespace CMZero.API.ServiceAgent
{
    public class OrganisationsServiceAgent : BaseServiceAgent, IOrganisationsServiceAgent
    {
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
                Path = string.Format("/application/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationResponse>(request);
        }

        public GetOrganisationsResponse Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/application/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationsResponse>(request);
        }

        public PostOrganisationResponse Post(Organisation organisation)
        {
            HttpRequestMessage request = CreatePostRequest(organisation, "/application/");

            return CheckResult<PostOrganisationResponse>(request);
        }

        public PutOrganisationResponse Put(Organisation organisation)
        {
            HttpRequestMessage request = CreatePutRequest(organisation, "/application/");

            return CheckResult<PutOrganisationResponse>(request);
        }
    }
    public class ApplicationsServiceAgent : BaseServiceAgent, IApplicationsServiceAgent
    {
        public ApplicationsServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
        {
        }

        public ApplicationsServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public GetApplicationResponse Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/application/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetApplicationResponse>(request);
        }

        public GetApplicationsResponse Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/application/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetApplicationsResponse>(request);
        }

        public PostApplicationResponse Post(Application application)
        {
            HttpRequestMessage request = CreatePostRequest(application, "/application/");

            return CheckResult<PostApplicationResponse>(request);
        }

        public PutApplicationResponse Put(Application application)
        {
            HttpRequestMessage request = CreatePutRequest(application, "/application/");

            return CheckResult<PutApplicationResponse>(request);
        }
    }
}
